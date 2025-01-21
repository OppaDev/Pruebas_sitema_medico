using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SistemaCitasMedicas.Controllers;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas;
using Microsoft.AspNetCore.Mvc;

namespace TestSitemaMedico.Controllers
{
    public class PacientesControllerTests
    {
        private readonly AppDBContext _context;
        private readonly PacientesController _controller;

        public PacientesControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
            _context = new AppDBContext(options);
            _controller = new PacientesController(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [Fact]
        public async Task GetPacientes_RetornaListaVacia()
        {
            // Act
            var resultado = await _controller.GetPacientes() as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            var pacientes = resultado.Value as IEnumerable<Pacientes>;
            Assert.NotNull(pacientes);
            Assert.Empty(pacientes);
        }
        [Fact]
        public async Task CreatePaciente_CreaPaciente()
        {
            // Arrange
            var paciente = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };
            // Act
            var resultado = await _controller.CreatePaciente(paciente) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var pacienteCreado = resultado.Value as Pacientes;
            Assert.NotNull(pacienteCreado);
            Assert.Equal(paciente.Nombre, pacienteCreado.Nombre);
            Assert.Equal(paciente.Apellido, pacienteCreado.Apellido);
            Assert.Equal(paciente.FechaNacimiento, pacienteCreado.FechaNacimiento);
        }
        [Fact]
        public async Task CreatePaciente_FallaPorNombreVacio()
        {
            // Arrange
            var paciente = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };
            // Act
            var resultado = await _controller.CreatePaciente(paciente) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Nombre y Apellido son requeridos", resultado.Value);
        }
        [Fact]
        public async Task UpdatePaciente_ActualizaPaciente()
        {
            // Arrange
            var paciente = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };
            await _controller.CreatePaciente(paciente);
            paciente.Nombre = "Pedro";
            // Act
            var resultado = await _controller.UpdatePaciente(paciente) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var pacienteActualizado = resultado.Value as Pacientes;
            Assert.NotNull(pacienteActualizado);
            Assert.Equal(paciente.Nombre, pacienteActualizado.Nombre);
            Assert.Equal(paciente.Apellido, pacienteActualizado.Apellido);
            Assert.Equal(paciente.FechaNacimiento, pacienteActualizado.FechaNacimiento);
        }
        [Fact]
        public async Task DeletePaciente_EliminaPaciente()
        {
            // Arrange
            var paciente = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };
            await _controller.CreatePaciente(paciente);
            // Act
            var resultado = await _controller.DeletePaciente(1) as OkResult;
            // Assert
            Assert.NotNull(resultado);
            var pacientes = await _context.Pacientes.ToListAsync();
            Assert.Empty(pacientes);
        }
        [Fact]
        public async Task DeletePaciente_FallaPorCitasAsignadas()
        {
            // Arrange
            var paciente = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };
            await _controller.CreatePaciente(paciente);
            var cita = new Citas
            {
                IDCita = 1,
                Fecha = new DateTime(2021, 1, 1),
                IDPaciente = 1,
                IDDoctor = 1
            };
            await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();
            // Act
            var resultado = await _controller.DeletePaciente(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No se puede eliminar el paciente, tiene citas asignadas", resultado.Value);
        }
        [Fact]
        public async Task DeletePaciente_FallaPorNoExiste()
        {
            // Act
            var resultado = await _controller.DeletePaciente(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No existe", resultado.Value);
        }
    }
}

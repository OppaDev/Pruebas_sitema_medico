using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaCitasMedicas.Controllers;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas;
using Microsoft.AspNetCore.Mvc;

namespace TestSitemaMedico.Controllers
{
    public class DoctoresControllerTests
    {
        private readonly AppDBContext _context;
        private readonly DoctoresController _controller;

        public DoctoresControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
            _context = new AppDBContext(options);
            _controller = new DoctoresController(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [Fact]
        public async Task GetDoctores_RetornaListaVacia()
        {
            // Act
            var resultado = await _controller.GetDoctores() as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var doctores = resultado.Value as IEnumerable<Doctores>;
            Assert.NotNull(doctores);
            Assert.Empty(doctores);
        }
        [Fact]
        public async Task CreateDoctor_CreaDoctor()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            // Act
            var resultado = await _controller.CreateDoctor(doctor) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var doctorCreado = resultado.Value as Doctores;
            Assert.NotNull(doctorCreado);
            Assert.Equal(doctor.Nombre, doctorCreado.Nombre);
        }
        [Fact]
        public async Task UpdateDoctor_ActualizaDoctor()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            await _controller.CreateDoctor(doctor);
            doctor.Nombre = "Pedro";
            // Act
            var resultado = await _controller.UpdateDoctor(doctor) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var doctorActualizado = resultado.Value as Doctores;
            Assert.NotNull(doctorActualizado);
            Assert.Equal(doctor.Nombre, doctorActualizado.Nombre);
        }
        [Fact]
        public async Task DeleteDoctor_EliminaDoctor()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            await _controller.CreateDoctor(doctor);
            // Act
            var resultado = await _controller.DeleteDoctor(1) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var doctorEliminado = resultado.Value as Doctores;
            Assert.NotNull(doctorEliminado);
            Assert.Equal(doctor.Nombre, doctorEliminado.Nombre);
        }
        [Fact]
        public async Task DeleteDoctor_FallaPorCitasAsignadas()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            await _controller.CreateDoctor(doctor);
            var cita = new Citas
            {
                IDCita = 1,
                Fecha = new DateTime(2021, 1, 1),
                IDDoctor = 1,
                IDPaciente = 1
            };
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            // Act
            var resultado = await _controller.DeleteDoctor(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No se puede eliminar el doctor, tiene citas asignadas", resultado.Value);
        }
        [Fact]
        public async Task DeleteDoctor_NoExisteDoctor()
        {
            // Act
            var resultado = await _controller.DeleteDoctor(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No existe", resultado.Value);
        }
        [Fact]
        public async Task CreateDoctor_FallaPorCamposRequeridos()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez"
            };
            // Act
            var resultado = await _controller.CreateDoctor(doctor) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Nombre, Apellido y Especialidad son requeridos", resultado.Value);
        }
        [Fact]
        public async Task UpdateDoctor_FallaPorNoExiste()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            // Act
            var resultado = await _controller.UpdateDoctor(doctor) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No existe", resultado.Value);
        }
        [Fact]
        public async Task UpdateDoctor_FallaPorCamposRequeridos()
        {
            // Arrange
            var doctor = new Doctores
            {
                IDDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiologia"
            };
            await _controller.CreateDoctor(doctor);
            doctor.Nombre = null;
            // Act
            var resultado = await _controller.UpdateDoctor(doctor) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Nombre, Apellido y Especialidad son requeridos", resultado.Value);
        }
    }
}

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
    public class CitasControllerTests
    {
        private readonly AppDBContext _context;
        private readonly CitasController _controller;

        public CitasControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            _context = new AppDBContext(options);
            _controller = new CitasController(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCitas_RetornaListaVacia()
        {
            // Act
            var resultado = await _controller.GetCitas() as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            var citas = resultado.Value as IEnumerable<Citas>;
            Assert.NotNull(citas);
            Assert.Empty(citas);
        }

        [Fact]
        public async Task GetCitas_RetornaListaDeCitas()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };
            await _controller.CreateCita(cita);

            // Act
            var resultado = await _controller.GetCitas() as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            var citas = resultado.Value as IEnumerable<Citas>;
            Assert.NotNull(citas);
            Assert.Single(citas);
        }

        [Fact]
        public async Task CreateCita_CreaCitaCorrectamente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };

            // Act
            var resultado = await _controller.CreateCita(cita) as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            var citaGuardada = await _context.Citas.FindAsync(cita.IDCita);
            Assert.NotNull(citaGuardada);
            Assert.Equal(cita.Fecha, citaGuardada.Fecha);
        }

        [Fact]
        public async Task CreateCita_FallaPorFechaPasada()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2020, 1, 1, 8, 0, 0)
            };

            // Act
            var resultado = await _controller.CreateCita(cita) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Fecha no aceptada", resultado.Value);
        }

        [Fact]
        public async Task CreateCita_FallaPorPacienteInexistente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 99, // Paciente inexistente
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };

            // Act
            var resultado = await _controller.CreateCita(cita) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("El paciente no existe", resultado.Value);
        }

        [Fact]
        public async Task CreateCita_FallaPorDoctorInexistente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 99, // Doctor inexistente
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };

            // Act
            var resultado = await _controller.CreateCita(cita) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("El Doctor no Existe", resultado.Value);
        }

        [Fact]
        public async Task UpdateCita_ActualizaCitaCorrectamente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };
            await _controller.CreateCita(cita);

            cita.Fecha = new DateTime(2023, 1, 1, 9, 0, 0);

            // Act
            var resultado = await _controller.UpdateCita(cita) as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            var citaActualizada = await _context.Citas.FindAsync(cita.IDCita);
            Assert.NotNull(citaActualizada);
            Assert.Equal(cita.Fecha, citaActualizada.Fecha);
        }

        [Fact]
        public async Task UpdateCita_FallaPorCitaInexistente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 99, // Cita inexistente
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };

            // Act
            var resultado = await _controller.UpdateCita(cita) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DeleteCita_EliminaCitaCorrectamente()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };
            await _controller.CreateCita(cita);

            // Act
            var resultado = await _controller.DeleteCita(cita.IDCita) as OkResult;

            // Assert
            Assert.NotNull(resultado);
            var citaEliminada = await _context.Citas.FindAsync(cita.IDCita);
            Assert.Null(citaEliminada);
        }

        [Fact]
        public async Task DeleteCita_FallaPorCitaInexistente()
        {
            // Act
            var resultado = await _controller.DeleteCita(99) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No existe", resultado.Value);
        }

        [Fact]
        public async Task DeleteCita_FallaPorCitaAsignadaAProcedimiento()
        {
            // Arrange
            var cita = new Citas
            {
                IDCita = 1,
                IDPaciente = 1,
                IDDoctor = 1,
                Fecha = new DateTime(2023, 1, 1, 8, 0, 0)
            };
            await _controller.CreateCita(cita);

            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                IDCita = 1,
                Descripcion = "Procedimiento",
                Costo = 100
            };
            await _context.Procedimientos.AddAsync(procedimiento);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _controller.DeleteCita(cita.IDCita) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
        }

    }
}

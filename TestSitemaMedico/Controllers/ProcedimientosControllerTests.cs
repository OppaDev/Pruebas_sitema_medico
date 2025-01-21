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
    public class ProcedimientosControllerTests
    {
        private readonly AppDBContext _context;
        private readonly ProcedimientosController _controller;

        public ProcedimientosControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
            _context = new AppDBContext(options);
            _controller = new ProcedimientosController(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [Fact]
        public async Task GetProcedimientos_RetornaListaVacia()
        {
            // Act
            var resultado = await _controller.GetProcedimientos() as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var procedimientos = resultado.Value as IEnumerable<Procedimientos>;
            Assert.NotNull(procedimientos);
            Assert.Empty(procedimientos);
        }
        [Fact]
        public async Task CreateProcedimiento_CreaProcedimiento()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            // Act
            var resultado = await _controller.CreateProcedimiento(procedimiento) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var procedimientoCreado = resultado.Value as Procedimientos;
            Assert.NotNull(procedimientoCreado);
            Assert.Equal(procedimiento.Descripcion, procedimientoCreado.Descripcion);
        }
        [Fact]
        public async Task CreateProcedimiento_FallaCreacion()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = -100,
                IDCita = 1
            };
            // Act
            var resultado = await _controller.CreateProcedimiento(procedimiento) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Precio Negativo no aceptado", resultado.Value);
        }
        [Fact]
        public async Task UpdateProcedimiento_ActualizaProcedimiento()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            await _controller.CreateProcedimiento(procedimiento);
            procedimiento.Descripcion = "Procedimiento 2";
            // Act
            var resultado = await _controller.UpdateProcedimiento(procedimiento) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var procedimientoActualizado = resultado.Value as Procedimientos;
            Assert.NotNull(procedimientoActualizado);
            Assert.Equal(procedimiento.Descripcion, procedimientoActualizado.Descripcion);
        }
        [Fact]
        public async Task UpdateProcedimiento_FallaActualizacion()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            await _controller.CreateProcedimiento(procedimiento);
            procedimiento.Costo = -100;
            // Act
            var resultado = await _controller.UpdateProcedimiento(procedimiento) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Precio Negativo no aceptado", resultado.Value);
        }
        [Fact]
        public async Task DeleteProcedimiento_EliminaProcedimiento()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            await _controller.CreateProcedimiento(procedimiento);
            // Act
            var resultado = await _controller.DeleteProcedimiento(1) as OkObjectResult;
            // Assert
            Assert.NotNull(resultado);
            var procedimientoEliminado = resultado.Value as Procedimientos;
            Assert.NotNull(procedimientoEliminado);
            Assert.Equal(procedimiento.Descripcion, procedimientoEliminado.Descripcion);
        }
        [Fact]
        public async Task DeleteProcedimiento_FallaEliminacion()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            await _controller.CreateProcedimiento(procedimiento);
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
            var resultado = await _controller.DeleteProcedimiento(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("La cita no existe", resultado.Value);
        }
        [Fact]
        public async Task DeleteProcedimiento_FallaEliminacionCitaAsignada()
        {
            // Arrange
            var procedimiento = new Procedimientos
            {
                IDProcedimiento = 1,
                Descripcion = "Procedimiento 1",
                Costo = 100,
                IDCita = 1
            };
            await _controller.CreateProcedimiento(procedimiento);
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
            var resultado = await _controller.DeleteProcedimiento(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No se puede eliminar el procedimiento, tiene citas asignadas", resultado.Value);
        }
        [Fact]
        public async Task DeleteProcedimiento_NoExisteProcedimiento()
        {
            // Act
            var resultado = await _controller.DeleteProcedimiento(1) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("No existe", resultado.Value);
        }
    }
}

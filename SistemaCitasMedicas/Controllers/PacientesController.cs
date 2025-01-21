using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public PacientesController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //obtener poacientes
        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            return Ok(await _dbContext.Pacientes.ToListAsync());
        }

        //crear paciente
        [HttpPost]
        public async Task<IActionResult> CreatePaciente(Pacientes paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre) || 
                string.IsNullOrWhiteSpace(paciente.Apellido))
            {
                return BadRequest("Nombre y Apellido son requeridos");
            }

            _dbContext.Pacientes.Add(paciente);
            await _dbContext.SaveChangesAsync();
            return Ok(paciente);
        }

        //actualizar paciente
        [HttpPut]
        public async Task<IActionResult> UpdatePaciente(Pacientes paciente)
        {
            _dbContext.Entry(paciente).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(paciente);
        }
        //eliminar paciente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _dbContext.Pacientes.FindAsync(id);    
            var cita = await _dbContext.Citas.FirstOrDefaultAsync(x => x.IdPaciente == id);
            if (cita != null)
            {
                return BadRequest("No se puede eliminar el paciente, tiene citas asignadas");
            }
            if (paciente == null)
            {
                return BadRequest("No existe");
            }
            _dbContext.Pacientes.Remove(paciente);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

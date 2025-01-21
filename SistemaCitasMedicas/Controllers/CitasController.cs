using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public CitasController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Validaciones
        private async Task<IActionResult?> ValidarCita(Citas cita)
        {
            if (cita.Fecha < DateTime.Today)
            {
                return BadRequest("Fecha no aceptada");
            }

            if (cita.Hora < TimeSpan.FromHours(7) || cita.Hora > TimeSpan.FromHours(16))
            {
                return BadRequest("Hora no aceptada");
            }        

            var pacienteExiste = await _dbContext.Pacientes.AnyAsync(p => p.IdPaciente == cita.IdPaciente);
            if (!pacienteExiste)
            {
                return BadRequest("El paciente no existe");
            }

            var doctorExiste = await _dbContext.Doctores.AnyAsync(d => d.IdDoctor == cita.IdDoctor);
            if (!doctorExiste)
            {
                return BadRequest("El Doctor no Existe");
            }

            return null; 
        }

        [HttpGet]
        public async Task<IActionResult> GetCitas()
        {
            return Ok(await _dbContext.Citas.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCita(Citas cita)
        {
            var validationResult = await ValidarCita(cita);
            if (validationResult != null)
            {
                return validationResult;
            }

            _dbContext.Citas.Add(cita);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCita(Citas cita)
        {
            var validationResult = await ValidarCita(cita);
            if (validationResult != null)
            {
                return validationResult;
            }

            _dbContext.Entry(cita).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(cita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _dbContext.Citas.FindAsync(id);
            var procedimiento = await _dbContext.Procedimientos.FirstOrDefaultAsync(x => x.IdCita == id);
            if (procedimiento != null)
            {
                return BadRequest("No se puede eliminar la cita, tiene procedimientos asignados");
            }
            if (cita == null)
            {
                return BadRequest("No existe");
            }

            _dbContext.Citas.Remove(cita);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

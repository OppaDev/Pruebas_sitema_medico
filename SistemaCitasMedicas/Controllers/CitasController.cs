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
        //obtener citas
        [HttpGet]
        public async Task<IActionResult> GetCitas()
        {
            return Ok(await _dbContext.Citas.ToListAsync());
        }
        //crear cita
        [HttpPost]
        public async Task<IActionResult> CreateCita(Citas cita)
        {
            if (cita.Fecha < DateTime.Today)
            {
                return BadRequest("Fecha no aceptada");
            }
            var pacienteExiste = await _dbContext.Pacientes.AnyAsync(p => p.IDPaciente == cita.IDPaciente);
            if (!pacienteExiste) 
            {
                return BadRequest("El paciente no existe");
            }
            var doctorExiste = await _dbContext.Doctores.AnyAsync(d => d.IDDoctor == cita.IDDoctor);
            if (!doctorExiste) 
            {
                return BadRequest("El Doctor no Existe");
            }
            _dbContext.Citas.Add(cita);
            await _dbContext.SaveChangesAsync();
            return Ok();

        }
        //actualizar cita
        [HttpPut]
        public async Task<IActionResult> UpdateCita(Citas cita)
        {
            if (cita.Fecha < DateTime.Today)
            {
                return BadRequest("Fecha no aceptada");
            }
            var pacienteExiste = await _dbContext.Pacientes.AnyAsync(p => p.IDPaciente == cita.IDPaciente);
            if (!pacienteExiste)
            {
                return BadRequest("El paciente no existe");
            }
            _dbContext.Entry(cita).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(cita);
        }
        //eliminar cita
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _dbContext.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            _dbContext.Citas.Remove(cita);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

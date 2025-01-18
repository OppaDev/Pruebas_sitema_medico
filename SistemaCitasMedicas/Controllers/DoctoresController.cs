using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctoresController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public DoctoresController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //obtener doctores
        [HttpGet]
        public async Task<IActionResult> GetDoctores()
        {
            return Ok(await _dbContext.Doctores.ToListAsync());
        }
        //crear doctor
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(Doctores doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.Nombre) || 
                string.IsNullOrWhiteSpace(doctor.Apellido) || 
                string.IsNullOrWhiteSpace(doctor.Especialidad))
            {
                return BadRequest("Nombre, Apellido y Especialidad son requeridos");
            }
            _dbContext.Doctores.Add(doctor);
            await _dbContext.SaveChangesAsync();
            return Ok(doctor);
        }
        //actualizar doctor
        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(Doctores doctor)
        {
            _dbContext.Entry(doctor).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(doctor);
        }
        //eliminar doctor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _dbContext.Doctores.FindAsync(id);
            var cita = await _dbContext.Citas.FirstOrDefaultAsync(x => x.IDDoctor == id);
            if (cita != null)
            {
                return BadRequest("No se puede eliminar el doctor, tiene citas asignadas");
            }
            if (doctor == null)
            {
                return NotFound();
            }
            _dbContext.Doctores.Remove(doctor);
            await _dbContext.SaveChangesAsync();
            return Ok(doctor);
        }
    }
}

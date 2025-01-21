using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedimientosController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public ProcedimientosController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //obtener procedimientos
        [HttpGet]
        public async Task<IActionResult> GetProcedimientos()
        {
            return Ok(await _dbContext.Procedimientos.ToListAsync());
        }
        //crear procedimiento
        [HttpPost]
        public async Task<IActionResult> CreateProcedimiento(Procedimientos procedimiento)
        {
            if (procedimiento.)
            if (procedimiento.Costo < 0)
            {
                return BadRequest("Precio Negativo no aceptado");
            }
            var citaExiste = await _dbContext.Citas.AnyAsync(c => c.IdCita == procedimiento.IdCita);
            if (!citaExiste)
            {
                return BadRequest("La cita no existe");
            }
            _dbContext.Procedimientos.Add(procedimiento);
            await _dbContext.SaveChangesAsync();
            return Ok(procedimiento);
        }
        //actualizar procedimiento
        [HttpPut]
        public async Task<IActionResult> UpdateProcedimiento(Procedimientos procedimiento)
        {
            if (procedimiento.Costo < 0)
            {
                return BadRequest("Precio Negativo no aceptado");
            }
            var citaExiste = await _dbContext.Citas.AnyAsync(c => c.IdCita == procedimiento.IdCita);
            if (!citaExiste)
            {
                return BadRequest("La cita no existe");
            }
            _dbContext.Entry(procedimiento).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(procedimiento);
        }
        //eliminar procedimiento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedimiento(int id)
        {
            var procedimiento = await _dbContext.Procedimientos.FindAsync(id);
            var cita = await _dbContext.Citas.FirstOrDefaultAsync(x => x.IdCita == id);
            if (cita != null)
            {
                return BadRequest("No se puede eliminar el procedimiento, tiene citas asignadas");
            }
            if (procedimiento == null)
            {
                return BadRequest("No existe");
            }
            _dbContext.Procedimientos.Remove(procedimiento);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

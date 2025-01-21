using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {

        }

        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<Doctores> Doctores { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Procedimientos> Procedimientos { get; set; }

    }

    public class Pacientes
    {
        [Key]
        public required int IdPaciente { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }

    }
    
    public class Doctores
    {
        [Key]
        public required int IdDoctor { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Especialidad { get; set; }
    }

    public class Citas
    {
        [Key]
        public required int IdCita { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public int? IdPaciente { get; set; }
        public int? IdDoctor { get; set; }
    }

    public class Procedimientos
    {
        [Key]
        public required int IdProcedimiento { get; set; }
        public string? Descripcion { get; set; }
        [JsonRequired] public decimal Costo { get; set; }
        public int? IdCita { get; set; }
    }
}

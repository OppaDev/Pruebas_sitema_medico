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
        public required int IDPaciente { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }

    }
    
    public class Doctores
    {
        [Key]
        public required int IDDoctor { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Especialidad { get; set; }
    }

    public class Citas
    {
        [Key]
        public required int IDCita { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IDPaciente { get; set; }
        public int? IDDoctor { get; set; }
    }

    public class Procedimientos
    {
        [Key]
        public required int IDProcedimiento { get; set; }
        public string? Descripcion { get; set; }
        [JsonRequired] public decimal Costo { get; set; }
        public int? IDCita { get; set; }
    }
}

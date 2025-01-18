using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace SistemaCitasMedicas.Migrations
{
    /// <inheritdoc />
   
    public partial class InitialCreate : Migration
    {
      
        public static class Constants
        {
            public const string SqlServerIdentity = "SqlServer:Identity";
            public const string NVarCharMax = "nvarchar(max)";
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    IDCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation(Constants.SqlServerIdentity, "1, 1"),
                    Fecha = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    IDPaciente = table.Column<int>(type: "int", nullable: false),
                    IDDoctor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IDCita);
                });

            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    IDDoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation(Constants.SqlServerIdentity, "1, 1"),
                    Nombre = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    Apellido = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    Especialidad = table.Column<string>(type: Constants.NVarCharMax, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.IDDoctor);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IDPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation(Constants.SqlServerIdentity, "1, 1"),
                    Nombre = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    Apellido = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    FechaNacimiento = table.Column<string>(type: Constants.NVarCharMax, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IDPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Procedimientos",
                columns: table => new
                {
                    IDProcedimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation(Constants.SqlServerIdentity, "1, 1"),
                    Descripcion = table.Column<string>(type: Constants.NVarCharMax, nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IDCita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedimientos", x => x.IDProcedimiento);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Procedimientos");
        }
    }
}

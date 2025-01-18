using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaCitasMedicas;

namespace TestProjectSistemaMedico
{
    public class PacientesControllerTest
    {
        [Fact]
        public void Test1()
        {
            //configurar caso de prueba
            var pacientes = new Pacientes
            {
                IDPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };

            //validar caso de prueba
            Assert.Equal(1, pacientes.IDPaciente);

        }
    }
}

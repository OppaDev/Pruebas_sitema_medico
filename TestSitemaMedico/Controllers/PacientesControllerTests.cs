using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SistemaCitasMedicas.Controllers;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas;

namespace TestSitemaMedico.Controllers
{
    public class PacientesControllerTests
    {
        private readonly AppDBContext _context;
        private readonly PacientesController _controller;
    }
}

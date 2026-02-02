using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class LoginRequestDto
    {
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}

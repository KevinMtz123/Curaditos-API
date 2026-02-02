using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public Guid Serie { get; set; }

        public string Name { get; set; } = null!;
    }
    public class CategoriaCreateDto
    {
        public Guid Serie { get; set; }
        public string Name { get; set; } = null!;
    }
}

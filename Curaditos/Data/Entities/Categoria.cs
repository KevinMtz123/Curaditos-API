using Curaditos.Data.Entities;

namespace Curaditos.Infraestructure.Data.Data.Entities
{
    public partial class Categoria
    {
        public int Id { get; set; }
        public Guid Serie { get; set; }

        public string Name { get; set; } = null!;
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    }
}

using Curaditos.Data.Entities;

namespace Curaditos.Infraestructure.Data.Data.Entities
{
    public partial class Producto
    {
        public int Id { get; set; }
        public Guid Serie { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public int? CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public bool Active { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime Date { get; set; }

    }
}

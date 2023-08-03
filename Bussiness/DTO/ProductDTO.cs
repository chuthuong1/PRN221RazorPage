using WebApplication1.Bussiness.DTO;
using System.Collections.Generic;
namespace WebApplication1.Bussiness.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public string? ImgUrl { get; set; }

        public virtual CategoryDTO? Category { get; set; }
    }
}

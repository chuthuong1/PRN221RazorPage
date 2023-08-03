using System.Collections.Generic;

namespace WebApplication1.Bussiness.DTO
{
    public class CategoryDTO
    {
        public CategoryDTO()
        {
            Products = new HashSet<ProductDTO>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}

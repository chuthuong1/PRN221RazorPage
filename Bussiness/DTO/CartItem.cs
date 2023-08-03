using WebApplication1.DataAccess.Models;
namespace WebApplication1.Bussiness.DTO
{
    public class CartItem
    {
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public Product product { get; set; }
    }
}

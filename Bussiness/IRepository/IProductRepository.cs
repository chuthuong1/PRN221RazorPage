using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Bussiness.DTO;

namespace WebApplication1.Bussiness.IRepository
{
    public interface IProductRepository
    {
        public List<ProductDTO> GetProducts();
        public ProductDTO GetProduct(int id);
        public Task<ProductDTO> GetProductById(int id);
        void UpdateProduct(ProductDTO product);
        public List<ProductDTO> GetProductsByCategory(int? categoryId);
        public List<CategoryDTO> GetCategories();



        void AddProduct(ProductDTO product); // Thêm phương thức AddProduct


    }
}

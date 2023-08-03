using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unipluss.Sign.ExternalContract.Entities;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Mapping;

using WebApplication1.DataAccess.Manager;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthwindContext _context;
        Mapper mapper;
        public ProductRepository(NorthwindContext context)
        {
            _context = context;
            this.mapper = MapperConfig.InitializeAutomapper();
        }

        public ProductDTO GetProduct(int id)
        {
            ProductManager m = new ProductManager(_context);
            ProductDTO product = mapper.Map<ProductDTO>(m.GetProduct(id));
            return product;
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (productEntity != null)
            {
                var productDTO = ConvertToProductDTO(productEntity);
                return productDTO;
            }

            return null;
        }
        // Phương thức chuyển đổi từ entity sang DTO
        private ProductDTO ConvertToProductDTO(Product productEntity)
        {
            var productDTO = new ProductDTO
            {
                ProductId = productEntity.ProductId,
                ProductName = productEntity.ProductName,
                SupplierId = productEntity.SupplierId,
                CategoryId = productEntity.CategoryId,
                QuantityPerUnit = productEntity.QuantityPerUnit,
                UnitPrice = productEntity.UnitPrice,
                UnitsInStock = productEntity.UnitsInStock,
                UnitsOnOrder = productEntity.UnitsOnOrder,
                ReorderLevel = productEntity.ReorderLevel,
                Discontinued = productEntity.Discontinued,

            };

            // Chuyển đổi thông tin về danh mục sản phẩm
            if (productEntity.Category != null)
            {
                productDTO.Category = new CategoryDTO
                {
                    CategoryId = productEntity.Category.CategoryId,
                    CategoryName = productEntity.Category.CategoryName,
                    Description = productEntity.Category.Description,
                    Picture = productEntity.Category.Picture
                };
            }

            return productDTO;
        }

        public List<ProductDTO> GetProducts()
        {
            ProductManager m = new ProductManager(_context);
            List<ProductDTO> products = mapper.Map<List<ProductDTO>>(m.GetProducts());
            return products;
        }



        public void UpdateProduct(ProductDTO product)
        {
            var productEntity = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (productEntity != null)
            {
                // Cập nhật thông tin số lượng sản phẩm
                productEntity.UnitsInStock = (short?)product.UnitsInStock;
                productEntity.UnitsOnOrder = (short?)product.UnitsOnOrder;
                // Lưu các thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
        }
        public List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            // Lấy danh sách sản phẩm theo danh mục (category) từ cơ sở dữ liệu
            var products = _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToList();

            // Chuyển đổi danh sách sản phẩm thành DTO và trả về
            return mapper.Map<List<ProductDTO>>(products);
        }

        public List<CategoryDTO> GetCategories()
        {
            var categories = _context.Categories.ToList();
            var categoryDTOs = mapper.Map<List<CategoryDTO>>(categories);
            return categoryDTOs;
        }

        public List<ProductDTO> GetProductsByCategory(int? categoryId)
        {
            try
            {

                // Trường hợp categoryId không phải null, load sản phẩm theo categoryId
                var products = _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .ToList();

                return mapper.Map<List<ProductDTO>>(products);

            }
            catch (Exception ex)
            {
                throw new FormatException();
            }
        }

        public Product addproduct(Product product)
        {
            // add product
            var productEntity = new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = (short?)product.UnitsInStock,
                UnitsOnOrder = (short?)product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
            };
            return productEntity;

        }

        public void AddProduct(ProductDTO product)
        {
            var productEntity = new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = (short?)product.UnitsInStock,
                UnitsOnOrder = (short?)product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
            };
            _context.Add(productEntity);
            _context.SaveChanges();
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebApplication1.DataAccess.Models;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.DTO;

namespace WebApplication1.Pages.Products
{
    public class AddModel : PageModel
    {
        private readonly NorthwindContext _context;

        public AddModel(NorthwindContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public void OnGet()
        {
            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            Categories = _context.Categories.ToList();

            // Lấy danh sách các nhà cung cấp từ cơ sở dữ liệu
            Suppliers = _context.Suppliers.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, hiển thị form với thông báo lỗi
                Categories = _context.Categories.ToList();
                Suppliers = _context.Suppliers.ToList();
                return Page();
            }

            // Lưu thông tin sản phẩm vào cơ sở dữ liệu (sử dụng _context)
            AddProduct(Product);

            // Chuyển hướng về trang danh sách sản phẩm sau khi thêm thành công
            return RedirectToPage("/Admin/Products/List");
        }

        public void AddProduct(Product product)
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
                Discontinued = product.Discontinued,
            };

            // Thêm sản phẩm mới vào cơ sở dữ liệu
            _context.Products.Add(productEntity);

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
        }
    }
}

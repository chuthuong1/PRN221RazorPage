using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly NorthwindContext _context;

        public EditModel(NorthwindContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                // Xử lý nếu không tìm thấy sản phẩm với ID đã cho
                // Ví dụ: Chuyển hướng đến trang danh sách sản phẩm
                return RedirectToPage("/Admin/Products/List");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                return Page();
            }

            // Kiểm tra xem sản phẩm có tồn tại trong cơ sở dữ liệu hay không
            var existingProduct = await _context.Products.FindAsync(Product.ProductId);
            if (existingProduct == null)
            {
                // Xử lý nếu không tìm thấy sản phẩm với ID đã cho
                // Ví dụ: Chuyển hướng đến trang danh sách sản phẩm
                return RedirectToPage("/Admin/Products/List");
            }

            // Sao chép các thuộc tính từ Product sang existingProduct
            existingProduct.ProductName = Product.ProductName;
            existingProduct.CategoryId = Product.CategoryId;
            existingProduct.QuantityPerUnit = Product.QuantityPerUnit;
            existingProduct.UnitPrice = Product.UnitPrice;
            existingProduct.Discontinued = Product.Discontinued;

            await _context.SaveChangesAsync();
            TempData["msg"] = "Update success.";

            // Chuyển hướng đến trang danh sách sản phẩm sau khi cập nhật thành công
            return RedirectToPage("/Admin/Products/List");
        }
    }
}

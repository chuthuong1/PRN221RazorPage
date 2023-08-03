using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Products
{
    public class deleteModel : PageModel
    {
        private readonly NorthwindContext _context;
        public deleteModel(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                // Xử lý nếu không tìm thấy sản phẩm với ID đã cho
                // Ví dụ: Chuyển hướng đến trang danh sách sản phẩm
                return RedirectToPage("/Admin/Products/List");
            }

            // Xóa tất cả các đơn hàng liên quan đến sản phẩm
            //var orders = await _context.Orders.Where(o => o.ProductId == id).ToListAsync();
            //foreach (var order in orders)
            //{
            //    // Xóa tất cả các chi tiết đơn hàng liên quan đến đơn hàng
            //    var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == order.OrderId).ToListAsync();
            //    foreach (var orderDetail in orderDetails)
            //    {
            //        _context.OrderDetails.Remove(orderDetail);
            //    }

            //    _context.Orders.Remove(order);
            //}

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["msg"] = "Delete success.";

            // Chuyển hướng đến trang danh sách sản phẩm sau khi xóa thành công
            return RedirectToPage("/Admin/Products/List");
        }
    }
}

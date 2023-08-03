using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Orders
{
    public class DetailModel : PageModel
    {
        private readonly NorthwindContext _context;
        public DetailModel(NorthwindContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<OrderDetail> OrderDetail { get; set; }
        [BindProperty]
        public int OrderId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            OrderId = id;

            //OrderDetail = await _context.OrderDetails.Where(e => e.OrderId == id).ToList();
            OrderDetail = _context.OrderDetails.Include(e => e.Product).Where(e => e.OrderId == id).ToList();
            return Page();
        }
      
      
    }
}

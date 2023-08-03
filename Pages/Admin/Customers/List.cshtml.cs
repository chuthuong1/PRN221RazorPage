using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Admin.Customers
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _context;
        public ListModel(NorthwindContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNum, string search)
        {
            if (HttpContext.Session.GetString("account") != "Employees")
            {
                return Redirect("/errorpage?code=401");
            }

            if (_context.Customers != null)
            {
                ViewData["search"] = search;
                // IQueryable<Customer> query = _context.Customers.Where(e => (search == null) ? true : e.ContactName.Contains(search));
                IQueryable<Customer> query = _context.Customers;
                if (search != null)
                {
                    // Hàm Where sẽ lọc danh sách khách hàng dựa trên tên khách hàng (CompanyName) chứa chuỗi search.
                    query = query.Where(e => e.CompanyName.Contains(search));
                }
                Customer = await query.ToListAsync();
            }

            return Page();
        }
    }
}

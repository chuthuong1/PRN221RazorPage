using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Orders
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _context;
        public ListModel(NorthwindContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Customer> Customers { get; set; }
        [BindProperty]
        public List<Order> Order { get; set; }
        [BindProperty]
        public Order Orders { get; set; }
        public decimal totalFreight = 0;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Customers = new List<Customer>();
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                var account = HttpContext.Session.GetString("account");
                int employeId = int.Parse(HttpContext.Session.GetString("id"));

                ViewData["id"] = id;
                if (id == null)
                {
                    Order = _context.Orders.Where(e => e.EmployeeId == employeId).ToList();
                }
                else
                {
                    Order = _context.Orders.Where(d => d.CustomerId == id && d.EmployeeId == employeId).ToList();
                }

                foreach (var order in Order)
                {
                    // tính tien
                    totalFreight += order.Freight.Value;
                }

                ViewData["totalFreight"] = totalFreight;

                List<string> customerId = _context.Orders.Where(c => c.EmployeeId == employeId).Select(c => c.CustomerId).Distinct().ToList();

                foreach (string cu in customerId)
                {
                    Customers.Add((Customer)_context.Customers.Find(cu));
                }
                return Page();

            }
        }


        public async Task<IActionResult> OnPost(FormattableString id, string start, string end)
        {
            ViewData["id"] = id;
            int employeId = int.Parse(HttpContext.Session.GetString("id"));

            ViewData["start"] = start;
            ViewData["end"] = end;
            if (id == null)
            {

                    Order = _context.Orders.Where(c => c.EmployeeId == employeId).ToList();
                
            }
            
            foreach (var item in Order)
            {
                totalFreight += item.Freight.Value;
            }

            ViewData["totalFreight"] = totalFreight;

            List<String> customerid = _context.Orders.Where(c => c.EmployeeId == employeId).Select(c => c.CustomerId).Distinct().ToList();
            foreach (string item in customerid)
            {
                Customers.Add((Customer)_context.Customers.Find(item));
            }
            return Page();
        }
    }
}

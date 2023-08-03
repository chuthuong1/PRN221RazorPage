using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly NorthwindContext _context;
        public DetailsModel(NorthwindContext context)
        {
            _context = context;
        }

        public Customer Customer { get; set; }



        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, bool active)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return Redirect("/admin/customers/details?id=" + customer.CustomerId);
        }
    }
}

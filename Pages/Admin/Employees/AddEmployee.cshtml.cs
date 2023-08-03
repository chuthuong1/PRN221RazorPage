using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Employees
{
    public class AddEmployeeModel : PageModel
    {
        private readonly NorthwindContext _context;

        public AddEmployeeModel(NorthwindContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Employees/List");
        }
    }
}

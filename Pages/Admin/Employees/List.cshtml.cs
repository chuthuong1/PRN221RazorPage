using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Models;
namespace WebApplication1.Pages.Admin.Employees
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _context;
        public IList<Employee> Employee { get; set; }

        public ListModel(NorthwindContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Employee = _context.Employees.Include(e => e.ReportsToNavigation).ToList();
        }
    }
}

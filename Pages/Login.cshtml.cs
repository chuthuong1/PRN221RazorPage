using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages
{
    public class LoginModel : PageModel
    {
        private readonly NorthwindContext _context;

        public LoginModel(NorthwindContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewData["Message"] = "Email hoặc Mật khẩu không được để trống";
                return Page();
            }
            else
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
                if (account == null)
                {
                    ViewData["Message"] = "Email hoặc Mật khẩu không đúng";
                }
                else
                {
                    if (account.Role == 1)
                    {
                        string id = account.AccountId.ToString();
                        string name = account.Email;
                        HttpContext.Session.SetString("account", "Employees");
                        HttpContext.Session.SetString("id", id);
                        HttpContext.Session.SetString("name", name);
                        return RedirectToPage("/Admin/dashboard");
                    }
                    else
                    {
                        string id = account.AccountId.ToString();
                        HttpContext.Session.SetString("account", "Customer");
                        HttpContext.Session.SetString("id", id);
                        return RedirectToPage("/Products/List");
                    }
                }
            }
            return Page();
        }
    }
}

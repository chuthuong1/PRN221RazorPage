using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using System.Collections.Generic;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin.Products
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _context;
        public ListModel(NorthwindContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Product> Products { get; set; }

        [BindProperty]

        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGet(int cateId)
        {
            if (HttpContext.Session.GetString("msg") != null)
            {
                ViewData["msg"] = HttpContext.Session.GetString("msg");
            }
            string role = HttpContext.Session.GetString("account");
            ViewData["role"] = role;
            ViewData["SelectedId"] = cateId;
            if (cateId == 0)
            {
                Products = _context.Products.Include(c => c.Category).ToList();
            }
            else
            {
                Products = _context.Products.Include(c => c.Category).Where(c => c.CategoryId == cateId).ToList();
            }
            Categories = _context.Categories.ToList();
            return Page();


        }
        public async Task<IActionResult> OnGetDelete(int cateId, int id)
        {
            var count = _context.OrderDetails.Where(od => od.ProductId == id).Count();
            if (count > 0)
            {
                TempData["msg"] = "This product existed in Order details.";
                return RedirectToPage("./List");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            ViewData["SelectedId"] = cateId;
            ViewData["SelectedId"] = cateId;
            Categories = _context.Categories.ToList();
            TempData["msg"] = "Delete success.";
            return RedirectToPage("./List");
        }

    }
}

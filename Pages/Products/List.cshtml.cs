using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Helper;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DataAccess.Models;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.ChatHubSignarl;
using Unipluss.Sign.ExternalContract.Entities;

namespace WebApplication1.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IHubContext<CartHub> _hubContext;
        public IProductRepository productRepository { get; set; }
        public List<ProductDTO> Products { get; set; } // Danh sách sản phẩm
        public List<CategoryDTO> Categories { get; set; } // Danh sách danh mục

        public List<Product> products { get; set; }

        public ListModel(IProductRepository productRepository, IHubContext<CartHub> hubContext)
        {
            this.productRepository = productRepository;
            _hubContext = hubContext;
        }

        public void OnGet(int? categoryId)
        {
            // Lấy danh sách danh mục từ repository
            Categories = productRepository.GetCategories();

            // Lấy sản phẩm theo danh mục (nếu categoryId được chọn)
            Products = categoryId != null ? productRepository.GetProductsByCategory(categoryId.Value) : productRepository.GetProducts();
        }


        public IActionResult OnPostAddToCart(int? productId)
        {
            if (productId != null)
            {
                var product = productRepository.GetProduct(productId.Value);
                if (product != null)
                {
                    // Kiểm tra số lượng sản phẩm có lớn hơn 0 hay không
                    if (product.UnitsInStock > 0)
                    {
                        AddProductToCart(product);
                        ViewData["mess"] = "Thêm vào giỏ hàng thành công!";
                    }
                    else
                    {
                        ViewData["mess"] = "Sản phẩm hiện không có sẵn trong kho!";
                    }
                }
            }
            // GetData();
            return RedirectToPage("/Cart/Index");
        }


        private async Task AddProductToCart(ProductDTO product)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            var existingCartItem = cart.FirstOrDefault(c => c.Product.ProductId == product.ProductId);
            if (existingCartItem == null)
            {
                cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }
            else
            {
                existingCartItem.Quantity++;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            //await _hubContext.Clients.All.SendAsync("CartUpdated");
        }

        private void GetData()
        {

            // Lấy danh sách danh mục từ repository
            Categories = productRepository.GetCategories();

            // Lấy tất cả sản phẩm
            Products = productRepository.GetProducts();

        }




    }
}

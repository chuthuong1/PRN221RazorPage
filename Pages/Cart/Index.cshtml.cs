using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bussiness.DTO;
using WebApplication1.DataAccess.Models;
using WebApplication1.Helper;
using WebApplication1.Bussiness.IRepository;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.ChatHubSignarl;

namespace WebApplication1.Pages.Cart
{
    public class ListModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<CartHub> _hubContext;
        private readonly NorthwindContext _northWindContext;
        private readonly IOrdersRepository _ordersRepository;
        public List<CartItem> CartItems { get; set; }


        // Sử dụng Constructor Injection và Scoped DbContext
        public ListModel(IProductRepository productRepository, IHubContext<CartHub> hubContext, NorthwindContext northWindContext, IOrdersRepository ordersRepository)
        {
            _northWindContext = northWindContext;
            _productRepository = productRepository;
            _hubContext = hubContext;
            _ordersRepository = ordersRepository;
        }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        }

        public void OnPostUpdate(int? productId, int? quantity)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (productId != null)
            {
                var c = CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
                c.Quantity = quantity.Value;
                ViewData["mess"] = "Update Successful!";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", CartItems);
        }


        public async Task<IActionResult> OnPostCheckout()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            if (CartItems == null || CartItems.Count == 0)
            {
                ViewData["mess"] = "Giỏ hàng đang trống!";
                return Page();
            }
            else
            {
                var order = await _ordersRepository.createtOrdes();
                decimal totalFreight = 0;

                foreach (var cartItem in CartItems)
                {
                    var product = await _productRepository.GetProductById(cartItem.Product.ProductId);

                    if (product != null)
                    {
                        product.UnitsInStock -= cartItem.Quantity;
                        product.UnitsOnOrder += cartItem.Quantity;
                        _productRepository.UpdateProduct(product);

                        // Tính giá trị Freight cho từng sản phẩm và cộng vào tổng Freight
                        totalFreight += (cartItem.Quantity * (product.UnitPrice ?? 0));
                    }

                    var orderDetail = CreateOrderDetail(order.OrderId, product.ProductId, (short)(cartItem.Quantity), (decimal)product.UnitPrice);

                    _northWindContext.Add(orderDetail);
                    _northWindContext.SaveChanges();

                    _hubContext.Clients.All.SendAsync("CartUpdated", product.ProductId, product.UnitsInStock);
                }

                // Lưu giá trị tổng Freight vào đối tượng Order
                order.Freight = totalFreight;
                _northWindContext.Update(order);
                _northWindContext.SaveChanges();

                HttpContext.Session.Remove("cart");
                ViewData["mess"] = "Checkout Successful!";
                return RedirectToPage("/Products/List");
            }
        }





        private OrderDetail CreateOrderDetail(int orderId, int productId, short quantity, decimal unitPrice)
        {
            // Tạo đối tượng OrderDetail mới và truyền thông tin cần thiết
            var orderDetail = new OrderDetail
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = 0

            };

            return orderDetail; // Trả về đối tượng OrderDetail đã tạo
        }

        public void OnPostDelete(int? productId)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (productId != null)
            {
                var c = CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
                CartItems.Remove(c);
                ViewData["mess"] = "Delete Successful!";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", CartItems);
        }

    }
}
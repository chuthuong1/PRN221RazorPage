using WebApplication1.Bussiness.IRepository;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        NorthwindContext context;
        public OrdersRepository(NorthwindContext context)
        {
            this.context = context;
        }
        public async Task<Order> createtOrdes()
        {
            var order = new Order();
            order.OrderDate = DateTime.Now;
            order.RequiredDate = DateTime.Now;
            order.ShipAddress = "Hà Nội";
            order.EmployeeId = 4;
            order.CustomerId = "ALFKI";
            order.ShipName = "Rattlesnake Canyon Grocery";
            order.ShipVia = 3;
            order.ShipCity = "Albuquerque";
            order.ShipCountry = "USA";
            order.ShipPostalCode = "12345";
            order.ShippedDate = order.RequiredDate.Value.AddDays(3);
            // Sau khi thêm đối tượng Order vào cơ sở dữ liệu
            await context.AddAsync(order);
            await context.SaveChangesAsync();

            return order; // Trả về đối tượng Order đã tạo
        }
    }
}

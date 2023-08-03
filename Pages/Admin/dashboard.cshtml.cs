using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin
{
    public class dashboardModel : PageModel
    {
        private readonly NorthwindContext dBContext;
        public dashboardModel(NorthwindContext dBContext)
        {
            this.dBContext = dBContext;
            currentDateTime = DateTime.Now;
        }

        public float weeklySale = 0;
        public float totalOrders = 0;
        public int totalCustomerHasAccount = 0;
        public int totalGuest = 0;
        public int totalCustomer = 0;
        public int totalProduct = 0;
        private object currentDateTime;

        public IActionResult OnGet(int orderyear)
        {
            DateTime monday, sunday;
            monday = ((DateTime)currentDateTime).AddDays(-(int)((DateTime)currentDateTime).DayOfWeek + 1);
            sunday = monday.AddDays(7);

            decimal?[] income = GetIncomeByMonth(); // Lấy dữ liệu doanh thu theo tháng

            // Tính tổng doanh thu theo tháng
            decimal? totalIncome = 0;
            foreach (var item in income)
            {
                totalIncome += item;
            }

            var ordersByDay = dBContext.Orders
                .Where(order => order.OrderDate.HasValue && order.OrderDate.Value.Year == orderyear) // Đảm bảo có giá trị trong trường OrderDate (không null) và năm phù hợp
                .GroupBy(order => order.OrderDate.Value.Date) // Nhóm theo ngày, bỏ qua phần giờ, phút, giây...
                .Select(group => new
                {
                    OrderDate = group.Key, // Key chính là ngày của nhóm
                    OrderId = group.Count() // Số lượng đơn hàng trong nhóm (số lượng của các đơn hàng trong cùng một ngày)
                })
                .ToArray(); // Chuyển kết quả thành mảng các đối tượng Order

            var ordersByDayData = new
            {
                labels = ordersByDay.Select(o => o.OrderDate.ToString("dd/MM/yyyy")).ToArray(),
                data = ordersByDay.Select(o => o.OrderId).ToArray()
            };

            // Tính tổng số lượng đơn hàng theo ngày, tháng và năm
            var ordersByDateMonthYear = dBContext.Orders
                .Where(order => order.OrderDate.HasValue && order.OrderDate.Value.Year == orderyear) // Đảm bảo có giá trị trong trường OrderDate (không null) và năm phù hợp
                .GroupBy(order => new
                {
                    Day = order.OrderDate.Value.Day,
                    Month = order.OrderDate.Value.Month,
                    Year = order.OrderDate.Value.Year
                })
                .Select(group => new
                {
                    OrderDate = group.Key, // Key chính là ngày, tháng, năm của nhóm
                    OrderId = group.Count() // Số lượng đơn hàng trong nhóm (số lượng của các đơn hàng trong cùng một ngày, tháng, năm)
                })
                .ToArray(); // Chuyển kết quả thành mảng các đối tượng Order

            // Gán các thông tin thống kê vào các biến
            weeklySale = GetWeeklySale(monday, sunday);
            totalOrders = ordersByDayData.data.Sum();
            // Tính tổng số lượng khách hàng đăng ký tài khoản (tùy vào thiết kế và cấu trúc dữ liệu)
            // totalCustomerHasAccount = ...;
            // Tính tổng số lượng khách hàng guest (tùy vào thiết kế và cấu trúc dữ liệu)
            // totalGuest = ...;
            // Tính tổng số lượng khách hàng (tùy vào thiết kế và cấu trúc dữ liệu)
            // totalCustomer = ...;
            // Tính tổng số lượng sản phẩm (tùy vào thiết kế và cấu trúc dữ liệu)
            // totalProduct = ...;

          

            return Page();
        }

        public decimal?[] GetIncomeByMonth()
        {
            // Truy vấn dữ liệu doanh thu theo tháng từ bảng Orders
            var incomeByMonth = dBContext.Orders
                .Where(order => order.OrderDate.HasValue) // Đảm bảo có giá trị trong trường OrderDate (không null)
                .GroupBy(order => new { order.OrderDate.Value.Year, order.OrderDate.Value.Month }) // Nhóm theo năm và tháng
                .Select(group => new
                {
                    Year = group.Key.Year, // Năm của nhóm
                    Month = group.Key.Month, // Tháng của nhóm
                    Income = group.Sum(order => order.Freight) // Tính tổng doanh thu của các đơn hàng trong nhóm
                })
                .OrderBy(group => group.Year) // Sắp xếp theo năm
                .ThenBy(group => group.Month) // Sắp xếp theo tháng
                .Select(group => group.Income) // Chọn trường Income để lấy mảng doanh thu
                .ToArray(); // Chuyển kết quả thành mảng các số decimal

            return incomeByMonth;
        }

        public float GetWeeklySale(DateTime monday, DateTime sunday)
        {
            return (from o in dBContext.Orders
                    join od in dBContext.OrderDetails on o.OrderId equals od.OrderId
                    where o.OrderDate >= monday && o.OrderDate <= sunday
                    group od by o.OrderDate into orderMonth
                    select (float)orderMonth.Sum(od => od.Quantity * od.UnitPrice)
                   ).Sum();
        }
    }
}

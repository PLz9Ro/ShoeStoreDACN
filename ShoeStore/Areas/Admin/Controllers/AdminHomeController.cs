using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Models;
using System.Reflection;

namespace ShoeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminHomeController : Controller
    {
        private readonly ShoeStoreContext _context;

        public AdminHomeController(ShoeStoreContext context)
        {
            _context = context;
        }


        public IActionResult Index(string timeFilter)
        {
            var monthlyTotalRevenue = GetMonthlyTotalRevenue();
            var yearlyTotalRevenue = GetYearlyTotalRevenue();

            // Other logic for fetching filtered order details
            var orderDetails = GetFilteredOrderDetails(timeFilter);

            ViewBag.MonthlyTotalRevenue = monthlyTotalRevenue;
            ViewBag.YearlyTotalRevenue = yearlyTotalRevenue;

            return View(orderDetails);
        }
        private List<OrderDetail> GetFilteredOrderDetails(string timeFilter)
        {
            DateTime startDate;
            DateTime endDate;

            switch (timeFilter)
            {
                case "day":
                    startDate = DateTime.Today;
                    endDate = startDate.AddDays(1);
                    break;
                case "month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = startDate.AddMonths(1);
                    break;
                default:
                    // Set the start date to the minimum valid date supported by SQL Server
                    startDate = new DateTime(1753, 1, 1);
                    endDate = DateTime.MaxValue;
                    break;
            }

            // Rest of the code remains unchanged
            var filteredOrderDetails = _context.OrderDetails
                .Where(od => od.CreateDate >= startDate && od.CreateDate < endDate)
                .ToList();

            return filteredOrderDetails;
        }

        private decimal GetMonthlyTotalRevenue()
        {

            var currentMonth = DateTime.Today.Month;
            var currentYear = DateTime.Today.Year;

            var monthlyTotal = _context.OrderDetails
                .Where(od => od.CreateDate.HasValue && od.CreateDate.Value.Month == currentMonth && od.CreateDate.Value.Year == currentYear)
                .Sum(od => od.Total) ?? 0;

            return monthlyTotal;
        }

        private decimal GetYearlyTotalRevenue()
        {

            var currentYear = DateTime.Today.Year;

            var yearlyTotal = _context.OrderDetails
                .Where(od => od.CreateDate.HasValue && od.CreateDate.Value.Year == currentYear)
                .Sum(od => od.Total) ?? 0;

            return yearlyTotal;
        }

    }
}
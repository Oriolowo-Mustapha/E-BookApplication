namespace E_BookApplication.DTOs
{
    public class DashboardStatsDTO
    {
        public int TotalBooks { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public double AverageOrderValue { get; set; }
        public int NewCustomersThisMonth { get; set; }
    }

    public class SalesReportDTO
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal Revenue { get; set; }
        public int BooksSold { get; set; }
    }

    public class BookPerformanceDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }


    public class UserActivityDTO
    {
        public DateTime Date { get; set; }
        public int NewRegistrations { get; set; }
        public int ActiveUsers { get; set; }
        public int OrdersPlaced { get; set; }
    }

    public class RevenueReportDTO
    {
        public decimal TotalRevenue { get; set; }
        public IEnumerable<MonthlyRevenueDTO> MonthlyBreakdown { get; set; }
        public IEnumerable<GenreRevenueDTO> GenreBreakdown { get; set; }
    }

    public class MonthlyRevenueDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }


    public class GenreRevenueDTO
    {
        public string Genre { get; set; }
        public decimal Revenue { get; set; }
        public int BooksSold { get; set; }
    }
}

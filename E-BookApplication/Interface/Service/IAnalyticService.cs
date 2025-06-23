using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IAnalyticsService
    {
        Task<DashboardStatsDTO> GetDashboardStatsAsync(string vendorId = null);
        Task<IEnumerable<SalesReportDTO>> GetSalesReportAsync(DateTime startDate, DateTime endDate, string vendorId = null);
        Task<IEnumerable<BookPerformanceDTO>> GetTopSellingBooksAsync(int count = 10, string vendorId = null);
        Task<IEnumerable<UserActivityDTO>> GetUserActivityReportAsync(DateTime startDate, DateTime endDate);
        Task<RevenueReportDTO> GetRevenueReportAsync(DateTime startDate, DateTime endDate, string vendorId = null);
    }
}

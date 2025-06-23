using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string title, string message, string type = "info");
        Task SendBulkNotificationAsync(IEnumerable<string> userIds, string title, string message, string type = "info");
        Task NotifyNewBookAsync(BookDTO book);
        Task NotifyOrderStatusChangeAsync(OrderDTO order);
        Task NotifyLowStockAsync(BookDTO book, int threshold = 5);
    }
}

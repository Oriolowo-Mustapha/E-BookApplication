using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendWelcomeEmailAsync(string to, string fullName);
        Task SendOrderConfirmationEmailAsync(string to, OrderDTO order);
        Task SendPasswordResetEmailAsync(string to, string resetLink);
        Task SendNewBookNotificationAsync(string to, BookDTO book);
    }
}

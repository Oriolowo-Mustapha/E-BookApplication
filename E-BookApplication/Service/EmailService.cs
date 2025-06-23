using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace E_BookApplication.Service
{ 

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService()
        {
            _fromEmail = "your-email@gmail.com";

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var mailMessage = new MailMessage(_fromEmail, to, subject, body)
            {
                IsBodyHtml = isHtml
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendWelcomeEmailAsync(string to, string fullName)
        {
            var subject = "Welcome to Our Book Store!";
            var body = $"Hi {fullName},<br/>Thank you for joining our community!";
            await SendEmailAsync(to, subject, body, true);
        }

        public async Task SendOrderConfirmationEmailAsync(string to, OrderDTO order)
        {
            var subject = $"Order Confirmation - Order #{order.Id}";
            var body = $"Dear {order.FullName},<br/>Thank you for your order! Your total is {order.TotalAmount:C}.";
            await SendEmailAsync(to, subject, body, true);

        }

        public async Task SendPasswordResetEmailAsync(string to, string resetLink)
        {
            var subject = "Password Reset Request";
            var body = $"Click <a href='{resetLink}'>here</a> to reset your password.";
            await SendEmailAsync(to, subject, body, true);
        }

        public async Task SendNewBookNotificationAsync(string to, BookDTO book)
        {
            var subject = $"New Book Available: {book.Title}";
            var body = $"Check out our new book: <strong>{book.Title}</strong> by {book.Author}.";
            await SendEmailAsync(to, subject, body, true);
        }
    }

}

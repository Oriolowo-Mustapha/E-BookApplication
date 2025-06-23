using E_BookApplication.DTOs;
using E_BookApplication.Interface.Service;
using System.Net;
using System.Net.Mail;

namespace E_BookApplication.Implementation.Service
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;
		private readonly SmtpClient _smtpClient;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
			_smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
			{
				Port = int.Parse(_configuration["Email:Port"]),
				Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
				EnableSsl = bool.Parse(_configuration["Email:EnableSsl"])
			};
		}

		public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(_configuration["Email:From"]),
				Subject = subject,
				Body = body,
				IsBodyHtml = isHtml
			};

			mailMessage.To.Add(to);

			await _smtpClient.SendMailAsync(mailMessage);
		}

		public async Task SendWelcomeEmailAsync(string to, string fullName)
		{
			var subject = "Welcome to EBook Store!";
			var body = $@"
			<h2>Welcome to EBook Store, {fullName}!</h2>
			<p>Thank you for creating an account with us. We're excited to have you as part of our community.</p>
			<p>Start exploring our vast collection of books and enjoy reading!</p>
			<p>Best regards,<br>EBook Store Team</p>
		";

			await SendEmailAsync(to, subject, body, true);
		}

		public async Task SendOrderConfirmationEmailAsync(string to, OrderDTO order)
		{
			var subject = $"Order Confirmation - Order #{order.Id}";
			var body = $@"
			<h2>Order Confirmation</h2>
			<p>Thank you for your order! Here are the details:</p>
			<p><strong>Order ID:</strong> {order.Id}</p>
			<p><strong>Order Date:</strong> {order.OrderDate:yyyy-MM-dd}</p>
			<p><strong>Total Amount:</strong> ${order.TotalAmount:F2}</p>
			<p><strong>Status:</strong> {order.Status}</p>
			<p>We'll notify you when your order status changes.</p>
			<p>Best regards,<br>EBook Store Team</p>
		";

			await SendEmailAsync(to, subject, body, true);
		}

		public async Task SendPasswordResetEmailAsync(string to, string resetLink)
		{
			var subject = "Password Reset Request";
			var body = $@"
			<h2>Password Reset Request</h2>
			<p>You requested a password reset. Click the link below to reset your password:</p>
			<p><a href='{resetLink}'>Reset Password</a></p>
			<p>If you didn't request this, please ignore this email.</p>
			<p>Best regards,<br>EBook Store Team</p>
		";

			await SendEmailAsync(to, subject, body, true);
		}

		public async Task SendNewBookNotificationAsync(string to, BookDTO book)
		{
			var subject = "New Book Available!";
			var body = $@"
			<h2>New Book Available!</h2>
			<p>A new book has been added to our collection:</p>
			<p><strong>Title:</strong> {book.Title}</p>
			<p><strong>Author:</strong> {book.Author}</p>
			<p><strong>Price:</strong> ${book.Price:F2}</p>
			<p>Check it out now!</p>
			<p>Best regards,<br>EBook Store Team</p>
		";

			await SendEmailAsync(to, subject, body, true);
		}

		public void Dispose()
		{
			_smtpClient?.Dispose();
		}
	}
}

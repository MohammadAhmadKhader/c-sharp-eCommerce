using c_sharp_eCommerce.Core.IServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace c_sharp_eCommerce.Services
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<bool> SendEmailAsync(string subject, string toEmail, string message)
		{
			try
			{
				var emailMsg = new MimeMessage();
				emailMsg.Subject = subject;
				var fromMailBox = new MailboxAddress("c-sharp-ecommerce", _configuration["EmailSettings:FromEmail"]);
				var toMailBox = new MailboxAddress("", toEmail);
				emailMsg.From.Add(fromMailBox);
				emailMsg.To.Add(toMailBox);
				var textBody = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message };
				emailMsg.Body = textBody;

				var client = new SmtpClient();
				await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]),
					bool.Parse(_configuration["EmailSettings:UseSSL"]));
				await client.AuthenticateAsync(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);
				await client.SendAsync(emailMsg);
				await client.DisconnectAsync(true);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}

		}
	}
}

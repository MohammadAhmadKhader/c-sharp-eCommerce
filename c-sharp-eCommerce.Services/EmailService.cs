using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Services.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace c_sharp_eCommerce.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _settings;

    	public EmailService(IOptions<EmailSettings> settings)
    	{
    	    _settings = settings.Value;
    	}
		public async Task<bool> SendEmailAsync(string subject, string toEmail, string message)
		{
			try
			{
                var emailMsg = new MimeMessage
				{
					Subject = subject,
					Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message }
                };

				emailMsg.From.Add(new MailboxAddress("c-sharp-ecommerce", _settings.FromEmail));
				emailMsg.To.Add(new MailboxAddress("", toEmail));

				using var client = new SmtpClient();

				await client.ConnectAsync(_settings.SmtpServer, _settings.Port, _settings.UseSSL);
				await client.AuthenticateAsync(_settings.FromEmail, _settings.Password);
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

using c_shap_eCommerce.Core.IServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;


namespace c_sharp_eCommerce.Services
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration configuration;
		public EmailService(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public async Task<bool> SendEmailAsync(string subject, string toEmail, string message)
		{
			try
			{	var emailMsg = new MimeMessage();
				emailMsg.Subject = subject;
				var fromMailBox = new MailboxAddress("c-sharp-ecommerce", configuration["EmailSettings:FromEmail"]);
				var toMailBox = new MailboxAddress("", toEmail);
				emailMsg.From.Add(fromMailBox);
				emailMsg.To.Add(toMailBox);
				var textBody = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message};
				emailMsg.Body = textBody;

				var client = new SmtpClient();
				await client.ConnectAsync(configuration["EmailSettings:SmtpServer"], int.Parse(configuration["EmailSettings:Port"]),
					bool.Parse(configuration["EmailSettings:UseSSL"]));
				await client.AuthenticateAsync(configuration["EmailSettings:FromEmail"], configuration["EmailSettings:Password"]);
				await client.SendAsync(emailMsg);
				await client.DisconnectAsync(true);

				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}

		}
	}
}

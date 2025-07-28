namespace c_sharp_eCommerce.Core.IServices
{
	public interface IEmailService
	{
		Task<bool> SendEmailAsync(string subject, string toEmail, string message);
	}
}

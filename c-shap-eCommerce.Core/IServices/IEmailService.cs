using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IServices
{
	public interface IEmailService
	{
		Task<bool> SendEmailAsync(string subject, string toEmail, string message);
	}
}

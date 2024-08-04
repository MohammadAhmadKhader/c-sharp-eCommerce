using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
	public class User : IdentityUser<Guid>
	{
		[MinLength(6)]
		[MaxLength(128)]
		public string? Address { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
	}
}

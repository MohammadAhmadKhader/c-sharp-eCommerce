using System.ComponentModel.DataAnnotations;

namespace c_sharp_eCommerce.Services.Options;
public class ApiSettings
{
    [Required(ErrorMessage = "JWTSecretKey is required")]
    [MinLength(32, ErrorMessage = "JWTSecretKey must be at least 32 characters long")]
    public string JWTSecretKey { get; set; } = null!;
}
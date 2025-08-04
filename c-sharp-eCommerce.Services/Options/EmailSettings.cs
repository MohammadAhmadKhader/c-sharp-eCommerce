using System.ComponentModel.DataAnnotations;

namespace c_sharp_eCommerce.Services.Options;
public class EmailSettings
{
    [Required(ErrorMessage = "Port is required")]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int Port { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "FromEmail is required")]
    [EmailAddress(ErrorMessage = "FromEmail must be a valid email address")]
    public string FromEmail { get; set; } = null!;

    [Required(ErrorMessage = "SmtpServer is required")]
    [MinLength(3, ErrorMessage = "SmtpServer must be at least 3 characters long")]
    public string SmtpServer { get; set; } = null!;

    [Required(ErrorMessage = "UseSSL flag is required")]
    public bool UseSSL { get; set; }
}
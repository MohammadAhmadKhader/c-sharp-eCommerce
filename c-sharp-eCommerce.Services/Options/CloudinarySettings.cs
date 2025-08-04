using System.ComponentModel.DataAnnotations;

namespace c_sharp_eCommerce.Services.Options;
public class CloudinarySettings
{
    [Required(ErrorMessage = "ApiKey is required")]
    public string ApiKey { get; set; } = null!;

    [Required(ErrorMessage = "ApiSecret is required")]
    public string ApiSecret { get; set; } = null!;
}
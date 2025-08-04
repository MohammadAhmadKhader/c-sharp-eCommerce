using c_sharp_eCommerce.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace c_sharp_eCommerce.Services.Extensions;
public static class OptionsExtensions
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<EmailSettings>()
            .Bind(config.GetSection(nameof(EmailSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<CloudinarySettings>()
            .Bind(config.GetSection(nameof(CloudinarySettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ApiSettings>()
            .Bind(config.GetSection(nameof(ApiSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    
        return services;
    }
}
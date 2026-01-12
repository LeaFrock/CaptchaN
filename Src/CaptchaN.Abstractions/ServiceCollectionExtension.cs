using CaptchaN.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static CaptchaNBuilder AddCaptchaN(this IServiceCollection services)
        => new(services);
}
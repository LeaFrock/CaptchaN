using CaptchaN.DI.Microsoft;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static PainterBuilder AddCaptchaN(this IServiceCollection services) => new(services);
    }
}
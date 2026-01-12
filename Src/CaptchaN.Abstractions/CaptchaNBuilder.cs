using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaptchaN.Abstractions;

public sealed class CaptchaNBuilder
{
    internal CaptchaNBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public CaptchaNBuilder AddCodeTextGenerator<T>() where T : class, ICodeTextGenerator
    {
        Services.AddSingleton<ICodeTextGenerator, T>();
        return this;
    }

    public CaptchaNBuilder AddCodeTextGenerator(Func<IServiceProvider, ICodeTextGenerator> factory)
    {
        Services.AddSingleton(factory);
        return this;
    }

    public CaptchaNBuilder AddDefaultCodeTextGenerator() => AddCodeTextGenerator<DefaultCodeTextGenerator>();

    public CaptchaNBuilder AddPainter<T>() where T : class, IPainter
    {
        Services.AddSingleton<IPainter, T>();
        return this;
    }

    public CaptchaNBuilder AddPainter(Func<IServiceProvider, IPainter> factory)
    {
        Services.AddSingleton(factory);
        return this;
    }

    public CaptchaNBuilder AddPaintConfig(IConfiguration configuration)
    {
        Services.Configure<PaintConfig>(configuration);
        return this;
    }
}
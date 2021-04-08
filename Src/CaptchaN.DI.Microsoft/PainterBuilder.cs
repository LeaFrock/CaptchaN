using System;
using CaptchaN.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CaptchaN.DI.Microsoft
{
    public sealed class PainterBuilder
    {
        private readonly IServiceCollection _services;

        public PainterBuilder(IServiceCollection services)
        {
            services.AddSingleton<IPainter, Painter>()
                .AddSingleton<ICodeTextGenerator, CodeTextGenerator>()
                .AddSingleton<IColorRandomer, ColorRandomer>();
            _services = services;
        }

        public PainterBuilder UseCodeTextGenerator<T>() where T : class, ICodeTextGenerator
        {
            _services.AddSingleton<ICodeTextGenerator, T>();
            return this;
        }

        public PainterBuilder UseColorRandomer<T>() where T : class, IColorRandomer
        {
            _services.AddSingleton<IColorRandomer, T>();
            return this;
        }

        public PainterBuilder UseFontRandomer<T>() where T : class, IFontRandomer
        {
            _services.AddSingleton<IFontRandomer, T>();
            return this;
        }

        public PainterBuilder UseFontRandomer(IFontRandomerFactory fontRandomerFactory)
        {
            var fontRandomer = fontRandomerFactory.CreateFontRandomer();
            if (fontRandomer is null)
            {
                throw new NullReferenceException("Created FontRandomer is null.");
            }
            _services.AddSingleton(fontRandomer);
            return this;
        }
    }
}
using Microsoft.Extensions.Logging.File;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public static class FileLoggerFactoryExtensions
    {

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory)
        {
            return AddFile(factory, LogLevel.Information);
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, Func<string, LogLevel, bool> filter)
        {
            factory.AddProvider(new FileLoggerProvider(filter));
            return factory;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, LogLevel minLevel)
        {
            return AddFile(
               factory,
               (_, logLevel) => logLevel >= minLevel);
        }
    }
}

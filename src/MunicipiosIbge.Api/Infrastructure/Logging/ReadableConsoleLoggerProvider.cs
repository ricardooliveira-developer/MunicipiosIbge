namespace MunicipiosIbge.Api.Infrastructure.Logging;

public sealed class ReadableConsoleLoggerProvider : ILoggerProvider
{
    private static readonly object Lock = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new ReadableConsoleLogger(categoryName);
    }

    public void Dispose()
    {
    }

    private sealed class ReadableConsoleLogger(string categoryName) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);

            if (string.IsNullOrWhiteSpace(message) && exception is null)
            {
                return;
            }

            lock (Lock)
            {
                WriteColored(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss"), ConsoleColor.DarkGray);
                Console.Write(" | ");
                WriteColored(GetArea(categoryName), GetAreaColor(categoryName));
                Console.Write(" | ");
                WriteColored(GetLevel(logLevel), GetLevelColor(logLevel));
                Console.Write(" | ");
                Console.WriteLine(message);

                if (exception is not null)
                {
                    WriteColored("                    | ERROR  | ERROR | ", ConsoleColor.Red);
                    Console.WriteLine(exception);
                }
            }
        }

        private static string GetArea(string category)
        {
            if (category.Contains("RequestLoggingMiddleware", StringComparison.OrdinalIgnoreCase))
            {
                return "HTTP  ";
            }

            if (category.Contains("RetrieveMunicipalitiesBehavior", StringComparison.OrdinalIgnoreCase)
                || category.Contains("Cache", StringComparison.OrdinalIgnoreCase))
            {
                return "CACHE ";
            }

            if (category.Contains("Sync", StringComparison.OrdinalIgnoreCase))
            {
                return "SYNC  ";
            }

            if (category.Contains("Ibge", StringComparison.OrdinalIgnoreCase))
            {
                return "IBGE  ";
            }

            if (category.Contains("Repository", StringComparison.OrdinalIgnoreCase)
                || category.Contains("Persistence", StringComparison.OrdinalIgnoreCase))
            {
                return "DB    ";
            }

            if (category.Contains("Handler", StringComparison.OrdinalIgnoreCase))
            {
                return "QUERY ";
            }

            if (category.Contains("ExceptionHandlingMiddleware", StringComparison.OrdinalIgnoreCase))
            {
                return "ERROR ";
            }

            return "APP   ";
        }

        private static ConsoleColor GetAreaColor(string category)
        {
            if (category.Contains("RequestLoggingMiddleware", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Cyan;
            }

            if (category.Contains("RetrieveMunicipalitiesBehavior", StringComparison.OrdinalIgnoreCase)
                || category.Contains("Cache", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Magenta;
            }

            if (category.Contains("Sync", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Yellow;
            }

            if (category.Contains("Ibge", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Blue;
            }

            if (category.Contains("Repository", StringComparison.OrdinalIgnoreCase)
                || category.Contains("Persistence", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Green;
            }

            if (category.Contains("Handler", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.DarkCyan;
            }

            if (category.Contains("ExceptionHandlingMiddleware", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.Gray;
        }

        private static string GetLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "TRACE",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO ",
                LogLevel.Warning => "WARN ",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "FATAL",
                _ => "LOG  "
            };
        }

        private static ConsoleColor GetLevelColor(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Critical => ConsoleColor.DarkRed,
                LogLevel.Information => ConsoleColor.Green,
                LogLevel.Debug => ConsoleColor.DarkGray,
                LogLevel.Trace => ConsoleColor.DarkGray,
                _ => ConsoleColor.Gray
            };
        }

        private static void WriteColored(string value, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = previousColor;
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}

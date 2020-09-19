using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Alexr03.Common.Logging
{
    public class Logger
    {
        public string Application { get; }

        public Serilog.Core.Logger InternalLogger { get; }

        public Logger(string application)
        {
            Application = application;

            var consoleOutputTemplate =
                $"[{application}" + " {Timestamp:HH:mm:ss.ff} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: consoleOutputTemplate)
                .WriteTo.File($"./Logs/{application}/{application}.log", rollingInterval: RollingInterval.Day);

            InternalLogger = loggerConfiguration.CreateLogger();
        }

        public static Logger Create<T>()
        {
            return new Logger(typeof(T).Assembly.GetName().Name);
        }

        public void LogMessage(string message)
        {
            LogMessage(LogLevel.Information, message);
        }

        public void LogDebugMessage(string message)
        {
            LogMessage(LogLevel.Debug, message);
        }

        public void LogMessage(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    InternalLogger.Fatal(message);
                    LogReceived?.Invoke(LogEventLevel.Fatal, InternalLogger, message, Application);
                    break;
                case LogLevel.Debug:
                    InternalLogger.Debug(message);
                    LogReceived?.Invoke(LogEventLevel.Debug, InternalLogger, message, Application);
                    break;
                case LogLevel.Information:
                    InternalLogger.Information(message);
                    LogReceived?.Invoke(LogEventLevel.Information, InternalLogger, message, Application);
                    break;
                case LogLevel.Warning:
                    InternalLogger.Warning(message);
                    LogReceived?.Invoke(LogEventLevel.Warning, InternalLogger, message, Application);
                    break;
                case LogLevel.Error:
                    InternalLogger.Error(message);
                    LogReceived?.Invoke(LogEventLevel.Error, InternalLogger, message, Application);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public void LogMessage(LogEventLevel logLevel, string message)
        {
            InternalLogger.Write(logLevel, message);
            LogReceived?.Invoke(logLevel, InternalLogger, message, Application);
        }

        public void LogException(Exception exception)
        {
            InternalLogger.Write(LogEventLevel.Error, exception, exception.Message);
            LogExceptionReceived?.Invoke(LogEventLevel.Error, InternalLogger, exception, Application);
        }

        public static event LogRaised LogReceived;

        public static event LogExceptionRaised LogExceptionReceived;

        public delegate void LogRaised(LogEventLevel logLevel, Serilog.Core.Logger logger, string message,
            string application);

        public delegate void LogExceptionRaised(LogEventLevel logLevel, Serilog.Core.Logger logger, Exception exception,
            string application);
    }
}
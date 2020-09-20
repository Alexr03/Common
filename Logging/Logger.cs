using System;
using System.IO;
using Serilog;
using Serilog.Events;

namespace Alexr03.Common.Logging
{
    public class Logger
    {
        private const string LogBaseLocation = "./Components/{0}/Logs/{1}/{2}/{2}.log";
        public string Application { get; }
        public Serilog.Core.Logger InternalLogger { get; }
        internal Type Type { get; set; }

        public Logger(string application, Type type = null)
        {
            Console.WriteLine(2);
            Application = application;
            Type = type;

            var consoleOutputTemplate =
                $"[{application}" + " {Timestamp:HH:mm:ss.ff} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: consoleOutputTemplate);
            if (Type != null)
            {
                var assemblyName = Type.Assembly.GetName().Name;
                var logLocation = LogBaseLocation.Replace("{0}",
                        Path.Combine(assemblyName, Type.Namespace?
                                .Replace(assemblyName, "") ?? "")
                            .Replace(".", ""))
                    .Replace("{1}", Type?.Name)
                    .Replace("{2}", application);
                loggerConfiguration.WriteTo.File(logLocation, rollingInterval: RollingInterval.Day);
            }
            else
            {
                loggerConfiguration.WriteTo.File($"./Components/Misc/Logs/{application}/{application}.log",
                    rollingInterval: RollingInterval.Day);
            }

            InternalLogger = loggerConfiguration.CreateLogger();
        }

        public static Logger Create<T>(string application)
        {
            var logger = new Logger(application, typeof(T));
            return logger;
        }

        public static Logger Create<T>()
        {
            var logger = new Logger(typeof(T).Assembly.GetName().Name, typeof(T));
            return logger;
        }

        public void LogMessage(string message)
        {
            LogMessage(LogEventLevel.Information, message);
        }

        public void LogDebugMessage(string message)
        {
            LogMessage(LogEventLevel.Debug, message);
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
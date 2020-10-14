using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexr03.Common.Configuration;
using Alexr03.Common.TCAdmin.Objects;
using Serilog;
using Serilog.Events;
using TCAdmin.Interfaces.Logging;
using TCAdmin.SDK;

namespace Alexr03.Common.Logging
{
    public class Logger
    {
        private readonly string _logBaseLocation = "./Components/{0}/Logs/{1}/{2}/{2}.log";
        public string Application { get; }
        public Serilog.Core.Logger InternalLogger { get; }
        private string LogLocation { get; }
        private Type Type { get; set; }

        public Logger(string application, Type type = null)
        {
            Application = application;
            Type = type;

            var consoleOutputTemplate =
                $"[{application}" + " {Timestamp:HH:mm:ss.ff} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: consoleOutputTemplate);
            if (Utilities.IsRunningOnTcAdmin)
            {
                var arCommonSettings = ModuleConfiguration.GetModuleConfiguration(Globals.ModuleId, "ArCommonSettings").Parse<ArCommonSettings>();
                loggerConfiguration.MinimumLevel.Is(arCommonSettings.MinimumLogLevel);
            }
            else
            {
                var arCommonSettings = new LocalConfiguration<ArCommonSettings>().GetConfiguration();
                loggerConfiguration.MinimumLevel.Is(arCommonSettings.MinimumLogLevel);
            }
            
            if (Utilities.IsRunningOnTcAdmin)
            {
                _logBaseLocation = _logBaseLocation.Replace("./", Utility.GetMonitorLogPath());
            }
            
            if (Type != null)
            {
                var assemblyName = Type.Assembly.GetName().Name;
                LogLocation =
                    Path.Combine(
                        _logBaseLocation
                            .Replace("{0}", assemblyName)
                            .Replace("{1}", Type.Namespace?.Replace(assemblyName, "").Trim('.'))
                            .Replace("{2}", application));
                loggerConfiguration.WriteTo.File(LogLocation, rollingInterval: RollingInterval.Day, shared: true);
            }
            else
            {
                LogLocation = $"./Components/Misc/Logs/{application}/{application}.log";
                loggerConfiguration.WriteTo.File(LogLocation, rollingInterval: RollingInterval.Day, shared: true);
            }

            InternalLogger = loggerConfiguration.CreateLogger();
        }
        
        public static Logger Create(Type type)
        {
            var logger = new Logger(type.Name, type);
            return logger;
        }

        public static Logger Create<T>(string application)
        {
            var logger = new Logger(application, typeof(T));
            return logger;
        }

        public static Logger Create<T>()
        {
            var logger = new Logger(typeof(T).Name, typeof(T));
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

        public List<FileInfo> GetLogFiles()
        {
            var directoryInfo = new FileInfo(LogLocation).Directory;
            directoryInfo?.Create();
            return directoryInfo?.GetFiles().ToList();
        }

        public FileInfo GetCurrentLogFile()
        {
            var fileInfos = GetLogFiles().OrderByDescending(x => x.LastWriteTimeUtc).ToList();
            return fileInfos[0];
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
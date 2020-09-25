using Serilog.Events;

namespace Alexr03.Common
{
    public class ArCommonSettings
    {
        public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Information;
    }
}
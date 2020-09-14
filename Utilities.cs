using Newtonsoft.Json;

namespace Alexr03.Common
{
    public class Utilities
    {
        public static readonly JsonSerializerSettings NoErrorJsonSettings = new JsonSerializerSettings
        {
            Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            }
        };
    }
}
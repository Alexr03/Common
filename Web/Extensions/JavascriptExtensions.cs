using Newtonsoft.Json;

namespace Alexr03.Common.Web.Extensions
{
    public static class JavascriptExtensions
    {
        public static string ObjectToJson(object obj, string variableName)
        {
            return $"let {variableName} = JSON.parse('{JsonConvert.SerializeObject(obj)}');";
        }
    }
}
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alexr03.Common.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static JObject GetRequestData(this HttpRequest request)
        {
            return JsonConvert.DeserializeObject<JObject>(RequestBody(HttpContext.Current.Request.InputStream));
        }

        public static T GetRequestData<T>(this HttpRequest request)
        {
            return GetRequestData(request).ToObject<T>();
        }
        
        public static string RequestBody(Stream stream)
        {
            var bodyStream = new StreamReader(stream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            return bodyText;
        }
    }
}
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Alexr03.Common.Web.HttpResponses
{
    public class JsonNetResult : JsonResult
    {
        private readonly HttpStatusCode _httpStatus;

        public JsonNetResult(object data, HttpStatusCode httpStatus = HttpStatusCode.OK, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            Data = data;
            _httpStatus = httpStatus;
            JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)_httpStatus;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;
            response.Write(JsonConvert.SerializeObject(Data, Formatting.Indented, Utilities.JsonSerializerSettings));
        }
    }
}
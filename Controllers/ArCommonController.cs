using System.Web.Mvc;
using Alexr03.Common.TCAdmin.Objects;
using Alexr03.Common.Web.Helpers;
using Newtonsoft.Json.Linq;
using TCAdmin.SDK.Web.MVC.Controllers;

namespace Alexr03.Common.Controllers
{
    public class ArCommonController : BaseController
    {
        public ActionResult Configuration()
        {
            return View();
        }

        [ParentAction("Configuration")]
        public ActionResult Configure(string configName)
        {
            var config = ModuleConfiguration.GetModuleConfiguration(Globals.ModuleId, configName);
            var jObject = config.GetConfiguration<JObject>();
            ViewData.TemplateInfo = new TemplateInfo
            {
                HtmlFieldPrefix = config.Type.Name,
            };
            return PartialView(config.View, jObject.ToObject(config.Type));
        }

        [HttpPost]
        public ActionResult Configure(string configName, FormCollection model)
        {
            var config = ModuleConfiguration.GetModuleConfiguration(Globals.ModuleId, configName);
            var o = model.Parse(ControllerContext, config.Type);
            config.SetConfiguration(o);
            return PartialView(config.View, o);
        }
    }
}
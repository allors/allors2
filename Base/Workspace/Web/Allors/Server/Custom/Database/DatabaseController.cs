namespace Web.Controllers
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web;
    using Allors.Web.Database;

    public class DatabaseController : CustomController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Sync(SyncRequest syncRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new SyncResponseBuilder(this.AllorsSession, user, syncRequest);
            var response = responseBuilder.Build();
            return this.JsonSuccess(response);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Push(PushRequest pushRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new PushResponseBuilder(this.AllorsSession, user, pushRequest);
            var response = responseBuilder.Build();

            return this.JsonSuccess(response);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Invoke(InvokeRequest invokeRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new InvokeResponseBuilder(this.AllorsSession, user, invokeRequest);
            var response = responseBuilder.Build();
            return this.JsonSuccess(response);
        }

        [HttpGet]
        public ActionResult Translate(string lang)
        {
            var cultureInfo = new CultureInfo(lang);

            var workspaceMetaResources = Resources.WorkspaceMeta.ResourceManager.GetResourceSet(cultureInfo, true, true)
                .Cast<DictionaryEntry>()
                .ToDictionary(entry => "meta_" + entry.Key.ToString().Replace(".", "_"), entry => entry.Value.ToString());


            var workspaceResources = Resources.WorkspaceResources.ResourceManager.GetResourceSet(cultureInfo, true, true)
                .Cast<DictionaryEntry>()
                .ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToString());
                
            var resources = workspaceResources
                .Concat(workspaceMetaResources)
                .ToDictionary(x => x.Key, x => x.Value);

            return this.JsonSuccess(resources);
        }
    }
}
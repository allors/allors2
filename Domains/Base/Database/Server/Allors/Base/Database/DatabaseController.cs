namespace Allors.Server
{
    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Mvc;

    public class DatabaseController : Controller
    {
        public ISession AllorsSession { get; }

        public User AllorsUser { get; }

        public DatabaseController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
            this.AllorsUser = new Users(this.AllorsSession).FindBy(M.User.UserName, "administrator");
        }

        [HttpPost]
        public IActionResult Sync([FromBody]SyncRequest syncRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new SyncResponseBuilder(this.AllorsSession, user, syncRequest);
            var response = responseBuilder.Build();
            return this.Ok(response);
        }

        [HttpPost]
        public IActionResult Push([FromBody]PushRequest pushRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new PushResponseBuilder(this.AllorsSession, user, pushRequest);
            var response = responseBuilder.Build();
            return this.Ok(response);
        }

        [HttpPost]
        public IActionResult Invoke([FromBody]InvokeRequest invokeRequest)
        {
            var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
            var responseBuilder = new InvokeResponseBuilder(this.AllorsSession, user, invokeRequest);
            var response = responseBuilder.Build();
            return this.Ok(response);
        }
    }
}
namespace Allors.Server
{
    using System;
    using System.Threading.Tasks;

    using Allors.Domain;

    using Microsoft.AspNetCore.Mvc;

    public class DatabaseController : AllorsController
    {
        public DatabaseController(IAllorsContext allorsContext) : base(allorsContext)
        {
        }
        
        [HttpPost]
        public async Task<IActionResult> Sync([FromBody]SyncRequest syncRequest)
        {
            await this.OnInit();

            try
            {
                var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
                var responseBuilder = new SyncResponseBuilder(this.AllorsSession, user, syncRequest);
                var response = responseBuilder.Build();
                return this.Ok(response);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Push([FromBody]PushRequest pushRequest)
        {
            await this.OnInit();

            try
            {
                var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
                var responseBuilder = new PushResponseBuilder(this.AllorsSession, user, pushRequest);
                var response = responseBuilder.Build();
                return this.Ok(response);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Invoke([FromBody]InvokeRequest invokeRequest)
        {
            await this.OnInit();

            try
            {
                var user = this.AllorsUser ?? Singleton.Instance(this.AllorsSession).Guest;
                var responseBuilder = new InvokeResponseBuilder(this.AllorsSession, user, invokeRequest);
                var response = responseBuilder.Build();
                return this.Ok(response);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }
    }
}
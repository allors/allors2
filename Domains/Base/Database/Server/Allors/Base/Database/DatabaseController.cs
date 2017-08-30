namespace Allors.Server
{
    using System;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DatabaseController : Controller
    {
        private readonly IAllorsContext allors;

        public DatabaseController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Sync([FromBody]SyncRequest syncRequest)
        {
            try
            {
                var responseBuilder = new SyncResponseBuilder(this.allors.Session, this.allors.User, syncRequest);
                var response = responseBuilder.Build();
                return this.Ok(response);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Push([FromBody]PushRequest pushRequest)
        {
            try
            {
                var responseBuilder = new PushResponseBuilder(this.allors.Session, this.allors.User, pushRequest);
                var response = responseBuilder.Build();
                return this.Ok(response);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Invoke([FromBody]InvokeRequest invokeRequest)
        {
            try
            {
                var responseBuilder = new InvokeResponseBuilder(this.allors.Session, this.allors.User, invokeRequest);
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
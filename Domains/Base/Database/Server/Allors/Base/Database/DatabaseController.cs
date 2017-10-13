namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DatabaseController : Controller
    {
        public DatabaseController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        public IActionResult Sync([FromBody]SyncRequest syncRequest)
        {
            try
            {
                var responseBuilder = new SyncResponseBuilder(this.Session, this.Session.GetUser(), syncRequest);
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
        [AllowAnonymous]
        public IActionResult Push([FromBody]PushRequest pushRequest)
        {
            try
            {
                var responseBuilder = new PushResponseBuilder(this.Session, this.Session.GetUser(), pushRequest);
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
        [AllowAnonymous]
        public IActionResult Invoke([FromBody]InvokeRequest invokeRequest)
        {
            try
            {
                var responseBuilder = new InvokeResponseBuilder(this.Session, this.Session.GetUser(), invokeRequest);
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
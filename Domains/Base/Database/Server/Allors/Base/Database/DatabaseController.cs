// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseController.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Services;

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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
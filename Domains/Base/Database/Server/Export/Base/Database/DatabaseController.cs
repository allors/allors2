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
    using System.Data.Common;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Polly;

    public class DatabaseController : Controller
    {
        public DatabaseController(IDatabaseService databaseService, IPolicyService policyService, ILogger<DatabaseController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private ILogger<DatabaseController> Logger { get; set; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public IActionResult Sync([FromBody]SyncRequest syncRequest)
        {
            try
            {
                return this.PolicyService.SyncPolicy.Execute(
                    () =>
                        {
                            using (var session = this.DatabaseService.Database.CreateSession())
                            {
                                var user = session.GetUser();
                                var responseBuilder = new SyncResponseBuilder(session, user, syncRequest);
                                var response = responseBuilder.Build();
                                return this.Ok(response);
                            }
                        });
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
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
                return this.PolicyService.PushPolicy.Execute(
                    () =>
                        {
                            using (var session = this.DatabaseService.Database.CreateSession())
                            {
                                var responseBuilder = new PushResponseBuilder(session, session.GetUser(), pushRequest);
                                var response = responseBuilder.Build();
                                return this.Ok(response);
                            }
                        });
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
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
                return this.PolicyService.InvokePolicy.Execute(
                    () =>
                        {
                            using (var session = this.DatabaseService.Database.CreateSession())
                            {
                                var responseBuilder = new InvokeResponseBuilder(session, session.GetUser(), invokeRequest);
                                var response = responseBuilder.Build();
                                return this.Ok(response);
                            }
                        });
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.StatusCode(500, e.Message);
            }
        }
    }
}

// <copyright file="DatabaseController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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

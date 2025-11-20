// <copyright file="DatabaseController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using Allors.Domain;
    using Allors.Protocol.Remote.Push;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("allors/push")]
    public class PushController : ControllerBase
    {
        public PushController(IDatabaseService databaseService, IPolicyService policyService, ILogger<PushController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private ILogger<PushController> Logger { get; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<PushResponse> Post([FromBody]PushRequest request) =>
            this.PolicyService.PushPolicy.Execute(
                () =>
                {
                    try
                    {
                        using (var session = this.DatabaseService.Database.CreateSession())
                        {
                            var acls = new WorkspaceAccessControlLists(session.GetUser());
                            var responseBuilder = new PushResponseBuilder(session, request, acls);
                            var response = responseBuilder.Build();
                            return response;
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "PushRequest {request}", request);
                        throw;
                    }
                });
    }
}

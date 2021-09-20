// <copyright file="DatabaseController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;

    using Allors.Domain;
    using Allors.Protocol.Remote.Sync;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("allors/sync")]
    public class SyncController : ControllerBase
    {
        public SyncController(IDatabaseService databaseService, IPolicyService policyService, ILogger<SyncController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private ILogger<SyncController> Logger { get; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<SyncResponse> Post([FromBody]SyncRequest request) =>
            this.PolicyService.SyncPolicy.Execute(
                () =>
                {
                    try
                    {
                        using (var session = this.DatabaseService.Database.CreateSession())
                        {
                            var acls = new WorkspaceAccessControlLists(session.GetUser());
                            var responseBuilder = new SyncResponseBuilder(session, request, acls);
                            var response = responseBuilder.Build();
                            return response;
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "SyncRequest {request}", request);
                        throw;
                    }
                });
    }
}

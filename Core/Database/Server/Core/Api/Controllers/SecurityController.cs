// <copyright file="DatabaseController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using Allors.Protocol.Remote.Security;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("allors/security")]
    public class SecurityController : ControllerBase
    {
        public SecurityController(IDatabaseService databaseService, IPolicyService policyService, ILogger<SecurityController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private ILogger<SecurityController> Logger { get; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<SecurityResponse> Post([FromBody]SecurityRequest request) =>
            this.PolicyService.SyncPolicy.Execute(
                () =>
                {
                    try
                    {
                        using (var session = this.DatabaseService.Database.CreateSession())
                        {
                            var responseBuilder = new SecurityResponseBuilder(session, request);
                            var response = responseBuilder.Build();
                            return response;
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "SecurityRequest {request}", request);
                        throw;
                    }
                });
    }
}

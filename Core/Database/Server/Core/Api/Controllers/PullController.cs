// <copyright file="PullController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using System;
    using Allors.Domain;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Pull;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("allors/pull")]
    public class PullController : ControllerBase
    {
        public PullController(IDatabaseService databaseService, IPolicyService policyService, IExtentService extentService, IFetchService fetchService, ITreeService treeService, ILogger<PullController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.ExtentService = extentService;
            this.FetchService = fetchService;
            this.TreeService = treeService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IExtentService ExtentService { get; }

        private IFetchService FetchService { get; }

        private ILogger<PullController> Logger { get; }

        private IPolicyService PolicyService { get; }

        private ITreeService TreeService { get; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<PullResponse> Post([FromBody] PullRequest request) =>
            this.PolicyService.InvokePolicy.Execute(
                () =>
                {
                    try
                    {
                        using (var session = this.DatabaseService.Database.CreateSession())
                        {
                            var acls = new WorkspaceAccessControlLists(session.GetUser());
                            var response = new PullResponseBuilder(acls, this.TreeService);

                            if (request.P != null)
                            {
                                foreach (var p in request.P)
                                {
                                    var pull = p.Load(session);

                                    if (pull.Object != null)
                                    {
                                        var pullInstantiate = new PullInstantiate(session, pull, acls, this.FetchService);
                                        pullInstantiate.Execute(response);
                                    }
                                    else
                                    {
                                        var pullExtent = new PullExtent(session, pull, acls, this.ExtentService, this.FetchService);
                                        pullExtent.Execute(response);
                                    }
                                }
                            }

                            return response.Build();
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "PullRequest {request}", request);
                        throw;
                    }
                });
    }
}

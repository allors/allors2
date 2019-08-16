// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

    public class PullController : Controller
    {
        public PullController(IDatabaseService databaseService, IPolicyService policyService, IExtentService extentService, IFetchService fetchService, ILogger<PullController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.ExtentService = extentService;
            this.FetchService = fetchService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private IExtentService ExtentService { get; }

        private IFetchService FetchService { get; }

        private ILogger<PullController> Logger { get; set; }

        [HttpPost]
        [Authorize]
        public IActionResult Pull([FromBody] PullRequest req)
        {
            try
            {
                return this.PolicyService.InvokePolicy.Execute(
                    () =>
                        {
                            using (var session = this.DatabaseService.Database.CreateSession())
                            {
                                var user = session.GetUser();
                                var response = new PullResponseBuilder(user);

                                if (req.P != null)
                                {
                                    foreach (var p in req.P)
                                    {
                                        var pull = p.Load(session);

                                        if (pull.Object != null)
                                        {
                                            var pullInstantiate = new PullInstantiate(session, pull, user, this.FetchService);
                                            pullInstantiate.Execute(response);
                                        }
                                        else
                                        {
                                            var pullExtent = new PullExtent(session, pull, user, this.ExtentService, this.FetchService);
                                            pullExtent.Execute(response);
                                        }
                                    }
                                }

                                return this.Ok(response.Build());
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

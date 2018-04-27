// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullController.cs" company="Allors bvba">
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

namespace Allors.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class PullController : Controller
    {
        public PullController(IDatabaseService databaseService, IPolicyService policyService, ILogger<PullController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

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
                            var response = new PullResponseBuilder(session.GetUser());

                            if (req.Q != null)
                            {
                                var metaPopulation = (MetaPopulation)session.Database.MetaPopulation;
                                var queries = req.Q.Select(v => v.Parse(metaPopulation)).ToArray();
                                foreach (var query in queries)
                                {
                                    var validation = query.Validate();
                                    if (validation.HasErrors)
                                    {
                                        var message = validation.ToString();
                                        this.Logger.LogError(message);
                                        return this.StatusCode(400, message);
                                    }

                                    var extent = session.Query(query);
                                    if (query.Page != null)
                                    {
                                        var page = query.Page;
                                        var paged = extent.ToArray().Skip(page.Skip).Take(page.Take).ToArray();
                                        response.AddValue(query.Name + "_total", extent.Count);
                                        response.AddCollection(query.Name, paged, query.Include);
                                    }
                                    else
                                    {
                                        response.AddCollection(query.Name, extent.ToArray(), query.Include);
                                    }
                                }
                            }

                            if (req.F != null)
                            {
                                foreach (var fetch in req.F)
                                {
                                    fetch.Parse(session, out IObject @object, out Path path, out Tree include);

                                    if (path != null && @object != null)
                                    {
                                        var acls = new AccessControlListCache(session.GetUser());
                                        var result = path.Get(@object, acls);
                                        if (result != null)
                                        {
                                            if (result is IObject)
                                            {
                                                response.AddObject(fetch.Name, (IObject)result, include);
                                            }
                                            else
                                            {
                                                var objects = (result as HashSet<object>)?.Cast<IObject>() ?? ((Extent)result).ToArray();
                                                response.AddCollection(fetch.Name, objects, include);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response.AddObject(fetch.Name, @object, include);
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

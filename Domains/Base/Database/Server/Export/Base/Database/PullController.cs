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
    using System.Linq;

    using Allors.Domain;
    using Allors.Server.Protocol.Pull;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class PullController : Controller
    {
        public PullController(IDatabaseService databaseService, IPolicyService policyService, IExtentService extentService, ILogger<PullController> logger)
        {
            this.DatabaseService = databaseService;
            this.PolicyService = policyService;
            this.ExtentService = extentService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }

        private IPolicyService PolicyService { get; }

        private IExtentService ExtentService { get; }

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

                                // TODO: Pull
                                if (req.P != null)
                                {
                                    foreach (var p in req.P)
                                    {
                                        var pull = p.Load(session);

                                        if (pull.Object != null)
                                        {
                                            var @object = session.Instantiate(pull.Object);
                                           
                                            if (pull.Fetches != null)
                                            {
                                                foreach (var result in pull.Fetches)
                                                {
                                                    var name = result.Name;

                                                    if (result.Path != null)
                                                    {
                                                        var aclCache = new AccessControlListCache(user);

                                                        var propertyType = result.Path.End.PropertyType;

                                                        if (propertyType.IsOne)
                                                        {
                                                            name = name ?? propertyType.SingularName;

                                                            @object = (IObject)result.Path.Get(@object, aclCache);
                                                            response.AddObject(name, @object, result.Include);
                                                        }
                                                        else
                                                        {
                                                            name = name ?? propertyType.SingularName;

                                                            var objects = ((Extent)result.Path.Get(@object, aclCache))
                                                                .ToArray();

                                                            if (result.Skip.HasValue)
                                                            {
                                                                var paged = objects.Skip(result.Skip.Value);
                                                                if (result.Take.HasValue)
                                                                {
                                                                    paged = paged.Take(result.Take.Value);
                                                                }

                                                                paged = paged.ToArray();

                                                                response.AddValue(name + "_total", objects.Length);
                                                                response.AddCollection(name, paged, result.Include);
                                                            }
                                                            else
                                                            {
                                                                response.AddCollection(name, @objects, result.Include);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var name = pull.DefaultResultName();
                                                response.AddObject(name, @object);
                                            }
                                        }

                                        if (pull.Extent != null || pull.ExtentRef.HasValue)
                                        {

                                            var extent = pull.Extent ?? this.ExtentService.Get(pull.ExtentRef.Value);
                                            var objects = extent.Build(session, pull.Arguments).ToArray();

                                            if (pull.Fetches != null)
                                            {
                                                foreach (var result in pull.Fetches)
                                                {
                                                    var name = result.Name;

                                                    if (result.Path != null)
                                                    {
                                                        var aclCache = new AccessControlListCache(user);

                                                        var propertyType = result.Path.End.PropertyType;

                                                        if (propertyType.IsOne)
                                                        {
                                                            objects = objects.Select(v => result.Path.Get(v, aclCache))
                                                                .Where(v => v != null)
                                                                .Cast<IObject>()
                                                                .Distinct()
                                                                .ToArray();
                                                        }
                                                        else
                                                        {
                                                            objects = objects.SelectMany(v => ((Extent)result.Path.Get(v, aclCache)).ToArray())
                                                                .Distinct()
                                                                .ToArray();
                                                        }

                                                        name = name ?? propertyType.PluralName;
                                                    }

                                                    name = name ?? pull.DefaultResultName(result);

                                                    if (result.Skip.HasValue)
                                                    {
                                                        var paged = objects.Skip(result.Skip.Value);
                                                        if (result.Take.HasValue)
                                                        {
                                                            paged = paged.Take(result.Take.Value);
                                                        }

                                                        paged = paged.ToArray();

                                                        response.AddValue(name + "_total", extent.Build(session, pull.Arguments).Count);
                                                        response.AddCollection(name, paged, result.Include);
                                                    }
                                                    else
                                                    {
                                                        response.AddCollection(name, objects, result.Include);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var name = pull.DefaultResultName();
                                                response.AddCollection(name, objects);
                                            }
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

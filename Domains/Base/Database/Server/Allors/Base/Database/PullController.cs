namespace Allors.Server.Controllers
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PullController : Controller
    {
        public PullController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Pull([FromBody] PullRequest req)
        {
            try
            {
                var response = new PullResponseBuilder(this.Session.GetUser());

                if (req.Q != null)
                {
                    var metaPopulation = (MetaPopulation)this.Session.Database.MetaPopulation;
                    var queries = req.Q.Select(v => v.Parse(metaPopulation)).ToArray();
                    foreach (var query in queries)
                    {
                        Extent extent = this.Session.Query(query);
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
                        fetch.Parse(this.Session, out IObject @object, out Path path, out Tree include);

                        if (path != null)
                        {
                            var acls = new AccessControlListCache(this.Session.GetUser());
                            var result = path.Get(@object, acls);
                            if (result is IObject)
                            {
                                response.AddObject(fetch.Name, (IObject)result, include);
                            }
                            else
                            {
                                response.AddCollection(fetch.Name, ((Extent)result)?.ToArray(), include);
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
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}

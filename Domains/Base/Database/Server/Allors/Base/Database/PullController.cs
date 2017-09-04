namespace Allors.Server.Controllers
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Mvc;

    public class PullController : Controller
    {
        private readonly IAllorsContext allors;

        public PullController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull([FromBody] PullRequest req)
        {
            try
            {
                var response = new PullResponseBuilder(this.allors.User);

                if (req.Q != null)
                {
                    var metaPopulation = (MetaPopulation)this.allors.Session.Database.MetaPopulation;
                    var queries = req.Q.Select(v => v.Parse(metaPopulation)).ToArray();
                    foreach (var query in queries)
                    {
                        Extent extent = this.allors.Session.Query(query);
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
                        fetch.Parse(this.allors.Session, out IObject @object, out Path path, out Tree include);

                        if (path != null)
                        {
                            var acls = new AccessControlListCache(this.allors.User);
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

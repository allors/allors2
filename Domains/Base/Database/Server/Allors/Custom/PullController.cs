namespace Allors.Server.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Mvc;

    public class PullController : AllorsController
    {
        public PullController(IAllorsContext allorsContext) : base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] PullRequest req)
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            if (req.Q != null)
            {
                var metaPopulation = (MetaPopulation)this.AllorsSession.Database.MetaPopulation;
                var queries = req.Q.Select(v => v.Parse(metaPopulation)).ToArray();
                foreach (var query in queries)
                {
                    Extent<Organisation> extent = this.AllorsSession.Query(query);
                    if (query.Page != null)
                    {
                        var page = query.Page;
                        var paged = extent.Skip(page.Skip).Take(page.Take).ToArray();
                        response.AddValue(query.Name + "_count", extent.Count);
                        response.AddCollection(query.Name, paged, query.Include);
                    }
                    else
                    {
                        response.AddCollection(query.Name, extent, query.Include);
                    }
                }
            }

            if (req.F != null)
            {
                foreach (var fetch in req.F)
                {
                    fetch.Parse(this.AllorsSession, out IObject @object, out Tree include);
                    response.AddObject(fetch.Name, @object, include);
                }
            }

            return this.Ok(response.Build());
        }
    }
}

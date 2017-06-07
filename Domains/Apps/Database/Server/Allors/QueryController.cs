namespace Allors.Server.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Allors.Meta;

    using Microsoft.AspNetCore.Mvc;

    public class QueryController : PullController
    {
        public QueryController(IAllorsContext allorsContext) : base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] QueryRequestQuery[] req)
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var metaPopulation = (MetaPopulation)this.AllorsSession.Database.MetaPopulation;
            var queries = req.Select(v => v.Parse(metaPopulation)).ToArray();

            foreach (var query in queries)
            {
                Extent extent = this.AllorsSession.Query(query);
                if (query.Page != null)
                {
                    var page = query.Page;
                    var paged = extent.ToArray().Skip(page.Skip).Take(page.Take).ToArray();
                    response.AddValue(query.Name + "_count", extent.Count);
                    response.AddCollection(query.Name, paged, query.Fetch);
                }
                else
                {
                    response.AddCollection(query.Name, extent.ToArray(), query.Fetch);
                }
            }

            return this.Ok(response.Build());
        }
    }
}

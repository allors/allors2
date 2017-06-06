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
                var extent = this.AllorsSession.Query(query);
                response.AddCollection(query.Name, extent.ToArray(), query.Tree);
            }

            return this.Ok(response.Build());
        }
    }
}

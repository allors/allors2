using System.Collections.Generic;
using System.Linq;
using Allors;
using Allors.Domain;
using Allors.Meta;
using Allors.Web.Database;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public ISession AllorsSession { get; set; }

        public TestController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new Users(this.AllorsSession).Extent().Select(v=>v.UserName);
        }
    }
}

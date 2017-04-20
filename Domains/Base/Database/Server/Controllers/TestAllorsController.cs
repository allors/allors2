namespace Allors.Server
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class TestAllorsController : AllorsController
    {
        public TestAllorsController(IAllorsContext allorsContext) : base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> UserInfo()
        {
            await this.OnInit();

            return this.Ok(this.AllorsUser.UserName);
        }
    }
}
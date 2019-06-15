using Allors.Domain;

namespace Allors.Server.Controllers
{
    using System;

    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public class TestController : Controller
    {
        public TestController(IDatabaseService databaseService)
        {
            this.Database = databaseService.Database;
        }

        public IDatabase Database { get; set; }

        [HttpGet]
        public IActionResult Init()
        {
            var stateService = this.Database.ServiceProvider.GetRequiredService<IStateService>();

            var database = this.Database;
            database.Init();
            stateService.Clear();

            return this.Ok("Init");
        }

        [HttpGet]
        public IActionResult Setup(string population)
        {
            try
            {
                var stateService = this.Database.ServiceProvider.GetRequiredService<IStateService>();

                var database = this.Database;
                database.Init();
                stateService.Clear();

                using (var session = database.CreateSession())
                {
                    new Setup(session, null).Apply();
                    session.Derive();
                    session.Commit();

                    var administrator = new Users(session).GetUser("administrator");
                    session.SetUser(administrator);

                    new TestPopulation(session, population).Apply();
                    session.Derive();
                    session.Commit();
                }

                return this.Ok("Setup");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            var timeService = this.Database.ServiceProvider.GetRequiredService<ITimeService>();
            timeService.Shift = new TimeSpan(days, hours, minutes, seconds);
            return this.Ok();
        }
    }
}

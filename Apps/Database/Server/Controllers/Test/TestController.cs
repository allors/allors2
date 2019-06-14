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
            this.Database.Init();
            this.Database.ServiceProvider.GetRequiredService<IStateService>().Clear();

            using (var session = this.Database.CreateSession())
            {
                new Setup(session).Apply();

                session.Derive();
                session.Commit();

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                new Upgrade(session, null).Execute();

                session.Derive();
                session.Commit();

                new Demo(session, null).Execute();

                session.Derive();
                session.Commit();
            }

            return this.Ok("Setup");
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
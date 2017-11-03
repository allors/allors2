namespace Allors.Server.Controllers
{
    using System;

    using Allors.Domain;
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
        public IActionResult Setup()
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

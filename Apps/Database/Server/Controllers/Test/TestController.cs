namespace Allors.Server.Controllers
{
    using System;
    using Allors.Services;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        private readonly IDatabaseService databaseService;

        private readonly IStateService stateService;

        private readonly ITimeService timeService;

        public TestController(IDatabaseService databaseService, IStateService stateService, ITimeService timeService)
        {
            this.databaseService = databaseService;
            this.stateService = stateService;
            this.timeService = timeService;
        }
        
        [HttpGet]
        public IActionResult Init()
        {
            this.databaseService.Database.Init();
            this.stateService.Clear();

            return this.Ok("Init");
        }

        [HttpGet]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            this.timeService.Shift = new TimeSpan(days, hours, minutes, seconds);

            return this.Ok("TimeShift");
        }
    }
}

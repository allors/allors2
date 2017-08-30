namespace Allors.Server.Controllers
{
    using System;

    using Allors.Services.Base;

    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        public TestController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }

        public ISession AllorsSession { get; set; }

        [HttpGet]
        public IActionResult Init()
        {
            var database = this.AllorsSession.Database;
            database.Init();

            var timeService = new TimeService();
            var mailService = new TestMailService();
            var securityService = new SecurityService();
            var serviceLocator = new ServiceLocator
                                     {
                                         TimeServiceFactory = () => timeService,
                                         MailServiceFactory = () => mailService,
                                         SecurityServiceFactory = () => securityService
                                     };
            database.SetServiceLocator(serviceLocator);

            return this.Ok("Init");
        }

        [HttpGet]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            using (var timeService = this.AllorsSession.Database.GetServiceLocator().CreateTimeService())
            {
                timeService.Shift = new TimeSpan(days, hours, minutes, seconds);
            }

            return this.Ok();
        }
    }
}

// <copyright file="TestController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using System;
    using Allors.Domain;
    using Allors.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class TestController : Controller
    {
        public TestController(IDatabaseService databaseService, ILogger<TestController> logger)
        {
            this.Database = databaseService.Database;
            this.Logger = logger;
        }

        public IDatabase Database { get; set; }

        private ILogger<TestController> Logger { get; set; }

        [HttpGet]
        public IActionResult Ready() => this.Ok("Ready");

        [HttpGet]
        public IActionResult Init()
        {
            try
            {
                var stateService = this.Database.ServiceProvider.GetRequiredService<IStateService>();

                var database = this.Database;
                database.Init();
                stateService.Clear();

                return this.Ok("Init");
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
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
                    var config = new Config();
                    new Setup(session, config).Apply();
                    session.Derive();
                    session.Commit();

                    //session.SetUser(session.GetSingleton().Scheduler);

                    //session.Derive();
                    //session.Commit();
                }

                return this.Ok("Setup");
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            try
            {
                var timeService = this.Database.ServiceProvider.GetRequiredService<ITimeService>();
                timeService.Shift = new TimeSpan(days, hours, minutes, seconds);
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }
    }
}

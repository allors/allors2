// <copyright file="TestEmployeesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestEmployeesController : Controller
    {
        public TestEmployeesController(ISessionService sessionService) => this.Session = sessionService.Session;

        private ISession Session { get; }

        [HttpPost]
        public IActionResult Pull()
        {
            var response = new PullResponseBuilder(this.Session.GetUser());
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Owner, this.Session.GetUser());
            response.AddObject("root", organisation, M.Organisation.AngularEmployees);
            return this.Ok(response.Build());
        }
    }
}

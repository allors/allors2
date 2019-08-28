// <copyright file="TestHomeController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestHomeController : Controller
    {
        public TestHomeController(ISessionService sessionService, ITreeService treeService)
        {
            this.Session = sessionService.Session;
            this.TreeService = treeService;
        }

        private ISession Session { get; }

        public ITreeService TreeService { get; }

        [HttpPost]
        public IActionResult Pull()
        {
            var response = new PullResponseBuilder(this.Session.GetUser(), this.TreeService);
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Owner, this.Session.GetUser());
            response.AddObject("root", organisation, M.Organisation.AngularShareholders);
            return this.Ok(response.Build());
        }
    }
}

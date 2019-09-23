// <copyright file="PeopleController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Api.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Api;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PeopleController : Controller
    {
        public PeopleController(ISessionService sessionService, ITreeService treeService)
        {
            this.Session = sessionService.Session;
            this.TreeService = treeService;
        }

        private ISession Session { get; }

        public ITreeService TreeService { get; }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Pull()
        {
            var response = new PullResponseBuilder(this.Session.GetUser(), this.TreeService);
            var people = new People(this.Session).Extent().ToArray();
            response.AddCollection("people", people);
            return this.Ok(response.Build());
        }
    }
}

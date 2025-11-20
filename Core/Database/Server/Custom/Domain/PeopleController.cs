// <copyright file="PeopleController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Server;
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
            var acls = new WorkspaceAccessControlLists(this.Session.GetUser());
            var response = new PullResponseBuilder(acls, this.TreeService);
            var people = new People(this.Session).Extent().ToArray();
            response.AddCollection("people", people);
            return this.Ok(response.Build());
        }
    }
}

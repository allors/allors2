// <copyright file="OrganisationContactRelationshipController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class OrganisationContactRelationshipController : Controller
    {
        private readonly ISessionService allors;

        public OrganisationContactRelationshipController(ISessionService allorsContext, ITreeService treeService)
        {
            this.allors = allorsContext;
            this.TreeService = treeService;
        }

        public ITreeService TreeService { get; }


        [HttpPost]
        public IActionResult Pull([FromBody] Model model)
        {
            var acls = new WorkspaceAccessControlLists(this.allors.Session.GetUser());
            var response = new PullResponseBuilder(acls, this.TreeService);

            var organisationContactRelationship = (OrganisationContactRelationship)this.allors.Session.Instantiate(model.Id);
            response.AddObject("organisationContactRelationship", organisationContactRelationship);

            response.AddObject("contact", organisationContactRelationship.Contact);

            var locales = new Locales(this.allors.Session).Extent();
            response.AddCollection("locales", locales);

            var genders = new GenderTypes(this.allors.Session).Extent();
            response.AddCollection("genders", genders);

            var salutations = new Salutations(this.allors.Session).Extent();
            response.AddCollection("salutations", salutations);

            var contactKinds = new OrganisationContactKinds(this.allors.Session).Extent();
            response.AddCollection("organisationContactKinds", contactKinds);

            return this.Ok(response.Build());
        }

        public class Model
        {
            public string Id { get; set; }
        }
    }
}

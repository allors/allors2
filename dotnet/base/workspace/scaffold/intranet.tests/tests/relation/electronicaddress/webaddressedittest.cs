// <copyright file="WebAddressEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ElectronicAddressTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using libs.angular.material.@base.src.export.objects.webaddress.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class WebAddressEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        private readonly WebAddress editContactMechanism;

        public WebAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            this.editContactMechanism = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString("www.acme.com")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).Extent().First;

            var before = new WebAddresses(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var contactMechanismOverview = personOverview.ContactmechanismOverviewPanel.Click();
            contactMechanismOverview.Table.DefaultAction(editContactMechanism);

            var webAddressEdit = new WebAddressEditComponent(this.Driver);
            webAddressEdit
                .ElectronicAddressString.Set("wwww.allors.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new WebAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("wwww.allors.com", editContactMechanism.ElectronicAddressString);
            Assert.Equal("description", editContactMechanism.Description);
        }
    }
}

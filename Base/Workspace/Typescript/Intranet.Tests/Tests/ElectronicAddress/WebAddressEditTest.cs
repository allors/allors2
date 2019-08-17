// <copyright file="WebAddressEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ElectronicAddressTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Components;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;
    using src.allors.material.@base.objects.webaddress.edit;
    using Xunit;

    [Collection("Test collection")]
    public class WebAddressEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public WebAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new WebAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            this.personListPage.Table.DefaultAction(person);
            var webAddressCreate = new PersonOverviewComponent(this.personListPage.Driver).ContactmechanismOverviewPanel.Click().CreateWebAddress();

            webAddressCreate
                .ContactPurposes.Toggle("General Email Address")
                .ElectronicAddressString.Set("wwww.allors.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new WebAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();

            Assert.Equal("wwww.allors.com", contactMechanism.ElectronicAddressString);
            Assert.Equal("description", contactMechanism.Description);
        }

        [Fact]
        public void Edit()
        {
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            var editContactMechanism = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString("www.acme.com")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            var before = new WebAddresses(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var contactMechanismOverview = personOverview.ContactmechanismOverviewPanel.Click();
            var row = contactMechanismOverview.Table.FindRow(editContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

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

// <copyright file="TelecommunicationsNumberEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.TelecommunicationsNumberTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using libs.angular.material.@base.src.export.objects.telecommunicationsnumber.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class TelecommunicationsNumberEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly TelecommunicationsNumber editContactMechanism;

        public TelecommunicationsNumberEditTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            this.editContactMechanism = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("0032")
                .WithAreaCode("498")
                .WithContactNumber("123 456")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(this.editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).Extent().First;

            var before = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);

            var contactMechanismOverview = personOverview.ContactmechanismOverviewPanel.Click();
            var row = contactMechanismOverview.Table.FindRow(this.editContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            var editComponent = new TelecommunicationsNumberEditComponent(this.Driver);
            editComponent
                .CountryCode.Set("111")
                .AreaCode.Set("222")
                .ContactNumber.Set("333")
                .ContactMechanismType.Select(new ContactMechanismTypes(this.Session).MobilePhone)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("111", this.editContactMechanism.CountryCode);
            Assert.Equal("222", this.editContactMechanism.AreaCode);
            Assert.Equal("333", this.editContactMechanism.ContactNumber);
            Assert.Equal(new ContactMechanismTypes(this.Session).MobilePhone, this.editContactMechanism.ContactMechanismType);
            Assert.Equal("description", this.editContactMechanism.Description);
        }
    }
}

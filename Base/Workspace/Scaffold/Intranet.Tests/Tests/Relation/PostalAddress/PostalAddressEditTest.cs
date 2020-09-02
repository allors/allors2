// <copyright file="PostalAddressEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PostalAddressTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using libs.angular.material.@base.src.export.objects.postaladdress.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PostalAddressEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly PostalAddress editContactMechanism;

        public PostalAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            this.editContactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("city")
                .WithPostalCode("1111")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
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
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "NL");

            var person = new People(this.Session).Extent().First;

            var before = new PostalAddresses(this.Session).Extent().ToArray();

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);

            var panelComponent = personOverview.ContactmechanismOverviewPanel.Click();
            var row = panelComponent.Table.FindRow(this.editContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            var postalAddressEditComponent = new PostalAddressEditComponent(this.Driver);
            postalAddressEditComponent.Address1.Set("addressline 1")
                .Address2.Set("addressline 2")
                .Address3.Set("addressline 3")
                .Locality.Set("city")
                .PostalCode.Set("postalcode")
                .Country.Select(country)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PostalAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("addressline 1", this.editContactMechanism.Address1);
            Assert.Equal("addressline 2", this.editContactMechanism.Address2);
            Assert.Equal("addressline 3", this.editContactMechanism.Address3);
            Assert.Equal("addressline 1", this.editContactMechanism.Address1);
            Assert.Equal("city", this.editContactMechanism.Locality);
            Assert.Equal("postalcode", this.editContactMechanism.PostalCode);
            Assert.Equal(country, this.editContactMechanism.Country);
            Assert.Equal("description", this.editContactMechanism.Description);
        }
    }
}

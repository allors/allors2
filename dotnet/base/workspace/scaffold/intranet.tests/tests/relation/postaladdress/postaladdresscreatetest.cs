// <copyright file="PostalAddressCreateTest.cs" company="Allors bvba">
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
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PostalAddressCreateTest : Test
    {
        private readonly PersonListComponent people;

        private readonly PostalAddress editContactMechanism;

        public PostalAddressCreateTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            this.editContactMechanism = new PostalAddressBuilder(this.Session)
                .WithDefaults()
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(this.editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");

            var before = new PostalAddresses(this.Session).Extent().ToArray();

            var person = new People(this.Session).Extent().First;

            this.people.Table.DefaultAction(person);
            var postalAddressEditComponent = new PersonOverviewComponent(this.people.Driver).ContactmechanismOverviewPanel.Click().CreatePostalAddress();

            postalAddressEditComponent
                .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .Address1.Set("addressline 1")
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

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();
            var partyContactMechanism = contactMechanism.PartyContactMechanismsWhereContactMechanism.First;

            Assert.Equal("addressline 1", contactMechanism.Address1);
            Assert.Equal("addressline 2", contactMechanism.Address2);
            Assert.Equal("addressline 3", contactMechanism.Address3);
            Assert.Equal("addressline 1", contactMechanism.Address1);
            Assert.Equal("city", contactMechanism.Locality);
            Assert.Equal("postalcode", contactMechanism.PostalCode);
            Assert.Equal(country, contactMechanism.Country);
            Assert.Equal("description", contactMechanism.Description);
        }
    }
}

// <copyright file="PartyContactMechanismEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PartyContactMachanismTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using src.allors.material.@base.objects.person.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PartyContactMechanismEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly PartyContactMechanism editPartyContactMechanism;

        public PartyContactMechanismEditTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            var postalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("city")
                .WithPostalCode("1111")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            this.editPartyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(postalAddress).Build();
            person.AddPartyContactMechanism(this.editPartyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            // var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "NL");

            // var extent = new People(this.Session).Extent();
            // var person = extent.First(v => v.DisplayName().Equals("John Doe"));

            // var before = new PostalAddresses(this.Session).Extent().ToArray();

            // var postalAddress = (PostalAddress)person.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            // var personOverview = this.people.Select(person);

            // var page = personOverview.SelectPostalAddress(this.editPartyContactMechanism);
            // var contactMechanism = (PostalAddress)this.editPartyContactMechanism.ContactMechanism;

            // .FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 28);
            // .ThroughDate.Set(DateTimeFactory.CreateDate(DateTime.Now).AddYears(1);
            // .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name);
            // .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).HeadQuarters.Name);
            // .Address1.Set("addressline 1";
            // .Address2.Set("addressline 2";
            // .Address3.Set("addressline 3";
            // .Locality.Set("city";
            // .PostalCode.Set("postalcode";
            // .Country.Set(country.Name;
            // .UseAsDefault.Set(true;
            // .NonSolicitationIndicator.Set(true;
            // .Description.Set("description";

            // .Save.Click();

            // this.Driver.WaitForAngular();
            // this.Session.Rollback();

            // var after = new PostalAddresses(this.Session).Extent().ToArray();

            // Assert.Equal(after.Length, before.Length);

            ////Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 28).Date, this.editPartyContactMechanism.FromDate.ToUniversalTime().Date);
            ////Assert.Equal(DateTimeFactory.CreateDate(DateTime.Now).AddYears(1).Date, this.editPartyContactMechanism.ThroughDate.Value.ToUniversalTime().Date);
            // Assert.Equal(2, this.editPartyContactMechanism.ContactPurposes.Count);
            // Assert.Contains(new ContactMechanismPurposes(this.Session).BillingAddress, this.editPartyContactMechanism.ContactPurposes);
            // Assert.Contains(new ContactMechanismPurposes(this.Session).HeadQuarters, this.editPartyContactMechanism.ContactPurposes);
            // Assert.Equal("addressline 1", contactMechanism.Address1);
            // Assert.Equal("addressline 2", contactMechanism.Address2);
            // Assert.Equal("addressline 3", contactMechanism.Address3);
            // Assert.Equal("addressline 1", contactMechanism.Address1);
            // Assert.Equal("city", contactMechanism.PostalBoundary.Locality);
            // Assert.Equal("postalcode", contactMechanism.PostalBoundary.PostalCode);
            // Assert.Equal(country, contactMechanism.PostalBoundary.Country);
            // Assert.True(this.editPartyContactMechanism.UseAsDefault);
            // Assert.True(this.editPartyContactMechanism.NonSolicitationIndicator);
            // Assert.Equal("description", contactMechanism.Description);
        }
    }
}

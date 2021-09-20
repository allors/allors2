// <copyright file="WebAddressCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ElectronicAddressTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class WebAddressCreateTest : Test
    {
        private readonly PersonListComponent personListPage;

        public WebAddressCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new WebAddresses(this.Session).Extent().ToArray();

            var person = new People(this.Session).Extent().First;

            this.personListPage.Table.DefaultAction(person);
            var webAddressCreate = new PersonOverviewComponent(this.personListPage.Driver).ContactmechanismOverviewPanel.Click().CreateWebAddress();

            webAddressCreate
                .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).GeneralEmail)
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
    }
}

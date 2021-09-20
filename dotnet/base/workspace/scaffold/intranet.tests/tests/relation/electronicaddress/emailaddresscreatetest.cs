// <copyright file="EmailAddressCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ElectronicAddressTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class EmailAddressCreateTest : Test
    {
        private readonly PersonListComponent personListPage;

        public EmailAddressCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new EmailAddresses(this.Session).Extent().ToArray();

            var person = new People(this.Session).Extent().First;

            this.personListPage.Table.DefaultAction(person);
            var emailAddressCreate = new PersonOverviewComponent(this.personListPage.Driver).ContactmechanismOverviewPanel.Click().CreateEmailAddress();

            emailAddressCreate
                .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .ElectronicAddressString.Set("me@myself.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();

            Assert.Equal("me@myself.com", contactMechanism.ElectronicAddressString);
            Assert.Equal("description", contactMechanism.Description);
        }
    }
}

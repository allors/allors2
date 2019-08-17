// <copyright file="SerialisedItemOverviewTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SerialisedItemTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;

    [Collection("Test collection")]
    public class SerialisedItemOverviewTest : Test
    {
        private readonly PersonListComponent people;

        public SerialisedItemOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John");
            this.people.Table.DefaultAction(person);
            new PersonOverviewComponent(this.people.Driver);
            Assert.Equal("Person", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John");
            this.people.Table.DefaultAction(person);
            var overviewPage = new PersonOverviewComponent(this.people.Driver);
            Assert.Equal("Person", this.Driver.Title);

            overviewPage.People.Click();

            Assert.Equal("People", this.Driver.Title);
        }
    }
}

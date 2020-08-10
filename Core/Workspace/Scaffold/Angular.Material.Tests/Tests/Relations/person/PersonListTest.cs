// <copyright file="PersonListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Relations
{
    using Allors.Domain;
    using Allors.Meta;
    using libs.angular.material.custom.src.relations.people;
    using Xunit;

    [Collection("Test collection")]
    public class PersonListTest : Test
    {
        private readonly PeopleComponent page;

        public PersonListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title() => Assert.Equal("People", this.Driver.Title);

        [Fact]
        public void Table()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John");
            var row = this.page.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();
        }
    }
}

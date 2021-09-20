// <copyright file="PersonOverviewTest.cs" company="Allors bvba">
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
    public class PersonOverviewTest : Test
    {
        private readonly PeopleComponent people;

        public PersonOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.UserName, "john@example.com");
            this.people.Select(person);
            Assert.Equal("Person Overview", this.Driver.Title);
        }
    }
}

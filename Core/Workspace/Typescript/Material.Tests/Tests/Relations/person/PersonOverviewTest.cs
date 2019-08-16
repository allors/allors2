// <copyright file="PersonOverviewTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using src.allors.material.custom.relations.people;

namespace Tests.Relations
{
    using Allors.Domain;
    using Allors.Meta;


    using Xunit;

    [Collection("Test collection")]
    public class PersonOverviewTest : Test
    {
        private readonly src.allors.material.custom.relations.people.PeopleComponent people;

        public PersonOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.UserName, "john@doe.org");
            this.people.Select(person);
            Assert.Equal("Person Overview", this.Driver.Title);
        }
    }
}

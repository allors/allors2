// <copyright file="PersonEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PersonTests
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
    public class PersonEditTest : Test
    {
        private readonly PersonListComponent people;

        public PersonEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var before = new People(this.Session).Extent().ToArray();

            var person = before.First();
            var id = person.Id;

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);
            var personOverviewDetail = personOverview.PersonOverviewDetail.Click();

            personOverviewDetail.Salutation.Select(new Salutations(this.Session).Mr)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Select(new GenderTypes(this.Session).Male)
                .Locale.Select(this.Session.GetSingleton().AdditionalLocales.First)
                .Comment.Set("unpleasant person")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            person = after.First(v => v.Id.Equals(id));

            Assert.Equal(new Salutations(this.Session).Mr, person.Salutation);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male, person.Gender);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, person.Locale);
            Assert.Equal("unpleasant person", person.Comment);
        }
    }
}

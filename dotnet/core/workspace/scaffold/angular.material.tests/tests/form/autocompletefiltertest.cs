// <copyright file="AutoCompleteFilterTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using libs.angular.material.custom.src.tests.form;
    using Xunit;

    [Collection("Test collection")]
    public class AutoCompleteFilterTest : Test
    {
        private readonly FormComponent page;

        public AutoCompleteFilterTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Full()
        {
            var jane = new Users(this.Session).GetUser("jane@example.com");
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.AutocompleteFilter.Select("jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal(jane, data.AutocompleteFilter);
        }


        [Fact]
        public void PartialWithSelection()
        {
            var jane = new Users(this.Session).GetUser("jane@example.com");
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.AutocompleteFilter.Select("jane", "jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal(jane, data.AutocompleteFilter);
        }
    }
}

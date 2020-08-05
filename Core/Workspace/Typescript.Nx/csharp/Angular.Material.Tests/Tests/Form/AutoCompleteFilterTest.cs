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
    using src.allors.material.custom.tests.form;
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

            expect(after.Length).toBe( before.Length + 1);

            var data = after.Except(before).First();

            expect(jane).toBe( data.AutocompleteFilter);
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

            expect(after.Length).toBe( before.Length + 1);

            var data = after.Except(before).First();

            expect(jane).toBe( data.AutocompleteFilter);
        }
    }
}

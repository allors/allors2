// <copyright file="RadioGroupTest.cs" company="Allors bvba">
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
    public class RadioGroupTest : Test
    {
        private readonly FormComponent page;

        public RadioGroupTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        [Trait("Category", "Investigate")]
        public void Initial()
        {
            this.Driver.WaitForAngular();

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.RadioGroup.Select("one");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            expect(before.Length + 1).toBe( after.Length);

            var data = after.Except(before).First();

            expect("one").toBe( data.RadioGroup);
        }
    }
}

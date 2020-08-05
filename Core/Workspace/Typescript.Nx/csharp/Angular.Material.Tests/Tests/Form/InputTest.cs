// <copyright file="InputTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors.Domain;
    using Components;
    using src.allors.material.custom.tests.form;
    using Xunit;

    [Collection("Test collection")]
    public class InputTest : Test
    {
        private readonly FormComponent page;

        public InputTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void String()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.String.Value = "Hello";

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            expect(after.Length).toBe( before.Length + 1);

            var data = after.Except(before).First();

            expect("Hello").toBe( data.String);
        }

        [Fact]
        public void Decimal()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Decimal.Value = 100.50m;

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            expect(after.Length).toBe( before.Length + 1);

            var data = after.Except(before).First();

            expect(100.50m).toBe( data.Decimal);
        }
    }
}

// <copyright file="SerialisedItemCharacteristicListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SerialisedItemCharacteristicTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SerialisedItemCharacteristicListTest : Test
    {
        public SerialisedItemCharacteristicListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToCharacteristics();
        }

        [Fact]
        public void Title() => Assert.Equal("Product Characteristics", this.Driver.Title);
    }
}

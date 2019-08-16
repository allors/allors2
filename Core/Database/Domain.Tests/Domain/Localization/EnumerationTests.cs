
// <copyright file="EnumerationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Domain;

    using Xunit;

    public class EnumerationTests : DomainTest
    {
        [Fact]
        public void GivenLocalisedTextThenNameIsDerived()
        {
            var defaultLocale = this.Session.GetSingleton().DefaultLocale;

            var gender = new GenderBuilder(this.Session)
                .WithName("LGBT")
                .Build();

            this.Session.Derive(true);
            Assert.Equal("LGBT", gender.Name);
        }
    }
}

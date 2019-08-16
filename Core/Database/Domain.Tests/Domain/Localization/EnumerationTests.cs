// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerationTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

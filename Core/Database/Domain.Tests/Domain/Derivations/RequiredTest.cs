// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequiredTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using System;

    using Allors;
    using Allors.Domain;

    using Xunit;

    public class RequiredTest : DomainTest
    {
        [Fact]
        public void OnPostBuild()
        {
            var before = this.Session.Now();

            var units = new UnitSampleBuilder(this.Session).Build();

            this.Session.Derive(false);

            var after = this.Session.Now();

            Assert.False(units.ExistRequiredBinary);
            Assert.False(units.ExistRequiredString);

            Assert.True(units.ExistRequiredBoolean);
            Assert.True(units.ExistRequiredDateTime);
            Assert.True(units.ExistRequiredDecimal);
            Assert.True(units.ExistRequiredDouble);
            Assert.True(units.ExistRequiredInteger);
            Assert.True(units.ExistRequiredUnique);

            Assert.Equal(false, units.RequiredBoolean);
            Assert.True(units.RequiredDateTime > before && units.RequiredDateTime < after);
            Assert.Equal(0m, units.RequiredDecimal);
            Assert.Equal(0d, units.RequiredDouble);
            Assert.Equal(0, units.RequiredInteger);
            Assert.NotEqual(Guid.Empty, units.RequiredUnique);
        }
    }
}

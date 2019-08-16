// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors.Domain;

    using Xunit;

    public class BuilderTest : DomainTest
    {
        [Fact]
        public void BaseOnPostBuild()
        {
            var person = new PersonBuilder(this.Session).Build();

            Assert.True(person.ExistUniqueId);
        }
    }
}

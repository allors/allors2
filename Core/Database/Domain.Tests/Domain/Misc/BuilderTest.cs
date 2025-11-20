// <copyright file="BuilderTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

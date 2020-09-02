// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Xunit;

    public class PersonTests : DomainTest
    {
        [Fact]
        public void GivenPerson_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PersonBuilder(this.Session);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}

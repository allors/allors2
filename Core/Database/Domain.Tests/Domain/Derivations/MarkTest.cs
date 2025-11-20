// <copyright file="DerivationNodesTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Xunit;

    public class MarkTest : DomainTest
    {
        [Fact]
        public void PostDerive()
        {
            var post = new PostBuilder(this.Session).Build();

            this.Session.Derive();

            Assert.Equal(1, post.Counter);

            post.Counter = 3;

            this.Session.Derive();

            Assert.Equal(5, post.Counter);
        }
    }
}

// <copyright file="DemoTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Bogus;
    using Xunit;

    [Trait("Category", "Security")]
    public class SetupTests : DomainTest
    {
        public SetupTests()
            : base(false)
        {
        }

        [Fact]
        public void Twice()
        {
            var session = this.Session;

            var config = new Config();
            new Setup(session, config).Apply();

            session.Derive();
            session.Commit();

            var objects1 = new Objects(session).Extent().ToArray();

            new Setup(session, config).Apply();

            session.Derive();
            session.Commit();

            var objects2 = new Objects(session).Extent().ToArray();

            var diff = objects2.Except(objects1).ToArray();

            Assert.Empty(diff);
        }
    }
}

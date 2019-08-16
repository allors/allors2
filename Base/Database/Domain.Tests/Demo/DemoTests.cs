//-------------------------------------------------------------------------------------------------
// <copyright file="DemoTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Xunit;

    public class DemoTests : DomainTest
    {
        public DemoTests() : base(false)
        {
        }

        [Fact]
        public void TestPopulate()
        {
            var session = this.Session;

            var config = new Config { Demo = true };
            new Setup(session, config).Apply();

            session.Derive();
            session.Commit();

            var administrator = new Users(session).GetUser("administrator");
            session.SetUser(administrator);

            new Upgrade(session, null).Execute();

            session.Derive();
            session.Commit();
        }
    }
}

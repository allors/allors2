//------------------------------------------------------------------------------------------------- 
// <copyright file="DemoTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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

            new Setup(session, null).Apply();

            session.Derive();
            session.Commit();

            var administrator = new Users(session).GetUser("administrator");
            session.SetUser(administrator);

            new Allors.Upgrade(session, null).Execute();

            session.Derive();
            session.Commit();

            new Demo(session, null).Execute();

            session.Derive();
            session.Commit();

        }
    }
}
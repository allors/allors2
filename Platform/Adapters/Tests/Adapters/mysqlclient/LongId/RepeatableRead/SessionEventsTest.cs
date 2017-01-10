// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionEventsTest.cs" company="Allors bvba">
//   Copyright 2002-2010 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Special.MySqlClient.LongId.RepeatableRead
{
    using Allors;

    using NUnit.Framework;

    [TestFixture]
    public class SessionEventsTest : Special.SessionEventsTest
    {
        private readonly Profile profile = new Profile();

        protected override ISession Session
        {
            get { return this.profile.Session; }
        }

        [SetUp]
        protected override void Init()
        {
            this.profile.Init();
            base.Init();
        }

        [TearDown]
        protected void Dispose()
        {
            this.profile.Dispose();
        }

        public override void RollingBack()
        {
        }
    }
}
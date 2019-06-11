// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentTest.cs" company="Allors bvba">
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

using Xunit;

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using Allors;

    [Collection(Fixture.Collection)]
    public class ExtentTest : Npgsql.ExtentTest
    {
        private readonly Profile profile;

        public ExtentTest(Fixture fixture)
        {
            this.profile = new Profile(fixture.Server);
        }

        protected override IProfile Profile => this.profile;

        public override void Dispose()
        {
            this.profile.Dispose();
        }

        protected override ISession CreateSession()
        {
            return this.profile.CreateSession();
        }
    }
}

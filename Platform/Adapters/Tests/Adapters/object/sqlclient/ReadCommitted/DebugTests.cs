// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefetchTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient.ReadCommitted
{
    using Adapters;

    using Allors.Adapters.Object.SqlClient.Debug;
    using Allors.Adapters.Object.SqlClient.Caching.Debugging;

    using NUnit.Framework;

    [TestFixture]
    public class DebugTests : SqlClient.DebugTests
    {
        private DebugConnectionFactory connectionFactory;
        private DebugCacheFactory cacheFactory;
        private Profile profile;

        protected override IProfile Profile => this.profile;

        [SetUp]
        protected void SetUp()
        {
            this.connectionFactory = new DebugConnectionFactory();
            this.cacheFactory = new DebugCacheFactory();
            this.profile = new Profile(this.connectionFactory, this.cacheFactory);
        }
        
        [TearDown]
        protected void Dispose()
        {
            this.profile.Dispose();
        }
    }
}

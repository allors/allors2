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

using System;
using Allors.Adapters;

namespace Allors.Adapters.Object.SqlClient.Snapshot
{
    using Adapters;

    using Allors.Adapters.Object.SqlClient.Caching;
    using Allors.Adapters.Object.SqlClient.Debug;

    using Xunit;


    public class DebugTests : Allors.Adapters.Object.SqlClient.DebugTests, IDisposable
    {
        private readonly Profile profile;

        private DebugConnectionFactory connectionFactory;
        private DefaultCacheFactory cacheFactory;

        protected override IProfile Profile => this.profile;

        public DebugTests()
        {
            this.connectionFactory = new DebugConnectionFactory();
            this.cacheFactory = new DefaultCacheFactory();
            this.profile = new Profile(this.connectionFactory, this.cacheFactory);
        }

        public void Dispose() => this.profile.Dispose();
    }
}

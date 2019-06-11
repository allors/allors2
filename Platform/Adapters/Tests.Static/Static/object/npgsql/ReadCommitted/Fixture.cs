// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTest.cs" company="Allors bvba">
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

using MysticMind.PostgresEmbed;
using Xunit;

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using System;

    public class Fixture : IDisposable
    {
        public const string Collection = "Npgsql collection";

        public PgServer Server { get; private set; }

        public Fixture()
        {
            this.Server = new PgServer("10.7.1");
            this.Server.Start();
        }

        public void Dispose()
        {
            this.Server?.Stop();
            this.Server = null;
        }
    }
    
    [CollectionDefinition(Fixture.Collection)]
    public class Collection : ICollectionFixture<Fixture>
    {
    }

}

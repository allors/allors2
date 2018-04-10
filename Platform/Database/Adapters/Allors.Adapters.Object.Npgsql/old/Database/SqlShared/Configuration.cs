// --------------------------------------------------------------------------------------------------------------------
// <copyright file="database.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Data;

    using Allors.Adapters.Database.Caching;

    public abstract class Configuration : Adapters.Configuration
    {
        protected Configuration()
        {
            this.CacheFactory = new CacheFactory();
            this.CommandTimeout = 30;
            this.IsolationLevel = IsolationLevel.Unspecified;
        }

        public ICacheFactory CacheFactory { get; set; }

        public Guid Id { get; set; }

        public string ConnectionString { get; set; }

        public int CommandTimeout { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        public IDatabase Serializable { get; set; }    }
}
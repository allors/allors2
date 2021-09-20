//------------------------------------------------------------------------------------------------- 
// <copyright file="Profile.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
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
// <summary>Defines the Default type.</summary>
//------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Meta;
    using Microsoft.Extensions.DependencyInjection;
    using ObjectFactory = Allors.ObjectFactory;

    public class Profile : Adapters.Profile
    {
        private IDatabase repository;
        private IDatabase population2;
        
        public Profile()
        {
            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(C1));
            var services = new ServiceCollection();
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public ObjectFactory ObjectFactory { get; }

        public ServiceProvider ServiceProvider { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            this.repository = null;
            this.population2 = null;
        }

        public override IDatabase CreateMemoryPopulation()
        {
            return this.CreateDatabase();
        }

        public override IDatabase GetPopulation()
        {
            return this.repository;
        }

        public override IDatabase GetPopulation2()
        {
            return this.population2;
        }

        public override void Init()
        {
            this.repository = this.CreateDatabase();
            this.population2 = this.CreateDatabase();
        }

        public override bool IsRollbackSupported()
        {
            return true;
        }

        public IDatabase CreateDatabase() => new Database(this.ServiceProvider, new Configuration { ObjectFactory = this.ObjectFactory });
    }
}

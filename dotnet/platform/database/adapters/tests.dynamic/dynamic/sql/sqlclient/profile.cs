// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
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
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.General.SqlClient.Connected.IntegerId.ReadCommitted
{
    using Allors.Adapters;

    using Allors.Configuration;

    public class Profile : General.Profile
    {
        private IDatabase repository;
        private IDatabase population2;

        public override void Dispose()
        {
            base.Dispose();
            this.repository = null;
            this.population2 = null;
        }

        public override IDatabase CreateMemoryPopulation()
        {
            return AllorsConfiguration.CreateDatabase("MemoryIntegerId");
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
            this.repository = AllorsConfiguration.CreateDatabase("SqlClientIntegerId");
            this.repository.Init();

            this.population2 = AllorsConfiguration.CreateDatabase("SqlClientIntegerId2");
            this.population2.Init();
        }

        public override bool IsRollbackSupported()
        {
            return true;
        }
    }
}
//------------------------------------------------------------------------------------------------- 
// <copyright file="Default.cs" company="Allors bv">
// Copyright Allors bv.
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
    using Allors;
    using Adapters;
    using Allors.Meta;
    using Xunit;
    
    public class LifeCycleTest : Adapters.LifeCycleTest
    {
        private readonly Profile profile = new Profile();

        public LifeCycleTest()
        {
            this.profile.Init();
        }

        public override void Dispose()
        {
            this.profile.Dispose();
        }

        public override IObject[] CreateArray(ObjectType objectType, int count)
        {
            return this.profile.CreateArray(objectType, count);
        }

        public override IDatabase CreateMemoryPopulation()
        {
            return this.profile.CreateMemoryPopulation();
        }

        public override MetaPopulation GetMetaPopulation()
        {
            return (MetaPopulation)this.profile.GetPopulation().MetaPopulation;
        }

        public override MetaPopulation GetMetaPopulation2()
        {
            return (MetaPopulation)this.profile.GetPopulation2().MetaPopulation;
        }

        public override IDatabase GetPopulation()
        {
            return this.profile.GetPopulation();
        }

        public override IDatabase GetPopulation2()
        {
            return this.profile.GetPopulation2();
        }

        public override ISession GetSession()
        {
            return this.profile.GetSession();
        }

        public override ISession GetSession2()
        {
            return this.profile.GetSession2();
        }

        public override bool IsRollbackSupported()
        {
            return this.profile.IsRollbackSupported();
        }
    }
}

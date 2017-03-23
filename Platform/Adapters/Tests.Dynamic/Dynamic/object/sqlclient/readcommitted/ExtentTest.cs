// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentTest.cs" company="Allors bvba">
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
//   Defines the ExtentTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.General.SqlClient.Connected.IntegerId.ReadCommitted
{
    using Allors;
    using Allors.Adapters;
    using Allors.Meta;

    using Xunit;

    
    public class ExtentTest : General.ExtentTest
    {
        private readonly Profile profile = new Profile();

        public override IObject[] CreateArray(ObjectType objectType, int count)
        {
            return this.profile.CreateArray(objectType, count);
        }

        public override IDatabase CreateMemoryPopulation()
        {
            return this.profile.CreateMemoryPopulation();
        }

        public override Allors1.Meta.Domain GetMetaDomain()
        {
            return this.profile.GetMetaDomain();
        }

        public override Allors1.Meta.Domain GetMetaDomain2()
        {
            return this.profile.GetMetaDomain2();
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

        public override IDatabaseSession GetSession2()
        {
            return this.profile.GetSession2();
        }

        public override bool IsRollbackSupported()
        {
            return this.profile.IsRollbackSupported();
        }

        [SetUp]
        protected void Init()
        {
            this.profile.Init();
        }

        [TearDown]
        protected void Dispose()
        {
            this.profile.Dispose();
        }
    }
}
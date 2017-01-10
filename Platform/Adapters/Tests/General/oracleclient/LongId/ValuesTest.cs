// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuesTest.cs" company="Allors bvba">
//   Copyright 2002-2010 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Tests.Repositories.General.OracleClient.Connected.LongId
{
    using System;

    using Allors;
    using Allors.Repositories;

    using NUnit.Framework;

    using Domain = Allors.Meta.Domain;

    [TestFixture]
    public class ValuesTest : General.ValuesTest
    {
        private readonly Profile profile = new Profile();

        [SetUp]
        public void Init()
        {
            this.profile.Init();
            this.testValues.Dates = new[]
                                        {
                                            this.ValueGenerator.NormalizeDateTime(DateTime.Now.Date), 
                                            this.ValueGenerator.GenerateDate().Date, 
                                        };

            this.testValues.DateTimes = new[]
                                            {
                                                this.ValueGenerator.NormalizeDateTime(DateTime.Now), 
                                                this.ValueGenerator.GenerateDateTime()
                                            };
        }

        [TearDown]
        public void Dispose()
        {
            this.profile.Dispose();
        }

        public override IObject[] CreateArray(ObjectType objectType, int count)
        {
            return this.profile.CreateArray(objectType, count);
        }

        public override IRepository CreateMemoryPopulation()
        {
            return this.profile.CreateMemoryPopulation();
        }

        public override Domain GetMetaDomain()
        {
            return this.profile.GetMetaDomain();
        }

        public override Domain GetMetaDomain2()
        {
            return this.profile.GetMetaDomain2();
        }

        public override IRepository GetPopulation()
        {
            return this.profile.GetPopulation();
        }

        public override IRepository GetPopulation2()
        {
            return this.profile.GetPopulation2();
        }

        public override ISession GetSession()
        {
            return this.profile.GetSession();
        }

        public override IRepositorySession GetSession2()
        {
            return this.profile.GetSession2();
        }

        public override bool IsRollbackSupported()
        {
            return this.profile.IsRollbackSupported();
        }
    }
}
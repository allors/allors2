// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LifeCycleTest.cs" company="Allors bvba">
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

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using System.Collections.Generic;

    using Allors.Domain;
    using NUnit.Framework;

    [TestFixture]
    public abstract class LifeCycleLongIdTest : Adapters.LifeCycleLongIdTest
    {
        [Test]
        public override void CreateManyPopulations()
        {
            // SQLite doesn't supports overlapping transactions
            foreach (var init in this.Inits)
            {
                init();

                // don't garbage collect populations
                var populations = new List<IDatabase>();

                for (int i = 0; i < 100; i++)
                {
                    var population1 = this.CreatePopulation();
                    populations.Add(population1);
                    var session1 = population1.CreateSession();

                    var c1 = session1.Create<C1>();

                    session1.Commit();

                    var population2 = this.CreatePopulation();
                    populations.Add(population2);
                    var session2 = population2.CreateSession();

                    var c2 = session2.Create<C2>();

                    session2.Commit();
                }
            }
        }

        [Test]
        public override void DifferentSessions()
        {
            // SQLite doesn't supports overlapping transactions
        }
    }
}
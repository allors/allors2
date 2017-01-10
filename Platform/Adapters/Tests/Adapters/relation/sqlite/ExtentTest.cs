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
// --------------------------------------------------------------------------------------------------------------------

using Allors;
using Allors.Domain;

namespace Allors.Adapters.Relation.SQLite
{
    using Allors.Meta;

    using Domain;

    using NUnit.Framework;

    public abstract class ExtentTest : Adapters.ExtentTest
    {
        [Test]
        public override void SortOne()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                this.c1B.C1AllorsString = "3";
                this.c1C.C1AllorsString = "1";
                this.c1D.C1AllorsString = "2";

                this.Session.Commit();

                var extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString);

                var sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1A, sortedObjects[0]);
                Assert.AreEqual(this.c1C, sortedObjects[1]);
                Assert.AreEqual(this.c1D, sortedObjects[2]);
                Assert.AreEqual(this.c1B, sortedObjects[3]);

                extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1A, sortedObjects[0]);
                Assert.AreEqual(this.c1C, sortedObjects[1]);
                Assert.AreEqual(this.c1D, sortedObjects[2]);
                Assert.AreEqual(this.c1B, sortedObjects[3]);

                extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1B, sortedObjects[0]);
                Assert.AreEqual(this.c1D, sortedObjects[1]);
                Assert.AreEqual(this.c1C, sortedObjects[2]);
                Assert.AreEqual(this.c1A, sortedObjects[3]);

                foreach (var useOperator in this.UseOperator)
                {
                    if (useOperator)
                    {
                        var firstExtent = this.LocalExtent(Classes.C1);
                        firstExtent.Filter.AddLike(RoleTypes.C1AllorsString, "1");
                        var secondExtent = this.LocalExtent(Classes.C1);
                        extent = this.Session.Union(firstExtent, secondExtent);
                        secondExtent.Filter.AddLike(RoleTypes.C1AllorsString, "3");
                        extent.AddSort(RoleTypes.C1AllorsString);

                        sortedObjects = (C1[])extent.ToArray(typeof(C1));
                        Assert.AreEqual(2, sortedObjects.Length);
                        Assert.AreEqual(this.c1C, sortedObjects[0]);
                        Assert.AreEqual(this.c1B, sortedObjects[1]);
                    }
                }
            }
        }

        [Test]
        public override void SortTwo()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                this.c1B.C1AllorsString = "a";
                this.c1C.C1AllorsString = "b";
                this.c1D.C1AllorsString = "a";

                this.c1B.C1AllorsInteger = 2;
                this.c1C.C1AllorsInteger = 1;
                this.c1D.C1AllorsInteger = 0;

                this.Session.Commit();

                var extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString);
                extent.AddSort(RoleTypes.C1AllorsInteger);

                var sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1A, sortedObjects[0]);
                Assert.AreEqual(this.c1D, sortedObjects[1]);
                Assert.AreEqual(this.c1B, sortedObjects[2]);
                Assert.AreEqual(this.c1C, sortedObjects[3]);

                extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString);
                extent.AddSort(RoleTypes.C1AllorsInteger, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1A, sortedObjects[0]);
                Assert.AreEqual(this.c1D, sortedObjects[1]);
                Assert.AreEqual(this.c1B, sortedObjects[2]);
                Assert.AreEqual(this.c1C, sortedObjects[3]);

                extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString);
                extent.AddSort(RoleTypes.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1A, sortedObjects[0]);
                Assert.AreEqual(this.c1B, sortedObjects[1]);
                Assert.AreEqual(this.c1D, sortedObjects[2]);
                Assert.AreEqual(this.c1C, sortedObjects[3]);

                extent = this.LocalExtent(Classes.C1);
                extent.AddSort(RoleTypes.C1AllorsString, SortDirection.Descending);
                extent.AddSort(RoleTypes.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.AreEqual(4, sortedObjects.Length);
                Assert.AreEqual(this.c1C, sortedObjects[0]);
                Assert.AreEqual(this.c1B, sortedObjects[1]);
                Assert.AreEqual(this.c1D, sortedObjects[2]);
                Assert.AreEqual(this.c1A, sortedObjects[3]);
            }
        }

        [Test]
        public override void CombinationWithMultipleOperations()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Except + Union
                var firstExtent = this.LocalExtent(Classes.C1);
                firstExtent.Filter.AddNot().AddExists(RoleTypes.C1AllorsString);

                var secondExtent = this.LocalExtent(Classes.C1);
                secondExtent.Filter.AddLike(RoleTypes.C1AllorsString, "Abracadabra");

                var unionExtent = this.Session.Union(firstExtent, secondExtent);
                var topExtent = this.LocalExtent(Classes.C1);

                var exceptionThrown = false;
                try
                {
                    var count = this.Session.Except(topExtent, unionExtent).Count;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);

                // Except + Intersect
                firstExtent = this.LocalExtent(Classes.C1);
                firstExtent.Filter.AddExists(RoleTypes.C1AllorsString);

                secondExtent = this.LocalExtent(Classes.C1);
                secondExtent.Filter.AddLike(RoleTypes.C1AllorsString, "Abracadabra");

                var intersectExtent = this.Session.Intersect(firstExtent, secondExtent);
                topExtent = this.LocalExtent(Classes.C1);

                exceptionThrown = false;
                try
                {
                    var count = this.Session.Except(topExtent, intersectExtent).Count;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);


                // Intersect + Intersect + Intersect
                firstExtent = this.Session.Intersect(this.LocalExtent(Classes.C1), this.LocalExtent(Classes.C1));
                secondExtent = this.Session.Intersect(this.LocalExtent(Classes.C1), this.LocalExtent(Classes.C1));

                exceptionThrown = false;
                try
                {
                    var count = this.Session.Except(firstExtent, secondExtent).Count;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);

                // Except + Intersect + Intersect
                firstExtent = this.Session.Intersect(this.LocalExtent(Classes.C1), this.LocalExtent(Classes.C1));
                secondExtent = this.Session.Intersect(this.LocalExtent(Classes.C1), this.LocalExtent(Classes.C1));

                exceptionThrown = false;
                try
                {
                    var count = this.Session.Except(firstExtent, secondExtent).Count;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
            }
        }
    }
}
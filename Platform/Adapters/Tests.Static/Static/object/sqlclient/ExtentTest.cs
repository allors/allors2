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

namespace Allors.Adapters.Object.SqlClient
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class ExtentTest : Adapters.ExtentTest
    {
        [Fact]
        public override void SortOne()
        {
            foreach (var init in this.Inits)
            {
                foreach (var marker in this.Markers)
                {
                    init();
                    this.Populate();
                    this.Session.Commit();

                    this.c1B.C1AllorsString = "3";
                    this.c1C.C1AllorsString = "1";
                    this.c1D.C1AllorsString = "2";

                    this.Session.Commit();

                    marker();

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.AddSort(MetaC1.Instance.C1AllorsString);

                    var sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(this.c1A, sortedObjects[0]);
                    Assert.Equal(this.c1C, sortedObjects[1]);
                    Assert.Equal(this.c1D, sortedObjects[2]);
                    Assert.Equal(this.c1B, sortedObjects[3]);

                    marker();

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Ascending);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(this.c1A, sortedObjects[0]);
                    Assert.Equal(this.c1C, sortedObjects[1]);
                    Assert.Equal(this.c1D, sortedObjects[2]);
                    Assert.Equal(this.c1B, sortedObjects[3]);

                    marker();

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Ascending);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(this.c1A, sortedObjects[0]);
                    Assert.Equal(this.c1C, sortedObjects[1]);
                    Assert.Equal(this.c1D, sortedObjects[2]);
                    Assert.Equal(this.c1B, sortedObjects[3]);

                    marker();

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Descending);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(this.c1B, sortedObjects[0]);
                    Assert.Equal(this.c1D, sortedObjects[1]);
                    Assert.Equal(this.c1C, sortedObjects[2]);
                    Assert.Equal(this.c1A, sortedObjects[3]);

                    marker();

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Descending);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(this.c1B, sortedObjects[0]);
                    Assert.Equal(this.c1D, sortedObjects[1]);
                    Assert.Equal(this.c1C, sortedObjects[2]);
                    Assert.Equal(this.c1A, sortedObjects[3]);

                    foreach (var useOperator in this.UseOperator)
                    {
                        if (useOperator)
                        {
                            marker();

                            var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                            firstExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "1");
                            var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                            extent = this.Session.Union(firstExtent, secondExtent);
                            secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "3");
                            extent.AddSort(MetaC1.Instance.C1AllorsString);

                            sortedObjects = (C1[])extent.ToArray(typeof(C1));
                            Assert.Equal(2, sortedObjects.Length);
                            Assert.Equal(this.c1C, sortedObjects[0]);
                            Assert.Equal(this.c1B, sortedObjects[1]);
                        }
                    }
                }
            }
        }

        [Fact]
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

                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger);

                var sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1A, sortedObjects[0]);
                Assert.Equal(this.c1D, sortedObjects[1]);
                Assert.Equal(this.c1B, sortedObjects[2]);
                Assert.Equal(this.c1C, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1A, sortedObjects[0]);
                Assert.Equal(this.c1D, sortedObjects[1]);
                Assert.Equal(this.c1B, sortedObjects[2]);
                Assert.Equal(this.c1C, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1A, sortedObjects[0]);
                Assert.Equal(this.c1B, sortedObjects[1]);
                Assert.Equal(this.c1D, sortedObjects[2]);
                Assert.Equal(this.c1C, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Descending);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1C, sortedObjects[0]);
                Assert.Equal(this.c1B, sortedObjects[1]);
                Assert.Equal(this.c1D, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);
            }
        }

        [Fact]
        public override void SortDifferentSession()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);
                var c1B = C1.Create(this.Session);
                var c1C = C1.Create(this.Session);
                var c1D = C1.Create(this.Session);

                c1A.C1AllorsString = "2";
                c1B.C1AllorsString = "1";
                c1C.C1AllorsString = "3";

                var extent = this.Session.Extent(M.C1.Class);
                extent.AddSort(M.C1.C1AllorsString, SortDirection.Ascending);

                var sortedObjects = (C1[])extent.ToArray(typeof(C1));

                var names = sortedObjects.Select(v => v.C1AllorsString).ToArray();

                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(c1D, sortedObjects[0]);
                Assert.Equal(c1B, sortedObjects[1]);
                Assert.Equal(c1A, sortedObjects[2]);
                Assert.Equal(c1C, sortedObjects[3]);

                var c1AId = c1A.Id;

                this.Session.Commit();

                using (var session2 = this.CreateSession())
                {
                    c1A = (C1)session2.Instantiate(c1AId);

                    extent = session2.Extent(M.C1.Class);
                    extent.AddSort(M.C1.C1AllorsString, SortDirection.Ascending);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));

                    names = sortedObjects.Select(v => v.C1AllorsString).ToArray();

                    Assert.Equal(4, sortedObjects.Length);
                    Assert.Equal(c1D, sortedObjects[0]);
                    Assert.Equal(c1B, sortedObjects[1]);
                    Assert.Equal(c1A, sortedObjects[2]);
                    Assert.Equal(c1C, sortedObjects[3]);
                }
            }
        }

    }
}
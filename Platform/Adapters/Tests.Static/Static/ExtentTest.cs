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

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    using Domain;

    using Xunit;
    using System.Linq;

    public enum Zero2Four
    {
        Zero = 0, 
        One = 1, 
        Two = 2, 
        Three = 3, 
        Four = 4
    }

    public abstract class ExtentTest
    {
        protected static readonly bool[] TrueFalse = { true, false };

        #region Population
        protected C1 c1A;
        protected C1 c1B;
        protected C1 c1C;
        protected C1 c1D;
        protected C2 c2A;
        protected C2 c2B;
        protected C2 c2C;
        protected C2 c2D;
        protected C3 c3A;
        protected C3 c3B;
        protected C3 c3C;
        protected C3 c3D;
        protected C4 c4A;
        protected C4 c4B;
        protected C4 c4C;
        protected C4 c4D;
        #endregion

        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        protected virtual bool[] UseOperator => new[] { false, true };

        [Fact]
        public void AndGreaterThanLessThan()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var all = extent.Filter.AddAnd();
                all.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);
                all.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                (extent = this.Session.Extent(MetaI12.Instance.ObjectType)).Filter.AddAnd()
                    .AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0)
                    .AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                (extent = this.Session.Extent(MetaS1234.Instance.ObjectType)).Filter.AddAnd()
                    .AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0)
                    .AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));
            }
        }

        [Fact]
        public void AndLessThan()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();


                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var all = extent.Filter.AddAnd();
                all.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                (extent = this.Session.Extent(MetaI12.Instance.ObjectType)).Filter.AddAnd().AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                (extent = this.Session.Extent(MetaS1234.Instance.ObjectType)).Filter.AddAnd()
                    .AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));
            }
        }

        [Fact]
        public void AssociationMany2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useEnumerable in TrueFalse)
                {
                    foreach (var useOperator in this.UseOperator)
                    {
                        var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(0, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, false, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Full
                        inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(3, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, true, true);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Filtered
                        inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(1, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // ContainedIn Extent over Interface
                        // Empty
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(0, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, false, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Full
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(3, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, true, true);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Filtered
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);
                        }

                        Assert.Equal(1, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // RelationType from Class to Interface

                        // ContainedIn Extent over Class
                        // Empty
                        inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(0, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, false, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Full
                        inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(3, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, true, true);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Filtered
                        inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                            var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(1, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // ContainedIn Extent over Interface
                        // Empty
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(0, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, false, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Full
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(3, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, true, true);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);

                        // Filtered
                        inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        if (useOperator)
                        {
                            var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                            var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                            inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                            inExtent = this.Session.Union(inExtentA, inExtentB);
                        }

                        extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                        if (useEnumerable)
                        {
                            var enumerable = (IEnumerable<IObject>)((Extent<IObject>)inExtent);
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, enumerable);
                        }
                        else
                        {
                            extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);
                        }

                        Assert.Equal(1, extent.Count);
                        this.AssertC1(extent, false, false, false, false);
                        this.AssertC2(extent, false, true, false, false);
                        this.AssertC3(extent, false, false, false, false);
                        this.AssertC4(extent, false, false, false, false);
                    }
                }
            }
        }

        [Fact]
        public void AssociationMany2ManyContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddContains(MetaC2.Instance.C1sWhereC1C2many2many, this.c1C);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddContains(MetaC2.Instance.C1sWhereC1C2many2many, this.c1C);
                extent.Filter.AddContains(MetaC2.Instance.C1sWhereC1C2many2many, this.c1D);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddContains(MetaI12.Instance.C1sWhereC1I12many2many, this.c1C);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddContains(MetaS1234.Instance.S1234sWhereS1234many2many, this.c1B);

                Assert.Equal(2, extent.Count);
                Assert.True(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));
            }
        }

        [Fact]
        public void AssociationMany2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddExists(MetaC2.Instance.C1sWhereC1C2many2many);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddExists(MetaI2.Instance.I1sWhereI1I2many2many);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234sWhereS1234many2many);

                Assert.Equal(10, extent.Count);
                Assert.True(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC2.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC1.Instance.C1sWhereC1C1many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaI34.Instance.I12sWhereI12I34many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaS1234.Instance.S1234sWhereS1234many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void AssociationMany2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, true, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, true, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void AssociationMany2OneContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1sWhereC1C1many2one, this.c1C);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddContains(MetaC2.Instance.C1sWhereC1C2many2one, this.c1C);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddContains(MetaC4.Instance.C3sWhereC3C4many2one, this.c3C);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddContains(MetaI12.Instance.C1sWhereC1I12many2one, this.c1C);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // TODO: wrong relation
            }
        }

        [Fact]
        public void AssociationOne2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void AssociationOne2ManyEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC2.Instance.C1WhereC1C2one2many, this.c1B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC2.Instance.C1WhereC1C2one2many, this.c1C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI2.Instance.I1WhereI1I2one2many, this.c1B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI2.Instance.I1WhereI1I2one2many, this.c1C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234WhereS1234one2many, this.c1B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234WhereS1234one2many, this.c3C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void AssociationOne2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddExists(MetaC2.Instance.C1WhereC1C2one2many);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddExists(MetaI2.Instance.I1WhereI1I2one2many);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234WhereS1234one2many);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void AssociationOne2ManyInstanceof()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC2.Instance.C1WhereC1C2one2many, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI12.Instance.C1WhereC1I12one2many, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.S1234WhereS1234one2many, MetaC1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // TODO: wrong relation
            }
        }

        [Fact]
        public void AssociationOne2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class
                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1WhereC1C1one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    inExtent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC3.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC3.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC4.Instance.C3WhereC3C4one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1WhereC1C1one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC4.Instance.C3WhereC3C4one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // RelationType from Interface to Class

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.I12WhereI12C2one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC2.Instance.I12WhereI12C2one2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void AssociationOne2OneEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1WhereC1C1one2one, this.c1B);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC2.Instance.C1WhereC1C2one2one, this.c1B);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC4.Instance.C3WhereC3C4one2one, this.c3B);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI2.Instance.I1WhereI1I2one2one, this.c1B);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234WhereS1234one2one, this.c1C);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void AssociationOne2OneExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1WhereC1C1one2one);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddExists(MetaC2.Instance.C1WhereC1C2one2one);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddExists(MetaC4.Instance.C3WhereC3C4one2one);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddExists(MetaI2.Instance.I1WhereI1I2one2one);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234WhereS1234one2one);

                Assert.Equal(9, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void AssociationOne2OneInstanceof()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1WhereC1C1one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC2.Instance.C1WhereC1C2one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC4.Instance.C3WhereC3C4one2one, MetaC3.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI12.Instance.C1WhereC1I12one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.S1234WhereS1234one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1WhereC1C1one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC2.Instance.C1WhereC1C2one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC4.Instance.C3WhereC3C4one2one, MetaI3.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI12.Instance.C1WhereC1I12one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.S1234WhereS1234one2one, MetaS1234.Instance.ObjectType);

                Assert.Equal(9, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // TODO: wrong relation
            }
        }

        [Fact]
        public virtual void Combination()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Like and any
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "%nada%");

                var any1 = extent.Filter.AddOr();
                any1.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);
                any1.AddLessThan(MetaC1.Instance.C1AllorsInteger, 3);

                var any2 = extent.Filter.AddOr();
                any2.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);
                any2.AddLessThan(MetaC1.Instance.C1AllorsInteger, 3);

                extent.ToArray(typeof(C1));

                // Role + Value for Shared Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1C1one2manies);

                extent.Filter.AddExists(MetaI12.Instance.I12AllorsInteger);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12AllorsInteger);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, 0);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 0);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 0, 1);

                Assert.Equal(0, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Role In + Except
                var firstExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                firstExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbra%");

                var secondExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                var inExtent = this.Session.Except(firstExtent, secondExtent);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2manies, inExtent);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // AssociationType In + Except
                firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbra%");

                secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                inExtent = this.Session.Except(firstExtent, secondExtent);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));
            }
        }

        [Fact]
        public virtual void CombinationWithMultipleOperations()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Except + Union
                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddNot().AddExists(MetaC1.Instance.C1AllorsString);

                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var unionExtent = this.Session.Union(firstExtent, secondExtent);
                var topExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var extent = this.Session.Except(topExtent, unionExtent);

                Assert.Equal(1, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Except + Intersect
                firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddExists(MetaC1.Instance.C1AllorsString);

                secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var intersectExtent = this.Session.Intersect(firstExtent, secondExtent);
                topExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent = this.Session.Except(topExtent, intersectExtent);

                Assert.Equal(2, extent.Count);
                Assert.True(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Intersect + Intersect + Intersect
                firstExtent = this.Session.Intersect(
                    this.Session.Extent(MetaC1.Instance.ObjectType),
                    this.Session.Extent(MetaC1.Instance.ObjectType));
                secondExtent = this.Session.Intersect(
                    this.Session.Extent(MetaC1.Instance.ObjectType),
                    this.Session.Extent(MetaC1.Instance.ObjectType));

                extent = this.Session.Intersect(firstExtent, secondExtent);

                Assert.Equal(4, extent.Count);
                Assert.True(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Except + Intersect + Intersect
                firstExtent = this.Session.Intersect(
                    this.Session.Extent(MetaC1.Instance.ObjectType),
                    this.Session.Extent(MetaC1.Instance.ObjectType));
                secondExtent = this.Session.Intersect(
                    this.Session.Extent(MetaC1.Instance.ObjectType),
                    this.Session.Extent(MetaC1.Instance.ObjectType));

                extent = this.Session.Except(firstExtent, secondExtent);

                Assert.Equal(0, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));
            }
        }

        [Fact]
        public void NoConcreteClass()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var extent = this.Session.Extent(MetaInterfaceWithoutConcreteClass.Instance.ObjectType);

                Assert.Equal(0, extent.Count);
            }
        }

        [Fact]
        public void Equals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent.Filter.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent.Filter.AddEquals(this.c1B);

                Assert.Equal(0, extent.Count);

                // interface
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);

                extent.Filter.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent.Filter.AddEquals(this.c1B);

                Assert.Equal(0, extent.Count);

                // shared interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                extent.Filter.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent.Filter.AddEquals(this.c1B);

                Assert.Equal(0, extent.Count);
            }
        }

        [Fact]
        public void NotAndEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var not = extent.Filter.AddNot();
                var and = not.AddAnd();
                and.AddEquals(this.c1A);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                and.AddEquals(this.c1B);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // interface
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                not = extent.Filter.AddNot();
                and = not.AddAnd();
                and.AddEquals(this.c1A);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                and.AddEquals(this.c1B);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // shared interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                not = extent.Filter.AddNot();
                and = not.AddAnd();
                and.AddEquals(this.c1A);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                and.AddEquals(this.c1B);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void OrEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var or = extent.Filter.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c1B);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // interface
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                or = extent.Filter.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c1B);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // shared interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                or = extent.Filter.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c2B);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void NotOrEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var not = extent.Filter.AddNot();
                var or = not.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c1B);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // interface
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                not = extent.Filter.AddNot();
                or = not.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c1B);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // shared interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                not = extent.Filter.AddNot();
                or = not.AddOr();
                or.AddEquals(this.c1A);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                or.AddEquals(this.c2B);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public virtual void Except()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var extent = this.Session.Except(firstExtent, secondExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // interface
                firstExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                firstExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbra%");

                secondExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                extent = this.Session.Except(firstExtent, secondExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Shortcut
                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Except(firstExtent, secondExtent);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1C.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Except(firstExtent, secondExtent);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Different Classes
                firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent = this.Session.Extent(MetaC2.Instance.ObjectType);

                var exceptionThrown = false;
                try
                {
                    extent = this.Session.Except(firstExtent, secondExtent);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void Grow()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1AllorsString);
                Assert.Equal(3, extent.Count);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbra");
                Assert.Equal(1, extent.Count);

                // TODO: all possible combinations
            }
        }

        [Fact]
        public void InstanceOf()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class + Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class + Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class + Shared Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Inteface + Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface + Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI12.Instance.ObjectType);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface + Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI1.Instance.ObjectType);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaI12.Instance.ObjectType);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.ObjectType);

                Assert.Equal(16, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);
            }
        }

        [Fact]
        public virtual void Intersect()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // class
                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var extent = this.Session.Intersect(firstExtent, secondExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Shortcut
                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Intersect(firstExtent, secondExtent);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1C.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Intersect(firstExtent, secondExtent);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
                
                // Different Classes
                firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent = this.Session.Extent(MetaC2.Instance.ObjectType);

                var exceptionThrown = false;
                try
                {
                    this.Session.Intersect(firstExtent, secondExtent);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void Naming()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaS1234.Instance.ClassName, "c1");
                    extent.Filter.AddContains(MetaC1.Instance.C1C3one2manies, this.c3B);
                    extent.AddSort(MetaS1234.Instance.ClassName);
                    extent.ToArray(typeof(C1));
                }
            }
        }

        [Fact]
        public void NotAnd()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var none = extent.Filter.AddNot().AddAnd();
                none.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);
                none.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                (extent = this.Session.Extent(MetaI12.Instance.ObjectType)).Filter.AddNot()
                    .AddAnd()
                    .AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0)
                    .AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                (extent = this.Session.Extent(MetaS1234.Instance.ObjectType)).Filter.AddNot()
                    .AddAnd()
                    .AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0)
                    .AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddAnd();

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void NotAssociationMany2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over C2

                    // RelationType from C1 to C2

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C1 to I12

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotAssociationMany2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC2.Instance.C1sWhereC1C2many2many);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI2.Instance.I1sWhereI1I2many2many);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234sWhereS1234many2many);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC2.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC1.Instance.C1sWhereC1C1many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaI34.Instance.I12sWhereI12I34many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234sWhereS1234many2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotAssociationMany2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1sWhereC1C2many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1sWhereC1I12many2one, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotAssociationOne2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over C2

                    // RelationType from C1 to C2

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2many, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C1 to I12

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2many, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotAssociationOne2ManyEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1WhereC1C1one2many, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1WhereC1C1one2many, this.c1C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC2.Instance.C1WhereC1C2one2many, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC2.Instance.C1WhereC1C2one2many, this.c1C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI2.Instance.I1WhereI1I2one2many, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI2.Instance.I1WhereI1I2one2many, this.c1C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234WhereS1234one2many, this.c1B);

                Assert.Equal(15, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234WhereS1234one2many, this.c3C);

                Assert.Equal(14, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2many, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotAssociationOne2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC2.Instance.C1WhereC1C2one2many);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI2.Instance.I1WhereI1I2one2many);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234WhereS1234one2many);

                Assert.Equal(13, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2many);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotAssociationOne2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from C1 to C1

                    // ContainedIn Extent over Class
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1WhereC1C1one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1WhereC1C1one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C1 to C2

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.C1WhereC1C2one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C3 to C4

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC3.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC3.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC4.Instance.C3WhereC3C4one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, true, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC4.Instance.C3WhereC3C4one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, true, false, false, false);

                    // RelationType from I12 to C2

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.I12WhereI12C2one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, true, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC2.Instance.I12WhereI12C2one2one, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Extent over Interface

                    // RelationType from C1 to I12

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2one, inExtent);

                    Assert.Equal(5, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, true, false, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.C1WhereC1I12one2one, inExtent);

                    Assert.Equal(5, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, true, false, false, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotAssociationOne2OneEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1WhereC1C1one2one, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC2.Instance.C1WhereC1C2one2one, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC4.Instance.C3WhereC3C4one2one, this.c3B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, true, true);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI2.Instance.I1WhereI1I2one2one, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234WhereS1234one2one, this.c1C);

                Assert.Equal(15, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C3WhereC3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotAssociationOne2OneExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1WhereC1C1one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC2.Instance.C1WhereC1C2one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC2.Instance.C1WhereC1C2one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC4.Instance.C3WhereC3C4one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI2.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI2.Instance.I1WhereI1I2one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234WhereS1234one2one);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C3WhereC3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotAssociationOne2OneInstanceof()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1WhereC1C1one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC2.Instance.C1WhereC1C2one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC4.Instance.C3WhereC3C4one2one, MetaC3.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaI12.Instance.C1WhereC1I12one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(5, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaS1234.Instance.S1234WhereS1234one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(13, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, true, false, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Interface

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1WhereC1C1one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC2.Instance.C1WhereC1C2one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC4.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC4.Instance.C3WhereC3C4one2one, MetaI3.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaI12.Instance.C1WhereC1I12one2one, MetaI1.Instance.ObjectType);

                Assert.Equal(5, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaS1234.Instance.S1234WhereS1234one2one, MetaS1234.Instance.ObjectType);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, true, true, true);

                // TODO: wrong relation
            }
        }

        [Fact]
        public void NotOr()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var none = extent.Filter.AddNot().AddOr();
                none.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 1);
                none.AddLessThan(MetaC1.Instance.C1AllorsInteger, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                (extent = this.Session.Extent(MetaI12.Instance.ObjectType)).Filter.AddNot()
                    .AddOr()
                    .AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 1)
                    .AddLessThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                (extent = this.Session.Extent(MetaS1234.Instance.ObjectType)).Filter.AddNot()
                    .AddOr()
                    .AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 1)
                    .AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddOr();

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void NotRoleIntegerBetweenValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Between -10 and 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaC1.Instance.C1AllorsInteger, -10, 0);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaC1.Instance.C1AllorsInteger, 0, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaC1.Instance.C1AllorsInteger, 1, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaC1.Instance.C1AllorsInteger, 3, 10);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaI12.Instance.I12AllorsInteger, -10, 0);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaI12.Instance.I12AllorsInteger, 0, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaI12.Instance.I12AllorsInteger, 1, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaI12.Instance.I12AllorsInteger, 3, 10);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaS1234.Instance.S1234AllorsInteger, -10, 0);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Between 0 and 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaS1234.Instance.S1234AllorsInteger, 0, 1);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Between 1 and 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaS1234.Instance.S1234AllorsInteger, 1, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddBetween(MetaS1234.Instance.S1234AllorsInteger, 3, 10);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Between -10 and 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddBetween(MetaC2.Instance.C2AllorsInteger, -10, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddBetween(MetaC2.Instance.C2AllorsInteger, 0, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddBetween(MetaC2.Instance.C2AllorsInteger, 1, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddBetween(MetaC2.Instance.C2AllorsInteger, 3, 10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleIntegerLessThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Less Than 1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaC1.Instance.C1AllorsInteger, 1);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaC1.Instance.C1AllorsInteger, 3);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Less Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaI12.Instance.I12AllorsInteger, 3);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Less Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Less Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Less Than 3
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 3);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaC2.Instance.C2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaI2.Instance.I2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaS2.Instance.S2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaS2.Instance.S2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLessThan(MetaS2.Instance.S2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleIntegerGreaterThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Greater Than 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleIntegerExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1AllorsInteger);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12AllorsInteger);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234AllorsInteger);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaI2.Instance.I2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaS2.Instance.S2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleMany2ManyContainedInExtent()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from C1 to C2
                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C1 to C1

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from C1 to I12

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotRoleMany2ManyContainedInArray()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Extent over Class

                // RelationType from C1 to C2
                // ContainedIn Extent over Class
                // Empty
                var inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "Nothing here!");

                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // ContainedIn Extent over Class
                // Empty
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to C1

                // ContainedIn Extent over Class
                // Empty
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // ContainedIn Extent over Class
                // Empty
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to I12

                // ContainedIn Extent over Class
                // Empty
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                this.Session.Commit();

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // ContainedIn Extent over Class
                // Empty
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void NotRoleMany2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1C2many2manies);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12C2many2manies);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234C2many2manies);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleOne2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class
                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Emtpy Extent
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full Extent
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, true, false, false, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, true, false, false, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC4.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC4.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC3.Instance.C3C4one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, true, false, false, true);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Emtpy Extent
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(4, extent.Count);
                    this.AssertC1(extent, true, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full Extent
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, true, false, false, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, true, false, false, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC3.Instance.C3C4one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, true, false, false, true);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.I12C2one2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaI12.Instance.I12C2one2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, true, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotRoleOne2ManyContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContains(MetaC1.Instance.C1C2one2manies, this.c2C);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, true, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddContains(MetaC1.Instance.C1I12one2manies, this.c2C);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, true, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddContains(MetaS1234.Instance.S1234one2manies, this.c1B);

                Assert.Equal(15, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // TODO: wrong relation
            }
        }

        [Fact]
        public void NotRoleOne2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1C2one2manies);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12C2one2manies);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, true, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234C2one2manies);

                Assert.Equal(14, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, true, true, false, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleOne2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC4.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC4.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC3.Instance.C3C4one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, true, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C1one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1C2one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC3.Instance.C3C4one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, true, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12one2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, true, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddNot().AddContainedIn(MetaC1.Instance.C1I12one2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, true, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void NotRoleOne2OneEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1C1one2one, this.c1B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1C2one2one, this.c2B);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12C2one2one, this.c2A);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234C2one2one, this.c2A);

                Assert.Equal(15, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleOne2OneExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1C2one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1C2one2one);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12C2one2one);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234C2one2one);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, true, true, true, true);
                this.AssertC4(extent, true, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleOne2OneInstanceof()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1C1one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1C2one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1I12one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaS1234.Instance.S1234one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(13, extent.Count);
                this.AssertC1(extent, true, true, false, true);
                this.AssertC2(extent, true, true, false, true);
                this.AssertC3(extent, true, true, false, true);
                this.AssertC4(extent, true, true, true, true);

                // Interface

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1C2one2one, MetaI2.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1I12one2one, MetaI2.Instance.ObjectType);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaC1.Instance.C1I12one2one, MetaI12.Instance.ObjectType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddInstanceof(MetaS1234.Instance.S1234one2one, MetaS1234.Instance.ObjectType);

                Assert.Equal(7, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, true, true, true);

                // TODO: wrong relation
            }
        }

        [Fact]
        public void NotRoleStringEqualsValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Equal ""
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1AllorsString, string.Empty);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC3.Instance.C3AllorsString, string.Empty);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC3.Instance.C3AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaC3.Instance.C3AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Equal ""
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI1.Instance.I1AllorsString, string.Empty);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI3.Instance.I3AllorsString, string.Empty);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI1.Instance.I1AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI3.Instance.I3AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI1.Instance.I1AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI3.Instance.I3AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Shared Interface

                // Equal ""
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12AllorsString, string.Empty);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI34.Instance.I34AllorsString, string.Empty);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI23.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Super Interface

                // Equal ""
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234AllorsString, string.Empty);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234AllorsString, "ᴀbra");

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddEquals(MetaS1234.Instance.S1234AllorsString, "ᴀbracadabra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Class - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaI2.Instance.I2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaI2.Instance.I2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaI2.Instance.I2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaS2.Instance.S2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaS2.Instance.S2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddEquals(MetaS2.Instance.S2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleStringExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaC1.Instance.C1AllorsString);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaI12.Instance.I12AllorsString);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddExists(MetaS1234.Instance.S1234AllorsString);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaC2.Instance.C2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaI2.Instance.I2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddExists(MetaS2.Instance.S2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void NotRoleStringLike()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Like ""
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, string.Empty);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "notfound"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "notfound");

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%ra%"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "%ra%");

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%bra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "%bra");

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%cadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaC1.Instance.C1AllorsString, "%cadabra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Like ""
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaI12.Instance.I12AllorsString, string.Empty);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Like ""
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaS1234.Instance.S1234AllorsString, string.Empty);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaS1234.Instance.S1234AllorsString, "ᴀbra");

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddNot().AddLike(MetaS1234.Instance.S1234AllorsString, "ᴀbracadabra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC1(extent, false, true, false, false);

                // Class - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaC2.Instance.C2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaC2.Instance.C2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaC2.Instance.C2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaI2.Instance.I2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaI2.Instance.I2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaI2.Instance.I2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaS2.Instance.S2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaS2.Instance.S2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddNot().AddLike(MetaS2.Instance.S2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public virtual void Operation()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var extent = this.Session.Union(firstExtent, secondExtent);

                Assert.Equal(3, extent.Count);

                firstExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Oops");

                Assert.Equal(2, extent.Count);

                secondExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "I did it again");

                Assert.Equal(0, extent.Count);

                // TODO: all possible combinations
            }
        }

        [Fact]
        public void Optimizations()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Dangling empty And behind Or
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var or = extent.Filter.AddOr();

                or.AddAnd();
                or.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty Or behind Or
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                or = extent.Filter.AddOr();

                or.AddOr();
                or.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty Not behind Or
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                or = extent.Filter.AddOr();

                or.AddNot();
                or.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty And behind And
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var and = extent.Filter.AddAnd();

                and.AddAnd();
                and.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty Or behind And
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                and = extent.Filter.AddAnd();

                and.AddOr();
                and.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty Not behind And
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                and = extent.Filter.AddAnd();

                and.AddNot();
                and.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Dangling empty And
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddAnd();

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void Or()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                var any = extent.Filter.AddOr();
                any.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);
                any.AddLessThan(MetaC1.Instance.C1AllorsInteger, 3);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                (extent = this.Session.Extent(MetaI12.Instance.ObjectType)).Filter.AddOr()
                    .AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0)
                    .AddLessThan(MetaI12.Instance.I12AllorsInteger, 3);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                (extent = this.Session.Extent(MetaS1234.Instance.ObjectType)).Filter.AddOr()
                    .AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0)
                    .AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 3);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class Without predicates
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddOr();

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void OrContainedIn()
        {
           foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Association (Amgiguous Name)
                Extent<Company> parents = this.Session.Extent(MetaCompany.Instance.ObjectType);

                Extent<Company> children = this.Session.Extent(MetaCompany.Instance.ObjectType);
                children.Filter.AddContainedIn(MetaCompany.Instance.CompanyWhereChild, (Extent)parents);

                Extent<Person> persons = this.Session.Extent(MetaPerson.Instance.ObjectType);
                var or = persons.Filter.AddOr();
                or.AddContainedIn(MetaPerson.Instance.Company, (Extent)parents);
                or.AddContainedIn(MetaPerson.Instance.Company, (Extent)children);

                Assert.Equal(0, persons.Count);
            }
        }

        [Fact]
        public void RoleIntegerExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1AllorsInteger);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddExists(MetaI12.Instance.I12AllorsInteger);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234AllorsInteger);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaI2.Instance.I2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaS2.Instance.S2AllorsInteger);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerBetweenRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Between C1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsInteger, MetaC1.Instance.C1IntegerBetweenA, MetaC1.Instance.C1IntegerBetweenB);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // TODO: Greater than Role
                // Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 0, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 1, 2);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 0, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 1, 2);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Between 3 and 10
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Between -10 and 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, -10, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 0, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 1, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 3, 10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerLessThanRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Less Than 1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsInteger, MetaC1.Instance.C1IntegerLessThan);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // TODO: Less than Role
                // Interface

                // Less Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 3);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Less Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 3);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerGreaterThanRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // C1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, MetaC1.Instance.C1IntegerGreaterThan);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // TODO: Greater than Role
                // Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
                
                // Super Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Greater Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Greater Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerBetweenValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Between -10 and 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsInteger, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsInteger, 0, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsInteger, 1, 2);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsInteger, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 0, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 1, 2);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsInteger, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 0, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 1, 2);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Between 3 and 10
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsInteger, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Between -10 and 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, -10, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 0, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 1, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsInteger, 3, 10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerLessThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Less Than 1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsInteger, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsInteger, 3);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Less Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsInteger, 3);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Less Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
                
                // Less Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsInteger, 3);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsInteger, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerGreaterThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Greater Than 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 0);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 1);
                
                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                // Greater Than 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 0);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                // Greater Than 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 0);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Greater Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Greater Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType
                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleIntegerEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                // Equal 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);


                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                // Equal 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                // Equal 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, 2);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Class - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleFloatBetweenValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Between -10 and 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDouble, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDouble, 0, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDouble, 1, 2);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDouble, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDouble, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDouble, 0, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDouble, 1, 2);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDouble, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDouble, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDouble, 0, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDouble, 1, 2);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Between 3 and 10
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDouble, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Between -10 and 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDouble, -10, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDouble, 0, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDouble, 1, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDouble, 3, 10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleFloatLessThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Less Than 1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDouble, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDouble, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDouble, 3);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Less Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDouble, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDouble, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDouble, 3);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Less Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDouble, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDouble, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDouble, 3);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDouble, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDouble, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDouble, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleFloatGreaterThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Greater Than 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDouble, 0);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDouble, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDouble, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDouble, 0);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDouble, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDouble, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDouble, 0);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Greater Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDouble, 1);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Greater Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDouble, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleFloatEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                // Equal 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDouble, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDouble, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDouble, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                // Equal 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDouble, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDouble, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDouble, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                // Equal 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDouble, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDouble, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDouble, 2);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Class - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDouble, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDouble, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDouble, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleDateTimeBetweenValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var flag in TrueFalse)
                {
                    var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                    var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                    var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                    var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                    var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                    var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                    var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                    var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                    if (flag)
                    {
                        dateTime1 = dateTime1.ToLocalTime();
                        dateTime2 = dateTime2.ToLocalTime();
                        dateTime3 = dateTime3.ToLocalTime();
                        dateTime4 = dateTime4.ToLocalTime();
                        dateTime5 = dateTime5.ToLocalTime();
                        dateTime6 = dateTime6.ToLocalTime();
                        dateTime7 = dateTime7.ToLocalTime();
                        dateTime10 = dateTime10.ToLocalTime();
                    }

                    // Class
                    // Between 1 and 3
                    var extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddBetween(C1.Meta.C1AllorsDateTime, dateTime1, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 3 and 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddBetween(C1.Meta.C1AllorsDateTime, dateTime3, dateTime4);

                    Assert.Equal(1, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 4 and 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddBetween(C1.Meta.C1AllorsDateTime, dateTime4, dateTime5);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 6 and 10
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddBetween(C1.Meta.C1AllorsDateTime, dateTime6, dateTime10);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Interface
                    // Between 1 and 3
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDateTime, dateTime1, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 3 and 4
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDateTime, dateTime3, dateTime4);

                    Assert.Equal(2, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 4 and 5
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDateTime, dateTime4, dateTime5);

                    Assert.Equal(6, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 6 and 10
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDateTime, dateTime6, dateTime10);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Super Interface
                    // Between 1 and 3
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDateTime, dateTime1, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 3 and 4
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDateTime, dateTime3, dateTime4);

                    Assert.Equal(4, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Between 4 and 5
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDateTime, dateTime4, dateTime5);

                    Assert.Equal(12, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.True(extent.Contains(this.c3C));
                    Assert.True(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // Between 6 and 10
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDateTime, dateTime6, dateTime10);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Class - Wrong RelationType0
                    // Between 1 and 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    var exception = false;
                    try
                    {
                        extent.Filter.AddBetween(C2.Meta.C2AllorsDateTime, dateTime1, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Between 3 and 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddBetween(C2.Meta.C2AllorsDateTime, dateTime3, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Between 4 and 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddBetween(C2.Meta.C2AllorsDateTime, dateTime4, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Between 6 and 10
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddBetween(C2.Meta.C2AllorsDateTime, dateTime6, dateTime10);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);
                }
            }
        }

        [Fact]
        public void RoleDateTimeLessThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var flag in TrueFalse)
                {
                    var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                    var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                    var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                    var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                    var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                    var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                    var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                    var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                    if (flag)
                    {
                        dateTime1 = dateTime1.ToLocalTime();
                        dateTime2 = dateTime2.ToLocalTime();
                        dateTime3 = dateTime3.ToLocalTime();
                        dateTime4 = dateTime4.ToLocalTime();
                        dateTime5 = dateTime5.ToLocalTime();
                        dateTime6 = dateTime6.ToLocalTime();
                        dateTime7 = dateTime7.ToLocalTime();
                        dateTime10 = dateTime10.ToLocalTime();
                    }

                    // Class
                    // Less Than 4
                    var extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddLessThan(C1.Meta.C1AllorsDateTime, dateTime4);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddLessThan(C1.Meta.C1AllorsDateTime, dateTime5);

                    Assert.Equal(1, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 6
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddLessThan(C1.Meta.C1AllorsDateTime, dateTime6);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Interface
                    // Less Than 4
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDateTime, dateTime4);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 5
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDateTime, dateTime5);

                    Assert.Equal(2, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 6
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDateTime, dateTime6);

                    Assert.Equal(6, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Super Interface
                    // Less Than 4
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime4);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 5
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime5);

                    Assert.Equal(4, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Less Than 6
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime6);

                    Assert.Equal(12, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.True(extent.Contains(this.c3C));
                    Assert.True(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // Class - Wrong RelationType

                    // Less Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    var exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(C2.Meta.C2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(C2.Meta.C2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 6
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(C2.Meta.C2AllorsDateTime, dateTime6);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Interface - Wrong RelationType
                    // Less Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 6
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDateTime, dateTime6);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Super Interface - Wrong RelationType
                    // Less Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Less Than 6
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDateTime, dateTime6);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);
                }
            }
        }

        [Fact]
        public void RoleDateTimeGreaterThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var flag in TrueFalse)
                {
                    var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                    var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                    var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                    var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                    var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                    var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                    var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                    var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                    if (flag)
                    {
                        dateTime1 = dateTime1.ToLocalTime();
                        dateTime2 = dateTime2.ToLocalTime();
                        dateTime3 = dateTime3.ToLocalTime();
                        dateTime4 = dateTime4.ToLocalTime();
                        dateTime5 = dateTime5.ToLocalTime();
                        dateTime6 = dateTime6.ToLocalTime();
                        dateTime7 = dateTime7.ToLocalTime();
                        dateTime10 = dateTime10.ToLocalTime();
                    }

                    // Class
                    // Greater Than 3
                    var extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddGreaterThan(C1.Meta.C1AllorsDateTime, dateTime3);

                    Assert.Equal(3, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Greater Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddGreaterThan(C1.Meta.C1AllorsDateTime, dateTime4);

                    Assert.Equal(2, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Greater Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddGreaterThan(C1.Meta.C1AllorsDateTime, dateTime5);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Interface
                    // Greater Than 3
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDateTime, dateTime3);

                    Assert.Equal(6, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Greater Than 4
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDateTime, dateTime4);

                    Assert.Equal(4, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Greater Than 5
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDateTime, dateTime5);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Super Interface
                    // Greater Than 3
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime3);

                    Assert.Equal(12, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.True(extent.Contains(this.c3C));
                    Assert.True(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // Greater Than 4
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime4);

                    Assert.Equal(8, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.True(extent.Contains(this.c3C));
                    Assert.True(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // Greater Than 5
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDateTime, dateTime5);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Class - Wrong RelationType

                    // Greater Than 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    var exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(C2.Meta.C2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(C2.Meta.C2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(C2.Meta.C2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Interface - Wrong RelationType

                    // Greater Than 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Super Interface - Wrong RelationType

                    // Greater Than 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Greater Than 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);
                }
            }
        }

        [Fact]
        public void RoleDateTimeEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var flag in TrueFalse)
                {
                    var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                    var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                    var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                    var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                    var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                    var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                    var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                    var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                    if (flag)
                    {
                        dateTime1 = dateTime1.ToLocalTime();
                        dateTime2 = dateTime2.ToLocalTime();
                        dateTime3 = dateTime3.ToLocalTime();
                        dateTime4 = dateTime4.ToLocalTime();
                        dateTime5 = dateTime5.ToLocalTime();
                        dateTime6 = dateTime6.ToLocalTime();
                        dateTime7 = dateTime7.ToLocalTime();
                        dateTime10 = dateTime10.ToLocalTime();
                    }

                    // Class
                    // Equal 3
                    var extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddEquals(C1.Meta.C1AllorsDateTime, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddEquals(C1.Meta.C1AllorsDateTime, dateTime4);

                    Assert.Equal(1, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddEquals(C1.Meta.C1AllorsDateTime, dateTime5);

                    Assert.Equal(2, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Interface
                    // Equal 3
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDateTime, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 4
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDateTime, dateTime4);

                    Assert.Equal(2, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 5
                    extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDateTime, dateTime5);

                    Assert.Equal(4, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Super Interface
                    // Equal 3
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDateTime, dateTime3);

                    Assert.Equal(0, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 4
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDateTime, dateTime4);

                    Assert.Equal(4, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.True(extent.Contains(this.c1B));
                    Assert.False(extent.Contains(this.c1C));
                    Assert.False(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.True(extent.Contains(this.c2B));
                    Assert.False(extent.Contains(this.c2C));
                    Assert.False(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.True(extent.Contains(this.c3B));
                    Assert.False(extent.Contains(this.c3C));
                    Assert.False(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.True(extent.Contains(this.c4B));
                    Assert.False(extent.Contains(this.c4C));
                    Assert.False(extent.Contains(this.c4D));

                    // Equal 5
                    extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDateTime, dateTime5);

                    Assert.Equal(8, extent.Count);
                    Assert.False(extent.Contains(this.c1A));
                    Assert.False(extent.Contains(this.c1B));
                    Assert.True(extent.Contains(this.c1C));
                    Assert.True(extent.Contains(this.c1D));
                    Assert.False(extent.Contains(this.c2A));
                    Assert.False(extent.Contains(this.c2B));
                    Assert.True(extent.Contains(this.c2C));
                    Assert.True(extent.Contains(this.c2D));
                    Assert.False(extent.Contains(this.c3A));
                    Assert.False(extent.Contains(this.c3B));
                    Assert.True(extent.Contains(this.c3C));
                    Assert.True(extent.Contains(this.c3D));
                    Assert.False(extent.Contains(this.c4A));
                    Assert.False(extent.Contains(this.c4B));
                    Assert.True(extent.Contains(this.c4C));
                    Assert.True(extent.Contains(this.c4D));

                    // Class - Wrong RelationType
                    // Equal 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    var exception = false;
                    try
                    {
                        extent.Filter.AddEquals(C2.Meta.C2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(C2.Meta.C2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(C2.Meta.C2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Interface - Wrong RelationType
                    // Equal 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Super Interface - Wrong RelationType
                    // Equal 3
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDateTime, dateTime3);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 4
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDateTime, dateTime4);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    // Equal 5
                    extent = this.Session.Extent(C1.Meta.ObjectType);

                    exception = false;
                    try
                    {
                        extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDateTime, dateTime5);
                    }
                    catch
                    {
                        exception = true;
                    }

                    Assert.True(exception);
                }
            }
        }

        [Fact]
        public void RoleDecimalBetweenValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Between -10 and 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDecimal, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDecimal, 0, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDecimal, 1, 2);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddBetween(MetaC1.Instance.C1AllorsDecimal, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDecimal, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDecimal, 0, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDecimal, 1, 2);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 3 and 10
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddBetween(MetaI12.Instance.I12AllorsDecimal, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Between -10 and 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDecimal, -10, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Between 0 and 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDecimal, 0, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Between 1 and 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDecimal, 1, 2);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Between 3 and 10
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddBetween(MetaS1234.Instance.S1234AllorsDecimal, 3, 10);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Between -10 and 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDecimal, -10, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 0 and 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDecimal, 0, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 1 and 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDecimal, 1, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 10
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(MetaC2.Instance.C2AllorsDecimal, 3, 10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleDecimalLessThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Less Than 1
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDecimal, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDecimal, 2);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaC1.Instance.C1AllorsDecimal, 3);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Less Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDecimal, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDecimal, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaI12.Instance.I12AllorsDecimal, 3);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Less Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDecimal, 1);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Less Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDecimal, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Less Than 3
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLessThan(MetaS1234.Instance.S1234AllorsDecimal, 3);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaC2.Instance.C2AllorsDecimal, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaI2.Instance.I2AllorsDecimal, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Less Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 3
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(MetaS2.Instance.S2AllorsDecimal, 3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleDecimalGreaterThanValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Greater Than 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDecimal, 0);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDecimal, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaC1.Instance.C1AllorsDecimal, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDecimal, 0);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDecimal, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Greater Than 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaI12.Instance.I12AllorsDecimal, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Greater Than 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDecimal, 0);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Greater Than 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDecimal, 1);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Greater Than 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddGreaterThan(MetaS1234.Instance.S1234AllorsDecimal, 2);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaC2.Instance.C2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Greater Than 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(MetaI2.Instance.I2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleDecimalEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                // Equal 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDecimal, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDecimal, 1);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDecimal, 2);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                // Equal 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDecimal, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDecimal, 1);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsDecimal, 2);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                // Equal 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDecimal, 0);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false); 

                // Equal 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDecimal, 1);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false); 
                
                // Equal 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsDecimal, 2);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true); 

                // Class - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDecimal, 0);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDecimal, 1);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsDecimal, 2);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleEnumerationEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // AllorsInteger
                // Class

                // Equal 0
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, Zero2Four.Zero);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, Zero2Four.One);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, Zero2Four.Two);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Equal 0
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, Zero2Four.Zero);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, Zero2Four.One);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsInteger, Zero2Four.Two);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Equal 0
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, Zero2Four.Zero);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal 1
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, Zero2Four.One);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Equal 2
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsInteger, Zero2Four.Two);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Class - Wrong RelationType

                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, Zero2Four.Zero);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, Zero2Four.One);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsInteger, Zero2Four.Two);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, Zero2Four.Zero);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, Zero2Four.One);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsInteger, Zero2Four.Two);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Equal 0
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, Zero2Four.Zero);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 1
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, Zero2Four.One);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 2
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsInteger, Zero2Four.Two);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Wrong type
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exceptionThrown = false;
                C1 first = null;
                try
                {
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsBinary, Zero2Four.Zero);
                    first = (C1)extent.First;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.Null(first);
                Assert.True(exceptionThrown, "Only integer supports Enumeration");
            }
        }

        [Fact]
        public void RoleCompositeEqualsRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var exceptionThrown = false;

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                try
                {
                    extent.Filter.AddEquals(MetaC1.Instance.C1C2one2one, MetaI1.Instance.I1C1one2one);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void RoleMany2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Empty
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Shortcut
                    inExtent = this.c1C.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);

                    // if (useOperator)
                    // {
                    // var inExtentA = c1_1.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies);
                    // var inExtentB = c1_1.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies);
                    // inExtent = Session.Union(inExtentA, inExtentB);
                    // }
                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    // Empty
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    // Empty
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2manies, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void RoleMany2ManyContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1C2many2manies, this.c2C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1C2many2manies, this.c2B);
                extent.Filter.AddContains(MetaC1.Instance.C1C2many2manies, this.c2C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false); 

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1I12many2manies, this.c2C);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddContains(MetaS1234.Instance.S1234many2manies, this.c1A);

                Assert.Equal(9, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false); 

                // TODO: wrong relation
            }
        }

        [Fact]
        public void RoleMany2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1C2many2manies);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddExists(MetaI12.Instance.I12C2many2manies);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234C2many2manies);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2many2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleOne2ManyContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    // Emtpy Extent
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full Extent
                    inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, true, true, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, true, true, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC4.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC4.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Shared Interface
                    // Emtpy Extent
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "Nothing here!");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(0, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full Extent
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, true, true, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, true, true, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2manies, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Interface to Class

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.I12C2one2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaI12.Instance.I12C2one2manies, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void RoleOne2ManyContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1C2one2manies, this.c2C);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContains(MetaC1.Instance.C1I12one2manies, this.c2C);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddContains(MetaS1234.Instance.S1234one2manies, this.c1B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // TODO: wrong relation
            }
        }

        [Fact]
        public void RoleOne2ManyExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1C2one2manies);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddExists(MetaI12.Instance.I12C2one2manies);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, true, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
               
                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234C2one2manies);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, false);
                this.AssertC4(extent, false, false, false, false);
 
                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2manies);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleOne2OneContainedInExtent()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Class
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaC4.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC4.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC4.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, true);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Shared Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, true);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12one2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Shared Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12one2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void RoleOne2OneContainedInArray()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Extent over Class

                // RelationType from Class to Class

                // ContainedIn Extent over Class
                var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType).ToArray();

                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Session.Extent(MetaC2.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Session.Extent(MetaC4.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // ContainedIn Extent over Shared Interface
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1C1one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1C2one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Session.Extent(MetaI34.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC3.Instance.C3C4one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // ContainedIn Extent over Class
                inExtent = this.Session.Extent(MetaC2.Instance.ObjectType).ToArray();

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1I12one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // ContainedIn Extent over Shared Interface
                inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddContainedIn(MetaC1.Instance.C1I12one2one, (IEnumerable<IObject>)inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void RoleMany2OneContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                foreach (var useOperator in this.UseOperator)
                {
                    // Extent over Class

                    // RelationType from Class to Class

                    // ContainedIn Extent over Shared Interface

                    // With filter
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    inExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentA.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Session.Extent(MetaC1.Instance.ObjectType);
                        inExtentB.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2one, inExtent);

                    Assert.Equal(1, extent.Count);
                    this.AssertC1(extent, false, true, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C1many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1C2many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    inExtent = this.Session.Extent(MetaI34.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI34.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI34.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC3.Instance.C3C4many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, true);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // ContainedIn Extent over Class
                    inExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaC2.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaC2.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2one, inExtent);

                    Assert.Equal(2, extent.Count);
                    this.AssertC1(extent, false, false, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // ContainedIn Extent over Shared Interface
                    inExtent = this.Session.Extent(MetaI12.Instance.ObjectType);
                    if (useOperator)
                    {
                        var inExtentA = this.Session.Extent(MetaI12.Instance.ObjectType);
                        var inExtentB = this.Session.Extent(MetaI12.Instance.ObjectType);
                        inExtent = this.Session.Union(inExtentA, inExtentB);
                    }

                    extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1I12many2one, inExtent);

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, true, true, true);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }

        [Fact]
        public void RoleOne2OneEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1C1one2one, this.c1B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1C2one2one, this.c2B);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12C2one2one, this.c2A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234C2one2one, this.c2A);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC3.Instance.C3C2one2one, this.c2A);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleOne2OneExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1C1one2one);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1C2one2one);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddExists(MetaC3.Instance.C3C4one2one);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);
 
                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddExists(MetaI12.Instance.I12C2one2one);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234C2one2one);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaS12.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC3.Instance.C3C2one2one);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleOne2OneInstanceof()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1C1one2one, MetaC1.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1C2one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1I12one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.S1234one2one, MetaC2.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, true, false);
                this.AssertC2(extent, false, false, true, false);
                this.AssertC3(extent, false, false, true, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Class
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1C2one2one, MetaI2.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1I12one2one, MetaI2.Instance.ObjectType);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaC1.Instance.C1I12one2one, MetaI12.Instance.ObjectType);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddInstanceof(MetaS1234.Instance.S1234one2one, MetaS1234.Instance.ObjectType);

                Assert.Equal(9, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // TODO: wrong relation
            }
        }

        [Fact]
        public void RoleStringEqualsRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, MetaC1.Instance.C1AllorsString);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // TODO: Equal Role
            }
        }

        [Fact]
        public void RoleStringEqualsValue()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Equal ""
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC3.Instance.C3AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC3.Instance.C3AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaC3.Instance.C3AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Exclusive Interface

                // Equal ""
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI1.Instance.I1AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI3.Instance.I3AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI1.Instance.I1AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI3.Instance.I3AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaI1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI1.Instance.I1AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI3.Instance.I3AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, false, false);

                // Shared Interface

                // Equal ""
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI34.Instance.I34AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI23.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI23.Instance.I23AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                extent = this.Session.Extent(MetaC3.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                extent = this.Session.Extent(MetaI34.Instance.ObjectType);
                extent.Filter.AddEquals(MetaI34.Instance.I34AllorsString, "ᴀbracadabra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Super Interface

                // Equal ""
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddEquals(MetaS1234.Instance.S1234AllorsString, "ᴀbracadabra");

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true);

                // Class - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaC2.Instance.C2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaI2.Instance.I2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Equal ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(MetaS2.Instance.S2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }
        
        [Fact]
        public void RoleStringExist()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddExists(MetaC1.Instance.C1AllorsString);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddExists(MetaI12.Instance.I12AllorsString);

                Assert.Equal(6, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddExists(MetaS1234.Instance.S1234AllorsString);

                Assert.Equal(12, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, true, true, true);

                // Class - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddExists(MetaC2.Instance.C2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaI2.Instance.I2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddExists(MetaS2.Instance.S2AllorsString);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public void RoleStringLike()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Like ""
                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbra");

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "notfound"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "notfound");

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%ra%"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "%ra%");

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%bra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "%bra");

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "%cadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "%cadabra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Interface

                // Like ""
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLike(MetaI12.Instance.I12AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbra");

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaI12.Instance.ObjectType);
                extent.Filter.AddLike(MetaI12.Instance.I12AllorsString, "ᴀbracadabra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Super Interface

                // Like ""
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLike(MetaS1234.Instance.S1234AllorsString, string.Empty);

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLike(MetaS1234.Instance.S1234AllorsString, "ᴀbra");

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaS1234.Instance.ObjectType);
                extent.Filter.AddLike(MetaS1234.Instance.S1234AllorsString, "ᴀbracadabra");

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, true, true);
                this.AssertC4(extent, false, false, true, true); 

                // Class - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                var exception = false;
                try
                {
                    extent.Filter.AddLike(MetaC2.Instance.C2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaC2.Instance.C2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaC2.Instance.C2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaI2.Instance.I2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaI2.Instance.I2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaI2.Instance.I2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Like ""
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaS2.Instance.S2AllorsString, string.Empty);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaS2.Instance.S2AllorsString, "ᴀbra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Like "ᴀbracadabra"
                extent = this.Session.Extent(MetaC1.Instance.ObjectType);

                exception = false;
                try
                {
                    extent.Filter.AddLike(MetaS2.Instance.S2AllorsString, "ᴀbracadabra");
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }

        [Fact]
        public virtual void Shared()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var sharedExtent = this.Session.Extent(MetaC2.Instance.ObjectType);
                sharedExtent.Filter.AddLike(MetaC2.Instance.C2AllorsString, "%");
                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddContainedIn(MetaC1.Instance.C1C2many2manies, sharedExtent);
                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddContainedIn(MetaC1.Instance.C1C2many2manies, sharedExtent);
                var intersectExtent = this.Session.Intersect(firstExtent, secondExtent);
                intersectExtent.ToArray(typeof(C1));
            }
        }

        [Fact]
        public virtual void SortOne()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                this.c1B.C1AllorsString = "3";
                this.c1C.C1AllorsString = "1";
                this.c1D.C1AllorsString = "2";

                this.Session.Commit();

                var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);

                var sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1C, sortedObjects[0]);
                Assert.Equal(this.c1D, sortedObjects[1]);
                Assert.Equal(this.c1B, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1C, sortedObjects[0]);
                Assert.Equal(this.c1D, sortedObjects[1]);
                Assert.Equal(this.c1B, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1A, sortedObjects[0]);
                Assert.Equal(this.c1B, sortedObjects[1]);
                Assert.Equal(this.c1D, sortedObjects[2]);
                Assert.Equal(this.c1C, sortedObjects[3]);

                foreach (var useOperator in this.UseOperator)
                {
                    if (useOperator)
                    {
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

        [Fact]
        public virtual void SortTwo()
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
                Assert.Equal(this.c1D, sortedObjects[0]);
                Assert.Equal(this.c1B, sortedObjects[1]);
                Assert.Equal(this.c1C, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1D, sortedObjects[0]);
                Assert.Equal(this.c1B, sortedObjects[1]);
                Assert.Equal(this.c1C, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1B, sortedObjects[0]);
                Assert.Equal(this.c1D, sortedObjects[1]);
                Assert.Equal(this.c1C, sortedObjects[2]);
                Assert.Equal(this.c1A, sortedObjects[3]);

                extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                extent.AddSort(MetaC1.Instance.C1AllorsString, SortDirection.Descending);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger, SortDirection.Descending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));
                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(this.c1A, sortedObjects[0]);
                Assert.Equal(this.c1C, sortedObjects[1]);
                Assert.Equal(this.c1B, sortedObjects[2]);
                Assert.Equal(this.c1D, sortedObjects[3]);
            }
        }

        [Fact]
        public virtual void SortDifferentSession()
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
                Assert.Equal(c1B, sortedObjects[0]);
                Assert.Equal(c1A, sortedObjects[1]);
                Assert.Equal(c1C, sortedObjects[2]);
                Assert.Equal(c1D, sortedObjects[3]);

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
                    Assert.Equal(c1B, sortedObjects[0]);
                    Assert.Equal(c1A, sortedObjects[1]);
                    Assert.Equal(c1C, sortedObjects[2]);
                    Assert.Equal(c1D, sortedObjects[3]);
                }
            }
        }

        [Fact]
        public void Hierarchy()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var extent = this.Session.Extent(MetaI4.Instance.ObjectType);
                Assert.Equal(4, extent.Count);
                this.AssertC4(extent, true, true, true, true);
            }
        }

        [Fact]
        public virtual void Union()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Class

                // Filtered
                var firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                firstExtent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbra");

                var secondExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent.Filter.AddLike(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                var extent = this.Session.Union(firstExtent, secondExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Shortcut
                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Union(firstExtent, secondExtent);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                firstExtent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                secondExtent = this.c1C.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent = this.Session.Union(firstExtent, secondExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Different Classes
                firstExtent = this.Session.Extent(MetaC1.Instance.ObjectType);
                secondExtent = this.Session.Extent(MetaC2.Instance.ObjectType);

                var exceptionThrown = false;
                try
                {
                    this.Session.Union(firstExtent, secondExtent);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);

                // Name clashes
                Extent<Company> parents = this.Session.Extent(MetaCompany.Instance.ObjectType);

                Extent<Company> children = this.Session.Extent(MetaCompany.Instance.ObjectType);
                children.Filter.AddContainedIn(MetaCompany.Instance.CompanyWhereChild, (Extent)parents);

                Extent<Company> allCompanies = this.Session.Union(parents, children);

                Extent<Person> persons = this.Session.Extent(MetaPerson.Instance.ObjectType);
                persons.Filter.AddContainedIn(MetaPerson.Instance.Company, (Extent)allCompanies);

                Assert.Equal(0, persons.Count);
            }
        }

        [Fact]
        public void ValidateAssociationContainedIn()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.I12AllorsBoolean.RelationType.AssociationType, inExtent);
                    extent.ToArray();
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateAssociationContains()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContains(MetaC2.Instance.C1WhereC1C2one2many, this.c1C);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);

                exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC2.Instance.ObjectType);
                    extent.Filter.AddContains(MetaC2.Instance.C1WhereC1C2one2one, this.c1C);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateAssociationEquals()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1sWhereC1C1many2many, this.c1B);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);

                exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1sWhereC1C1many2one, this.c1B);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateAssociationExist()
        {
            //TODO:
        }

        [Fact]
        public void ValidateAssociationNotExist()
        {
            // TODO:
        }

        [Fact]
        public void ValidateRoleBetween()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddBetween(MetaC1.Instance.C1C2one2one, 0, 1);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleContainsFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContains(MetaC1.Instance.C1AllorsString, this.c2C);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleEqualFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1C2one2manies, this.c2B);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);

                // Wrong Parameters
                exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1C2many2manies, this.c2B);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleExistFilter()
        {
            // TODO:
        }

        [Fact]
        public void ValidateRoleGreaterThanFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddGreaterThan(MetaC1.Instance.C1C2one2one, 0);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleInFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var inExtent = this.Session.Extent(MetaC1.Instance.ObjectType);

                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddContainedIn(MetaC1.Instance.C1AllorsString, inExtent);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleLessThanFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddLessThan(MetaC1.Instance.C1C2one2one, 1);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleLikeThanFilter()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Wrong Parameters
                var exceptionThrown = false;
                try
                {
                    var extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddLike(MetaC1.Instance.C1AllorsBoolean, string.Empty);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void ValidateRoleNotExistFilter()
        {
            // TODO:
        }

        [Fact]
        public void Shortcut()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // Sortcut
                // Shortcut
                var extent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // With Filter
                // Shortcut
                extent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "ᴀbracadabra");

                Assert.Equal(0, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // With Sort
                // Shortcut
                extent = this.c1B.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType);
                extent.AddSort(MetaC1.Instance.C1AllorsInteger);

                Assert.Equal(1, extent.Count);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }

        [Fact]
        public void RoleContainsMany2ManyAndContained()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // many2many contains
                var c1 = C1.Create(this.Session);
                var c2 = C2.Create(this.Session);
                var c3 = C3.Create(this.Session);

                c2.AddC3Many2Many(c3);
                c1.C1C2many2one = c2;

                var c2s = this.Session.Extent(MetaC2.Instance.ObjectType);
                c2s.Filter.AddContains(MetaC2.Instance.C3Many2Manies, c3);

                Extent<C1> c1s = this.Session.Extent(MetaC1.Instance.ObjectType);
                c1s.Filter.AddContainedIn(MetaC1.Instance.C1C2many2one, (Extent)c2s);

                Assert.Equal(1, c1s.Count);
                Assert.Equal(c1, c1s[0]);
            }
        }

        [Fact]
        public void RoleContainsOne2ManySharedClassAndContained()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // manymany contains
                var c2 = C2.Create(this.Session);
                var c3 = C3.Create(this.Session);
                var c4 = C4.Create(this.Session);

                c3.AddC3C4one2many(c4);
                c2.C3Many2One = c3;

                var c3s = this.Session.Extent(MetaC3.Instance.ObjectType);
                c3s.Filter.AddContains(MetaC3.Instance.C3C4one2manies, c4);

                Extent<C2> c2s = this.Session.Extent(MetaC2.Instance.ObjectType);
                c2s.Filter.AddContainedIn(MetaC2.Instance.C3Many2One, (Extent)c3s);

                Assert.Equal(1, c2s.Count);
                Assert.Equal(c2, c2s[0]);
            }
        }

        [Fact]
        public void AssociationContainsMany2ManyAndContained()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                // many2many contains
                var c1 = C1.Create(this.Session);
                var c2 = C2.Create(this.Session);
                var c3 = C3.Create(this.Session);

                c3.AddC3C2many2many(c2);
                c1.C1C2many2one = c2;

                var c2s = this.Session.Extent(MetaC2.Instance.ObjectType);
                c2s.Filter.AddContains(MetaC2.Instance.C3sWhereC3C2many2many, c3);

                Extent<C1> c1s = this.Session.Extent(MetaC1.Instance.ObjectType);
                c1s.Filter.AddContainedIn(MetaC1.Instance.C1C2many2one, (Extent)c2s);

                Assert.Equal(1, c1s.Count);
                Assert.Equal(c1, c1s[0]);
            }
        }

        protected abstract ISession CreateSession();

        protected void Populate()
        {
            var population = new TestPopulation(this.Session);

            this.c1A = population.C1A;
            this.c1B = population.C1B;
            this.c1C = population.C1C;
            this.c1D = population.C1D;
            
            this.c2A = population.C2A;
            this.c2B = population.C2B;
            this.c2C = population.C2C;
            this.c2D = population.C2D;
            
            this.c3A = population.C3A;
            this.c3B = population.C3B;
            this.c3C = population.C3C;
            this.c3D = population.C3D;

            this.c4A = population.C4A;
            this.c4B = population.C4B;
            this.c4C = population.C4C;
            this.c4D = population.C4D;
        }

        // ISession.Extent for Repositories and
        // IWorkspaceSession.WorkspaceExtent for Workspaces.

        private static Unit GetAllorsString(IObjectType objectType)
        {
            return (Unit)objectType.MetaPopulation.Find(UnitIds.String);
        }

        private void AssertC1(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
        {
            if (assert0)
            {
                Assert.True(extent.Contains(this.c1A), "C1_1");
            }
            else
            {
                Assert.False(extent.Contains(this.c1A), "C1_1");
            }

            if (assert1)
            {
                Assert.True(extent.Contains(this.c1B), "C1_2");
            }
            else
            {
                Assert.False(extent.Contains(this.c1B), "C1_2");
            }

            if (assert2)
            {
                Assert.True(extent.Contains(this.c1C), "C1_3");
            }
            else
            {
                Assert.False(extent.Contains(this.c1C), "C1_3");
            }

            if (assert3)
            {
                Assert.True(extent.Contains(this.c1D), "C1_4");
            }
            else
            {
                Assert.False(extent.Contains(this.c1D), "C1_4");
            }
        }

        private void AssertC2(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
        {
            if (assert0)
            {
                Assert.True(extent.Contains(this.c2A), "C2_1");
            }
            else
            {
                Assert.False(extent.Contains(this.c2A), "C2_1");
            }

            if (assert1)
            {
                Assert.True(extent.Contains(this.c2B), "C2_2");
            }
            else
            {
                Assert.False(extent.Contains(this.c2B), "C2_2");
            }

            if (assert2)
            {
                Assert.True(extent.Contains(this.c2C), "C2_3");
            }
            else
            {
                Assert.False(extent.Contains(this.c2C), "C2_3");
            }

            if (assert3)
            {
                Assert.True(extent.Contains(this.c2D), "C2_4");
            }
            else
            {
                Assert.False(extent.Contains(this.c2D), "C2_4");
            }
        }

        private void AssertC3(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
        {
            if (assert0)
            {
                Assert.True(extent.Contains(this.c3A), "C3_1");
            }
            else
            {
                Assert.False(extent.Contains(this.c3A), "C3_1");
            }

            if (assert1)
            {
                Assert.True(extent.Contains(this.c3B), "C3_2");
            }
            else
            {
                Assert.False(extent.Contains(this.c3B), "C3_2");
            }

            if (assert2)
            {
                Assert.True(extent.Contains(this.c3C), "C3_3");
            }
            else
            {
                Assert.False(extent.Contains(this.c3C), "C3_3");
            }

            if (assert3)
            {
                Assert.True(extent.Contains(this.c3D), "C3_4");
            }
            else
            {
                Assert.False(extent.Contains(this.c3D), "C3_4");
            }
        }

        private void AssertC4(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
        {
            if (assert0)
            {
                Assert.True(extent.Contains(this.c4A), "C4_1");
            }
            else
            {
                Assert.False(extent.Contains(this.c4A), "C4_1");
            }

            if (assert1)
            {
                Assert.True(extent.Contains(this.c4B), "C4_2");
            }
            else
            {
                Assert.False(extent.Contains(this.c4B), "C4_2");
            }

            if (assert2)
            {
                Assert.True(extent.Contains(this.c4C), "C4_3");
            }
            else
            {
                Assert.False(extent.Contains(this.c4C), "C4_3");
            }

            if (assert3)
            {
                Assert.True(extent.Contains(this.c4D), "C4_4");
            }
            else
            {
                Assert.False(extent.Contains(this.c4D), "C4_4");
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;
    using System.Collections;
    using System.Text;

    using Adapters;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    /// <summary>
    /// WeakReference test must have their own proper method,
    /// otherwise the GC.Collect() doesn't work.
    /// </summary>
    public abstract class UnitTest : IDisposable
    {
        protected virtual bool UseFloatMaximum => true;

        protected virtual bool UseFloatMinimum => true;

        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void AllorsBoolean()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    {
                        // True
                        var values = C1.Create(this.Session);
                        values.C1AllorsBoolean = true;
                        values.I1AllorsBoolean = true;
                        values.S1AllorsBoolean = true;

                        mark();

                        Assert.True(values.C1AllorsBoolean);
                        Assert.True(values.I1AllorsBoolean);
                        Assert.True(values.S1AllorsBoolean);
                    }

                    {
                        // False
                        var values = C1.Create(this.Session);
                        values.C1AllorsBoolean = false;
                        values.I1AllorsBoolean = false;
                        values.S1AllorsBoolean = false;

                        mark();

                        Assert.False(values.C1AllorsBoolean);
                        Assert.False(values.I1AllorsBoolean);
                        Assert.False(values.S1AllorsBoolean);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        bool? value = null;

                        mark();
                        value = values.C1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsBoolean);
                        Assert.False(values.ExistI1AllorsBoolean);
                        Assert.False(values.ExistS1AllorsBoolean);

                        this.Session.Commit();

                        value = null;
                        mark();
                        value = values.C1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsBoolean);
                        Assert.False(values.ExistI1AllorsBoolean);
                        Assert.False(values.ExistS1AllorsBoolean);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsBoolean = true;
                        values.I1AllorsBoolean = true;
                        values.S1AllorsBoolean = true;

                        mark();

                        Assert.True(values.ExistC1AllorsBoolean);
                        Assert.True(values.ExistI1AllorsBoolean);
                        Assert.True(values.ExistS1AllorsBoolean);

                        values.RemoveC1AllorsBoolean();
                        values.RemoveI1AllorsBoolean();
                        values.RemoveS1AllorsBoolean();

                        bool? value = null;
                        mark();
                        value = values.C1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsBoolean);
                        Assert.False(values.ExistI1AllorsBoolean);
                        Assert.False(values.ExistS1AllorsBoolean);

                        value = null;

                        mark();
                        value = values.C1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsBoolean;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsBoolean);
                        Assert.False(values.ExistI1AllorsBoolean);
                        Assert.False(values.ExistS1AllorsBoolean);
                    }
                }
            }
        }

        [Fact]
        public void AllorsBooleanWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsBoolean = true;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsBoolean, true);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.True(c1.C1AllorsBoolean);
                }
            }
        }

        [Fact]
        public virtual void AllorsDateTimeUnspecified()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        // unspecified
                        var values = C1.Create(this.Session);

                        var exceptionThrown = false;
                        try
                        {
                            values.C1AllorsDateTime = new DateTime(1973, 03, 27);
                        }
                        catch
                        {
                            exceptionThrown = true;
                        }

                        Assert.True(exceptionThrown);

                        mark();
                        
                        Assert.False(values.ExistC1AllorsDateTime);
                    }

                    {
                        // Minimum
                        var values = C1.Create(this.Session);
                        values.C1AllorsDateTime = DateTime.MinValue;
                        values.I1AllorsDateTime = DateTime.MinValue;
                        values.S1AllorsDateTime = DateTime.MinValue;

                        mark();
                        Assert.Equal(DateTime.MinValue, values.C1AllorsDateTime);
                        Assert.Equal(DateTime.MinValue, values.I1AllorsDateTime);
                        Assert.Equal(DateTime.MinValue, values.S1AllorsDateTime);
                    }

                    {
                        // Maximum
                        var values = C1.Create(this.Session);
                        values.C1AllorsDateTime = DateTime.MaxValue;
                        values.I1AllorsDateTime = DateTime.MaxValue;
                        values.S1AllorsDateTime = DateTime.MaxValue;

                        mark();

                        Assert.Equal(DateTime.MaxValue.ToLongTimeString(), values.C1AllorsDateTime?.ToLongTimeString());
                        Assert.Equal(DateTime.MaxValue.ToLongTimeString(), values.I1AllorsDateTime?.ToLongTimeString());
                        Assert.Equal(DateTime.MaxValue.ToLongTimeString(), values.S1AllorsDateTime?.ToLongTimeString());
                    }
                }
            }
        }

        [Fact]
        public virtual void AllorsDateTimeLocal()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        // year, day & month
                        var values = C1.Create(this.Session);
                        values.C1AllorsDateTime = new DateTime(1973, 03, 27, 0, 0, 0, 0, DateTimeKind.Local);
                        values.I1AllorsDateTime = new DateTime(1973, 03, 27, 0, 0, 0, 0, DateTimeKind.Local);
                        values.S1AllorsDateTime = new DateTime(1973, 03, 27, 0, 0, 0, 0, DateTimeKind.Local);

                        mark();
                        Assert.Equal(new DateTime(1973, 03, 27).ToUniversalTime(), values.C1AllorsDateTime);
                        Assert.Equal(new DateTime(1973, 03, 27).ToUniversalTime(), values.I1AllorsDateTime);
                        Assert.Equal(new DateTime(1973, 03, 27).ToUniversalTime(), values.S1AllorsDateTime);
                    }

                    {
                        // Now
                        var now = this.StripNanoSeconds(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local));
                        var values = C1.Create(this.Session);
                        values.C1AllorsDateTime = now;
                        values.I1AllorsDateTime = now;
                        values.S1AllorsDateTime = now;

                        mark();
                        var nowUniversal = now.ToUniversalTime();
                        Assert.Equal(nowUniversal, values.C1AllorsDateTime);
                        Assert.Equal(nowUniversal, values.I1AllorsDateTime);
                        Assert.Equal(nowUniversal, values.S1AllorsDateTime);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        DateTime? value = null;

                        mark();
                        value = values.C1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDateTime);
                        Assert.False(values.ExistI1AllorsDateTime);
                        Assert.False(values.ExistS1AllorsDateTime);

                        mark();

                        value = null;

                        mark();
                        value = values.C1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDateTime);
                        Assert.False(values.ExistI1AllorsDateTime);
                        Assert.False(values.ExistS1AllorsDateTime);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsDateTime = DateTime.Now;
                        values.I1AllorsDateTime = DateTime.Now;
                        values.S1AllorsDateTime = DateTime.Now;

                        mark();

                        Assert.True(values.ExistC1AllorsDateTime);
                        Assert.True(values.ExistI1AllorsDateTime);
                        Assert.True(values.ExistS1AllorsDateTime);

                        values.RemoveC1AllorsDateTime();
                        values.RemoveI1AllorsDateTime();
                        values.RemoveS1AllorsDateTime();

                        DateTime? value = null;

                        mark();
                        value = values.C1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDateTime);
                        Assert.False(values.ExistI1AllorsDateTime);
                        Assert.False(values.ExistS1AllorsDateTime);

                        this.Session.Commit();

                        value = null;
                        mark();
                        value = values.C1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDateTime;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDateTime);
                        Assert.False(values.ExistI1AllorsDateTime);
                        Assert.False(values.ExistS1AllorsDateTime);
                    }
                }
            }
        }

        [Fact]
        public virtual void AllorsDateTimeUtc()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var universal = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);

                    var c1 = C1.Create(this.Session);
                    c1.C1AllorsDateTime = universal;
                    c1.I1AllorsDateTime = universal;
                    c1.S1AllorsDateTime = universal;

                    var dateTime = new DateTime(1973, 03, 27, 12, 1, 2, 3, DateTimeKind.Utc);

                    mark();
                    Assert.Equal(dateTime, c1.C1AllorsDateTime);
                    Assert.Equal(dateTime, c1.I1AllorsDateTime);
                    Assert.Equal(dateTime, c1.S1AllorsDateTime);

                    Assert.Equal(DateTimeKind.Utc, c1.C1AllorsDateTime.Value.Kind);
                    Assert.Equal(DateTimeKind.Utc, c1.I1AllorsDateTime.Value.Kind);
                    Assert.Equal(DateTimeKind.Utc, c1.S1AllorsDateTime.Value.Kind);

                    mark();

                    dateTime = new DateTime(1973, 03, 27, 12, 1, 2, 3, DateTimeKind.Utc);

                    mark();

                    Assert.Equal(dateTime, c1.C1AllorsDateTime);
                    Assert.Equal(dateTime, c1.I1AllorsDateTime);
                    Assert.Equal(dateTime, c1.S1AllorsDateTime);

                    Assert.Equal(DateTimeKind.Utc, c1.C1AllorsDateTime.Value.Kind);
                    Assert.Equal(DateTimeKind.Utc, c1.I1AllorsDateTime.Value.Kind);
                    Assert.Equal(DateTimeKind.Utc, c1.S1AllorsDateTime.Value.Kind);
                }
            }
        }

        [Fact]
        public virtual void AllorsDateTimeWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsDateTime = new DateTime(1973, 03, 27, 1, 2, 3, 4, DateTimeKind.Utc);

                    this.Session.Commit();

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(C1.Meta.ObjectType);
                    extent.Filter.AddEquals(C1.Meta.C1AllorsDateTime, new DateTime(1973, 03, 27, 1, 2, 3, 4, DateTimeKind.Utc));
                    var first = extent.First;
                    Assert.NotNull(first);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(new DateTime(1973, 03, 27, 1, 2, 3, 4, DateTimeKind.Utc), c1.C1AllorsDateTime);
                }
            }
        }

        [Fact]
        public virtual void AllorsDecimal()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        // Positive
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = 10.10m;
                        values.I1AllorsDecimal = 10.10m;
                        values.S1AllorsDecimal = 10.10m;

                        mark();
                        Assert.Equal(10.10m, values.C1AllorsDecimal);
                        Assert.Equal(10.10m, values.I1AllorsDecimal);
                        Assert.Equal(10.10m, values.S1AllorsDecimal);
                    }

                    {
                        // Negative
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = -10.10m;
                        values.I1AllorsDecimal = -10.10m;
                        values.S1AllorsDecimal = -10.10m;

                        mark();
                        Assert.Equal(-10.10m, values.C1AllorsDecimal);
                        Assert.Equal(-10.10m, values.I1AllorsDecimal);
                        Assert.Equal(-10.10m, values.S1AllorsDecimal);
                    }

                    {
                        // Zero
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = 0m;
                        values.I1AllorsDecimal = 0m;
                        values.S1AllorsDecimal = 0m;

                        mark();
                        Assert.Equal(0m, values.C1AllorsDecimal);
                        Assert.Equal(0m, values.I1AllorsDecimal);
                        Assert.Equal(0m, values.S1AllorsDecimal);
                    }

                    {
                        // Minimum
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = decimal.MinValue;
                        values.I1AllorsDecimal = decimal.MinValue;
                        values.S1AllorsDecimal = decimal.MinValue;

                        mark();
                        Assert.Equal(decimal.MinValue, values.C1AllorsDecimal);
                        Assert.Equal(decimal.MinValue, values.I1AllorsDecimal);
                        Assert.Equal(decimal.MinValue, values.S1AllorsDecimal);
                    }

                    {
                        // Maximum
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = decimal.MaxValue;
                        values.I1AllorsDecimal = decimal.MaxValue;
                        values.S1AllorsDecimal = decimal.MaxValue;

                        mark();
                        Assert.Equal(decimal.MaxValue, values.C1AllorsDecimal);
                        Assert.Equal(decimal.MaxValue, values.I1AllorsDecimal);
                        Assert.Equal(decimal.MaxValue, values.S1AllorsDecimal);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        decimal? value = null;

                        mark();
                        value = values.C1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDecimal);
                        Assert.False(values.ExistI1AllorsDecimal);
                        Assert.False(values.ExistS1AllorsDecimal);

                        this.Session.Commit();

                        mark();
                        value = values.C1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDecimal;
                        Assert.Null(value);

                        Assert.False(values.ExistC1AllorsDecimal);
                        Assert.False(values.ExistI1AllorsDecimal);
                        Assert.False(values.ExistS1AllorsDecimal);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsDecimal = 10.10m;
                        values.I1AllorsDecimal = 10.10m;
                        values.S1AllorsDecimal = 10.10m;

                        mark();

                        Assert.True(values.ExistC1AllorsDecimal);
                        Assert.True(values.ExistI1AllorsDecimal);
                        Assert.True(values.ExistS1AllorsDecimal);

                        values.RemoveC1AllorsDecimal();
                        values.RemoveI1AllorsDecimal();
                        values.RemoveS1AllorsDecimal();

                        decimal? value = null;

                        mark();
                        value = values.C1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDecimal);
                        Assert.False(values.ExistI1AllorsDecimal);
                        Assert.False(values.ExistS1AllorsDecimal);

                        mark();
                        value = null;

                        mark();
                        value = values.C1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDecimal;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDecimal);
                        Assert.False(values.ExistI1AllorsDecimal);
                        Assert.False(values.ExistS1AllorsDecimal);
                    }
                }
            }
        }

        [Fact]
        public virtual void AllorsDecimalWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsDecimal = 1M;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDecimal, 1M);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(1M, c1.C1AllorsDecimal);
                }
            }
        }

        [Fact]
        public void AllorsDouble()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        // Positive
                        var values = C1.Create(this.Session);
                        values.C1AllorsDouble = 10.10d;
                        values.I1AllorsDouble = 10.10d;
                        values.S1AllorsDouble = 10.10d;

                        mark();
                        Assert.Equal(10.10d, values.C1AllorsDouble);
                        Assert.Equal(10.10d, values.I1AllorsDouble);
                        Assert.Equal(10.10d, values.S1AllorsDouble);
                    }

                    {
                        // Negative
                        var values = C1.Create(this.Session);
                        values.C1AllorsDouble = -10.10d;
                        values.I1AllorsDouble = -10.10d;
                        values.S1AllorsDouble = -10.10d;

                        mark();
                        Assert.Equal(-10.10d, values.C1AllorsDouble);
                        Assert.Equal(-10.10d, values.I1AllorsDouble);
                        Assert.Equal(-10.10d, values.S1AllorsDouble);
                    }

                    {
                        // Zero
                        var values = C1.Create(this.Session);
                        values.C1AllorsDouble = 0d;
                        values.I1AllorsDouble = 0d;
                        values.S1AllorsDouble = 0d;

                        mark();
                        Assert.Equal(0d, values.C1AllorsDouble);
                        Assert.Equal(0d, values.I1AllorsDouble);
                        Assert.Equal(0d, values.S1AllorsDouble);
                    }

                    // Minimum
                    if (this.UseFloatMinimum)
                    {
                        C1 values = C1.Create(this.Session);
                        values.C1AllorsDouble = double.MinValue;
                        values.I1AllorsDouble = double.MinValue;
                        values.S1AllorsDouble = double.MinValue;

                        mark();
                        Assert.Equal(double.MinValue, values.C1AllorsDouble);
                        Assert.Equal(double.MinValue, values.I1AllorsDouble);
                        Assert.Equal(double.MinValue, values.S1AllorsDouble);
                    }

                    // Maximum
                    if (this.UseFloatMaximum)
                    {
                        var values = C1.Create(this.Session);
                        values.C1AllorsDouble = double.MaxValue;
                        values.I1AllorsDouble = double.MaxValue;
                        values.S1AllorsDouble = double.MaxValue;

                        mark();
                        Assert.Equal(double.MaxValue, values.C1AllorsDouble);
                        Assert.Equal(double.MaxValue, values.I1AllorsDouble);
                        Assert.Equal(double.MaxValue, values.S1AllorsDouble);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        double? value = null;

                        mark();
                        value = values.C1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDouble);
                        Assert.False(values.ExistI1AllorsDouble);
                        Assert.False(values.ExistS1AllorsDouble);

                        mark();

                        mark();
                        value = values.C1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDouble);
                        Assert.False(values.ExistI1AllorsDouble);
                        Assert.False(values.ExistS1AllorsDouble);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsDouble = 10.10d;
                        values.I1AllorsDouble = 10.10d;
                        values.S1AllorsDouble = 10.10d;

                        mark();
                        Assert.True(values.ExistC1AllorsDouble);
                        Assert.True(values.ExistI1AllorsDouble);
                        Assert.True(values.ExistS1AllorsDouble);

                        values.RemoveC1AllorsDouble();
                        values.RemoveI1AllorsDouble();
                        values.RemoveS1AllorsDouble();

                        double? value = null;

                        mark();
                        value = values.C1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDouble);
                        Assert.False(values.ExistI1AllorsDouble);
                        Assert.False(values.ExistS1AllorsDouble);

                        mark();
                        value = values.C1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsDouble;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsDouble);
                        Assert.False(values.ExistI1AllorsDouble);
                        Assert.False(values.ExistS1AllorsDouble);
                    }
                }
            }
        }

        [Fact]
        public void AllorsDoubleWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    C1 c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsDouble = 1;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsDouble, 1);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(1, c1.C1AllorsDouble);
                }
            }
        }

        [Fact]
        public void AllorsInteger()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        // Positive
                        var values = C1.Create(this.Session);
                        values.C1AllorsInteger = 10;
                        values.I1AllorsInteger = 10;
                        values.S1AllorsInteger = 10;

                        mark();
                        Assert.Equal(10, values.C1AllorsInteger);
                        Assert.Equal(10, values.I1AllorsInteger);
                        Assert.Equal(10, values.S1AllorsInteger);
                    }

                    {
                        // Negative
                        var values = C1.Create(this.Session);
                        values.C1AllorsInteger = -10;
                        values.I1AllorsInteger = -10;
                        values.S1AllorsInteger = -10;

                        mark();
                        Assert.Equal(-10, values.C1AllorsInteger);
                        Assert.Equal(-10, values.I1AllorsInteger);
                        Assert.Equal(-10, values.S1AllorsInteger);
                    }

                    {
                        // Zero
                        var values = C1.Create(this.Session);
                        values.C1AllorsInteger = 0;
                        values.I1AllorsInteger = 0;
                        values.S1AllorsInteger = 0;

                        mark();
                        Assert.Equal(0, values.C1AllorsInteger);
                        Assert.Equal(0, values.I1AllorsInteger);
                        Assert.Equal(0, values.S1AllorsInteger);
                    }

                    {
                        // Minimum
                        var values = C1.Create(this.Session);
                        values.C1AllorsInteger = int.MinValue;
                        values.I1AllorsInteger = int.MinValue;
                        values.S1AllorsInteger = int.MinValue;

                        mark();
                        Assert.Equal(int.MinValue, values.C1AllorsInteger);
                        Assert.Equal(int.MinValue, values.I1AllorsInteger);
                        Assert.Equal(int.MinValue, values.S1AllorsInteger);
                    }

                    {
                        // Maximum
                        var values = C1.Create(this.Session);
                        values.C1AllorsInteger = int.MaxValue;
                        values.I1AllorsInteger = int.MaxValue;
                        values.S1AllorsInteger = int.MaxValue;

                        mark();
                        Assert.Equal(int.MaxValue, values.C1AllorsInteger);
                        Assert.Equal(int.MaxValue, values.I1AllorsInteger);
                        Assert.Equal(int.MaxValue, values.S1AllorsInteger);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        int? value = null;

                        mark();
                        value = values.C1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsInteger);
                        Assert.False(values.ExistI1AllorsInteger);
                        Assert.False(values.ExistS1AllorsInteger);

                        mark();

                        mark();
                        value = values.C1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsInteger);
                        Assert.False(values.ExistI1AllorsInteger);
                        Assert.False(values.ExistS1AllorsInteger);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);

                        this.Session.Commit();

                        values.C1AllorsInteger = 10;
                        values.I1AllorsInteger = 10;
                        values.S1AllorsInteger = 10;

                        Assert.True(values.ExistC1AllorsInteger);
                        Assert.True(values.ExistI1AllorsInteger);
                        Assert.True(values.ExistS1AllorsInteger);

                        mark();

                        values.RemoveC1AllorsInteger();
                        values.RemoveI1AllorsInteger();
                        values.RemoveS1AllorsInteger();

                        int? value = null;

                        mark();
                        value = values.C1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsInteger);
                        Assert.False(values.ExistI1AllorsInteger);
                        Assert.False(values.ExistS1AllorsInteger);

                        mark();

                        mark();
                        value = values.C1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsInteger;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsInteger;
                        Assert.Null(value);

                        mark();

                        Assert.False(values.ExistC1AllorsInteger);
                        Assert.False(values.ExistI1AllorsInteger);
                        Assert.False(values.ExistS1AllorsInteger);
                    }
                }
            }
        }

        [Fact]
        public void AllorsIntegerWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsInteger = 1;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsInteger, 1);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(1, c1.C1AllorsInteger);
                }
            }
        }

        [Fact]
        public void AllorsLargeString()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    var aLarge = new StringBuilder().Insert(0, "a", 100000).ToString();
                    var bLarge = new StringBuilder().Insert(0, "b", 100000).ToString();
                    var cLarge = new StringBuilder().Insert(0, "c", 100000).ToString();

                    {
                        var values = C1.Create(this.Session);
                        values.C1StringLarge = aLarge;
                        values.I1StringLarge = bLarge;
                        values.S1StringLarge = cLarge;

                        mark();
                        Assert.True(values.ExistC1StringLarge);
                        Assert.True(values.ExistI1StringLarge);
                        Assert.True(values.ExistS1StringLarge);

                        Assert.Equal(aLarge, values.C1StringLarge);
                        Assert.Equal(bLarge, values.I1StringLarge);
                        Assert.Equal(cLarge, values.S1StringLarge);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        mark();
                        Assert.False(values.ExistC1StringLarge);
                        Assert.False(values.ExistI1StringLarge);
                        Assert.False(values.ExistS1StringLarge);

                        Assert.True(values.C1StringLarge == null);
                        Assert.True(values.I1StringLarge == null);
                        Assert.True(values.S1StringLarge == null);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1StringLarge = aLarge;
                        values.I1StringLarge = bLarge;
                        values.S1StringLarge = cLarge;

                        mark();
                        Assert.True(values.ExistC1StringLarge);
                        Assert.True(values.ExistI1StringLarge);
                        Assert.True(values.ExistS1StringLarge);

                        values.RemoveC1StringLarge();
                        values.RemoveI1StringLarge();
                        values.RemoveS1StringLarge();

                        mark();
                        Assert.False(values.ExistC1StringLarge);
                        Assert.False(values.ExistI1StringLarge);
                        Assert.False(values.ExistS1StringLarge);

                        Assert.True(values.C1StringLarge == null);
                        Assert.True(values.I1StringLarge == null);
                        Assert.True(values.S1StringLarge == null);
                    }

                    {
                        // reset null
                        var values = C1.Create(this.Session);
                        values.C1StringLarge = aLarge;
                        values.I1StringLarge = bLarge;
                        values.S1StringLarge = cLarge;

                        mark();
                        Assert.True(values.ExistC1StringLarge);
                        Assert.True(values.ExistI1StringLarge);
                        Assert.True(values.ExistS1StringLarge);

                        values.C1StringLarge = null;
                        values.I1StringLarge = null;
                        values.S1StringLarge = null;

                        mark();
                        Assert.False(values.ExistC1StringLarge);
                        Assert.False(values.ExistI1StringLarge);
                        Assert.False(values.ExistS1StringLarge);

                        Assert.True(values.C1StringLarge == null);
                        Assert.True(values.I1StringLarge == null);
                        Assert.True(values.S1StringLarge == null);
                    }

                    {
                        // large string in small string
                        var exceptionThrown = false;
                        var values = C1.Create(this.Session);
                        try
                        {
                            mark();
                            values.C1AllorsString = aLarge;
                        }
                        catch (ArgumentException)
                        {
                            exceptionThrown = true;
                        }

                        Assert.True(exceptionThrown);
                        Assert.False(values.ExistC1AllorsString);

                        exceptionThrown = false;
                        values = C1.Create(this.Session);
                        try
                        {
                            mark();
                            values.I1AllorsString = aLarge;
                        }
                        catch (ArgumentException)
                        {
                            exceptionThrown = true;
                        }

                        Assert.True(exceptionThrown);
                        Assert.False(values.ExistI1AllorsString);

                        exceptionThrown = false;
                        values = C1.Create(this.Session);
                        try
                        {
                            mark();
                            values.S1AllorsString = aLarge;
                        }
                        catch (ArgumentException)
                        {
                            exceptionThrown = true;
                        }

                        Assert.True(exceptionThrown);
                        Assert.False(values.ExistS1AllorsString);

                        this.Session.Commit();
                    }
                }
            }
        }

        [Fact]
        public void AllorsLargeStringWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var aLarge = new StringBuilder().Insert(0, "a", 100000).ToString();

                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1StringLarge = aLarge;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1StringLarge, aLarge);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(aLarge, c1.C1StringLarge);
                }
            }
        }

        [Fact]
        public void AllorsSmallBinary()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    var binary1 = new byte[] { 0 };
                    var binary2 = new byte[] { 1, 2 };
                    var binary3 = new byte[] { 3, 4, 5 };

                    {
                        var values = C1.Create(this.Session);
                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        mark();
                        Assert.Equal(binary1, values.C1AllorsBinary);
                        Assert.Equal(binary1, values.C1AllorsBinary);
                        Assert.Equal(binary2, values.I1AllorsBinary);
                        Assert.Equal(binary2, values.I1AllorsBinary);
                        Assert.Equal(binary3, values.S1AllorsBinary);
                        Assert.Equal(binary3, values.S1AllorsBinary);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        mark();
                        Assert.False(values.ExistC1AllorsBinary);
                        Assert.False(values.ExistI1AllorsBinary);
                        Assert.False(values.ExistS1AllorsBinary);

                        Assert.Null(values.C1AllorsBinary);
                        Assert.Null(values.I1AllorsBinary);
                        Assert.Null(values.S1AllorsBinary);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        mark();
                        Assert.True(values.ExistC1AllorsBinary);
                        Assert.True(values.ExistI1AllorsBinary);
                        Assert.True(values.ExistS1AllorsBinary);

                        values.RemoveC1AllorsBinary();
                        values.RemoveI1AllorsBinary();
                        values.RemoveS1AllorsBinary();

                        mark();
                        Assert.False(values.ExistC1AllorsBinary);
                        Assert.False(values.ExistI1AllorsBinary);
                        Assert.False(values.ExistS1AllorsBinary);

                        Assert.Null(values.C1AllorsBinary);
                        Assert.Null(values.I1AllorsBinary);
                        Assert.Null(values.S1AllorsBinary);
                    }

                    {
                        // reset null
                        var values = C1.Create(this.Session);
                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        mark();
                        Assert.True(values.ExistC1AllorsBinary);
                        Assert.True(values.ExistI1AllorsBinary);
                        Assert.True(values.ExistS1AllorsBinary);

                        values.C1AllorsBinary = null;
                        values.I1AllorsBinary = null;
                        values.S1AllorsBinary = null;

                        mark();
                        Assert.False(values.ExistC1AllorsBinary);
                        Assert.False(values.ExistI1AllorsBinary);
                        Assert.False(values.ExistS1AllorsBinary);

                        Assert.Null(values.C1AllorsBinary);
                        Assert.Null(values.I1AllorsBinary);
                        Assert.Null(values.S1AllorsBinary);
                    }

                    {
                        // rollback set
                        var values = C1.Create(this.Session);

                        this.Session.Commit();

                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        this.Session.Rollback();

                        Assert.False(values.ExistC1AllorsBinary);
                        Assert.False(values.ExistI1AllorsBinary);
                        Assert.False(values.ExistS1AllorsBinary);
                    }

                    {
                        // rollback reset
                        var values = C1.Create(this.Session);

                        mark();

                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        this.Session.Commit();

                        values.C1AllorsBinary = new byte[] { 9 };
                        values.I1AllorsBinary = new byte[] { 9 };
                        values.S1AllorsBinary = new byte[] { 9 };

                        this.Session.Rollback();

                        Assert.True(values.ExistC1AllorsBinary);
                        Assert.True(values.ExistI1AllorsBinary);
                        Assert.True(values.ExistS1AllorsBinary);

                        Assert.Equal(binary1, values.C1AllorsBinary);
                        Assert.Equal(binary2, values.I1AllorsBinary);
                        Assert.Equal(binary3, values.S1AllorsBinary);
                    }

                    {
                        // rollback reset null
                        var values = C1.Create(this.Session);

                        mark();

                        values.C1AllorsBinary = binary1;
                        values.I1AllorsBinary = binary2;
                        values.S1AllorsBinary = binary3;

                        this.Session.Commit();

                        values.RemoveC1AllorsBinary();
                        values.RemoveI1AllorsBinary();
                        values.RemoveS1AllorsBinary();

                        this.Session.Rollback();

                        Assert.True(values.ExistC1AllorsBinary);
                        Assert.True(values.ExistI1AllorsBinary);
                        Assert.True(values.ExistS1AllorsBinary);

                        Assert.Equal(binary1, values.C1AllorsBinary);
                        Assert.Equal(binary2, values.I1AllorsBinary);
                        Assert.Equal(binary3, values.S1AllorsBinary);
                    }
                }
            }
        }

        [Fact]
        public void AllorsSmallBinaryWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var binary1 = new byte[] { 0 };

                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsBinary = binary1;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsBinary, binary1);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(binary1, c1.C1AllorsBinary);
                }
            }
        }

        [Fact]
        public void AllorsSmallString()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        var values = C1.Create(this.Session);
                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        mark();
                        Assert.Equal("a", values.C1AllorsString);
                        Assert.Equal("a", values.C1AllorsString);
                        Assert.Equal("b", values.I1AllorsString);
                        Assert.Equal("b", values.I1AllorsString);
                        Assert.Equal("c", values.S1AllorsString);
                        Assert.Equal("c", values.S1AllorsString);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        mark();
                        Assert.False(values.ExistC1AllorsString);
                        Assert.False(values.ExistI1AllorsString);
                        Assert.False(values.ExistS1AllorsString);

                        Assert.Null(values.C1AllorsString);
                        Assert.Null(values.I1AllorsString);
                        Assert.Null(values.S1AllorsString);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        mark();
                        Assert.True(values.ExistC1AllorsString);
                        Assert.True(values.ExistI1AllorsString);
                        Assert.True(values.ExistS1AllorsString);

                        values.RemoveC1AllorsString();
                        values.RemoveI1AllorsString();
                        values.RemoveS1AllorsString();

                        mark();
                        Assert.False(values.ExistC1AllorsString);
                        Assert.False(values.ExistI1AllorsString);
                        Assert.False(values.ExistS1AllorsString);

                        Assert.Null(values.C1AllorsString);
                        Assert.Null(values.I1AllorsString);
                        Assert.Null(values.S1AllorsString);
                    }

                    {
                        // reset null
                        var values = C1.Create(this.Session);
                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        mark();
                        Assert.True(values.ExistC1AllorsString);
                        Assert.True(values.ExistI1AllorsString);
                        Assert.True(values.ExistS1AllorsString);

                        values.C1AllorsString = null;
                        values.I1AllorsString = null;
                        values.S1AllorsString = null;

                        mark();
                        Assert.False(values.ExistC1AllorsString);
                        Assert.False(values.ExistI1AllorsString);
                        Assert.False(values.ExistS1AllorsString);

                        Assert.Null(values.C1AllorsString);
                        Assert.Null(values.I1AllorsString);
                        Assert.Null(values.S1AllorsString);
                    }

                    {
                        // rollback set
                        var values = C1.Create(this.Session);

                        this.Session.Commit();

                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        this.Session.Rollback();

                        Assert.False(values.ExistC1AllorsString);
                        Assert.False(values.ExistI1AllorsString);
                        Assert.False(values.ExistS1AllorsString);
                    }

                    {
                        // rollback reset
                        var values = C1.Create(this.Session);

                        mark();

                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        this.Session.Commit();

                        values.C1AllorsString = "q";
                        values.I1AllorsString = "r";
                        values.S1AllorsString = "s";

                        this.Session.Rollback();

                        Assert.True(values.ExistC1AllorsString);
                        Assert.True(values.ExistI1AllorsString);
                        Assert.True(values.ExistS1AllorsString);

                        Assert.Equal("a", values.C1AllorsString);
                        Assert.Equal("b", values.I1AllorsString);
                        Assert.Equal("c", values.S1AllorsString);
                    }

                    {
                        // rollback reset null
                        var values = C1.Create(this.Session);

                        mark();

                        values.C1AllorsString = "a";
                        values.I1AllorsString = "b";
                        values.S1AllorsString = "c";

                        this.Session.Commit();

                        values.RemoveC1AllorsString();
                        values.RemoveI1AllorsString();
                        values.RemoveS1AllorsString();

                        this.Session.Rollback();

                        Assert.True(values.ExistC1AllorsString);
                        Assert.True(values.ExistI1AllorsString);
                        Assert.True(values.ExistS1AllorsString);

                        Assert.Equal("a", values.C1AllorsString);
                        Assert.Equal("b", values.I1AllorsString);
                        Assert.Equal("c", values.S1AllorsString);
                    }
                }
            }
        }
        
        [Fact]
        public void AllorsSmallStringWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    var c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsString = "a";

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsString, "a");
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal("a", c1.C1AllorsString);
                }
            }
        }

        [Fact]
        public virtual void AllorsUnique()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    {
                        var unique1 = Guid.NewGuid();
                        var unique2 = Guid.NewGuid();
                        var unique3 = Guid.NewGuid();

                        var values = C1.Create(this.Session);
                        values.C1AllorsUnique = unique1;
                        values.I1AllorsUnique = unique2;
                        values.S1AllorsUnique = unique3;

                        mark();
                        Assert.Equal(unique1, values.C1AllorsUnique);
                        Assert.Equal(unique2, values.I1AllorsUnique);
                        Assert.Equal(unique3, values.S1AllorsUnique);
                    }

                    {
                        // Empty Guid
                        var values = C1.Create(this.Session);
                        values.C1AllorsUnique = Guid.Empty;
                        values.I1AllorsUnique = Guid.Empty;
                        values.S1AllorsUnique = Guid.Empty;

                        mark();
                        Assert.Equal(Guid.Empty, values.C1AllorsUnique);
                        Assert.Equal(Guid.Empty, values.I1AllorsUnique);
                        Assert.Equal(Guid.Empty, values.S1AllorsUnique);
                    }

                    {
                        // initial empty
                        var values = C1.Create(this.Session);

                        Guid? value = null;

                        mark();
                        value = values.C1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsUnique);
                        Assert.False(values.ExistI1AllorsUnique);
                        Assert.False(values.ExistS1AllorsUnique);

                        mark();
                        value = values.C1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsUnique);
                        Assert.False(values.ExistI1AllorsUnique);
                        Assert.False(values.ExistS1AllorsUnique);
                    }

                    {
                        // reset empty
                        var values = C1.Create(this.Session);
                        values.C1AllorsUnique = Guid.NewGuid();
                        values.I1AllorsUnique = Guid.NewGuid();
                        values.S1AllorsUnique = Guid.NewGuid();

                        mark();
                        Assert.True(values.ExistC1AllorsUnique);
                        Assert.True(values.ExistI1AllorsUnique);
                        Assert.True(values.ExistS1AllorsUnique);

                        values.RemoveC1AllorsUnique();
                        values.RemoveI1AllorsUnique();
                        values.RemoveS1AllorsUnique();

                        Guid? value = null;

                        mark();
                        value = values.C1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsUnique);
                        Assert.False(values.ExistI1AllorsUnique);
                        Assert.False(values.ExistS1AllorsUnique);

                        mark();

                        mark();
                        value = values.C1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.I1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        value = values.S1AllorsUnique;
                        Assert.Null(value);

                        mark();
                        Assert.False(values.ExistC1AllorsUnique);
                        Assert.False(values.ExistI1AllorsUnique);
                        Assert.False(values.ExistS1AllorsUnique);
                    }
                }
            }
        }

        [Fact]
        public virtual void AllorsUniqueWeakReference()
        {
            foreach (var init in this.Inits)
            {
                init();
                if (this.Session is ISession)
                {
                    Guid unique = Guid.NewGuid();

                    C1 c1 = C1.Create(this.Session);
                    var c1Id = c1.Id.ToString();

                    c1.C1AllorsUnique = unique;

                    // Force a Flush
                    Extent<C1> extent = this.Session.Extent(MetaC1.Instance.ObjectType);
                    extent.Filter.AddEquals(MetaC1.Instance.C1AllorsUnique, unique);
                    Assert.NotNull(extent.First);

                    // Garbage Collect
                    c1 = null;
                    extent = null;

                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    c1 = (C1)this.Session.Instantiate(c1Id);

                    Assert.Equal(unique, c1.C1AllorsUnique);
                }
            }
        }

        [Fact]
        public virtual void Dirty()
        {
            foreach (var init in this.Inits)
            {
                init();
                var values = new ArrayList();
                for (int i = 0; i < 4000; i++)
                {
                    values.Add(SingleUnit.Create(this.Session));
                }

                this.Session.Commit();

                foreach (SingleUnit singleValue in values)
                {
                    singleValue.AllorsInteger = int.Parse(singleValue.Strategy.ObjectId.ToString());
                }

                this.Session.Commit();
            }
        }

        [Fact]
        public void RelationChecks()
        {
            foreach (var init in this.Inits)
            {
                init();
                foreach (var mark in this.Markers)
                {
                    var c1A = C1.Create(this.Session);
                    var c1B = C1.Create(this.Session);

                    // Illegal Role
                    // Illegal values
                    var exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsBoolean.RelationType, "Oops");
                    }
                    catch (ArgumentException)
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsDecimal.RelationType, "Oops");
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsDouble.RelationType, "Oops");
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsInteger.RelationType, "Oops");
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsString.RelationType, 0);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsInteger.RelationType, 0L);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal objects
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsBoolean.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsDecimal.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsDouble.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsInteger.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1AllorsString.RelationType, 0);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal AssociationType
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC2.Instance.C2AllorsBoolean.RelationType, true);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC2.Instance.C2AllorsDecimal.RelationType, DateTime.Now);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC2.Instance.C2AllorsDouble.RelationType, 0.01m);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC2.Instance.C2AllorsInteger.RelationType, 1);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC2.Instance.C2AllorsString.RelationType, "hello");
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal Role
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.SetUnitRole(MetaC1.Instance.C1C1one2one.RelationType, "Ooops");
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);
                }
            }
        }

        protected DateTime StripNanoSeconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Kind);
        }
    }
}
// <copyright file="One2OneTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    // TODO: Add remove with null and zero array
    public abstract class One2OneTest : IDisposable
    {
        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void C1_C1one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var run = 0; run < Settings.NumberOfRuns; run++)
                    {
                        var from = C1.Create(this.Session);
                        var fromAnother = C1.Create(this.Session);

                        var to = C1.Create(this.Session);
                        var toAnother = C1.Create(this.Session);

                        // To 1 and back
                        // Get
                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // 1-1
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);

                        // 0-0
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.C1C1one2one = to;
                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // Exist
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);

                        // 1-1
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);

                        // 0-0
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;
                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);

                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // Exist
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);

                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // Same From / Different To
                        // Get
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = toAnother;
                        from.C1C1one2one = toAnother;

                        mark();
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Equal(toAnother, from.C1C1one2one);
                        Assert.Equal(toAnother, from.C1C1one2one);
                        Assert.Equal(from, toAnother.C1WhereC1C1one2one);
                        Assert.Equal(from, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        // Exist
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = toAnother;
                        from.C1C1one2one = toAnother;

                        mark();
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = toAnother;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(toAnother, fromAnother.C1C1one2one);
                        Assert.Equal(toAnother, fromAnother.C1C1one2one);
                        Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);
                        Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Equal(toAnother, fromAnother.C1C1one2one);
                        Assert.Equal(toAnother, fromAnother.C1C1one2one);
                        Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);
                        Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = null;
                        fromAnother.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);
                        Assert.Null(toAnother.C1WhereC1C1one2one);

                        // Exist
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = toAnother;
                        fromAnother.C1C1one2one = toAnother;

                        mark();
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);
                        Assert.True(toAnother.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = null;
                        fromAnother.C1C1one2one = null;

                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);
                        Assert.False(toAnother.ExistC1WhereC1C1one2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = to;
                        fromAnother.C1C1one2one = to;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Equal(to, fromAnother.C1C1one2one);
                        Assert.Equal(to, fromAnother.C1C1one2one);
                        Assert.Equal(fromAnother, to.C1WhereC1C1one2one);
                        Assert.Equal(fromAnother, to.C1WhereC1C1one2one);

                        fromAnother.RemoveC1C1one2one();
                        fromAnother.RemoveC1C1one2one();

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(fromAnother.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        fromAnother.C1C1one2one = to;
                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Equal(to, fromAnother.C1C1one2one);
                        fromAnother.RemoveC1C1one2one();

                        // Exist
                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.True(from.ExistC1C1one2one);
                        Assert.True(from.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = to;
                        fromAnother.C1C1one2one = to;

                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(fromAnother.ExistC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);
                        Assert.True(to.ExistC1WhereC1C1one2one);

                        fromAnother.RemoveC1C1one2one();
                        fromAnother.RemoveC1C1one2one();

                        mark();
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(from.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(fromAnother.ExistC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);
                        Assert.False(to.ExistC1WhereC1C1one2one);

                        // Null
                        // Set Null
                        // Get
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        // Get
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);

                        from = C1.Create(this.Session);
                        fromAnother = C1.Create(this.Session);

                        to = C1.Create(this.Session);
                        toAnother = C1.Create(this.Session);

                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // 1-1
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);

                        mark();
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(to, from.C1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);
                        Assert.Equal(from, to.C1WhereC1C1one2one);

                        // 0-0
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, null);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, null);
                        mark();
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(from.C1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);
                        Assert.Null(to.C1WhereC1C1one2one);

                        // From - Middle - To
                        from = C1.Create(this.Session);
                        var middle = C1.Create(this.Session);
                        to = C1.Create(this.Session);

                        from.C1C1one2one = middle;
                        middle.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.Null(middle.C1WhereC1C1one2one);
                        Assert.Null(middle.C1C1one2one);
                    }
                }
            }
        }

        [Fact]
        public void C1_C2one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to = C2.Create(this.Session);
                    var toAnother = C2.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.C1C2one2one);
                    Assert.Null(to.C1WhereC1C2one2one);

                    // 1-1
                    from.C1C2one2one = to;

                    mark();
                    Assert.Equal(to, from.C1C2one2one);
                    Assert.Equal(from, to.C1WhereC1C2one2one);

                    // 0-0
                    from.RemoveC1C2one2one();
                    from.RemoveC1C2one2one();
                    from.C1C2one2one = to;
                    from.C1C2one2one = null;
                    from.C1C2one2one = null;

                    mark();
                    Assert.Null(from.C1C2one2one);
                    Assert.Null(to.C1WhereC1C2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C2one2one = to;
                    from.C1C2one2one = to;

                    mark();
                    Assert.Equal(to, from.C1C2one2one);
                    Assert.Equal(from, to.C1WhereC1C2one2one);

                    from.RemoveC1C2one2one();

                    // Same From / Different To
                    from.C1C2one2one = to;
                    from.C1C2one2one = toAnother;

                    mark();
                    Assert.Null(to.C1WhereC1C2one2one);
                    Assert.Equal(toAnother, from.C1C2one2one);
                    Assert.Equal(from, toAnother.C1WhereC1C2one2one);

                    // Different From / Different To
                    from.C1C2one2one = to;
                    fromAnother.C1C2one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.C1C2one2one);
                    Assert.Equal(from, to.C1WhereC1C2one2one);
                    Assert.Equal(toAnother, fromAnother.C1C2one2one);
                    Assert.Equal(fromAnother, toAnother.C1WhereC1C2one2one);

                    // Different From / Same To
                    from.C1C2one2one = to;
                    fromAnother.C1C2one2one = to;

                    mark();
                    Assert.Null(from.C1C2one2one);
                    Assert.Equal(to, fromAnother.C1C2one2one);
                    Assert.Equal(fromAnother, to.C1WhereC1C2one2one);

                    fromAnother.RemoveC1C2one2one();

                    // Null
                    // Set Null
                    from.C1C2one2one = null;
                    mark();
                    Assert.Null(from.C1C2one2one);
                    from.C1C2one2one = to;
                    from.C1C2one2one = null;
                    mark();
                    Assert.Null(from.C1C2one2one);
                }
            }
        }

        [Fact]
        public void C1_I1one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to = C1.Create(this.Session);
                    var toAnother = C1.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.C1I1one2one);
                    Assert.Null(to.C1WhereC1I1one2one);

                    // 1-1
                    from.C1I1one2one = to;

                    mark();
                    Assert.Equal(to, from.C1I1one2one);
                    Assert.Equal(from, to.C1WhereC1I1one2one);

                    // 0-0
                    from.RemoveC1I1one2one();
                    from.RemoveC1I1one2one();
                    from.C1I1one2one = to;
                    from.C1I1one2one = null;
                    from.C1I1one2one = null;

                    mark();
                    Assert.Null(from.C1I1one2one);
                    Assert.Null(to.C1WhereC1I1one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C1one2one = to;
                    from.C1C1one2one = to;

                    mark();
                    Assert.Equal(to, from.C1C1one2one);
                    Assert.Equal(from, to.C1WhereC1C1one2one);

                    from.RemoveC1C1one2one();

                    // Same From / Different To
                    from.C1C1one2one = to;
                    from.C1C1one2one = toAnother;

                    mark();
                    Assert.Null(to.C1WhereC1C1one2one);
                    Assert.Equal(toAnother, from.C1C1one2one);
                    Assert.Equal(from, toAnother.C1WhereC1C1one2one);

                    // Different From / Different To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.C1C1one2one);
                    Assert.Equal(from, to.C1WhereC1C1one2one);
                    Assert.Equal(toAnother, fromAnother.C1C1one2one);
                    Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);

                    // Different From / Same To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = to;

                    mark();
                    Assert.Null(from.C1C1one2one);
                    Assert.Equal(to, fromAnother.C1C1one2one);
                    Assert.Equal(fromAnother, to.C1WhereC1C1one2one);

                    fromAnother.RemoveC1C1one2one();

                    // Null
                    // Set Null
                    from.C1C1one2one = null;
                    mark();
                    Assert.Null(from.C1C1one2one);
                    from.C1C1one2one = to;
                    from.C1C1one2one = null;
                    mark();
                    Assert.Null(from.C1C1one2one);
                }
            }
        }

        [Fact]
        public void C1_I2one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to = C2.Create(this.Session);
                    var toAnother = C2.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.C1I2one2one);
                    Assert.Null(to.C1WhereC1I2one2one);

                    // 1-1
                    from.C1I2one2one = to;

                    mark();
                    Assert.Equal(to, from.C1I2one2one);
                    Assert.Equal(from, to.C1WhereC1I2one2one);

                    // 0-0
                    from.RemoveC1I2one2one();
                    from.RemoveC1I2one2one();
                    from.C1I2one2one = to;
                    from.C1I2one2one = null;
                    from.C1I2one2one = null;

                    mark();
                    Assert.Null(from.C1I2one2one);
                    Assert.Null(to.C1WhereC1I2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1I2one2one = to;
                    from.C1I2one2one = to;

                    mark();
                    Assert.Equal(to, from.C1I2one2one);
                    Assert.Equal(from, to.C1WhereC1I2one2one);

                    from.RemoveC1I2one2one();

                    // Same From / Different To
                    from.C1I2one2one = to;
                    from.C1I2one2one = toAnother;

                    mark();
                    Assert.Null(to.C1WhereC1I2one2one);
                    Assert.Equal(toAnother, from.C1I2one2one);
                    Assert.Equal(from, toAnother.C1WhereC1I2one2one);

                    // Different From / Different To
                    from.C1I2one2one = to;
                    fromAnother.C1I2one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.C1I2one2one);
                    Assert.Equal(from, to.C1WhereC1I2one2one);
                    Assert.Equal(toAnother, fromAnother.C1I2one2one);
                    Assert.Equal(fromAnother, toAnother.C1WhereC1I2one2one);

                    // Different From / Same To
                    from.C1I2one2one = to;
                    fromAnother.C1I2one2one = to;

                    mark();
                    Assert.Null(from.C1I2one2one);
                    Assert.Equal(to, fromAnother.C1I2one2one);
                    Assert.Equal(fromAnother, to.C1WhereC1I2one2one);

                    fromAnother.RemoveC1I2one2one();

                    // Null
                    // Set Null
                    from.C1I2one2one = null;
                    mark();
                    Assert.Null(from.C1I2one2one);
                    from.C1I2one2one = to;
                    from.C1I2one2one = null;
                    mark();
                    Assert.Null(from.C1I2one2one);
                }
            }
        }

        [Fact]
        public void C3_C4one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C3.Create(this.Session);
                    var fromAnother = C3.Create(this.Session);

                    var to = C4.Create(this.Session);
                    var toAnother = C4.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.C3C4one2one);
                    Assert.Null(to.C3WhereC3C4one2one);

                    // 1-1
                    from.C3C4one2one = to;

                    mark();
                    Assert.Equal(to, from.C3C4one2one);
                    Assert.Equal(from, to.C3WhereC3C4one2one);

                    // 0-0
                    from.RemoveC3C4one2one();
                    from.RemoveC3C4one2one();
                    from.C3C4one2one = to;
                    from.C3C4one2one = null;
                    from.C3C4one2one = null;

                    mark();
                    Assert.Null(from.C3C4one2one);
                    Assert.Null(to.C3WhereC3C4one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C3C4one2one = to;
                    from.C3C4one2one = to;

                    mark();
                    Assert.Equal(to, from.C3C4one2one);
                    Assert.Equal(from, to.C3WhereC3C4one2one);

                    from.RemoveC3C4one2one();

                    // Same From / Different To
                    from.C3C4one2one = to;
                    from.C3C4one2one = toAnother;

                    mark();
                    Assert.Null(to.C3WhereC3C4one2one);
                    Assert.Equal(toAnother, from.C3C4one2one);
                    Assert.Equal(from, toAnother.C3WhereC3C4one2one);

                    // Different From / Different To
                    from.C3C4one2one = to;
                    fromAnother.C3C4one2one = toAnother;

                    this.Session.Commit();

                    mark();
                    Assert.Equal(to, from.C3C4one2one);
                    Assert.Equal(from, to.C3WhereC3C4one2one);
                    Assert.Equal(toAnother, fromAnother.C3C4one2one);
                    Assert.Equal(fromAnother, toAnother.C3WhereC3C4one2one);

                    // Different From / Same To
                    from.C3C4one2one = to;
                    fromAnother.C3C4one2one = to;

                    mark();
                    Assert.Null(from.C3C4one2one);
                    Assert.Equal(to, fromAnother.C3C4one2one);
                    Assert.Equal(fromAnother, to.C3WhereC3C4one2one);

                    fromAnother.RemoveC3C4one2one();

                    // Null
                    // Set Null
                    from.C3C4one2one = null;
                    mark();
                    Assert.Null(from.C3C4one2one);
                    from.C3C4one2one = to;
                    from.C3C4one2one = null;
                    mark();
                    Assert.Null(from.C3C4one2one);
                }
            }
        }

        [Fact]
        public void I1_I12one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var nrOfRuns = Settings.NumberOfRuns;
                    for (var i = 0; i < nrOfRuns; i++)
                    {
                        var from = C1.Create(this.Session);
                        var fromAnother = C1.Create(this.Session);

                        var to = C1.Create(this.Session);
                        var toAnother = C2.Create(this.Session);

                        // To 1 and back
                        // Get
                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        // 1-1
                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);

                        // 0-0
                        from.RemoveI1I12one2one();
                        from.RemoveI1I12one2one();
                        from.I1I12one2one = to;
                        from.I1I12one2one = null;
                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);

                        // 1-1
                        from.I1I12one2one = to;

                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);

                        // 0-0
                        from.RemoveI1I12one2one();
                        from.RemoveI1I12one2one();
                        from.I1I12one2one = to;
                        from.I1I12one2one = null;
                        from.I1I12one2one = null;

                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        from.I1I12one2one = to;
                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);

                        from.RemoveI1I12one2one();

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;
                        from.I1I12one2one = to;

                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);

                        from.RemoveI1I12one2one();

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        // Same From / Different To
                        // Get
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = toAnother;

                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Equal(toAnother, from.I1I12one2one);
                        Assert.Equal(toAnother, from.I1I12one2one);
                        Assert.Equal(from, toAnother.I1WhereI1I12one2one);
                        Assert.Equal(from, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        // Exist
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = toAnother;

                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = toAnother;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        Assert.Null(toAnother.I1WhereI1I12one2one);

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = toAnother;

                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = null;

                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        Assert.Equal(from, to.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = to;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Equal(to, fromAnother.I1I12one2one);
                        Assert.Equal(to, fromAnother.I1I12one2one);
                        Assert.Equal(fromAnother, to.I1WhereI1I12one2one);
                        Assert.Equal(fromAnother, to.I1WhereI1I12one2one);

                        fromAnother.RemoveI1I12one2one();

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(fromAnother.I1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);
                        Assert.Null(to.I1WhereI1I12one2one);

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        Assert.True(from.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = to;

                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(fromAnother.ExistI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        Assert.True(to.ExistI1WhereI1I12one2one);

                        fromAnother.RemoveI1I12one2one();

                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(from.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(fromAnother.ExistI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        Assert.False(to.ExistI1WhereI1I12one2one);

                        // Null
                        // Set Null
                        // Get
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);

                        // Get
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        Assert.Equal(to, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.Null(from.I1I12one2one);
                        Assert.Null(from.I1I12one2one);
                    }

                    for (var i = 0; i < nrOfRuns; i++)
                    {
                        var from = C1.Create(this.Session);
                        mark();
                        var fromAnother = C1.Create(this.Session);
                        mark();

                        var to = C1.Create(this.Session);
                        mark();
                        var toAnother = C2.Create(this.Session);
                        mark();

                        // To 1 and back
                        // Get
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        // 1-1
                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();

                        // 0-0
                        from.RemoveI1I12one2one();
                        mark();
                        from.RemoveI1I12one2one();
                        mark();
                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = null;
                        mark();
                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();

                        // 1-1
                        from.I1I12one2one = to;
                        mark();

                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();

                        // 0-0
                        from.RemoveI1I12one2one();
                        mark();
                        from.RemoveI1I12one2one();
                        mark();
                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = null;
                        mark();
                        from.I1I12one2one = null;
                        mark();

                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();

                        from.RemoveI1I12one2one();
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = to;
                        mark();

                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.RemoveI1I12one2one();
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        // Same From / Different To
                        // Get
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = toAnother;
                        mark();

                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(toAnother, from.I1I12one2one);
                        mark();
                        Assert.Equal(toAnother, from.I1I12one2one);
                        mark();
                        Assert.Equal(from, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = toAnother;
                        mark();

                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        // Different From / Different To
                        // Get
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = toAnother;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(toAnother.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = toAnother;
                        mark();

                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = null;
                        mark();

                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        // Different From / Same To
                        // Get
                        mark();
                        this.Session.Commit();
                        mark();
                        this.Session.Commit();
                        mark();
                        this.Session.Commit();
                        mark();
                        this.Session.Commit();
                        mark();
                        this.Session.Commit();
                        mark();
                        this.Session.Commit();

                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(from, to.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = to;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Equal(to, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(to, fromAnother.I1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, to.I1WhereI1I12one2one);
                        mark();
                        Assert.Equal(fromAnother, to.I1WhereI1I12one2one);
                        mark();

                        fromAnother.RemoveI1I12one2one();
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(fromAnother.I1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();
                        Assert.Null(to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.True(from.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = to;
                        mark();

                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.True(to.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.RemoveI1I12one2one();
                        mark();

                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(from.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.False(to.ExistI1WhereI1I12one2one);
                        mark();

                        // Null
                        // Set Null
                        // Get
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();

                        // Get
                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.Equal(to, from.I1I12one2one);
                        mark();
                        Assert.Equal(to, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.Null(from.I1I12one2one);
                        mark();
                        Assert.Null(from.I1I12one2one);
                        mark();
                    }
                }
            }
        }

        [Fact]
        public void I1_I1one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to = C1.Create(this.Session);
                    var toAnother = C1.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.I1I1one2one);
                    Assert.Null(to.I1WhereI1I1one2one);

                    // 1-1
                    from.I1I1one2one = to;

                    mark();
                    Assert.Equal(to, from.I1I1one2one);
                    Assert.Equal(from, to.I1WhereI1I1one2one);

                    // 0-0
                    from.RemoveI1I1one2one();
                    from.RemoveI1I1one2one();
                    from.I1I1one2one = to;
                    from.I1I1one2one = null;
                    from.I1I1one2one = null;

                    mark();
                    Assert.Null(from.I1I1one2one);
                    Assert.Null(to.I1WhereI1I1one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C1one2one = to;
                    from.C1C1one2one = to;

                    mark();
                    Assert.Equal(to, from.C1C1one2one);
                    Assert.Equal(from, to.C1WhereC1C1one2one);

                    from.RemoveC1C1one2one();

                    // Same From / Different To
                    from.C1C1one2one = to;
                    from.C1C1one2one = toAnother;

                    mark();
                    Assert.Null(to.C1WhereC1C1one2one);
                    Assert.Equal(toAnother, from.C1C1one2one);
                    Assert.Equal(from, toAnother.C1WhereC1C1one2one);

                    // Different From / Different To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.C1C1one2one);
                    Assert.Equal(from, to.C1WhereC1C1one2one);
                    Assert.Equal(toAnother, fromAnother.C1C1one2one);
                    Assert.Equal(fromAnother, toAnother.C1WhereC1C1one2one);

                    // Different From / Same To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = to;

                    mark();
                    Assert.Null(from.C1C1one2one);
                    Assert.Equal(to, fromAnother.C1C1one2one);
                    Assert.Equal(fromAnother, to.C1WhereC1C1one2one);

                    fromAnother.RemoveC1C1one2one();

                    // Null
                    // Set Null
                    from.C1C1one2one = null;
                    mark();
                    Assert.Null(from.C1C1one2one);
                    from.C1C1one2one = to;
                    from.C1C1one2one = null;
                    mark();
                    Assert.Null(from.C1C1one2one);

                    // From - Middle - To
                    from = C1.Create(this.Session);
                    var middle = C1.Create(this.Session);
                    to = C1.Create(this.Session);

                    from.I1I1one2one = middle;
                    middle.I1I1one2one = to;
                    from.I1I1one2one = to;

                    mark();
                    Assert.Null(middle.I1WhereI1I1one2one);
                    Assert.Null(middle.I1I1one2one);
                }
            }
        }

        [Fact]
        public void I1_I2one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to = C2.Create(this.Session);
                    var toAnother = C2.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.I1I2one2one);
                    Assert.Null(to.I1WhereI1I2one2one);

                    // 1-1
                    from.I1I2one2one = to;

                    mark();
                    Assert.Equal(to, from.I1I2one2one);
                    Assert.Equal(from, to.I1WhereI1I2one2one);

                    // 0-0
                    from.RemoveI1I2one2one();
                    from.RemoveI1I2one2one();
                    from.I1I2one2one = to;
                    from.I1I2one2one = null;
                    from.I1I2one2one = null;

                    mark();
                    Assert.Null(from.I1I2one2one);
                    Assert.Null(to.I1WhereI1I2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.I1I2one2one = to;
                    from.I1I2one2one = to;

                    mark();
                    Assert.Equal(to, from.I1I2one2one);
                    Assert.Equal(from, to.I1WhereI1I2one2one);

                    from.RemoveI1I2one2one();

                    // Same From / Different To
                    from.I1I2one2one = to;
                    from.I1I2one2one = toAnother;

                    mark();
                    Assert.Null(to.I1WhereI1I2one2one);
                    Assert.Equal(toAnother, from.I1I2one2one);
                    Assert.Equal(from, toAnother.I1WhereI1I2one2one);

                    // Different From / Different To
                    from.I1I2one2one = to;
                    fromAnother.I1I2one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.I1I2one2one);
                    Assert.Equal(from, to.I1WhereI1I2one2one);
                    Assert.Equal(toAnother, fromAnother.I1I2one2one);
                    Assert.Equal(fromAnother, toAnother.I1WhereI1I2one2one);

                    // Different From / Same To
                    from.I1I2one2one = to;
                    fromAnother.I1I2one2one = to;

                    mark();
                    Assert.Null(from.I1I2one2one);
                    Assert.Equal(to, fromAnother.I1I2one2one);
                    Assert.Equal(fromAnother, to.I1WhereI1I2one2one);

                    fromAnother.RemoveI1I2one2one();

                    // Null
                    // Set Null
                    from.I1I2one2one = null;
                    mark();
                    Assert.Null(from.I1I2one2one);
                    from.I1I2one2one = to;
                    from.I1I2one2one = null;
                    mark();
                    Assert.Null(from.I1I2one2one);
                }
            }
        }

        [Fact]
        public void I1_I34one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < Settings.NumberOfRuns; i++)
                    {
                        var from = C1.Create(this.Session);
                        var fromAnother = C1.Create(this.Session);

                        var to = C3.Create(this.Session);
                        var toAnother = C4.Create(this.Session);

                        // To 1 and back
                        // Get
                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        // 1-1
                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);

                        // 0-0
                        from.RemoveI1I34one2one();
                        from.RemoveI1I34one2one();
                        from.I1I34one2one = to;
                        from.I1I34one2one = null;
                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        // Exist
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);

                        // 1-1
                        from.I1I34one2one = to;

                        mark();
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);

                        // 0-0
                        from.RemoveI1I34one2one();
                        from.RemoveI1I34one2one();
                        from.I1I34one2one = to;
                        from.I1I34one2one = null;
                        from.I1I34one2one = null;

                        mark();
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        from.I1I34one2one = to;
                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);

                        from.RemoveI1I34one2one();

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        // Exist
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;
                        from.I1I34one2one = to;

                        mark();
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);

                        from.RemoveI1I34one2one();

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        // Same From / Different To
                        // Get
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = toAnother;

                        mark();
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Equal(toAnother, from.I1I34one2one);
                        Assert.Equal(toAnother, from.I1I34one2one);
                        Assert.Equal(from, toAnother.I1WhereI1I34one2one);
                        Assert.Equal(from, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        // Exist
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = toAnother;

                        mark();
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = toAnother;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(toAnother, fromAnother.I1I34one2one);
                        Assert.Equal(toAnother, fromAnother.I1I34one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I34one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Equal(toAnother, fromAnother.I1I34one2one);
                        Assert.Equal(toAnother, fromAnother.I1I34one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I34one2one);
                        Assert.Equal(fromAnother, toAnother.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);
                        Assert.Null(toAnother.I1WhereI1I34one2one);

                        // Exist
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = toAnother;

                        mark();
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);
                        Assert.True(toAnother.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = null;

                        mark();
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);
                        Assert.False(toAnother.ExistI1WhereI1I34one2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);
                        Assert.Equal(from, to.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = to;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Equal(to, fromAnother.I1I34one2one);
                        Assert.Equal(to, fromAnother.I1I34one2one);
                        Assert.Equal(fromAnother, to.I1WhereI1I34one2one);
                        Assert.Equal(fromAnother, to.I1WhereI1I34one2one);

                        fromAnother.RemoveI1I34one2one();

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(fromAnother.I1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);
                        Assert.Null(to.I1WhereI1I34one2one);

                        // Exist
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.True(from.ExistI1I34one2one);
                        Assert.True(from.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = to;

                        mark();
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(fromAnother.ExistI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);
                        Assert.True(to.ExistI1WhereI1I34one2one);

                        fromAnother.RemoveI1I34one2one();

                        mark();
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(from.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(fromAnother.ExistI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);
                        Assert.False(to.ExistI1WhereI1I34one2one);

                        // Null
                        // Set Null
                        // Get
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);

                        // Get
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.Equal(to, from.I1I34one2one);
                        Assert.Equal(to, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.Null(from.I1I34one2one);
                        Assert.Null(from.I1I34one2one);
                    }
                }
            }
        }

        [Fact]
        public void I3_I4one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C3.Create(this.Session);
                    var fromAnother = C3.Create(this.Session);

                    var to = C4.Create(this.Session);
                    var toAnother = C4.Create(this.Session);

                    // To 1 and back
                    mark();
                    Assert.Null(from.I3I4one2one);
                    Assert.Null(to.I3WhereI3I4one2one);

                    // 1-1
                    from.I3I4one2one = to;

                    mark();
                    Assert.Equal(to, from.I3I4one2one);
                    Assert.Equal(from, to.I3WhereI3I4one2one);

                    // 0-0
                    from.RemoveI3I4one2one();
                    from.RemoveI3I4one2one();
                    from.I3I4one2one = to;
                    from.I3I4one2one = null;
                    from.I3I4one2one = null;

                    mark();
                    Assert.Null(from.I3I4one2one);
                    Assert.Null(to.I3WhereI3I4one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.I3I4one2one = to;
                    from.I3I4one2one = to;

                    mark();
                    Assert.Equal(to, from.I3I4one2one);
                    Assert.Equal(from, to.I3WhereI3I4one2one);

                    from.RemoveI3I4one2one();

                    // Same From / Different To
                    from.I3I4one2one = to;
                    from.I3I4one2one = toAnother;

                    mark();
                    Assert.Null(to.I3WhereI3I4one2one);
                    Assert.Equal(toAnother, from.I3I4one2one);
                    Assert.Equal(from, toAnother.I3WhereI3I4one2one);

                    // Different From / Different To
                    from.I3I4one2one = to;
                    fromAnother.I3I4one2one = toAnother;

                    mark();
                    Assert.Equal(to, from.I3I4one2one);
                    Assert.Equal(from, to.I3WhereI3I4one2one);
                    Assert.Equal(toAnother, fromAnother.I3I4one2one);
                    Assert.Equal(fromAnother, toAnother.I3WhereI3I4one2one);

                    // Different From / Same To
                    from.I3I4one2one = to;
                    fromAnother.I3I4one2one = to;

                    mark();
                    Assert.Null(from.I3I4one2one);
                    Assert.Equal(to, fromAnother.I3I4one2one);
                    Assert.Equal(fromAnother, to.I3WhereI3I4one2one);

                    fromAnother.RemoveI3I4one2one();

                    // Null
                    // Set Null
                    from.I3I4one2one = null;
                    mark();
                    Assert.Null(from.I3I4one2one);
                    from.I3I4one2one = to;
                    from.I3I4one2one = null;
                    mark();
                    Assert.Null(from.I3I4one2one);
                }
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
                    var c1a = C1.Create(this.Session);
                    var c1b = C1.Create(this.Session);

                    var c2a = C2.Create(this.Session);
                    var c2b = C2.Create(this.Session);

                    var c3a = C3.Create(this.Session);
                    var c3b = C3.Create(this.Session);

                    var c4a = C4.Create(this.Session);
                    var c4b = C4.Create(this.Session);

                    // Illegal Role
                    var exceptionThrown = false;
                    try
                    {
                        mark();
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1C2one2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1I2one2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1S2one2one.RelationType, c1b);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal RelationType
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1a.Strategy.SetCompositeRole(MetaC2.Instance.C1one2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC2.Instance.C2C2one2one.RelationType, c2b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1AllorsString.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1C2one2manies.RelationType, c2b);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);
                }
            }
        }
    }
}

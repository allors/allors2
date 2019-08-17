// <copyright file="Many2OneTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>

namespace Allors.Adapters
{
    using System;

    using Adapters;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class Many2OneTest : IDisposable
    {
        public static int NR_OF_RUNS = Settings.NumberOfRuns;

        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void C1_C1many2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        var from2 = C1.Create(this.Session);
                        var from3 = C1.Create(this.Session);
                        var from4 = C1.Create(this.Session);
                        var to = C1.Create(this.Session);
                        var toAnother = C1.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-1
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-2
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-3
                        from3.C1C1many2one = to;
                        from3.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-4
                        from4.C1C1many2one = to;
                        from4.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Contains(from4, to.C1sWhereC1C1many2one);
                        Assert.Contains(from4, to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from4.C1C1many2one);
                        Assert.Equal(to, from4.C1C1many2one);

                        // 1-3
                        from4.RemoveC1C1many2one();
                        from4.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-2
                        from3.RemoveC1C1many2one();
                        from3.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 1-1
                        from2.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // 0-0
                        from1.RemoveC1C1many2one();
                        from1.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from3.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);
                        Assert.Null(from4.C1C1many2one);

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-1
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-2
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-3
                        from3.C1C1many2one = to;
                        from3.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-4
                        from4.C1C1many2one = to;
                        from4.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.True(from4.ExistC1C1many2one);
                        Assert.True(from4.ExistC1C1many2one);

                        // 1-3
                        from4.RemoveC1C1many2one();
                        from4.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.True(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-2
                        from3.RemoveC1C1many2one();
                        from3.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 1-1
                        from2.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // 0-0
                        from1.RemoveC1C1many2one();
                        from1.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from3.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);
                        Assert.False(from4.ExistC1C1many2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        from1.C1C1many2one = toAnother;
                        from1.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        Assert.Contains(from1, toAnother.C1sWhereC1C1many2one);
                        Assert.Contains(from1, toAnother.C1sWhereC1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = toAnother;
                        from1.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        from2.C1C1many2one = toAnother;
                        from2.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        Assert.Contains(from2, toAnother.C1sWhereC1C1many2one);
                        Assert.Contains(from2, toAnother.C1sWhereC1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        from2.C1C1many2one = null;
                        from2.C1C1many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        from2.C1C1many2one = toAnother;
                        from2.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        from2.C1C1many2one = null;
                        from2.C1C1many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Single(to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.DoesNotContain(from2, to.C1sWhereC1C1many2one);
                        Assert.DoesNotContain(from2, to.C1sWhereC1C1many2one);
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        from1.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.False(from2.ExistC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from1.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(from2.ExistC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        from1.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Null(from2.C1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        Assert.Empty(to.C1sWhereC1C1many2one);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        Assert.Null(from1.C1C1many2one);

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        Assert.False(from1.ExistC1C1many2one);
                    }

                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        mark();
                        var from2 = C1.Create(this.Session);
                        mark();
                        var from3 = C1.Create(this.Session);
                        mark();
                        var from4 = C1.Create(this.Session);
                        mark();
                        var to = C1.Create(this.Session);
                        mark();
                        var toAnother = C1.Create(this.Session);
                        mark();

                        // From 0-4-0
                        // Get
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-1
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-2
                        from2.C1C1many2one = to;
                        mark();
                        from2.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-3
                        from3.C1C1many2one = to;
                        mark();
                        from3.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-4
                        from4.C1C1many2one = to;
                        mark();
                        from4.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from4, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from4, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Equal(to, from4.C1C1many2one);
                        mark();
                        Assert.Equal(to, from4.C1C1many2one);
                        mark();

                        // 1-3
                        from4.RemoveC1C1many2one();
                        mark();
                        from4.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from3, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Equal(to, from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-2
                        from3.RemoveC1C1many2one();
                        mark();
                        from3.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 1-1
                        from2.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // 0-0
                        from1.RemoveC1C1many2one();
                        mark();
                        from1.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from3.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();
                        Assert.Null(from4.C1C1many2one);
                        mark();

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-1
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-2
                        from2.C1C1many2one = to;
                        mark();
                        from2.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-3
                        from3.C1C1many2one = to;
                        mark();
                        from3.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-4
                        from4.C1C1many2one = to;
                        mark();
                        from4.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.True(from4.ExistC1C1many2one);
                        mark();
                        Assert.True(from4.ExistC1C1many2one);
                        mark();

                        // 1-3
                        from4.RemoveC1C1many2one();
                        mark();
                        from4.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.True(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-2
                        from3.RemoveC1C1many2one();
                        mark();
                        from3.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 1-1
                        from2.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // 0-0
                        from1.RemoveC1C1many2one();
                        mark();
                        from1.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from3.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();
                        Assert.False(from4.ExistC1C1many2one);
                        mark();

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        mark();
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, toAnother.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        mark();
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, toAnother.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        from2.C1C1many2one = null;
                        mark();
                        from2.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(toAnother.C1sWhereC1C1many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        from2.C1C1many2one = null;
                        mark();
                        from2.C1C1many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(toAnother.ExistC1sWhereC1C1many2one);
                        mark();

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Single(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.DoesNotContain(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.DoesNotContain(from2, to.C1sWhereC1C1many2one);
                        mark();
                        from2.C1C1many2one = to;
                        mark();
                        from2.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from1, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Contains(from2, to.C1sWhereC1C1many2one);
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.False(to.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.False(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        from2.C1C1many2one = to;
                        mark();
                        from2.C1C1many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from1.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(from2.ExistC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        Assert.True(to.ExistC1sWhereC1C1many2one);
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Null(from2.C1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();
                        Assert.Empty(to.C1sWhereC1C1many2one);
                        mark();

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.C1C1many2one);
                        mark();
                        Assert.Null(from1.C1C1many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                        Assert.False(from1.ExistC1C1many2one);
                        mark();
                    }
                }
            }
        }

        [Fact]
        public void C1_C2many2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        var from2 = C1.Create(this.Session);
                        var from3 = C1.Create(this.Session);
                        var from4 = C1.Create(this.Session);
                        var to = C2.Create(this.Session);
                        var toAnother = C2.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-1
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-2
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-3
                        from3.C1C2many2one = to;
                        from3.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-4
                        from4.C1C2many2one = to;
                        from4.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(4, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Contains(from4, to.C1sWhereC1C2many2one);
                        Assert.Contains(from4, to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from4.C1C2many2one);
                        Assert.Equal(to, from4.C1C2many2one);

                        // 1-3
                        from4.RemoveC1C2many2one();
                        from4.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Contains(from3, to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-2
                        from3.RemoveC1C2many2one();
                        from3.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 1-1
                        from2.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // 0-0
                        from1.RemoveC1C2many2one();
                        from1.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from3.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);
                        Assert.Null(from4.C1C2many2one);

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-1
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-2
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-3
                        from3.C1C2many2one = to;
                        from3.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-4
                        from4.C1C2many2one = to;
                        from4.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.True(from4.ExistC1C2many2one);
                        Assert.True(from4.ExistC1C2many2one);

                        // 1-3
                        from4.RemoveC1C2many2one();
                        from4.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.True(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-2
                        from3.RemoveC1C2many2one();
                        from3.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 1-1
                        from2.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // 0-0
                        from1.RemoveC1C2many2one();
                        from1.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from3.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);
                        Assert.False(from4.ExistC1C2many2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        from1.RemoveC1C2many2one();
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);

                        // Exist
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        from1.RemoveC1C2many2one();
                        mark();
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        from1.C1C2many2one = toAnother;
                        from1.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Equal(toAnother, from1.C1C2many2one);
                        Assert.Equal(toAnother, from1.C1C2many2one);
                        Assert.Single(toAnother.C1sWhereC1C2many2one);
                        Assert.Single(toAnother.C1sWhereC1C2many2one);
                        Assert.Contains(from1, toAnother.C1sWhereC1C2many2one);
                        Assert.Contains(from1, toAnother.C1sWhereC1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);

                        // Exist
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = toAnother;
                        from1.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        from2.C1C2many2one = toAnother;
                        from2.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Equal(toAnother, from2.C1C2many2one);
                        Assert.Equal(toAnother, from2.C1C2many2one);
                        Assert.Single(toAnother.C1sWhereC1C2many2one);
                        Assert.Single(toAnother.C1sWhereC1C2many2one);
                        Assert.Contains(from2, toAnother.C1sWhereC1C2many2one);
                        Assert.Contains(from2, toAnother.C1sWhereC1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        from2.C1C2many2one = null;
                        from2.C1C2many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);
                        Assert.Empty(toAnother.C1sWhereC1C2many2one);

                        // Exist
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        from2.C1C2many2one = toAnother;
                        from2.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.True(toAnother.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        from2.C1C2many2one = null;
                        from2.C1C2many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);
                        Assert.False(toAnother.ExistC1sWhereC1C2many2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Single(to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.DoesNotContain(from2, to.C1sWhereC1C2many2one);
                        Assert.DoesNotContain(from2, to.C1sWhereC1C2many2one);
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from1, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        Assert.Contains(from2, to.C1sWhereC1C2many2one);
                        from1.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one();
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);

                        // Exist
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        Assert.False(to.ExistC1sWhereC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.False(from2.ExistC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from1.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(from2.ExistC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        Assert.True(to.ExistC1sWhereC1C2many2one);
                        from1.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one();
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Null(from2.C1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);
                        Assert.Empty(to.C1sWhereC1C2many2one);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C1C2many2one);
                        Assert.Null(from1.C1C2many2one);

                        // Exist
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC1C2many2one);
                        Assert.False(from1.ExistC1C2many2one);
                    }
                }
            }
        }

        [Fact]
        public void C3_C4many2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C3.Create(this.Session);
                        var from2 = C3.Create(this.Session);
                        var from3 = C3.Create(this.Session);
                        var from4 = C3.Create(this.Session);
                        var to = C4.Create(this.Session);
                        var toAnother = C4.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-1
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-2
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-3
                        from3.C3C4many2one = to;
                        from3.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-4
                        from4.C3C4many2one = to;
                        from4.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(4, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Contains(from4, to.C3sWhereC3C4many2one);
                        Assert.Contains(from4, to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from4.C3C4many2one);
                        Assert.Equal(to, from4.C3C4many2one);

                        // 1-3
                        from4.RemoveC3C4many2one();
                        from4.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Contains(from3, to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-2
                        from3.RemoveC3C4many2one();
                        from3.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 1-1
                        from2.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // 0-0
                        from1.RemoveC3C4many2one();
                        from1.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from3.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);
                        Assert.Null(from4.C3C4many2one);

                        // Exist
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-1
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-2
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-3
                        from3.C3C4many2one = to;
                        from3.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-4
                        from4.C3C4many2one = to;
                        from4.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.True(from4.ExistC3C4many2one);
                        Assert.True(from4.ExistC3C4many2one);

                        // 1-3
                        from4.RemoveC3C4many2one();
                        from4.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.True(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-2
                        from3.RemoveC3C4many2one();
                        from3.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 1-1
                        from2.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // 0-0
                        from1.RemoveC3C4many2one();
                        from1.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from3.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);
                        Assert.False(from4.ExistC3C4many2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        from1.RemoveC3C4many2one();
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);

                        // Exist
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        from1.RemoveC3C4many2one();
                        mark();
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        from1.C3C4many2one = toAnother;
                        from1.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Equal(toAnother, from1.C3C4many2one);
                        Assert.Equal(toAnother, from1.C3C4many2one);
                        Assert.Single(toAnother.C3sWhereC3C4many2one);
                        Assert.Single(toAnother.C3sWhereC3C4many2one);
                        Assert.Contains(from1, toAnother.C3sWhereC3C4many2one);
                        Assert.Contains(from1, toAnother.C3sWhereC3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);

                        // Exist
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = toAnother;
                        from1.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.True(toAnother.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        from2.C3C4many2one = toAnother;
                        from2.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Equal(toAnother, from2.C3C4many2one);
                        Assert.Equal(toAnother, from2.C3C4many2one);
                        Assert.Single(toAnother.C3sWhereC3C4many2one);
                        Assert.Single(toAnother.C3sWhereC3C4many2one);
                        Assert.Contains(from2, toAnother.C3sWhereC3C4many2one);
                        Assert.Contains(from2, toAnother.C3sWhereC3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        from2.C3C4many2one = null;
                        from2.C3C4many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);
                        Assert.Empty(toAnother.C3sWhereC3C4many2one);

                        // Exist
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        from2.C3C4many2one = toAnother;
                        from2.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.True(toAnother.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        from2.C3C4many2one = null;
                        from2.C3C4many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);
                        Assert.False(toAnother.ExistC3sWhereC3C4many2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Single(to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.DoesNotContain(from2, to.C3sWhereC3C4many2one);
                        Assert.DoesNotContain(from2, to.C3sWhereC3C4many2one);
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from1, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        Assert.Contains(from2, to.C3sWhereC3C4many2one);
                        from1.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one();
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);

                        // Exist
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        Assert.False(to.ExistC3sWhereC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.False(from2.ExistC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from1.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(from2.ExistC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        Assert.True(to.ExistC3sWhereC3C4many2one);
                        from1.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one();
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Null(from2.C3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);
                        Assert.Empty(to.C3sWhereC3C4many2one);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Null(from1.C3C4many2one);
                        Assert.Null(from1.C3C4many2one);

                        // Exist
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistC3C4many2one);
                        Assert.False(from1.ExistC3C4many2one);
                    }
                }
            }
        }

        [Fact]
        public void I1_I12many2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        var from2 = C1.Create(this.Session);
                        var from3 = C1.Create(this.Session);
                        var from4 = C1.Create(this.Session);
                        var to = C1.Create(this.Session);
                        var toAnother = C1.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-1
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-2
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-3
                        from3.I1I12many2one = to;
                        from3.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-4
                        from4.I1I12many2one = to;
                        from4.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Contains(from4, to.I1sWhereI1I12many2one);
                        Assert.Contains(from4, to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from4.I1I12many2one);
                        Assert.Equal(to, from4.I1I12many2one);

                        // 1-3
                        from4.RemoveI1I12many2one();
                        from4.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-2
                        from3.RemoveI1I12many2one();
                        from3.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 1-1
                        from2.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // 0-0
                        from1.RemoveI1I12many2one();
                        from1.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from3.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);
                        Assert.Null(from4.I1I12many2one);

                        // Exist
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-1
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-2
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-3
                        from3.I1I12many2one = to;
                        from3.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-4
                        from4.I1I12many2one = to;
                        from4.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.True(from4.ExistI1I12many2one);
                        Assert.True(from4.ExistI1I12many2one);

                        // 1-3
                        from4.RemoveI1I12many2one();
                        from4.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.True(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-2
                        from3.RemoveI1I12many2one();
                        from3.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 1-1
                        from2.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // 0-0
                        from1.RemoveI1I12many2one();
                        from1.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from3.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);
                        Assert.False(from4.ExistI1I12many2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        from1.I1I12many2one = toAnother;
                        from1.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        Assert.Contains(from1, toAnother.I1sWhereI1I12many2one);
                        Assert.Contains(from1, toAnother.I1sWhereI1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);

                        // Exist
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = toAnother;
                        from1.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        from2.I1I12many2one = toAnother;
                        from2.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        Assert.Contains(from2, toAnother.I1sWhereI1I12many2one);
                        Assert.Contains(from2, toAnother.I1sWhereI1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        from2.I1I12many2one = null;
                        from2.I1I12many2one = null; // Twice
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        from2.I1I12many2one = toAnother;
                        from2.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        from2.I1I12many2one = null;
                        from2.I1I12many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Single(to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.DoesNotContain(from2, to.I1sWhereI1I12many2one);
                        Assert.DoesNotContain(from2, to.I1sWhereI1I12many2one);
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        from1.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.False(from2.ExistI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from1.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(from2.ExistI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        from1.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Null(from2.I1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        Assert.Empty(to.I1sWhereI1I12many2one);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        Assert.Null(from1.I1I12many2one);

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        Assert.False(from1.ExistI1I12many2one);
                    }

                    for (var i = 0; i < NR_OF_RUNS; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        mark();
                        var from2 = C1.Create(this.Session);
                        mark();
                        var from3 = C1.Create(this.Session);
                        mark();
                        var from4 = C1.Create(this.Session);
                        mark();
                        var to = C1.Create(this.Session);
                        mark();
                        var toAnother = C1.Create(this.Session);
                        mark();

                        // From 0-4-0
                        // Get
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-1
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-2
                        from2.I1I12many2one = to;
                        mark();
                        from2.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-3
                        from3.I1I12many2one = to;
                        mark();
                        from3.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-4
                        from4.I1I12many2one = to;
                        mark();
                        from4.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from4, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from4, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Equal(to, from4.I1I12many2one);
                        mark();
                        Assert.Equal(to, from4.I1I12many2one);
                        mark();

                        // 1-3
                        from4.RemoveI1I12many2one();
                        mark();
                        from4.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from3, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Equal(to, from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-2
                        from3.RemoveI1I12many2one();
                        mark();
                        from3.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 1-1
                        from2.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // 0-0
                        from1.RemoveI1I12many2one();
                        mark();
                        from1.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from3.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();
                        Assert.Null(from4.I1I12many2one);
                        mark();

                        // Exist
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-1
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-2
                        from2.I1I12many2one = to;
                        mark();
                        from2.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-3
                        from3.I1I12many2one = to;
                        mark();
                        from3.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-4
                        from4.I1I12many2one = to;
                        mark();
                        from4.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.True(from4.ExistI1I12many2one);
                        mark();
                        Assert.True(from4.ExistI1I12many2one);
                        mark();

                        // 1-3
                        from4.RemoveI1I12many2one();
                        mark();
                        from4.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.True(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-2
                        from3.RemoveI1I12many2one();
                        mark();
                        from3.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 1-1
                        from2.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // 0-0
                        from1.RemoveI1I12many2one();
                        mark();
                        from1.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from3.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();
                        Assert.False(from4.ExistI1I12many2one);
                        mark();

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();

                        // Same From / Different To
                        // Get
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        mark();
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, toAnother.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();

                        // Exist
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();

                        // Different From / Different To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        mark();
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, toAnother.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        from2.I1I12many2one = null;
                        mark();
                        from2.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(toAnother.I1sWhereI1I12many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        from2.I1I12many2one = null;
                        mark();
                        from2.I1I12many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(toAnother.ExistI1sWhereI1I12many2one);
                        mark();

                        // Different From / Same To
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Single(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.DoesNotContain(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.DoesNotContain(from2, to.I1sWhereI1I12many2one);
                        mark();
                        from2.I1I12many2one = to;
                        mark();
                        from2.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from1, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Contains(from2, to.I1sWhereI1I12many2one);
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.False(to.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.False(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        from2.I1I12many2one = to;
                        mark();
                        from2.I1I12many2one = to;
                        mark(); // Twice
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from1.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(from2.ExistI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        Assert.True(to.ExistI1sWhereI1I12many2one);
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Null(from2.I1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();
                        Assert.Empty(to.I1sWhereI1I12many2one);
                        mark();

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Null(from1.I1I12many2one);
                        mark();
                        Assert.Null(from1.I1I12many2one);
                        mark();

                        // Exist
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                        Assert.False(from1.ExistI1I12many2one);
                        mark();
                    }
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1C2many2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1I2many2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1S2many2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC2.Instance.C1many2one.RelationType, c1b);
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
                        c1a.Strategy.SetCompositeRole(MetaC2.Instance.C2C2many2one.RelationType, c2b);
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
                        c1a.Strategy.SetCompositeRole(MetaC1.Instance.C1C2many2manies.RelationType, c2b);
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

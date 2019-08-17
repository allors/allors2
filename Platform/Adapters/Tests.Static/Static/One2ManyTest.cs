// <copyright file="One2ManyTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class One2ManyTest : IDisposable
    {
        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void C1_C1one2manies()
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

                        var to1 = C1.Create(this.Session);
                        var to2 = C1.Create(this.Session);
                        var to3 = C1.Create(this.Session);
                        var to4 = C1.Create(this.Session);

                        C1[] to1Array = { to1 };
                        C1[] to2Array = { to2 };
                        C1[] to12Array = { to1, to2 };

                        // To 0-4-0
                        mark();

                        // Get
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Null(to1.C1WhereC1C1one2many);
                        Assert.Null(to1.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-1
                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(to1); // Add Twice

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-2
                        from.AddC1C1one2many(to2);

                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-3
                        from.AddC1C1one2many(to3);

                        mark();
                        Assert.Equal(3, from.C1C1one2manies.Count);
                        Assert.Equal(3, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-4
                        from.AddC1C1one2many(to4);

                        mark();
                        Assert.Equal(4, from.C1C1one2manies.Count);
                        Assert.Equal(4, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Contains(to4, from.C1C1one2manies);
                        Assert.Contains(to4, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Equal(from, to4.C1WhereC1C1one2many);
                        Assert.Equal(from, to4.C1WhereC1C1one2many);

                        // 1-3
                        from.RemoveC1C1one2many(to4);

                        mark();
                        Assert.Equal(3, from.C1C1one2manies.Count);
                        Assert.Equal(3, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Contains(to3, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Equal(from, to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-2
                        from.RemoveC1C1one2many(to3);

                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 1-1
                        from.RemoveC1C1one2many(to2);
                        from.RemoveC1C1one2many(to2); // Delete Twice

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // 0-0
                        from.RemoveC1C1one2many(to1);
                        from.RemoveC1C1one2many(to1);
                        from.AddC1C1one2many(to1);
                        from.RemoveC1C1one2manies();
                        from.RemoveC1C1one2manies();
                        from.AddC1C1one2many(to1);
                        from.C1C1one2manies = null;
                        from.C1C1one2manies = null;
                        from.AddC1C1one2many(to1);
                        C1[] emptyArray = { };
                        from.C1C1one2manies = emptyArray;
                        from.C1C1one2manies = emptyArray;

                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Null(to1.C1WhereC1C1one2many);
                        Assert.Null(to1.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to2.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to3.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);
                        Assert.Null(to4.C1WhereC1C1one2many);

                        // Exist
                        Assert.False(from.ExistC1C1one2manies);
                        Assert.False(from.ExistC1C1one2manies);
                        Assert.False(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-1
                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(to1); // Add Twice

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-2
                        from.AddC1C1one2many(to2);

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-3
                        from.AddC1C1one2many(to3);

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-4
                        from.AddC1C1one2many(to4);

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.True(to4.ExistC1WhereC1C1one2many);
                        Assert.True(to4.ExistC1WhereC1C1one2many);

                        // 1-3
                        from.RemoveC1C1one2many(to4);

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.True(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-2
                        from.RemoveC1C1one2many(to3);

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.True(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 1-1
                        from.RemoveC1C1one2many(to2);
                        from.RemoveC1C1one2many(to2); // Delete Twice

                        mark();
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(from.ExistC1C1one2manies);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.True(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // 0-0
                        from.RemoveC1C1one2many(to1);
                        from.RemoveC1C1one2many(to1);
                        from.AddC1C1one2many(to1);
                        from.RemoveC1C1one2manies();
                        from.RemoveC1C1one2manies();
                        from.AddC1C1one2many(to1);
                        from.C1C1one2manies = null;
                        from.C1C1one2manies = null;
                        from.AddC1C1one2many(to1);
                        from.C1C1one2manies = emptyArray;
                        from.C1C1one2manies = emptyArray;

                        mark();
                        Assert.False(from.ExistC1C1one2manies);
                        Assert.False(from.ExistC1C1one2manies);
                        Assert.False(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to1.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to2.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to3.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);
                        Assert.False(to4.ExistC1WhereC1C1one2many);

                        // Multiplicity
                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(to1);

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);

                        //TODO: Replicate to other variants
                        fromAnother.RemoveC1C1one2many(to1);

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        from.C1C1one2manies = to1Array;
                        from.C1C1one2manies = to1Array;

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(to2);

                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        from.C1C1one2manies = to1Array;
                        from.C1C1one2manies = to2Array;

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Null(to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);

                        from.C1C1one2manies = to12Array;

                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        from.AddC1C1one2many(to1);
                        fromAnother.AddC1C1one2many(to1);

                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Single(fromAnother.C1C1one2manies);
                        Assert.Contains(to1, fromAnother.C1C1one2manies);
                        Assert.Equal(fromAnother, to1.C1WhereC1C1one2many);

                        fromAnother.RemoveC1C1one2manies();

                        // Replicate to others
                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(to2);
                        fromAnother.AddC1C1one2many(to1);

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Single(fromAnother.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        Assert.Contains(to1, fromAnother.C1C1one2manies);
                        Assert.Equal(from, to2.C1WhereC1C1one2many);
                        Assert.Equal(fromAnother, to1.C1WhereC1C1one2many);

                        fromAnother.RemoveC1C1one2manies();

                        from.C1C1one2manies = to1Array;
                        mark();
                        Assert.Single(from.C1C1one2manies); //TODO: Add this to others
                        fromAnother.C1C1one2manies = to1Array;
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        Assert.Single(fromAnother.C1C1one2manies);
                        Assert.Contains(to1, fromAnother.C1C1one2manies);
                        Assert.Equal(fromAnother, to1.C1WhereC1C1one2many);

                        fromAnother.RemoveC1C1one2manies();

                        from.AddC1C1one2many(to1);
                        fromAnother.AddC1C1one2many(to2);

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Single(fromAnother.C1C1one2manies);
                        Assert.Contains(to2, fromAnother.C1C1one2manies);
                        Assert.Equal(fromAnother, to2.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();
                        fromAnother.RemoveC1C1one2manies();

                        from.C1C1one2manies = to1Array;
                        fromAnother.C1C1one2manies = to2Array;

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);
                        Assert.Single(fromAnother.C1C1one2manies);
                        Assert.Contains(to2, fromAnother.C1C1one2manies);
                        Assert.Equal(fromAnother, to2.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();
                        fromAnother.RemoveC1C1one2manies();

                        // Null & Empty Array

                        // Add Null
                        from.AddC1C1one2many(null);
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        from.AddC1C1one2many(to1);
                        from.AddC1C1one2many(null);
                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        // Delete Null
                        from.RemoveC1C1one2many(null);
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        from.AddC1C1one2many(to1);
                        from.RemoveC1C1one2many(null);
                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Equal(from, to1.C1WhereC1C1one2many);

                        from.RemoveC1C1one2manies();

                        // Set Null
                        from.C1C1one2manies = null;
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        from.AddC1C1one2many(to1);
                        from.C1C1one2manies = null;
                        mark();
                        Assert.Empty(from.C1C1one2manies);

                        // Set Empty Array
                        from.C1C1one2manies = new C1[] { null };
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        from.C1C1one2manies = new C1[0];
                        mark();
                        Assert.Empty(from.C1C1one2manies);

                        // Set Array with only a null
                        from.C1C1one2manies = new C1[1];
                        mark();
                        Assert.Empty(from.C1C1one2manies);
                        from.C1C1one2manies = new C1[1];
                        mark();
                        Assert.Empty(from.C1C1one2manies);

                        // Set Array with a null in the front
                        from.C1C1one2manies = new C1[] { null, to1, to2 };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        from.C1C1one2manies = new C1[] { null, to1, to2 };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);

                        // Set Array with a null in the middle
                        from.C1C1one2manies = new C1[] { to1, null, to2 };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        from.C1C1one2manies = new C1[] { to1, null, to2 };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);

                        // Set Array with a null in the back
                        from.C1C1one2manies = new C1[] { to1, to2, null };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);
                        from.C1C1one2manies = new C1[] { to1, to2, null };
                        mark();
                        Assert.Equal(2, from.C1C1one2manies.Count);
                        Assert.Contains(to1, from.C1C1one2manies);
                        Assert.Contains(to2, from.C1C1one2manies);

                        // Remove and Add
                        from = C1.Create(this.Session);
                        var to = C1.Create(this.Session);

                        from.AddC1C1one2many(to);

                        this.Session.Commit();

                        from.RemoveC1C1one2many(to);
                        from.AddC1C1one2many(to);

                        this.Session.Commit();

                        // Add and Remove
                        from = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);
                        to2 = C1.Create(this.Session);

                        from.AddC1C1one2many(to1);

                        this.Session.Commit();

                        from.AddC1C1one2many(to2);
                        from.RemoveC1C1one2many(to2);

                        this.Session.Commit();

                        // From - Middle - To
                        from = C1.Create(this.Session);
                        var middle = C1.Create(this.Session);
                        to = C1.Create(this.Session);

                        from.AddC1C1one2many(middle);
                        middle.AddC1C1one2many(to);
                        from.AddC1C1one2many(to);

                        mark();
                        Assert.True(middle.ExistC1WhereC1C1one2many);
                        Assert.False(middle.ExistC1C1one2manies);

                        // Very Big Array
                        var bigArray = C1.Create(this.Session, Settings.LargeArraySize);
                        from.C1C1one2manies = bigArray;
                        C1[] getBigArray = from.C1C1one2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            Assert.Contains(bigArrayObject, objects);
                        }

                        from = C1.Create(this.Session);

                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2manies.RelationType, to1Array);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2manies.RelationType, to1Array);

                        mark();
                        Assert.Single(from.C1C1one2manies);
                        Assert.Contains(to1Array[0], from.C1C1one2manies);
                        Assert.Equal(from, to1Array[0].C1WhereC1C1one2many);

                        // Extent.ToArray()
                        from = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from.AddC1C1one2many(to1);

                        mark();
                        Assert.Single(from.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType).ToArray());
                        Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1one2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray()
                        from = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from.AddC1C1one2many(to1);

                        mark();
                        Assert.Single(from.C1C1one2manies.ToArray());
                        Assert.Equal(to1, from.C1C1one2manies.ToArray()[0]);
                    }
                }
            }
        }

        [Fact]
        public void C1_C2one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to1 = C2.Create(this.Session);
                    var to2 = C2.Create(this.Session);
                    var to3 = C2.Create(this.Session);
                    var to4 = C2.Create(this.Session);

                    mark();

                    // To 0-4-0
                    Assert.Empty(from.C1C2one2manies);
                    Assert.Null(to1.C1WhereC1C2one2many);
                    Assert.Null(to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-1
                    from.AddC1C2one2many(to1);
                    from.AddC1C2one2many(to1); // Add Twice

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Null(to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-2
                    from.AddC1C2one2many(to2);

                    mark();

                    Assert.Equal(2, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-3
                    from.AddC1C2one2many(to3);

                    mark();

                    Assert.Equal(3, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Contains(to3, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);
                    Assert.Equal(from, to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-4
                    from.AddC1C2one2many(to4);

                    mark();

                    Assert.Equal(4, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Contains(to3, from.C1C2one2manies);
                    Assert.Contains(to4, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);
                    Assert.Equal(from, to3.C1WhereC1C2one2many);
                    Assert.Equal(from, to4.C1WhereC1C2one2many);

                    // 1-3
                    from.RemoveC1C2one2many(to4);

                    mark();

                    Assert.Equal(3, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Contains(to3, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);
                    Assert.Equal(from, to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-2
                    from.RemoveC1C2one2many(to3);

                    mark();

                    Assert.Equal(2, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 1-1
                    from.RemoveC1C2one2many(to2);
                    from.RemoveC1C2one2many(to2); // Delete Twice

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Null(to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // 0-0
                    from.RemoveC1C2one2many(to1);

                    mark();

                    Assert.Empty(from.C1C2one2manies);
                    Assert.Null(to1.C1WhereC1C2one2many);
                    Assert.Null(to2.C1WhereC1C2one2many);
                    Assert.Null(to3.C1WhereC1C2one2many);
                    Assert.Null(to4.C1WhereC1C2one2many);

                    // Multiplicity
                    C2[] to1Array = { to1 };
                    C2[] to2Array = { to2 };
                    C2[] to12Array = { to1, to2 };

                    from.AddC1C2one2many(to1);
                    from.AddC1C2one2many(to1);

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    from.C1C2one2manies = to1Array;
                    from.C1C2one2manies = to1Array;

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    from.AddC1C2one2many(to1);
                    from.AddC1C2one2many(to2);

                    mark();

                    Assert.Equal(2, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    from.C1C2one2manies = to1Array;
                    from.C1C2one2manies = to2Array;

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Null(to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);

                    from.C1C2one2manies = to12Array;

                    mark();

                    Assert.Equal(2, from.C1C2one2manies.Count);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Contains(to2, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Equal(from, to2.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    from.AddC1C2one2many(to1);
                    fromAnother.AddC1C2one2many(to1);

                    mark();

                    Assert.Empty(from.C1C2one2manies);
                    Assert.Single(fromAnother.C1C2one2manies);
                    Assert.Contains(to1, fromAnother.C1C2one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1C2one2many);

                    fromAnother.RemoveC1C2one2manies();

                    from.C1C2one2manies = to1Array;
                    fromAnother.C1C2one2manies = to1Array;

                    mark();

                    Assert.Empty(from.C1C2one2manies);
                    Assert.Single(fromAnother.C1C2one2manies);
                    Assert.Contains(to1, fromAnother.C1C2one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1C2one2many);

                    fromAnother.RemoveC1C2one2manies();

                    from.AddC1C2one2many(to1);
                    fromAnother.AddC1C2one2many(to2);

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Single(fromAnother.C1C2one2manies);
                    Assert.Contains(to2, fromAnother.C1C2one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();
                    fromAnother.RemoveC1C2one2manies();

                    from.C1C2one2manies = to1Array;
                    fromAnother.C1C2one2manies = to2Array;

                    mark();

                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);
                    Assert.Single(fromAnother.C1C2one2manies);
                    Assert.Contains(to2, fromAnother.C1C2one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();
                    fromAnother.RemoveC1C2one2manies();

                    // Null & Empty Array

                    // Add Null
                    from.AddC1C2one2many(null);
                    mark();
                    Assert.Empty(from.C1C2one2manies);
                    from.AddC1C2one2many(to1);
                    from.AddC1C2one2many(null);
                    mark();
                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    // Delete Null
                    from.RemoveC1C2one2many(null);
                    mark();
                    Assert.Empty(from.C1C2one2manies);
                    from.AddC1C2one2many(to1);
                    from.RemoveC1C2one2many(null);
                    mark();
                    Assert.Single(from.C1C2one2manies);
                    Assert.Contains(to1, from.C1C2one2manies);
                    Assert.Equal(from, to1.C1WhereC1C2one2many);

                    from.RemoveC1C2one2manies();

                    // Set Null
                    from.C1C2one2manies = null;
                    mark();
                    Assert.Empty(from.C1C2one2manies);
                    from.AddC1C2one2many(to1);
                    from.C1C2one2manies = null;
                    mark();
                    Assert.Empty(from.C1C2one2manies);

                    // Set Empty Array
                    from.C1C2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.C1C2one2manies);
                    from.AddC1C2one2many(to1);
                    from.C1C2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.C1C2one2manies);

                    // Very Big Array
                    var bigArray = C2.Create(this.Session, Settings.LargeArraySize);
                    from.C1C2one2manies = bigArray;
                    C2[] getBigArray = from.C1C2one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // Extent.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddC1C2one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaC1.Instance.C1C2one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaC1.Instance.C1C2one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddC1C2one2many(to1);

                    mark();
                    Assert.Single(from.C1C2one2manies.ToArray());
                    Assert.Single(from.C1C2one2manies.ToArray());
                    Assert.Equal(to1, from.C1C2one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void C1_I1one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to1 = C1.Create(this.Session);
                    var to2 = C1.Create(this.Session);
                    var to3 = C1.Create(this.Session);
                    var to4 = C1.Create(this.Session);

                    mark();

                    // To 0-4-0
                    Assert.Empty(from.C1I1one2manies);
                    Assert.Null(to1.C1WhereC1I1one2many);
                    Assert.Null(to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-1
                    from.AddC1I1one2many(to1);
                    from.AddC1I1one2many(to1); // Add Twice

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Null(to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-2
                    from.AddC1I1one2many(to2);

                    mark();

                    Assert.Equal(2, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-3
                    from.AddC1I1one2many(to3);

                    mark();

                    Assert.Equal(3, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Contains(to3, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);
                    Assert.Equal(from, to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-4
                    from.AddC1I1one2many(to4);

                    mark();

                    Assert.Equal(4, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Contains(to3, from.C1I1one2manies);
                    Assert.Contains(to4, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);
                    Assert.Equal(from, to3.C1WhereC1I1one2many);
                    Assert.Equal(from, to4.C1WhereC1I1one2many);

                    // 1-3
                    from.RemoveC1I1one2many(to4);

                    mark();

                    Assert.Equal(3, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Contains(to3, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);
                    Assert.Equal(from, to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-2
                    from.RemoveC1I1one2many(to3);

                    mark();

                    Assert.Equal(2, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 1-1
                    from.RemoveC1I1one2many(to2);
                    from.RemoveC1I1one2many(to2); // Delete Twice

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Null(to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // 0-0
                    from.RemoveC1I1one2many(to1);

                    mark();

                    Assert.Empty(from.C1I1one2manies);
                    Assert.Null(to1.C1WhereC1I1one2many);
                    Assert.Null(to2.C1WhereC1I1one2many);
                    Assert.Null(to3.C1WhereC1I1one2many);
                    Assert.Null(to4.C1WhereC1I1one2many);

                    // Multiplicity
                    C1[] to1Array = { to1 };
                    C1[] to2Array = { to2 };
                    C1[] to12Array = { to1, to2 };

                    from.AddC1I1one2many(to1);
                    from.AddC1I1one2many(to1);

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    from.C1I1one2manies = to1Array;
                    from.C1I1one2manies = to1Array;

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    from.AddC1I1one2many(to1);
                    from.AddC1I1one2many(to2);

                    mark();

                    Assert.Equal(2, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    from.C1I1one2manies = to1Array;
                    from.C1I1one2manies = to2Array;

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Null(to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);

                    from.C1I1one2manies = to12Array;

                    mark();

                    Assert.Equal(2, from.C1I1one2manies.Count);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Contains(to2, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Equal(from, to2.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    from.AddC1I1one2many(to1);
                    fromAnother.AddC1I1one2many(to1);

                    mark();

                    Assert.Empty(from.C1I1one2manies);
                    Assert.Single(fromAnother.C1I1one2manies);
                    Assert.Contains(to1, fromAnother.C1I1one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1I1one2many);

                    fromAnother.RemoveC1I1one2manies();

                    from.C1I1one2manies = to1Array;
                    fromAnother.C1I1one2manies = to1Array;

                    mark();

                    Assert.Empty(from.C1I1one2manies);
                    Assert.Single(fromAnother.C1I1one2manies);
                    Assert.Contains(to1, fromAnother.C1I1one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1I1one2many);

                    fromAnother.RemoveC1I1one2manies();

                    from.AddC1I1one2many(to1);
                    fromAnother.AddC1I1one2many(to2);

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Single(fromAnother.C1I1one2manies);
                    Assert.Contains(to2, fromAnother.C1I1one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();
                    fromAnother.RemoveC1I1one2manies();

                    from.C1I1one2manies = to1Array;
                    fromAnother.C1I1one2manies = to2Array;

                    mark();

                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);
                    Assert.Single(fromAnother.C1I1one2manies);
                    Assert.Contains(to2, fromAnother.C1I1one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();
                    fromAnother.RemoveC1I1one2manies();

                    // Null & Empty Array

                    // Add Null
                    from.AddC1I1one2many(null);
                    mark();
                    Assert.Empty(from.C1I1one2manies);
                    from.AddC1I1one2many(to1);
                    from.AddC1I1one2many(null);
                    mark();
                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    // Delete Null
                    from.RemoveC1I1one2many(null);
                    mark();
                    Assert.Empty(from.C1I1one2manies);
                    from.AddC1I1one2many(to1);
                    from.RemoveC1I1one2many(null);
                    mark();
                    Assert.Single(from.C1I1one2manies);
                    Assert.Contains(to1, from.C1I1one2manies);
                    Assert.Equal(from, to1.C1WhereC1I1one2many);

                    from.RemoveC1I1one2manies();

                    // Set Null
                    from.C1I1one2manies = null;
                    mark();
                    Assert.Empty(from.C1I1one2manies);
                    from.AddC1I1one2many(to1);
                    from.C1I1one2manies = null;
                    mark();
                    Assert.Empty(from.C1I1one2manies);

                    // Set Empty Array
                    from.C1I1one2manies = new C1[0];
                    mark();
                    Assert.Empty(from.C1I1one2manies);
                    from.AddC1I1one2many(to1);
                    from.C1I1one2manies = new C1[0];
                    mark();
                    Assert.Empty(from.C1I1one2manies);

                    // Very Big Array
                    var bigArray = C1.Create(Session, Settings.LargeArraySize);
                    from.C1I1one2manies = bigArray;
                    I1[] getBigArray = from.C1I1one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        mark();
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // Extent.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C1.Create(this.Session);

                    from.AddC1I1one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaC1.Instance.C1I1one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaC1.Instance.C1I1one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C1.Create(this.Session);

                    from.AddC1I1one2many(to1);

                    mark();
                    Assert.Single(from.C1I1one2manies.ToArray());
                    Assert.Single(from.C1I1one2manies.ToArray());
                    Assert.Equal(to1, from.C1I1one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void C1_I2one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to1 = C2.Create(this.Session);
                    var to2 = C2.Create(this.Session);
                    var to3 = C2.Create(this.Session);
                    var to4 = C2.Create(this.Session);

                    // To 0-4-0
                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    Assert.Null(to1.C1WhereC1I2one2many);
                    Assert.Null(to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-1
                    from.AddC1I2one2many(to1);
                    from.AddC1I2one2many(to1); // Add Twice

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Null(to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-2
                    from.AddC1I2one2many(to2);

                    mark();
                    Assert.Equal(2, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-3
                    from.AddC1I2one2many(to3);

                    mark();
                    Assert.Equal(3, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Contains(to3, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);
                    Assert.Equal(from, to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-4
                    from.AddC1I2one2many(to4);

                    mark();
                    Assert.Equal(4, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Contains(to3, from.C1I2one2manies);
                    Assert.Contains(to4, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);
                    Assert.Equal(from, to3.C1WhereC1I2one2many);
                    Assert.Equal(from, to4.C1WhereC1I2one2many);

                    // 1-3
                    from.RemoveC1I2one2many(to4);

                    mark();
                    Assert.Equal(3, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Contains(to3, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);
                    Assert.Equal(from, to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-2
                    from.RemoveC1I2one2many(to3);

                    mark();
                    Assert.Equal(2, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 1-1
                    from.RemoveC1I2one2many(to2);
                    from.RemoveC1I2one2many(to2); // Delete Twice

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Null(to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // 0-0
                    from.RemoveC1I2one2many(to1);

                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    Assert.Null(to1.C1WhereC1I2one2many);
                    Assert.Null(to2.C1WhereC1I2one2many);
                    Assert.Null(to3.C1WhereC1I2one2many);
                    Assert.Null(to4.C1WhereC1I2one2many);

                    // Multiplicity
                    C2[] to1Array = { to1 };
                    C2[] to2Array = { to2 };
                    C2[] to12Array = { to1, to2 };

                    from.AddC1I2one2many(to1);
                    from.AddC1I2one2many(to1);

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    from.C1I2one2manies = to1Array;
                    from.C1I2one2manies = to1Array;

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    from.AddC1I2one2many(to1);
                    from.AddC1I2one2many(to2);

                    mark();
                    Assert.Equal(2, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    from.C1I2one2manies = to1Array;
                    from.C1I2one2manies = to2Array;

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Null(to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);

                    from.C1I2one2manies = to12Array;

                    mark();
                    Assert.Equal(2, from.C1I2one2manies.Count);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Contains(to2, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Equal(from, to2.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    from.AddC1I2one2many(to1);
                    fromAnother.AddC1I2one2many(to1);

                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    Assert.Single(fromAnother.C1I2one2manies);
                    Assert.Contains(to1, fromAnother.C1I2one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1I2one2many);

                    fromAnother.RemoveC1I2one2manies();

                    from.C1I2one2manies = to1Array;
                    fromAnother.C1I2one2manies = to1Array;

                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    Assert.Single(fromAnother.C1I2one2manies);
                    Assert.Contains(to1, fromAnother.C1I2one2manies);
                    Assert.Equal(fromAnother, to1.C1WhereC1I2one2many);

                    fromAnother.RemoveC1I2one2manies();

                    from.AddC1I2one2many(to1);
                    fromAnother.AddC1I2one2many(to2);

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Single(fromAnother.C1I2one2manies);
                    Assert.Contains(to2, fromAnother.C1I2one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();
                    fromAnother.RemoveC1I2one2manies();

                    from.C1I2one2manies = to1Array;
                    fromAnother.C1I2one2manies = to2Array;

                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);
                    Assert.Single(fromAnother.C1I2one2manies);
                    Assert.Contains(to2, fromAnother.C1I2one2manies);
                    Assert.Equal(fromAnother, to2.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();
                    fromAnother.RemoveC1I2one2manies();

                    // Null & Empty Array

                    // Add Null
                    from.AddC1I2one2many(null);
                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    from.AddC1I2one2many(to1);
                    from.AddC1I2one2many(null);
                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    // Delete Null
                    from.RemoveC1I2one2many(null);
                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    from.AddC1I2one2many(to1);
                    from.RemoveC1I2one2many(null);
                    mark();
                    Assert.Single(from.C1I2one2manies);
                    Assert.Contains(to1, from.C1I2one2manies);
                    Assert.Equal(from, to1.C1WhereC1I2one2many);

                    from.RemoveC1I2one2manies();

                    // Set Null
                    from.C1I2one2manies = null;
                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    from.AddC1I2one2many(to1);
                    from.C1I2one2manies = null;
                    mark();
                    Assert.Empty(from.C1I2one2manies);

                    // Set Empty Array
                    from.C1I2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.C1I2one2manies);
                    from.AddC1I2one2many(to1);
                    from.C1I2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.C1I2one2manies);

                    // Very Big Array
                    var bigArray = C2.Create(Session, Settings.LargeArraySize);
                    from.C1I2one2manies = bigArray;
                    I2[] getBigArray = from.C1I2one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        mark();
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // Extent.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddC1I2one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaC1.Instance.C1I2one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaC1.Instance.C1I2one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddC1I2one2many(to1);

                    mark();
                    Assert.Single(from.C1I2one2manies.ToArray());
                    Assert.Single(from.C1I2one2manies.ToArray());
                    Assert.Equal(to1, from.C1I2one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void C3_C4one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C3.Create(this.Session);
                    var fromAnother = C3.Create(this.Session);

                    var to1 = C4.Create(this.Session);
                    var to2 = C4.Create(this.Session);
                    var to3 = C4.Create(this.Session);
                    var to4 = C4.Create(this.Session);

                    // To 0-4-0
                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    Assert.Null(to1.C3WhereC3C4one2many);
                    Assert.Null(to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-1
                    from.AddC3C4one2many(to1);
                    from.AddC3C4one2many(to1); // Add Twice

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Null(to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-2
                    from.AddC3C4one2many(to2);

                    mark();
                    Assert.Equal(2, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-3
                    from.AddC3C4one2many(to3);

                    mark();
                    Assert.Equal(3, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Contains(to3, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);
                    Assert.Equal(from, to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-4
                    from.AddC3C4one2many(to4);

                    mark();
                    Assert.Equal(4, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Contains(to3, from.C3C4one2manies);
                    Assert.Contains(to4, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);
                    Assert.Equal(from, to3.C3WhereC3C4one2many);
                    Assert.Equal(from, to4.C3WhereC3C4one2many);

                    // 1-3
                    from.RemoveC3C4one2many(to4);

                    mark();
                    Assert.Equal(3, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Contains(to3, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);
                    Assert.Equal(from, to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-2
                    from.RemoveC3C4one2many(to3);

                    mark();
                    Assert.Equal(2, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 1-1
                    from.RemoveC3C4one2many(to2);
                    from.RemoveC3C4one2many(to2); // Delete Twice

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Null(to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // 0-0
                    from.RemoveC3C4one2many(to1);

                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    Assert.Null(to1.C3WhereC3C4one2many);
                    Assert.Null(to2.C3WhereC3C4one2many);
                    Assert.Null(to3.C3WhereC3C4one2many);
                    Assert.Null(to4.C3WhereC3C4one2many);

                    // Multiplicity
                    C4[] to1Array = { to1 };
                    C4[] to2Array = { to2 };
                    C4[] to12Array = { to1, to2 };

                    from.AddC3C4one2many(to1);
                    from.AddC3C4one2many(to1);

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    from.C3C4one2manies = to1Array;
                    from.C3C4one2manies = to1Array;

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    from.AddC3C4one2many(to1);
                    from.AddC3C4one2many(to2);

                    mark();
                    Assert.Equal(2, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    from.C3C4one2manies = to1Array;
                    from.C3C4one2manies = to2Array;

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Null(to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);

                    from.C3C4one2manies = to12Array;

                    mark();
                    Assert.Equal(2, from.C3C4one2manies.Count);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Contains(to2, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Equal(from, to2.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    from.AddC3C4one2many(to1);
                    fromAnother.AddC3C4one2many(to1);

                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    Assert.Single(fromAnother.C3C4one2manies);
                    Assert.Contains(to1, fromAnother.C3C4one2manies);
                    Assert.Equal(fromAnother, to1.C3WhereC3C4one2many);

                    fromAnother.RemoveC3C4one2manies();

                    from.C3C4one2manies = to1Array;
                    fromAnother.C3C4one2manies = to1Array;

                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    Assert.Single(fromAnother.C3C4one2manies);
                    Assert.Contains(to1, fromAnother.C3C4one2manies);
                    Assert.Equal(fromAnother, to1.C3WhereC3C4one2many);

                    fromAnother.RemoveC3C4one2manies();

                    from.AddC3C4one2many(to1);
                    fromAnother.AddC3C4one2many(to2);

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Single(fromAnother.C3C4one2manies);
                    Assert.Contains(to2, fromAnother.C3C4one2manies);
                    Assert.Equal(fromAnother, to2.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();
                    fromAnother.RemoveC3C4one2manies();

                    from.C3C4one2manies = to1Array;
                    fromAnother.C3C4one2manies = to2Array;

                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);
                    Assert.Single(fromAnother.C3C4one2manies);
                    Assert.Contains(to2, fromAnother.C3C4one2manies);
                    Assert.Equal(fromAnother, to2.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();
                    fromAnother.RemoveC3C4one2manies();

                    // Null & Empty Array
                    // Add Null
                    from.AddC3C4one2many(null);
                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    from.AddC3C4one2many(to1);
                    from.AddC3C4one2many(null);
                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    // Delete Null
                    from.RemoveC3C4one2many(null);
                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    from.AddC3C4one2many(to1);
                    from.RemoveC3C4one2many(null);
                    mark();
                    Assert.Single(from.C3C4one2manies);
                    Assert.Contains(to1, from.C3C4one2manies);
                    Assert.Equal(from, to1.C3WhereC3C4one2many);

                    from.RemoveC3C4one2manies();

                    // Set Null
                    from.C3C4one2manies = null;
                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    from.AddC3C4one2many(to1);
                    from.C3C4one2manies = null;
                    mark();
                    Assert.Empty(from.C3C4one2manies);

                    // Set Empty Array
                    from.C3C4one2manies = new C4[0];
                    mark();
                    Assert.Empty(from.C3C4one2manies);
                    from.AddC3C4one2many(to1);
                    from.C3C4one2manies = new C4[0];
                    mark();
                    Assert.Empty(from.C3C4one2manies);

                    // Very Big Array
                    var bigArray = C4.Create(Session, Settings.LargeArraySize);
                    from.C3C4one2manies = bigArray;
                    C4[] getBigArray = from.C3C4one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        mark();
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // Extent.ToArray()
                    from = C3.Create(this.Session);
                    to1 = C4.Create(this.Session);

                    from.AddC3C4one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaC3.Instance.C3C4one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaC3.Instance.C3C4one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C3.Create(this.Session);
                    to1 = C4.Create(this.Session);

                    from.AddC3C4one2many(to1);

                    mark();
                    Assert.Single(from.C3C4one2manies.ToArray());
                    Assert.Single(from.C3C4one2manies.ToArray());
                    Assert.Equal(to1, from.C3C4one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void I1_I12one2manies()
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

                        var to1 = C1.Create(this.Session);
                        var to2 = C1.Create(this.Session);
                        var to3 = C2.Create(this.Session);
                        var to4 = C2.Create(this.Session);

                        // To 0-4-0
                        // Get
                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Null(to1.I1WhereI1I12one2many);
                        Assert.Null(to1.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-1
                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(to1); // Add Twice

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-2
                        from.AddI1I12one2many(to2);

                        mark();
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-3
                        from.AddI1I12one2many(to3);

                        mark();
                        Assert.Equal(3, from.I1I12one2manies.Count);
                        Assert.Equal(3, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-4
                        from.AddI1I12one2many(to4);

                        mark();
                        Assert.Equal(4, from.I1I12one2manies.Count);
                        Assert.Equal(4, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Contains(to4, from.I1I12one2manies);
                        Assert.Contains(to4, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Equal(from, to4.I1WhereI1I12one2many);
                        Assert.Equal(from, to4.I1WhereI1I12one2many);

                        // 1-3
                        from.RemoveI1I12one2many(to4);

                        mark();
                        Assert.Equal(3, from.I1I12one2manies.Count);
                        Assert.Equal(3, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Contains(to3, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Equal(from, to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-2
                        from.RemoveI1I12one2many(to3);

                        mark();
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 1-1
                        from.RemoveI1I12one2many(to2);
                        from.RemoveI1I12one2many(to2); // Delete Twice

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // 0-0
                        from.RemoveI1I12one2many(to1);
                        from.RemoveI1I12one2many(to1);
                        from.AddI1I12one2many(to1);
                        from.RemoveI1I12one2manies();
                        from.RemoveI1I12one2manies();
                        from.AddI1I12one2many(to1);
                        from.I1I12one2manies = null;
                        from.I1I12one2manies = null;
                        from.AddI1I12one2many(to1);
                        C2[] emptyArray = { };
                        from.I1I12one2manies = emptyArray;
                        from.I1I12one2manies = emptyArray;

                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Null(to1.I1WhereI1I12one2many);
                        Assert.Null(to1.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to2.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to3.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);
                        Assert.Null(to4.I1WhereI1I12one2many);

                        // Exist
                        Assert.False(from.ExistI1I12one2manies);
                        Assert.False(from.ExistI1I12one2manies);
                        Assert.False(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-1
                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(to1); // Add Twice

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-2
                        from.AddI1I12one2many(to2);

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-3
                        from.AddI1I12one2many(to3);

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-4
                        from.AddI1I12one2many(to4);

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.True(to4.ExistI1WhereI1I12one2many);
                        Assert.True(to4.ExistI1WhereI1I12one2many);

                        // 1-3
                        from.RemoveI1I12one2many(to4);

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.True(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-2
                        from.RemoveI1I12one2many(to3);

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.True(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 1-1
                        from.RemoveI1I12one2many(to2);
                        from.RemoveI1I12one2many(to2); // Delete Twice

                        mark();
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(from.ExistI1I12one2manies);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.True(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // 0-0
                        from.RemoveI1I12one2many(to1);
                        from.RemoveI1I12one2many(to1);
                        from.AddI1I12one2many(to1);
                        from.RemoveI1I12one2manies();
                        from.RemoveI1I12one2manies();
                        from.AddI1I12one2many(to1);
                        from.I1I12one2manies = null;
                        from.I1I12one2manies = null;
                        from.AddI1I12one2many(to1);
                        from.I1I12one2manies = emptyArray;
                        from.I1I12one2manies = emptyArray;

                        mark();
                        Assert.False(from.ExistI1I12one2manies);
                        Assert.False(from.ExistI1I12one2manies);
                        Assert.False(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to1.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to2.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to3.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);
                        Assert.False(to4.ExistI1WhereI1I12one2many);

                        // Multiplicity
                        C1[] to1Array = { to1 };
                        C1[] to2Array = { to2 };
                        C1[] to12Array = { to1, to2 };

                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(to1);

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);

                        // TODO: Replicate to other variants
                        fromAnother.RemoveI1I12one2many(to1);

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        from.I1I12one2manies = to1Array;
                        from.I1I12one2manies = to1Array;

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(to2);

                        mark();
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        from.I1I12one2manies = to1Array;
                        from.I1I12one2manies = to2Array;

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Null(to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);

                        from.I1I12one2manies = to12Array;

                        mark();
                        Assert.Equal(2, from.I1I12one2manies.Count);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        from.AddI1I12one2many(to1);
                        fromAnother.AddI1I12one2many(to1);

                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Single(fromAnother.I1I12one2manies);
                        Assert.Contains(to1, fromAnother.I1I12one2manies);
                        Assert.Equal(fromAnother, to1.I1WhereI1I12one2many);

                        fromAnother.RemoveI1I12one2manies();

                        // Replicate to others
                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(to2);
                        fromAnother.AddI1I12one2many(to1);

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Single(fromAnother.I1I12one2manies);
                        Assert.Contains(to2, from.I1I12one2manies);
                        Assert.Contains(to1, fromAnother.I1I12one2manies);
                        Assert.Equal(from, to2.I1WhereI1I12one2many);
                        Assert.Equal(fromAnother, to1.I1WhereI1I12one2many);

                        fromAnother.RemoveI1I12one2manies();

                        from.I1I12one2manies = to1Array;
                        fromAnother.I1I12one2manies = to1Array;

                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        Assert.Single(fromAnother.I1I12one2manies);
                        Assert.Contains(to1, fromAnother.I1I12one2manies);
                        Assert.Equal(fromAnother, to1.I1WhereI1I12one2many);

                        fromAnother.RemoveI1I12one2manies();

                        from.AddI1I12one2many(to1);
                        fromAnother.AddI1I12one2many(to2);

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Single(fromAnother.I1I12one2manies);
                        Assert.Contains(to2, fromAnother.I1I12one2manies);
                        Assert.Equal(fromAnother, to2.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();
                        fromAnother.RemoveI1I12one2manies();

                        from.I1I12one2manies = to1Array;
                        fromAnother.I1I12one2manies = to2Array;

                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);
                        Assert.Single(fromAnother.I1I12one2manies);
                        Assert.Contains(to2, fromAnother.I1I12one2manies);
                        Assert.Equal(fromAnother, to2.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();
                        fromAnother.RemoveI1I12one2manies();

                        // Null & Empty Array
                        // Add Null
                        from.AddI1I12one2many(null);
                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        from.AddI1I12one2many(to1);
                        from.AddI1I12one2many(null);
                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        // Delete Null
                        from.RemoveI1I12one2many(null);
                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        from.AddI1I12one2many(to1);
                        from.RemoveI1I12one2many(null);
                        mark();
                        Assert.Single(from.I1I12one2manies);
                        Assert.Contains(to1, from.I1I12one2manies);
                        Assert.Equal(from, to1.I1WhereI1I12one2many);

                        from.RemoveI1I12one2manies();

                        // Set Null
                        from.I1I12one2manies = null;
                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        from.AddI1I12one2many(to1);
                        from.I1I12one2manies = null;
                        mark();
                        Assert.Empty(from.I1I12one2manies);

                        // Set Empty Array
                        from.I1I12one2manies = new C1[0];
                        mark();
                        Assert.Empty(from.I1I12one2manies);
                        from.AddI1I12one2many(to1);
                        from.I1I12one2manies = new C2[0];
                        mark();
                        Assert.Empty(from.I1I12one2manies);

                        // Very Big Array
                        var bigArray = C2.Create(this.Session, Settings.LargeArraySize);
                        from.I1I12one2manies = bigArray;
                        I12[] getBigArray = from.I1I12one2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            mark();
                            Assert.Contains(bigArrayObject, objects);
                        }

                        // Extent.ToArray() I12->C1
                        from = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from.AddI1I12one2many(to1);

                        mark();
                        Assert.Single(from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12one2manies.RelationType).ToArray());
                        Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12one2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray() I12->C1
                        from = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from.AddI1I12one2many(to1);

                        mark();
                        Assert.Single(from.I1I12one2manies.ToArray());
                        Assert.Equal(to1, from.I1I12one2manies.ToArray()[0]);

                        // Extent.ToArray() I12->C2
                        from = C1.Create(this.Session);
                        to3 = C2.Create(this.Session);

                        from.AddI1I12one2many(to3);

                        mark();
                        Assert.Single(from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12one2manies.RelationType).ToArray());
                        Assert.Equal(to3, from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12one2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray() I12->C2
                        from = C1.Create(this.Session);
                        to3 = C2.Create(this.Session);

                        from.AddI1I12one2many(to3);

                        mark();
                        Assert.Single(from.I1I12one2manies.ToArray());
                        Assert.Equal(to3, from.I1I12one2manies.ToArray()[0]);
                    }
                }
            }
        }

        [Fact]
        public void I1_I1one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to1 = C1.Create(this.Session);
                    var to2 = C1.Create(this.Session);
                    var to3 = C1.Create(this.Session);
                    var to4 = C1.Create(this.Session);

                    // To 0-4-0
                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    Assert.Null(to1.I1WhereI1I1one2many);
                    Assert.Null(to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-1
                    from.AddI1I1one2many(to1);
                    from.AddI1I1one2many(to1); // Add Twice

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Null(to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-2
                    from.AddI1I1one2many(to2);

                    mark();
                    Assert.Equal(2, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-3
                    from.AddI1I1one2many(to3);

                    mark();
                    Assert.Equal(3, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Contains(to3, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);
                    Assert.Equal(from, to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-4
                    from.AddI1I1one2many(to4);

                    mark();
                    Assert.Equal(4, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Contains(to3, from.I1I1one2manies);
                    Assert.Contains(to4, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);
                    Assert.Equal(from, to3.I1WhereI1I1one2many);
                    Assert.Equal(from, to4.I1WhereI1I1one2many);

                    // 1-3
                    from.RemoveI1I1one2many(to4);

                    mark();
                    Assert.Equal(3, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Contains(to3, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);
                    Assert.Equal(from, to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-2
                    from.RemoveI1I1one2many(to3);

                    mark();
                    Assert.Equal(2, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 1-1
                    from.RemoveI1I1one2many(to2);
                    from.RemoveI1I1one2many(to2); // Delete Twice

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Null(to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // 0-0
                    from.RemoveI1I1one2many(to1);

                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    Assert.Null(to1.I1WhereI1I1one2many);
                    Assert.Null(to2.I1WhereI1I1one2many);
                    Assert.Null(to3.I1WhereI1I1one2many);
                    Assert.Null(to4.I1WhereI1I1one2many);

                    // Multiplicity
                    C1[] to1Array = { to1 };
                    C1[] to2Array = { to2 };
                    C1[] to12Array = { to1, to2 };

                    from.AddI1I1one2many(to1);
                    from.AddI1I1one2many(to1);

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    from.I1I1one2manies = to1Array;
                    from.I1I1one2manies = to1Array;

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    from.AddI1I1one2many(to1);
                    from.AddI1I1one2many(to2);

                    mark();
                    Assert.Equal(2, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    from.I1I1one2manies = to1Array;
                    from.I1I1one2manies = to2Array;

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Null(to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);

                    from.I1I1one2manies = to12Array;

                    mark();
                    Assert.Equal(2, from.I1I1one2manies.Count);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Contains(to2, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Equal(from, to2.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    from.AddI1I1one2many(to1);
                    fromAnother.AddI1I1one2many(to1);

                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    Assert.Single(fromAnother.I1I1one2manies);
                    Assert.Contains(to1, fromAnother.I1I1one2manies);
                    Assert.Equal(fromAnother, to1.I1WhereI1I1one2many);

                    fromAnother.RemoveI1I1one2manies();

                    from.I1I1one2manies = to1Array;
                    fromAnother.I1I1one2manies = to1Array;

                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    Assert.Single(fromAnother.I1I1one2manies);
                    Assert.Contains(to1, fromAnother.I1I1one2manies);
                    Assert.Equal(fromAnother, to1.I1WhereI1I1one2many);

                    fromAnother.RemoveI1I1one2manies();

                    from.AddI1I1one2many(to1);
                    fromAnother.AddI1I1one2many(to2);

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Single(fromAnother.I1I1one2manies);
                    Assert.Contains(to2, fromAnother.I1I1one2manies);
                    Assert.Equal(fromAnother, to2.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();
                    fromAnother.RemoveI1I1one2manies();

                    from.I1I1one2manies = to1Array;
                    fromAnother.I1I1one2manies = to2Array;

                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);
                    Assert.Single(fromAnother.I1I1one2manies);
                    Assert.Contains(to2, fromAnother.I1I1one2manies);
                    Assert.Equal(fromAnother, to2.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();
                    fromAnother.RemoveI1I1one2manies();

                    // Null & Empty Array
                    // Add Null
                    from.AddI1I1one2many(null);
                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    from.AddI1I1one2many(to1);
                    from.AddI1I1one2many(null);
                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    // Delete Null
                    from.RemoveI1I1one2many(null);
                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    from.AddI1I1one2many(to1);
                    from.RemoveI1I1one2many(null);
                    mark();
                    Assert.Single(from.I1I1one2manies);
                    Assert.Contains(to1, from.I1I1one2manies);
                    Assert.Equal(from, to1.I1WhereI1I1one2many);

                    from.RemoveI1I1one2manies();

                    // Set Null
                    from.I1I1one2manies = null;
                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    from.AddI1I1one2many(to1);
                    from.I1I1one2manies = null;
                    mark();
                    Assert.Empty(from.I1I1one2manies);

                    // Set Empty Array
                    from.I1I1one2manies = new C1[0];
                    mark();
                    Assert.Empty(from.I1I1one2manies);
                    from.AddI1I1one2many(to1);
                    from.I1I1one2manies = new C1[0];
                    mark();
                    Assert.Empty(from.I1I1one2manies);

                    // Very Big Array
                    var bigArray = C1.Create(Session, Settings.LargeArraySize);
                    from.I1I1one2manies = bigArray;
                    I1[] getBigArray = from.I1I1one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        mark();
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // From - Middle - To
                    from = C1.Create(this.Session);
                    var middle = C1.Create(this.Session);
                    var to = C1.Create(this.Session);

                    from.AddI1I1one2many(middle);
                    middle.AddI1I1one2many(to);
                    from.AddI1I1one2many(to);

                    mark();
                    Assert.True(middle.ExistI1WhereI1I1one2many);
                    Assert.False(middle.ExistI1I1one2manies);

                    // Extent.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C1.Create(this.Session);

                    from.AddI1I1one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I1one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I1one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C1.Create(this.Session);

                    from.AddI1I1one2many(to1);

                    mark();
                    Assert.Single(from.I1I1one2manies.ToArray());
                    Assert.Equal(to1, from.I1I1one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void I1_I2one2manies()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(this.Session);
                    var fromAnother = C1.Create(this.Session);

                    var to1 = C2.Create(this.Session);
                    var to2 = C2.Create(this.Session);
                    var to3 = C2.Create(this.Session);
                    var to4 = C2.Create(this.Session);

                    // To 0-4-0
                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    Assert.Null(to1.I1WhereI1I2one2many);
                    Assert.Null(to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-1
                    from.AddI1I2one2many(to1);
                    from.AddI1I2one2many(to1); // Add Twice

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Null(to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-2
                    from.AddI1I2one2many(to2);

                    mark();
                    Assert.Equal(2, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-3
                    from.AddI1I2one2many(to3);

                    mark();
                    Assert.Equal(3, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Contains(to3, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);
                    Assert.Equal(from, to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-4
                    from.AddI1I2one2many(to4);

                    mark();
                    Assert.Equal(4, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Contains(to3, from.I1I2one2manies);
                    Assert.Contains(to4, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);
                    Assert.Equal(from, to3.I1WhereI1I2one2many);
                    Assert.Equal(from, to4.I1WhereI1I2one2many);

                    // 1-3
                    from.RemoveI1I2one2many(to4);

                    mark();
                    Assert.Equal(3, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Contains(to3, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);
                    Assert.Equal(from, to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-2
                    from.RemoveI1I2one2many(to3);

                    mark();
                    Assert.Equal(2, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 1-1
                    from.RemoveI1I2one2many(to2);
                    from.RemoveI1I2one2many(to2); // Delete Twice

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Null(to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // 0-0
                    from.RemoveI1I2one2many(to1);

                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    Assert.Null(to1.I1WhereI1I2one2many);
                    Assert.Null(to2.I1WhereI1I2one2many);
                    Assert.Null(to3.I1WhereI1I2one2many);
                    Assert.Null(to4.I1WhereI1I2one2many);

                    // Multiplicity
                    C2[] to1Array = { to1 };
                    C2[] to2Array = { to2 };
                    C2[] to12Array = { to1, to2 };

                    from.AddI1I2one2many(to1);
                    from.AddI1I2one2many(to1);

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    from.I1I2one2manies = to1Array;
                    from.I1I2one2manies = to1Array;

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    from.AddI1I2one2many(to1);
                    from.AddI1I2one2many(to2);

                    mark();
                    Assert.Equal(2, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    from.I1I2one2manies = to1Array;
                    from.I1I2one2manies = to2Array;

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Null(to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);

                    from.I1I2one2manies = to12Array;

                    mark();
                    Assert.Equal(2, from.I1I2one2manies.Count);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Contains(to2, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Equal(from, to2.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    from.AddI1I2one2many(to1);
                    fromAnother.AddI1I2one2many(to1);

                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    Assert.Single(fromAnother.I1I2one2manies);
                    Assert.Contains(to1, fromAnother.I1I2one2manies);
                    Assert.Equal(fromAnother, to1.I1WhereI1I2one2many);

                    fromAnother.RemoveI1I2one2manies();

                    from.I1I2one2manies = to1Array;
                    fromAnother.I1I2one2manies = to1Array;

                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    Assert.Single(fromAnother.I1I2one2manies);
                    Assert.Contains(to1, fromAnother.I1I2one2manies);
                    Assert.Equal(fromAnother, to1.I1WhereI1I2one2many);

                    fromAnother.RemoveI1I2one2manies();

                    from.AddI1I2one2many(to1);
                    fromAnother.AddI1I2one2many(to2);

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Single(fromAnother.I1I2one2manies);
                    Assert.Contains(to2, fromAnother.I1I2one2manies);
                    Assert.Equal(fromAnother, to2.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();
                    fromAnother.RemoveI1I2one2manies();

                    from.I1I2one2manies = to1Array;
                    fromAnother.I1I2one2manies = to2Array;

                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);
                    Assert.Single(fromAnother.I1I2one2manies);
                    Assert.Contains(to2, fromAnother.I1I2one2manies);
                    Assert.Equal(fromAnother, to2.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();
                    fromAnother.RemoveI1I2one2manies();

                    // Null & Empty Array
                    // Add Null
                    from.AddI1I2one2many(null);
                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    from.AddI1I2one2many(to1);
                    from.AddI1I2one2many(null);
                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    // Delete Null
                    from.RemoveI1I2one2many(null);
                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    from.AddI1I2one2many(to1);
                    from.RemoveI1I2one2many(null);
                    mark();
                    Assert.Single(from.I1I2one2manies);
                    Assert.Contains(to1, from.I1I2one2manies);
                    Assert.Equal(from, to1.I1WhereI1I2one2many);

                    from.RemoveI1I2one2manies();

                    // Set Null
                    from.I1I2one2manies = null;
                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    from.AddI1I2one2many(to1);
                    from.I1I2one2manies = null;
                    mark();
                    Assert.Empty(from.I1I2one2manies);

                    // Set Empty Array
                    from.I1I2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.I1I2one2manies);
                    from.AddI1I2one2many(to1);
                    from.I1I2one2manies = new C2[0];
                    mark();
                    Assert.Empty(from.I1I2one2manies);

                    // Very Big Array
                    var bigArray = C2.Create(Session, Settings.LargeArraySize);
                    from.I1I2one2manies = bigArray;
                    I2[] getBigArray = from.I1I2one2manies;

                    mark();
                    Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                    var objects = new HashSet<IObject>(getBigArray);
                    foreach (var bigArrayObject in bigArray)
                    {
                        mark();
                        Assert.Contains(bigArrayObject, objects);
                    }

                    // Extent.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddI1I2one2many(to1);

                    mark();
                    Assert.Single(from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I2one2manies.RelationType).ToArray());
                    Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I2one2manies.RelationType).ToArray()[0]);

                    // Extent<T>.ToArray()
                    from = C1.Create(this.Session);
                    to1 = C2.Create(this.Session);

                    from.AddI1I2one2many(to1);

                    mark();
                    Assert.Single(from.I1I2one2manies.ToArray());
                    Assert.Single(from.I1I2one2manies.ToArray());
                    Assert.Equal(to1, from.I1I2one2manies.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void I1_I34one2manies()
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

                        var to1 = C3.Create(this.Session);
                        var to2 = C3.Create(this.Session);
                        var to3 = C4.Create(this.Session);
                        var to4 = C4.Create(this.Session);

                        // To 0-4-0
                        // Get
                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Null(to1.I1WhereI1I34one2many);
                        Assert.Null(to1.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-1
                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(to1); // Add Twice

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-2
                        from.AddI1I34one2many(to2);

                        mark();
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-3
                        from.AddI1I34one2many(to3);

                        mark();
                        Assert.Equal(3, from.I1I34one2manies.Count);
                        Assert.Equal(3, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-4
                        from.AddI1I34one2many(to4);

                        mark();
                        Assert.Equal(4, from.I1I34one2manies.Count);
                        Assert.Equal(4, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Contains(to4, from.I1I34one2manies);
                        Assert.Contains(to4, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Equal(from, to4.I1WhereI1I34one2many);
                        Assert.Equal(from, to4.I1WhereI1I34one2many);

                        // 1-3
                        from.RemoveI1I34one2many(to4);

                        mark();
                        Assert.Equal(3, from.I1I34one2manies.Count);
                        Assert.Equal(3, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Contains(to3, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Equal(from, to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-2
                        from.RemoveI1I34one2many(to3);

                        mark();
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 1-1
                        from.RemoveI1I34one2many(to2);
                        from.RemoveI1I34one2many(to2); // Delete Twice

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // 0-0
                        from.RemoveI1I34one2many(to1);
                        from.RemoveI1I34one2many(to1);
                        from.AddI1I34one2many(to1);
                        from.RemoveI1I34one2manies();
                        from.RemoveI1I34one2manies();
                        from.AddI1I34one2many(to1);
                        from.I1I34one2manies = null;
                        from.I1I34one2manies = null;
                        from.AddI1I34one2many(to1);
                        C3[] emptyArray = { };
                        from.I1I34one2manies = emptyArray;
                        from.I1I34one2manies = emptyArray;

                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Null(to1.I1WhereI1I34one2many);
                        Assert.Null(to1.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to2.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to3.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);
                        Assert.Null(to4.I1WhereI1I34one2many);

                        // Exist
                        mark();
                        Assert.False(from.ExistI1I34one2manies);
                        Assert.False(from.ExistI1I34one2manies);
                        Assert.False(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-1
                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(to1); // Add Twice

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-2
                        from.AddI1I34one2many(to2);

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-3
                        from.AddI1I34one2many(to3);

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-4
                        from.AddI1I34one2many(to4);

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.True(to4.ExistI1WhereI1I34one2many);
                        Assert.True(to4.ExistI1WhereI1I34one2many);

                        // 1-3
                        from.RemoveI1I34one2many(to4);

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.True(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-2
                        from.RemoveI1I34one2many(to3);

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.True(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 1-1
                        from.RemoveI1I34one2many(to2);
                        from.RemoveI1I34one2many(to2); // Delete Twice

                        mark();
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(from.ExistI1I34one2manies);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.True(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // 0-0
                        from.RemoveI1I34one2many(to1);
                        from.RemoveI1I34one2many(to1);
                        from.AddI1I34one2many(to1);
                        from.RemoveI1I34one2manies();
                        from.RemoveI1I34one2manies();
                        from.AddI1I34one2many(to1);
                        from.I1I34one2manies = null;
                        from.I1I34one2manies = null;
                        from.AddI1I34one2many(to1);
                        from.I1I34one2manies = emptyArray;
                        from.I1I34one2manies = emptyArray;

                        mark();
                        Assert.False(from.ExistI1I34one2manies);
                        Assert.False(from.ExistI1I34one2manies);
                        Assert.False(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to1.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to2.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to3.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);
                        Assert.False(to4.ExistI1WhereI1I34one2many);

                        // Multiplicity
                        C3[] to1Array = { to1 };
                        C3[] to2Array = { to2 };
                        C3[] to12Array = { to1, to2 };

                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(to1);

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);

                        // TODO: Replicate to other variants
                        fromAnother.RemoveI1I34one2many(to1);

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        from.I1I34one2manies = to1Array;
                        from.I1I34one2manies = to1Array;

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(to2);

                        mark();
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        from.I1I34one2manies = to1Array;
                        from.I1I34one2manies = to2Array;

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Null(to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);

                        from.I1I34one2manies = to12Array;

                        mark();
                        Assert.Equal(2, from.I1I34one2manies.Count);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        from.AddI1I34one2many(to1);
                        fromAnother.AddI1I34one2many(to1);

                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Single(fromAnother.I1I34one2manies);
                        Assert.Contains(to1, fromAnother.I1I34one2manies);
                        Assert.Equal(fromAnother, to1.I1WhereI1I34one2many);

                        fromAnother.RemoveI1I34one2manies();

                        // Replicate to others
                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(to2);
                        fromAnother.AddI1I34one2many(to1);

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Single(fromAnother.I1I34one2manies);
                        Assert.Contains(to2, from.I1I34one2manies);
                        Assert.Contains(to1, fromAnother.I1I34one2manies);
                        Assert.Equal(from, to2.I1WhereI1I34one2many);
                        Assert.Equal(fromAnother, to1.I1WhereI1I34one2many);

                        fromAnother.RemoveI1I34one2manies();

                        from.I1I34one2manies = to1Array;
                        fromAnother.I1I34one2manies = to1Array;

                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        Assert.Single(fromAnother.I1I34one2manies);
                        Assert.Contains(to1, fromAnother.I1I34one2manies);
                        Assert.Equal(fromAnother, to1.I1WhereI1I34one2many);

                        fromAnother.RemoveI1I34one2manies();

                        from.AddI1I34one2many(to1);
                        fromAnother.AddI1I34one2many(to2);

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Single(fromAnother.I1I34one2manies);
                        Assert.Contains(to2, fromAnother.I1I34one2manies);
                        Assert.Equal(fromAnother, to2.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();
                        fromAnother.RemoveI1I34one2manies();

                        from.I1I34one2manies = to1Array;
                        fromAnother.I1I34one2manies = to2Array;

                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);
                        Assert.Single(fromAnother.I1I34one2manies);
                        Assert.Contains(to2, fromAnother.I1I34one2manies);
                        Assert.Equal(fromAnother, to2.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();
                        fromAnother.RemoveI1I34one2manies();

                        // Null & Empty Array
                        // Add Null
                        from.AddI1I34one2many(null);
                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        from.AddI1I34one2many(to1);
                        from.AddI1I34one2many(null);
                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        // Delete Null
                        from.RemoveI1I34one2many(null);
                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        from.AddI1I34one2many(to1);
                        from.RemoveI1I34one2many(null);
                        mark();
                        Assert.Single(from.I1I34one2manies);
                        Assert.Contains(to1, from.I1I34one2manies);
                        Assert.Equal(from, to1.I1WhereI1I34one2many);

                        from.RemoveI1I34one2manies();

                        // Set Null
                        from.I1I34one2manies = null;
                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        from.AddI1I34one2many(to1);
                        from.I1I34one2manies = null;
                        mark();
                        Assert.Empty(from.I1I34one2manies);

                        // Set Empty Array
                        from.I1I34one2manies = new C3[0];
                        mark();
                        Assert.Empty(from.I1I34one2manies);
                        from.AddI1I34one2many(to1);
                        from.I1I34one2manies = new C4[0];
                        mark();
                        Assert.Empty(from.I1I34one2manies);

                        // Very Big Array
                        var bigArray = C4.Create(this.Session, Settings.LargeArraySize);
                        from.I1I34one2manies = bigArray;
                        I34[] getBigArray = from.I1I34one2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            mark();
                            Assert.Contains(bigArrayObject, objects);
                        }

                        // Extent.ToArray()
                        from = C1.Create(this.Session);
                        to1 = C3.Create(this.Session);

                        from.AddI1I34one2many(to1);

                        mark();
                        Assert.Single(from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I34one2manies.RelationType).ToArray());
                        Assert.Equal(to1, from.Strategy.GetCompositeRoles(MetaI1.Instance.I1I34one2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray()
                        from = C1.Create(this.Session);
                        to1 = C3.Create(this.Session);

                        from.AddI1I34one2many(to1);

                        mark();
                        Assert.Single(from.I1I34one2manies.ToArray());
                        Assert.Single(from.I1I34one2manies.ToArray());
                        Assert.Equal(to1, from.I1I34one2manies.ToArray()[0]);
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
                    var c1A = C1.Create(this.Session);
                    var c1B = C1.Create(this.Session);
                    C1[] c1Bs = { c1B };

                    var c2A = C2.Create(this.Session);
                    C2[] c2As = { c2A };

                    // Illegal role
                    // Class
                    var exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2one2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1C2one2manies.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2one2manies.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Interface
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1I2one2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1I2one2manies.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1I2one2manies.RelationType, c1B);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Superinterface
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1S2one2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1S2one2manies.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1S2one2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC2.Instance.C1one2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC2.Instance.C1one2manies.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC2.Instance.C1one2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC2.Instance.C1one2manies.RelationType, c2A);
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
                        c1A.Strategy.SetCompositeRoles(MetaC2.Instance.C1one2manies.RelationType, c2As);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC2.Instance.C1one2manies.RelationType, c2A);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal Role IObjectType
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1AllorsString.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1AllorsString.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1AllorsString.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1AllorsDecimal.RelationType, c2A);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1AllorsDecimal.RelationType, c2As);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1AllorsDecimal.RelationType, c2A);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    // Illegal Role Multiplicity
                    exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2one2one.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC2.Instance.C1one2one.RelationType, c1Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2one2one.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2one2one.RelationType, c2A);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1C2one2one.RelationType, c2As);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2one2one.RelationType, c2A);
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

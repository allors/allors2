// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Many2ManyTest.cs" company="Allors bvba">
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

    using Allors.Domain;
    using Allors.Meta;
    using Adapters;

    using Xunit;

    public abstract class Many2ManyTest
    {
        protected abstract IProfile Profile { get; }

        protected ISession Session
        {
            get
            {
                return this.Profile.Session;
            }
        }

        protected Action[] Markers
        {
            get
            {
                return this.Profile.Markers;
            }
        }

        protected Action[] Inits
        {
            get
            {
                return this.Profile.Inits;
            }
        }

        [Fact]
        public void C1_C1many2many()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < Settings.NumberOfRuns; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        var from2 = C1.Create(this.Session);
                        var from3 = C1.Create(this.Session);
                        var from4 = C1.Create(this.Session);

                        var to1 = C1.Create(this.Session);
                        var to2 = C1.Create(this.Session);
                        var to3 = C1.Create(this.Session);
                        var to4 = C1.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        var tuttefrut = to1.Strategy.IsDeleted;
                        mark();
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-1
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-2
                        from2.AddC1C1many2many(to1);
                        from2.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-3
                        from3.AddC1C1many2many(to1);
                        from3.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-4
                        from4.AddC1C1many2many(to1);
                        from4.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(4, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(4, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from4));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from4));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(to1, from4.C1C1many2manies[0]);
                        Assert.Equal(to1, from4.C1C1many2manies[0]);

                        // 1-3
                        from4.RemoveC1C1many2many(to1);
                        from4.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from3));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(to1, from3.C1C1many2manies[0]);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-2
                        from3.RemoveC1C1many2many(to1);
                        from3.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(to1, from2.C1C1many2manies[0]);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 1-1
                        from2.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(to1, from1.C1C1many2manies[0]);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // 0-0        
                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from3.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);
                        Assert.Equal(0, from4.C1C1many2manies.Count);

                        // Exist
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-1
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-2
                        from2.AddC1C1many2many(to1);
                        from2.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-3
                        from3.AddC1C1many2many(to1);
                        from3.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-4
                        from4.AddC1C1many2many(to1);
                        from4.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(4, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(4, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.True(from4.ExistC1C1many2manies);
                        Assert.True(from4.ExistC1C1many2manies);

                        // 1-3
                        from4.RemoveC1C1many2many(to1);
                        from4.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(3, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.True(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-2
                        from3.RemoveC1C1many2many(to1);
                        from3.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 1-1
                        from2.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // 0-0        
                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from3.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);
                        Assert.False(from4.ExistC1C1many2manies);

                        // To 0-4-0
                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-1
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-2
                        from1.AddC1C1many2many(to2);
                        from1.AddC1C1many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-3
                        from1.AddC1C1many2many(to3);
                        from1.AddC1C1many2many(to3);

                        mark();
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-4
                        from1.AddC1C1many2many(to4);
                        from1.AddC1C1many2many(to4);

                        mark();
                        Assert.Equal(4, from1.C1C1many2manies.Count);
                        Assert.Equal(4, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.True(from1.C1C1many2manies.Contains(to4));
                        Assert.True(from1.C1C1many2manies.Contains(to4));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to4.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to4.C1sWhereC1C1many2many[0]);

                        // 1-3
                        from1.RemoveC1C1many2many(to4);
                        from1.RemoveC1C1many2many(to4);

                        mark();
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.True(from1.C1C1many2manies.Contains(to3));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to3.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-2
                        from1.RemoveC1C1many2many(to3);
                        from1.RemoveC1C1many2many(to3);

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to2.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 1-1
                        from1.RemoveC1C1many2many(to2);
                        from1.RemoveC1C1many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(from1, to1.C1sWhereC1C1many2many[0]);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // 0-0        
                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to3.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to4.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-1
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-2
                        from1.AddC1C1many2many(to2);
                        from1.AddC1C1many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-3
                        from1.AddC1C1many2many(to3);
                        from1.AddC1C1many2many(to3);

                        mark();
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-4
                        from1.AddC1C1many2many(to4);
                        from1.AddC1C1many2many(to4);

                        mark();
                        Assert.Equal(4, from1.C1C1many2manies.Count);
                        Assert.Equal(4, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.True(to4.ExistC1sWhereC1C1many2many);
                        Assert.True(to4.ExistC1sWhereC1C1many2many);

                        // 1-3
                        from1.RemoveC1C1many2many(to4);
                        from1.RemoveC1C1many2many(to4);

                        mark();
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.Equal(3, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.True(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-2
                        from1.RemoveC1C1many2many(to3);
                        from1.RemoveC1C1many2many(to3);

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 1-1
                        from1.RemoveC1C1many2many(to2);
                        from1.RemoveC1C1many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // 0-0        
                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to3.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);
                        Assert.False(to4.ExistC1sWhereC1C1many2many);

                        // Multiplicity
                        C1[] to1Array = { to1 };
                        C1[] to2Array = { to2 };
                        C1[] to12Array = { to1, to2 };

                        // Get
                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2many(to1);
                        from1.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);
                        from2.RemoveC1C1many2many(to1);

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.False(from1.C1C1many2manies.Contains(to2));
                        Assert.False(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to2);
                        from1.AddC1C1many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to2);
                        from1.AddC1C1many2many(to2);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.False(from1.C1C1many2manies.Contains(to2));
                        Assert.False(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.C1C1many2manies = to2Array;
                        from1.C1C1many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.False(from1.C1C1many2manies.Contains(to1));
                        Assert.False(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));

                        from1.C1C1many2manies = to12Array;

                        mark();
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.Equal(2, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.True(from1.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to2Array;
                        from1.C1C1many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to12Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from2.AddC1C1many2many(to1);
                        from2.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.True(from2.C1C1many2manies.Contains(to1));
                        Assert.True(from2.C1C1many2manies.Contains(to1));
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from2.AddC1C1many2many(to1);
                        from2.AddC1C1many2many(to1);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from2.C1C1many2manies = to1Array;
                        from2.C1C1many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.True(from2.C1C1many2manies.Contains(to1));
                        Assert.True(from2.C1C1many2manies.Contains(to1));
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(2, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from2));

                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from2.C1C1many2manies = to1Array;
                        from2.C1C1many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from2.AddC1C1many2many(to2);
                        from2.AddC1C1many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.True(from2.C1C1many2manies.Contains(to2));
                        Assert.True(from2.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from2));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from2.AddC1C1many2many(to2);
                        from2.AddC1C1many2many(to2);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        from2.C1C1many2manies = to2Array;
                        from2.C1C1many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.Equal(1, from2.C1C1many2manies.Count);
                        Assert.True(from2.C1C1many2manies.Contains(to2));
                        Assert.True(from2.C1C1many2manies.Contains(to2));
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to2.C1sWhereC1C1many2many.Count);
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from2));
                        Assert.True(to2.C1sWhereC1C1many2many.Contains(from2));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, from2.C1C1many2manies.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to2.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from1.C1C1many2manies = to1Array;
                        from1.C1C1many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        from2.C1C1many2manies = to2Array;
                        from2.C1C1many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(from2.ExistC1C1many2manies);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);
                        Assert.True(to2.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();
                        from2.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(from2.ExistC1C1many2manies);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);
                        Assert.False(to2.ExistC1sWhereC1C1many2many);

                        // Null & Empty Array
                        // Add Null
                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many((C1)null);
                        from1.AddC1C1many2many((C1)null);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many((C1)null);
                        from1.AddC1C1many2many((C1)null);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many((C1)null);
                        from1.AddC1C1many2many((C1)null);

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many((C1)null);
                        from1.AddC1C1many2many((C1)null);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Delete Null
                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.RemoveC1C1many2many((C1)null);
                        from1.RemoveC1C1many2many((C1)null);

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.RemoveC1C1many2many((C1)null);
                        from1.RemoveC1C1many2many((C1)null);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.Equal(1, from1.C1C1many2manies.Count);
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.True(from1.C1C1many2manies.Contains(to1));
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(1, to1.C1sWhereC1C1many2many.Count);
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));
                        Assert.True(to1.C1sWhereC1C1many2many.Contains(from1));

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);
                        Assert.Equal(0, to1.C1sWhereC1C1many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2many((C1)null);
                        from1.RemoveC1C1many2many((C1)null);

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.RemoveC1C1many2many((C1)null);
                        from1.RemoveC1C1many2many((C1)null);

                        mark();
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(from1.ExistC1C1many2manies);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);
                        Assert.True(to1.ExistC1sWhereC1C1many2many);

                        from1.RemoveC1C1many2manies();
                        from1.RemoveC1C1many2manies();

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);
                        Assert.False(to1.ExistC1sWhereC1C1many2many);

                        // Set Null
                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        from1.C1C1many2manies = null;
                        from1.C1C1many2manies = null;

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.C1C1many2manies = null;
                        from1.C1C1many2manies = null;

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        from1.C1C1many2manies = null;
                        from1.C1C1many2manies = null;

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.C1C1many2manies = null;
                        from1.C1C1many2manies = null;

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        // Set Empty Array
                        // Get
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        from1.C1C1many2manies = new C1[0];
                        from1.C1C1many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.C1C1many2manies = new C1[0];
                        from1.C1C1many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.C1C1many2manies.Count);
                        Assert.Equal(0, from1.C1C1many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        from1.C1C1many2manies = new C1[0];
                        from1.C1C1many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        from1.AddC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);
                        from1.C1C1many2manies = new C1[0];
                        from1.C1C1many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistC1C1many2manies);
                        Assert.False(from1.ExistC1C1many2manies);

                        // Remove and Add
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from1.AddC1C1many2many(to1);

                        this.Session.Commit();

                        from1.RemoveC1C1many2many(to1);
                        from1.AddC1C1many2many(to1);

                        this.Session.Commit();

                        // Add and Remove
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);
                        to2 = C1.Create(this.Session);

                        from1.AddC1C1many2many(to1);

                        this.Session.Commit();

                        from1.AddC1C1many2many(to2);
                        from1.RemoveC1C1many2many(to2);

                        this.Session.Commit();

                        // Very Big Array
                        var bigArray = C1.Create(this.Session, Settings.LargeArraySize);
                        from1.C1C1many2manies = bigArray;
                        C1[] getBigArray = from1.C1C1many2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            Assert.True(objects.Contains(bigArrayObject));
                        }

                        // Extent.ToArray()
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1many2manies.RelationType).ToArray().Length);
                        Assert.Equal(to1, from1.Strategy.GetCompositeRoles(MetaC1.Instance.C1C1many2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray()
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from1.AddC1C1many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C1C1many2manies.ToArray().Length);
                        Assert.Equal(to1, from1.C1C1many2manies.ToArray()[0]);

                        // Rollback
                        // TODO: Add to Rollback to other tests
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);
                        to2 = C1.Create(this.Session);
                        
                        from1.AddC1C1many2many(to1);

                        Assert.Contains(to1, from1.C1C1many2manies);

                        this.Session.Commit();

                        Assert.Contains(to1, from1.C1C1many2manies);

                        this.Session.Commit();

                        Assert.Contains(to1, from1.C1C1many2manies);

                        this.Session.Commit();

                        from1.RemoveC1C1many2many(to1);
                       
                        this.Session.Rollback();

                        Assert.Contains(to1, from1.C1C1many2manies);

                        this.Session.Rollback();

                        from1.RemoveC1C1many2many(to1);
                        from1.AddC1C1many2many(to2);

                        Assert.Contains(to2, from1.C1C1many2manies);

                        this.Session.Rollback();

                        Assert.Contains(to1, from1.C1C1many2manies);
                    }
                }
            }
        }

        [Fact]
        public void I1_I12many2many()
        {
                       foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < Settings.NumberOfRuns; i++)
                    {
                        var from1 = C1.Create(this.Session);
                        var from2 = C1.Create(this.Session);
                        var from3 = C1.Create(this.Session);
                        var from4 = C1.Create(this.Session);

                        var to1 = C1.Create(this.Session);
                        var to2 = C1.Create(this.Session);
                        var to3 = C2.Create(this.Session);
                        var to4 = C2.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        var tuttefrut = to1.Strategy.IsDeleted;
                        mark();
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-1
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-2
                        from2.AddI1I12many2many(to1);
                        from2.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-3
                        from3.AddI1I12many2many(to1);
                        from3.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-4
                        from4.AddI1I12many2many(to1);
                        from4.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(4, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(4, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from4));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from4));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(to1, from4.I1I12many2manies[0]);
                        Assert.Equal(to1, from4.I1I12many2manies[0]);

                        // 1-3
                        from4.RemoveI1I12many2many(to1);
                        from4.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from3));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(to1, from3.I1I12many2manies[0]);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-2
                        from3.RemoveI1I12many2many(to1);
                        from3.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(to1, from2.I1I12many2manies[0]);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 1-1
                        from2.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(to1, from1.I1I12many2manies[0]);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // 0-0        
                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from3.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);
                        Assert.Equal(0, from4.I1I12many2manies.Count);

                        // Exist
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-1
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-2
                        from2.AddI1I12many2many(to1);
                        from2.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-3
                        from3.AddI1I12many2many(to1);
                        from3.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-4
                        from4.AddI1I12many2many(to1);
                        from4.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(4, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(4, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.True(from4.ExistI1I12many2manies);
                        Assert.True(from4.ExistI1I12many2manies);

                        // 1-3
                        from4.RemoveI1I12many2many(to1);
                        from4.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(3, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.True(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-2
                        from3.RemoveI1I12many2many(to1);
                        from3.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 1-1
                        from2.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // 0-0        
                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from3.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);
                        Assert.False(from4.ExistI1I12many2manies);

                        // To 0-4-0
                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-1
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-2
                        from1.AddI1I12many2many(to2);
                        from1.AddI1I12many2many(to2);

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-3
                        from1.AddI1I12many2many(to3);
                        from1.AddI1I12many2many(to3);

                        mark();
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-4
                        from1.AddI1I12many2many(to4);
                        from1.AddI1I12many2many(to4);

                        mark();
                        Assert.Equal(4, from1.I1I12many2manies.Count);
                        Assert.Equal(4, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.True(from1.I1I12many2manies.Contains(to4));
                        Assert.True(from1.I1I12many2manies.Contains(to4));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to4.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to4.I1sWhereI1I12many2many[0]);

                        // 1-3
                        from1.RemoveI1I12many2many(to4);
                        from1.RemoveI1I12many2many(to4);

                        mark();
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.True(from1.I1I12many2manies.Contains(to3));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to3.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-2
                        from1.RemoveI1I12many2many(to3);
                        from1.RemoveI1I12many2many(to3);

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to2.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 1-1
                        from1.RemoveI1I12many2many(to2);
                        from1.RemoveI1I12many2many(to2);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(from1, to1.I1sWhereI1I12many2many[0]);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // 0-0        
                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to3.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to4.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-1
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-2
                        from1.AddI1I12many2many(to2);
                        from1.AddI1I12many2many(to2);

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-3
                        from1.AddI1I12many2many(to3);
                        from1.AddI1I12many2many(to3);

                        mark();
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-4
                        from1.AddI1I12many2many(to4);
                        from1.AddI1I12many2many(to4);

                        mark();
                        Assert.Equal(4, from1.I1I12many2manies.Count);
                        Assert.Equal(4, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.True(to4.ExistI1sWhereI1I12many2many);
                        Assert.True(to4.ExistI1sWhereI1I12many2many);

                        // 1-3
                        from1.RemoveI1I12many2many(to4);
                        from1.RemoveI1I12many2many(to4);

                        mark();
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.Equal(3, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.True(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-2
                        from1.RemoveI1I12many2many(to3);
                        from1.RemoveI1I12many2many(to3);

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 1-1
                        from1.RemoveI1I12many2many(to2);
                        from1.RemoveI1I12many2many(to2);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // 0-0        
                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to3.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);
                        Assert.False(to4.ExistI1sWhereI1I12many2many);

                        // Multiplicity
                        C1[] to1Array = { to1 };
                        C1[] to2Array = { to2 };
                        C1[] to12Array = { to1, to2 };

                        // Get
                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2many(to1);
                        from1.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);
                        from2.RemoveI1I12many2many(to1);

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.False(from1.I1I12many2manies.Contains(to2));
                        Assert.False(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to2);
                        from1.AddI1I12many2many(to2);

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to2);
                        from1.AddI1I12many2many(to2);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.False(from1.I1I12many2manies.Contains(to2));
                        Assert.False(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.I1I12many2manies = to2Array;
                        from1.I1I12many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.False(from1.I1I12many2manies.Contains(to1));
                        Assert.False(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));

                        from1.I1I12many2manies = to12Array;

                        mark();
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.Equal(2, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.True(from1.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to2Array;
                        from1.I1I12many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to12Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from2.AddI1I12many2many(to1);
                        from2.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.True(from2.I1I12many2manies.Contains(to1));
                        Assert.True(from2.I1I12many2manies.Contains(to1));
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from2.AddI1I12many2many(to1);
                        from2.AddI1I12many2many(to1);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from2.I1I12many2manies = to1Array;
                        from2.I1I12many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.True(from2.I1I12many2manies.Contains(to1));
                        Assert.True(from2.I1I12many2manies.Contains(to1));
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(2, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from2));

                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from2.I1I12many2manies = to1Array;
                        from2.I1I12many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from2.AddI1I12many2many(to2);
                        from2.AddI1I12many2many(to2);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.True(from2.I1I12many2manies.Contains(to2));
                        Assert.True(from2.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from2));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from2.AddI1I12many2many(to2);
                        from2.AddI1I12many2many(to2);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        from2.I1I12many2manies = to2Array;
                        from2.I1I12many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.Equal(1, from2.I1I12many2manies.Count);
                        Assert.True(from2.I1I12many2manies.Contains(to2));
                        Assert.True(from2.I1I12many2manies.Contains(to2));
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to2.I1sWhereI1I12many2many.Count);
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from2));
                        Assert.True(to2.I1sWhereI1I12many2many.Contains(from2));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, from2.I1I12many2manies.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to2.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from1.I1I12many2manies = to1Array;
                        from1.I1I12many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        from2.I1I12many2manies = to2Array;
                        from2.I1I12many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(from2.ExistI1I12many2manies);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);
                        Assert.True(to2.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();
                        from2.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(from2.ExistI1I12many2manies);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);
                        Assert.False(to2.ExistI1sWhereI1I12many2many);

                        // Null & Empty Array
                        // Add Null
                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many((I12)null);
                        from1.AddI1I12many2many((I12)null);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many((I12)null);
                        from1.AddI1I12many2many((I12)null);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many((I12)null);
                        from1.AddI1I12many2many((I12)null);

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many((I12)null);
                        from1.AddI1I12many2many((I12)null);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Delete Null
                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.RemoveI1I12many2many((I12)null);
                        from1.RemoveI1I12many2many((I12)null);

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.RemoveI1I12many2many((I12)null);
                        from1.RemoveI1I12many2many((I12)null);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.Equal(1, from1.I1I12many2manies.Count);
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.True(from1.I1I12many2manies.Contains(to1));
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(1, to1.I1sWhereI1I12many2many.Count);
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));
                        Assert.True(to1.I1sWhereI1I12many2many.Contains(from1));

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);
                        Assert.Equal(0, to1.I1sWhereI1I12many2many.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2many((I12)null);
                        from1.RemoveI1I12many2many((I12)null);

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.RemoveI1I12many2many((I12)null);
                        from1.RemoveI1I12many2many((I12)null);

                        mark();
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(from1.ExistI1I12many2manies);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);
                        Assert.True(to1.ExistI1sWhereI1I12many2many);

                        from1.RemoveI1I12many2manies();
                        from1.RemoveI1I12many2manies();

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);
                        Assert.False(to1.ExistI1sWhereI1I12many2many);

                        // Set Null
                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        from1.I1I12many2manies = null;
                        from1.I1I12many2manies = null;

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.I1I12many2manies = null;
                        from1.I1I12many2manies = null;

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        from1.I1I12many2manies = null;
                        from1.I1I12many2manies = null;

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.I1I12many2manies = null;
                        from1.I1I12many2manies = null;

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        // Set Empty Array
                        // Get
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        from1.I1I12many2manies = new C1[0];
                        from1.I1I12many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.I1I12many2manies = new C1[0];
                        from1.I1I12many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.I1I12many2manies.Count);
                        Assert.Equal(0, from1.I1I12many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        from1.I1I12many2manies = new C1[0];
                        from1.I1I12many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        from1.AddI1I12many2many(to1);
                        from1.AddI1I12many2many(to1);
                        from1.I1I12many2manies = new C1[0];
                        from1.I1I12many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistI1I12many2manies);
                        Assert.False(from1.ExistI1I12many2manies);

                        // Very Big Array
                        var bigArray = C2.Create(this.Session, Settings.LargeArraySize);
                        from1.I1I12many2manies = bigArray;
                        I12[] getBigArray = from1.I1I12many2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            mark();
                            Assert.True(objects.Contains(bigArrayObject));
                        }

                        // Extent.ToArray() I12->C1
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12many2manies.RelationType).ToArray().Length);
                        Assert.Equal(to1, from1.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12many2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray() I12->C1
                        from1 = C1.Create(this.Session);
                        to1 = C1.Create(this.Session);

                        from1.AddI1I12many2many(to1);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.ToArray().Length);
                        Assert.Equal(to1, from1.I1I12many2manies.ToArray()[0]);

                        // Extent.ToArray() I12->C2
                        from1 = C1.Create(this.Session);
                        to3 = C2.Create(this.Session);

                        from1.AddI1I12many2many(to3);

                        mark();
                        Assert.Equal(1, from1.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12many2manies.RelationType).ToArray().Length);
                        Assert.Equal(to3, from1.Strategy.GetCompositeRoles(MetaI1.Instance.I1I12many2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray() I12->C2
                        from1 = C1.Create(this.Session);
                        to3 = C2.Create(this.Session);

                        from1.AddI1I12many2many(to3);

                        mark();
                        Assert.Equal(1, from1.I1I12many2manies.ToArray().Length);
                        Assert.Equal(to3, from1.I1I12many2manies.ToArray()[0]);
                    }
                }
            }
        }

        [Fact]
        public void C3_C4many2many()
        {
                      foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var i = 0; i < Settings.NumberOfRuns; i++)
                    {
                        var from1 = C3.Create(this.Session);
                        var from2 = C3.Create(this.Session);
                        var from3 = C3.Create(this.Session);
                        var from4 = C3.Create(this.Session);

                        var to1 = C4.Create(this.Session);
                        var to2 = C4.Create(this.Session);
                        var to3 = C4.Create(this.Session);
                        var to4 = C4.Create(this.Session);

                        // From 0-4-0
                        // Get
                        mark();
                        var tuttefrut = to1.Strategy.IsDeleted;
                        mark();
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-1
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-2
                        from2.AddC3C4many2many(to1);
                        from2.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-3
                        from3.AddC3C4many2many(to1);
                        from3.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-4
                        from4.AddC3C4many2many(to1);
                        from4.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(4, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(4, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from4));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from4));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(to1, from4.C3C4many2manies[0]);
                        Assert.Equal(to1, from4.C3C4many2manies[0]);

                        // 1-3
                        from4.RemoveC3C4many2many(to1);
                        from4.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from3));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(to1, from3.C3C4many2manies[0]);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-2
                        from3.RemoveC3C4many2many(to1);
                        from3.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(to1, from2.C3C4many2manies[0]);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 1-1
                        from2.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(to1, from1.C3C4many2manies[0]);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // 0-0        
                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from3.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);
                        Assert.Equal(0, from4.C3C4many2manies.Count);

                        // Exist
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-1
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-2
                        from2.AddC3C4many2many(to1);
                        from2.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-3
                        from3.AddC3C4many2many(to1);
                        from3.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-4
                        from4.AddC3C4many2many(to1);
                        from4.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(4, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(4, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.True(from4.ExistC3C4many2manies);
                        Assert.True(from4.ExistC3C4many2manies);

                        // 1-3
                        from4.RemoveC3C4many2many(to1);
                        from4.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(3, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.True(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-2
                        from3.RemoveC3C4many2many(to1);
                        from3.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 1-1
                        from2.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // 0-0        
                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from3.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);
                        Assert.False(from4.ExistC3C4many2manies);

                        // To 0-4-0
                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-1
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-2
                        from1.AddC3C4many2many(to2);
                        from1.AddC3C4many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-3
                        from1.AddC3C4many2many(to3);
                        from1.AddC3C4many2many(to3);

                        mark();
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-4
                        from1.AddC3C4many2many(to4);
                        from1.AddC3C4many2many(to4);

                        mark();
                        Assert.Equal(4, from1.C3C4many2manies.Count);
                        Assert.Equal(4, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.True(from1.C3C4many2manies.Contains(to4));
                        Assert.True(from1.C3C4many2manies.Contains(to4));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to4.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to4.C3sWhereC3C4many2many[0]);

                        // 1-3
                        from1.RemoveC3C4many2many(to4);
                        from1.RemoveC3C4many2many(to4);

                        mark();
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.True(from1.C3C4many2manies.Contains(to3));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to3.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-2
                        from1.RemoveC3C4many2many(to3);
                        from1.RemoveC3C4many2many(to3);

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to2.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 1-1
                        from1.RemoveC3C4many2many(to2);
                        from1.RemoveC3C4many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(from1, to1.C3sWhereC3C4many2many[0]);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // 0-0        
                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to3.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to4.C3sWhereC3C4many2many.Count);

                        // Exist
                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-1
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-2
                        from1.AddC3C4many2many(to2);
                        from1.AddC3C4many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-3
                        from1.AddC3C4many2many(to3);
                        from1.AddC3C4many2many(to3);

                        mark();
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-4
                        from1.AddC3C4many2many(to4);
                        from1.AddC3C4many2many(to4);

                        mark();
                        Assert.Equal(4, from1.C3C4many2manies.Count);
                        Assert.Equal(4, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.True(to4.ExistC3sWhereC3C4many2many);
                        Assert.True(to4.ExistC3sWhereC3C4many2many);

                        // 1-3
                        from1.RemoveC3C4many2many(to4);
                        from1.RemoveC3C4many2many(to4);

                        mark();
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.Equal(3, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.True(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-2
                        from1.RemoveC3C4many2many(to3);
                        from1.RemoveC3C4many2many(to3);

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 1-1
                        from1.RemoveC3C4many2many(to2);
                        from1.RemoveC3C4many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // 0-0        
                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to3.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);
                        Assert.False(to4.ExistC3sWhereC3C4many2many);

                        // Multiplicity
                        C4[] to1Array = { to1 };
                        C4[] to2Array = { to2 };
                        C4[] to12Array = { to1, to2 };

                        // Get
                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2many(to1);
                        from1.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);
                        from2.RemoveC3C4many2many(to1);

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.False(from1.C3C4many2manies.Contains(to2));
                        Assert.False(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to2);
                        from1.AddC3C4many2many(to2);

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to2);
                        from1.AddC3C4many2many(to2);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.False(from1.C3C4many2manies.Contains(to2));
                        Assert.False(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.C3C4many2manies = to2Array;
                        from1.C3C4many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.False(from1.C3C4many2manies.Contains(to1));
                        Assert.False(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));

                        from1.C3C4many2manies = to12Array;

                        mark();
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.Equal(2, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.True(from1.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to2Array;
                        from1.C3C4many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to12Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from2.AddC3C4many2many(to1);
                        from2.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.True(from2.C3C4many2manies.Contains(to1));
                        Assert.True(from2.C3C4many2manies.Contains(to1));
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from2.AddC3C4many2many(to1);
                        from2.AddC3C4many2many(to1);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from2.C3C4many2manies = to1Array;
                        from2.C3C4many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.True(from2.C3C4many2manies.Contains(to1));
                        Assert.True(from2.C3C4many2manies.Contains(to1));
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(2, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from2));

                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from2.C3C4many2manies = to1Array;
                        from2.C3C4many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from2.AddC3C4many2many(to2);
                        from2.AddC3C4many2many(to2);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.True(from2.C3C4many2manies.Contains(to2));
                        Assert.True(from2.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from2));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from2.AddC3C4many2many(to2);
                        from2.AddC3C4many2many(to2);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        from2.C3C4many2manies = to2Array;
                        from2.C3C4many2manies = to2Array;

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.Equal(1, from2.C3C4many2manies.Count);
                        Assert.True(from2.C3C4many2manies.Contains(to2));
                        Assert.True(from2.C3C4many2manies.Contains(to2));
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to2.C3sWhereC3C4many2many.Count);
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from2));
                        Assert.True(to2.C3sWhereC3C4many2many.Contains(from2));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, from2.C3C4many2manies.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to2.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from1.C3C4many2manies = to1Array;
                        from1.C3C4many2manies = to1Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        from2.C3C4many2manies = to2Array;
                        from2.C3C4many2manies = to2Array;

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(from2.ExistC3C4many2manies);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);
                        Assert.True(to2.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();
                        from2.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(from2.ExistC3C4many2manies);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);
                        Assert.False(to2.ExistC3sWhereC3C4many2many);

                        // Null & Empty Array
                        // Add Null
                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many((C4)null);
                        from1.AddC3C4many2many((C4)null);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many((C4)null);
                        from1.AddC3C4many2many((C4)null);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many((C4)null);
                        from1.AddC3C4many2many((C4)null);

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many((C4)null);
                        from1.AddC3C4many2many((C4)null);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Delete Null
                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.RemoveC3C4many2many((C4)null);
                        from1.RemoveC3C4many2many((C4)null);

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.RemoveC3C4many2many((C4)null);
                        from1.RemoveC3C4many2many((C4)null);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.Equal(1, from1.C3C4many2manies.Count);
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.True(from1.C3C4many2manies.Contains(to1));
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(1, to1.C3sWhereC3C4many2many.Count);
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));
                        Assert.True(to1.C3sWhereC3C4many2many.Contains(from1));

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);
                        Assert.Equal(0, to1.C3sWhereC3C4many2many.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2many((C4)null);
                        from1.RemoveC3C4many2many((C4)null);

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.RemoveC3C4many2many((C4)null);
                        from1.RemoveC3C4many2many((C4)null);

                        mark();
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(from1.ExistC3C4many2manies);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);
                        Assert.True(to1.ExistC3sWhereC3C4many2many);

                        from1.RemoveC3C4many2manies();
                        from1.RemoveC3C4many2manies();

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);
                        Assert.False(to1.ExistC3sWhereC3C4many2many);

                        // Set Null
                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        from1.C3C4many2manies = null;
                        from1.C3C4many2manies = null;

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.C3C4many2manies = null;
                        from1.C3C4many2manies = null;

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        from1.C3C4many2manies = null;
                        from1.C3C4many2manies = null;

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.C3C4many2manies = null;
                        from1.C3C4many2manies = null;

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        // Set Empty Array
                        // Get
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        from1.C3C4many2manies = new C1[0];
                        from1.C3C4many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.C3C4many2manies = new C1[0];
                        from1.C3C4many2manies = new C1[0];

                        mark();
                        Assert.Equal(0, from1.C3C4many2manies.Count);
                        Assert.Equal(0, from1.C3C4many2manies.Count);

                        // Exist
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        from1.C3C4many2manies = new C1[0];
                        from1.C3C4many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        from1.AddC3C4many2many(to1);
                        from1.AddC3C4many2many(to1);
                        from1.C3C4many2manies = new C1[0];
                        from1.C3C4many2manies = new C1[0];

                        mark();
                        Assert.False(from1.ExistC3C4many2manies);
                        Assert.False(from1.ExistC3C4many2manies);

                        // Extent.ToArray()
                        from1 = C3.Create(this.Session);
                        to1 = C4.Create(this.Session);

                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.Strategy.GetCompositeRoles(MetaC3.Instance.C3C4many2manies.RelationType).ToArray().Length);
                        Assert.Equal(to1, from1.Strategy.GetCompositeRoles(MetaC3.Instance.C3C4many2manies.RelationType).ToArray()[0]);

                        // Extent<T>.ToArray()
                        from1 = C3.Create(this.Session);
                        to1 = C4.Create(this.Session);

                        from1.AddC3C4many2many(to1);

                        mark();
                        Assert.Equal(1, from1.C3C4many2manies.ToArray().Length);
                        Assert.Equal(to1, from1.C3C4many2manies.ToArray()[0]);

                        // Very Big Array
                        var bigArray = C4.Create(this.Session, Settings.LargeArraySize);
                        from1.C3C4many2manies = bigArray;
                        C4[] getBigArray = from1.C3C4many2manies;

                        mark();
                        Assert.Equal(Settings.LargeArraySize, getBigArray.Length);

                        var objects = new HashSet<IObject>(getBigArray);
                        foreach (var bigArrayObject in bigArray)
                        {
                            mark();
                            Assert.True(objects.Contains(bigArrayObject));
                        }
                    }
                }
            }
        }

        [Fact]
        public void SingularPlural()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (var run = 0; run < Settings.NumberOfRuns; run++)
                    {
                        var john = Person.Create(this.Session);
                        var janet = Person.Create(this.Session);
                        var fred = Person.Create(this.Session);

                        var acme = Company.Create(this.Session, "acme");

                        acme.AddEmployee(john);
                        acme.AddEmployee(janet);

                        if (acme.ExistEmployees)
                        {
                            foreach (Person employee in acme.Employees)
                            {
                                mark();
                                acme.RemoveEmployee(employee);
                            }
                        }

                        mark();
                        acme.RemoveEmployees();

                        mark();
                        acme.AddEmployee(john);
                        acme.AddEmployee(janet);

                        mark();
                        acme.AddOwner(fred);

                        if (john.CompanyWhereEmployee.ExistManager)
                        {
                            foreach (Person owner in john.CompanyWhereEmployee.Owners)
                            {
                                foreach (Company company in owner.CompaniesWhereOwner)
                                {
                                    mark();
                                    company.RemoveManager();
                                }
                            }
                        }
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
                    C1[] c1bs = { c1B };

                    var c2A = C2.Create(this.Session);
                    var c2B = C2.Create(this.Session);
                    C2[] c2Bs = { c2B };

                    var c3A = C3.Create(this.Session);
                    var c3B = C3.Create(this.Session);

                    var c4A = C4.Create(this.Session);
                    var c4B = C4.Create(this.Session);

                    // Illegal role
                    // Class
                    var exceptionThrown = false;
                    try
                    {
                        mark();
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2many2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1C2many2manies.RelationType, c1bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2many2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1I2many2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1I2many2manies.RelationType, c1bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1I2many2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1S2many2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1S2many2manies.RelationType, c1bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1S2many2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC2.Instance.C1many2manies.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC2.Instance.C1many2manies.RelationType, c1bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC2.Instance.C1many2manies.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC2.Instance.C1many2manies.RelationType, c2B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC2.Instance.C1many2manies.RelationType, c2Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC2.Instance.C1many2manies.RelationType, c2B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1AllorsString.RelationType, c1bs);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1AllorsDecimal.RelationType, c2B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1AllorsDecimal.RelationType, c2Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1AllorsDecimal.RelationType, c2B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2many2one.RelationType, c1B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1C1many2one.RelationType, c1bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2many2one.RelationType, c1B);
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
                        c1A.Strategy.AddCompositeRole(MetaC1.Instance.C1C2many2one.RelationType, c2B);
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
                        c1A.Strategy.SetCompositeRoles(MetaC1.Instance.C1C2many2one.RelationType, c2Bs);
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
                        c1A.Strategy.RemoveCompositeRole(MetaC1.Instance.C1C2many2one.RelationType, c2B);
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
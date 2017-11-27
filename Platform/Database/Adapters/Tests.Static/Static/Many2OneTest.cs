// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Many2OneTest.cs" company="Allors bvba">
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
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;

    using Allors;

    using Allors.Domain;
    using Allors.Meta;
    using Adapters;

    using Xunit;

    public abstract class Many2OneTest
    {
        public static int NR_OF_RUNS = Settings.NumberOfRuns;

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
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-1
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-2
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-3
                        from3.C1C1many2one = to;
                        from3.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-4
                        from4.C1C1many2one = to;
                        from4.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(4, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from4));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from4));
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(to, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-2
                        from3.RemoveC1C1many2one();
                        from3.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 1-1
                        from2.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

                        // 0-0        
                        from1.RemoveC1C1many2one();
                        from1.RemoveC1C1many2one(); // Twice
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from3.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);
                        Assert.Equal(null, from4.C1C1many2one);

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
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);

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
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        from1.C1C1many2one = toAnother;
                        from1.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from1));
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);

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
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        from2.C1C1many2one = toAnother;
                        from2.C1C1many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from2));
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        from2.C1C1many2one = null;
                        from2.C1C1many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);

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
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.False(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.False(to.C1sWhereC1C1many2one.Contains(from2));
                        from2.C1C1many2one = to;
                        from2.C1C1many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from1.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(to, from2.C1C1many2one);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C1many2one.Count);
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        from1.RemoveC1C1many2one();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);

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
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(null, from2.C1C1many2one);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);
                        from1.C1C1many2one = to;
                        from1.C1C1many2one = to; // Twice
                        from1.C1C1many2one = null;
                        from1.C1C1many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        Assert.Equal(null, from1.C1C1many2one);

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
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();

                        // 1-1
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
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
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from4));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from4));
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from3));
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
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(to, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();

                        // 1-1
                        from2.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(from1, to.C1sWhereC1C1many2one[0]);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();

                        // 0-0        
                        from1.RemoveC1C1many2one();
                        mark();
                        from1.RemoveC1C1many2one();
                        mark(); // Twice
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from3.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
                        mark();
                        Assert.Equal(null, from4.C1C1many2one);
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
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
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
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark();
                        from1.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from1.C1C1many2one);
                        mark();
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
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
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark();
                        from2.C1C1many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        mark();
                        Assert.Equal(toAnother, from2.C1C1many2one);
                        mark();
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(toAnother.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        from2.C1C1many2one = null;
                        mark();
                        from2.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.C1sWhereC1C1many2one.Count);
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
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(to, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.False(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.False(to.C1sWhereC1C1many2one.Contains(from2));
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
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from1));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        Assert.True(to.C1sWhereC1C1many2one.Contains(from2));
                        mark();
                        from1.RemoveC1C1many2one();
                        mark();
                        from2.RemoveC1C1many2one();
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
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
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(null, from2.C1C1many2one);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C1many2one.Count);
                        mark();

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        from1.C1C1many2one = to;
                        mark();
                        from1.C1C1many2one = to;
                        mark(); // Twice
                        from1.C1C1many2one = null;
                        mark();
                        from1.C1C1many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.C1C1many2one);
                        mark();
                        Assert.Equal(null, from1.C1C1many2one);
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
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-1
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-2
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-3
                        from3.C1C2many2one = to;
                        from3.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(3, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-4
                        from4.C1C2many2one = to;
                        from4.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(4, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from4));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from4));
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
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from3));
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(to, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-2
                        from3.RemoveC1C2many2one();
                        from3.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 1-1
                        from2.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(from1, to.C1sWhereC1C2many2one[0]);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

                        // 0-0        
                        from1.RemoveC1C2many2one();
                        from1.RemoveC1C2many2one(); // Twice
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from3.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);
                        Assert.Equal(null, from4.C1C2many2one);

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
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        from1.RemoveC1C2many2one();
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);

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
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        from1.C1C2many2one = toAnother;
                        from1.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(toAnother, from1.C1C2many2one);
                        Assert.Equal(toAnother, from1.C1C2many2one);
                        Assert.Equal(1, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.True(toAnother.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(toAnother.C1sWhereC1C2many2one.Contains(from1));
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);

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
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        from2.C1C2many2one = toAnother;
                        from2.C1C2many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.Equal(toAnother, from2.C1C2many2one);
                        Assert.Equal(toAnother, from2.C1C2many2one);
                        Assert.Equal(1, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.True(toAnother.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(toAnother.C1sWhereC1C2many2one.Contains(from2));
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        from2.C1C2many2one = null;
                        from2.C1C2many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, toAnother.C1sWhereC1C2many2one.Count);

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
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(1, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.False(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.False(to.C1sWhereC1C2many2one.Contains(from2));
                        from2.C1C2many2one = to;
                        from2.C1C2many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from1.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(to, from2.C1C2many2one);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(2, to.C1sWhereC1C2many2one.Count);
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from1));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        Assert.True(to.C1sWhereC1C2many2one.Contains(from2));
                        from1.RemoveC1C2many2one();
                        from2.RemoveC1C2many2one();
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);

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
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(null, from2.C1C2many2one);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);
                        Assert.Equal(0, to.C1sWhereC1C2many2one.Count);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);
                        from1.C1C2many2one = to;
                        from1.C1C2many2one = to; // Twice
                        from1.C1C2many2one = null;
                        from1.C1C2many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C1C2many2one);
                        Assert.Equal(null, from1.C1C2many2one);

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
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-1
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-2
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-3
                        from3.C3C4many2one = to;
                        from3.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(3, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-4
                        from4.C3C4many2one = to;
                        from4.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(4, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from4));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from4));
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
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from3));
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(to, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-2
                        from3.RemoveC3C4many2one();
                        from3.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 1-1
                        from2.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(from1, to.C3sWhereC3C4many2one[0]);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

                        // 0-0        
                        from1.RemoveC3C4many2one();
                        from1.RemoveC3C4many2one(); // Twice
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from3.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);
                        Assert.Equal(null, from4.C3C4many2one);

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
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        from1.RemoveC3C4many2one();
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);

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
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        from1.C3C4many2one = toAnother;
                        from1.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(toAnother, from1.C3C4many2one);
                        Assert.Equal(toAnother, from1.C3C4many2one);
                        Assert.Equal(1, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.True(toAnother.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(toAnother.C3sWhereC3C4many2one.Contains(from1));
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);

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
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        from2.C3C4many2one = toAnother;
                        from2.C3C4many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.Equal(toAnother, from2.C3C4many2one);
                        Assert.Equal(toAnother, from2.C3C4many2one);
                        Assert.Equal(1, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.True(toAnother.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(toAnother.C3sWhereC3C4many2one.Contains(from2));
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        from2.C3C4many2one = null;
                        from2.C3C4many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, toAnother.C3sWhereC3C4many2one.Count);

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
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(1, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.False(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.False(to.C3sWhereC3C4many2one.Contains(from2));
                        from2.C3C4many2one = to;
                        from2.C3C4many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from1.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(to, from2.C3C4many2one);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(2, to.C3sWhereC3C4many2one.Count);
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from1));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        Assert.True(to.C3sWhereC3C4many2one.Contains(from2));
                        from1.RemoveC3C4many2one();
                        from2.RemoveC3C4many2one();
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);

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
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(null, from2.C3C4many2one);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);
                        Assert.Equal(0, to.C3sWhereC3C4many2one.Count);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);
                        from1.C3C4many2one = to;
                        from1.C3C4many2one = to; // Twice
                        from1.C3C4many2one = null;
                        from1.C3C4many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.C3C4many2one);
                        Assert.Equal(null, from1.C3C4many2one);

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
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-1
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-2
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-3
                        from3.I1I12many2one = to;
                        from3.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(3, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-4
                        from4.I1I12many2one = to;
                        from4.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(4, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from4));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from4));
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(to, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-2
                        from3.RemoveI1I12many2one();
                        from3.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 1-1
                        from2.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

                        // 0-0        
                        from1.RemoveI1I12many2one();
                        from1.RemoveI1I12many2one(); // Twice
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from3.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);
                        Assert.Equal(null, from4.I1I12many2one);

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
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);

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
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        from1.I1I12many2one = toAnother;
                        from1.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from1));
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);

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
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        from2.I1I12many2one = toAnother;
                        from2.I1I12many2one = toAnother; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from2));
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        from2.I1I12many2one = null;
                        from2.I1I12many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);

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
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.False(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.False(to.I1sWhereI1I12many2one.Contains(from2));
                        from2.I1I12many2one = to;
                        from2.I1I12many2one = to; // Twice
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from1.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(to, from2.I1I12many2one);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(2, to.I1sWhereI1I12many2one.Count);
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        from1.RemoveI1I12many2one();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);

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
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(null, from2.I1I12many2one);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);
                        from1.I1I12many2one = to;
                        from1.I1I12many2one = to; // Twice
                        from1.I1I12many2one = null;
                        from1.I1I12many2one = null; // Twice
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        Assert.Equal(null, from1.I1I12many2one);

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
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();

                        // 1-1
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
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
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from4));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from4));
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from3));
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
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(to, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();

                        // 1-1
                        from2.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(from1, to.I1sWhereI1I12many2one[0]);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();

                        // 0-0        
                        from1.RemoveI1I12many2one();
                        mark();
                        from1.RemoveI1I12many2one();
                        mark(); // Twice
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from3.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
                        mark();
                        Assert.Equal(null, from4.I1I12many2one);
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
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
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
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark();
                        from1.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from1.I1I12many2one);
                        mark();
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
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
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark();
                        from2.I1I12many2one = toAnother;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        mark();
                        Assert.Equal(toAnother, from2.I1I12many2one);
                        mark();
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(toAnother.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        from2.I1I12many2one = null;
                        mark();
                        from2.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, toAnother.I1sWhereI1I12many2one.Count);
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
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(to, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(1, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.False(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.False(to.I1sWhereI1I12many2one.Contains(from2));
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
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from1));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        Assert.True(to.I1sWhereI1I12many2one.Contains(from2));
                        mark();
                        from1.RemoveI1I12many2one();
                        mark();
                        from2.RemoveI1I12many2one();
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
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
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(null, from2.I1I12many2one);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();
                        Assert.Equal(0, to.I1sWhereI1I12many2one.Count);
                        mark();

                        // Null & Empty Array
                        // Set Null
                        // Get
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        from1.I1I12many2one = to;
                        mark();
                        from1.I1I12many2one = to;
                        mark(); // Twice
                        from1.I1I12many2one = null;
                        mark();
                        from1.I1I12many2one = null;
                        mark(); // Twice
                        Assert.Equal(null, from1.I1I12many2one);
                        mark();
                        Assert.Equal(null, from1.I1I12many2one);
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
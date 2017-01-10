// --------------------------------------------------------------------------------------------------------------------
// <copyright file="One2OneTest.cs" company="Allors bvba">
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

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    //TODO: Add remove with null and zero array
    public abstract class One2OneTest
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

        [Test]
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
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // 1-1
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);

                        // 0-0        
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.C1C1one2one = to;
                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // Exist
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);

                        // 1-1
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);

                        // 0-0        
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.C1C1one2one = to;
                        from.C1C1one2one = to;
                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);

                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // Exist
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);

                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // Same From / Different To
                        // Get
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = toAnother;
                        from.C1C1one2one = toAnother;

                        mark();
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(toAnother, from.C1C1one2one);
                        Assert.AreEqual(toAnother, from.C1C1one2one);
                        Assert.AreEqual(from, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(from, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        // Exist
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = toAnother;
                        from.C1C1one2one = toAnother;

                        mark();
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        // Different From / Different To
                        // Get
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = toAnother;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                        Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                        Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                        Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                        Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = null;
                        fromAnother.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);
                        Assert.AreEqual(null, toAnother.C1WhereC1C1one2one);

                        // Exist
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = toAnother;
                        fromAnother.C1C1one2one = toAnother;

                        mark();
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(toAnother.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = null;
                        fromAnother.C1C1one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(toAnother.ExistC1WhereC1C1one2one);

                        // Different From / Same To
                        // Get
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);

                        fromAnother.C1C1one2one = to;
                        fromAnother.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(to, fromAnother.C1C1one2one);
                        Assert.AreEqual(to, fromAnother.C1C1one2one);
                        Assert.AreEqual(fromAnother, to.C1WhereC1C1one2one);
                        Assert.AreEqual(fromAnother, to.C1WhereC1C1one2one);

                        fromAnother.RemoveC1C1one2one();
                        fromAnother.RemoveC1C1one2one();

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, fromAnother.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        fromAnother.C1C1one2one = to;
                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(to, fromAnother.C1C1one2one);
                        fromAnother.RemoveC1C1one2one();

                        // Exist
                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsTrue(from.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);

                        fromAnother.C1C1one2one = to;
                        fromAnother.C1C1one2one = to;

                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(fromAnother.ExistC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);
                        Assert.IsTrue(to.ExistC1WhereC1C1one2one);

                        fromAnother.RemoveC1C1one2one();
                        fromAnother.RemoveC1C1one2one();

                        mark();
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(from.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(fromAnother.ExistC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);
                        Assert.IsFalse(to.ExistC1WhereC1C1one2one);

                        // Null 
                        // Set Null
                        // Get
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        // Get
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        from.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);

                        from.C1C1one2one = null;
                        from.C1C1one2one = null;

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);

                        from = C1.Create(this.Session);
                        fromAnother = C1.Create(this.Session);

                        to = C1.Create(this.Session);
                        toAnother = C1.Create(this.Session);

                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // 1-1
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);

                        mark();
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(to, from.C1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);
                        Assert.AreEqual(from, to.C1WhereC1C1one2one);

                        // 0-0        
                        from.RemoveC1C1one2one();
                        from.RemoveC1C1one2one();
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, to);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, null);
                        from.Strategy.SetRole(MetaC1.Instance.C1C1one2one.RelationType, null);
                        mark();
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, from.C1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);
                        Assert.AreEqual(null, to.C1WhereC1C1one2one);

                        // From - Middle - To
                        from = C1.Create(this.Session);
                        var middle = C1.Create(this.Session);
                        to = C1.Create(this.Session);

                        from.C1C1one2one = middle;
                        middle.C1C1one2one = to;
                        from.C1C1one2one = to;

                        mark();
                        Assert.IsNull(middle.C1WhereC1C1one2one);
                        Assert.IsNull(middle.C1C1one2one);
                    }
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.C1C2one2one);
                    Assert.AreEqual(null, to.C1WhereC1C2one2one);

                    // 1-1
                    from.C1C2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1C2one2one);
                    Assert.AreEqual(from, to.C1WhereC1C2one2one);

                    // 0-0        
                    from.RemoveC1C2one2one();
                    from.RemoveC1C2one2one();
                    from.C1C2one2one = to;
                    from.C1C2one2one = null;
                    from.C1C2one2one = null;

                    mark();
                    Assert.AreEqual(null, from.C1C2one2one);
                    Assert.AreEqual(null, to.C1WhereC1C2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C2one2one = to;
                    from.C1C2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1C2one2one);
                    Assert.AreEqual(from, to.C1WhereC1C2one2one);

                    from.RemoveC1C2one2one();

                    // Same From / Different To
                    from.C1C2one2one = to;
                    from.C1C2one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.C1WhereC1C2one2one);
                    Assert.AreEqual(toAnother, from.C1C2one2one);
                    Assert.AreEqual(from, toAnother.C1WhereC1C2one2one);

                    // Different From / Different To
                    from.C1C2one2one = to;
                    fromAnother.C1C2one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.C1C2one2one);
                    Assert.AreEqual(from, to.C1WhereC1C2one2one);
                    Assert.AreEqual(toAnother, fromAnother.C1C2one2one);
                    Assert.AreEqual(fromAnother, toAnother.C1WhereC1C2one2one);

                    // Different From / Same To
                    from.C1C2one2one = to;
                    fromAnother.C1C2one2one = to;

                    mark();
                    Assert.AreEqual(null, from.C1C2one2one);
                    Assert.AreEqual(to, fromAnother.C1C2one2one);
                    Assert.AreEqual(fromAnother, to.C1WhereC1C2one2one);

                    fromAnother.RemoveC1C2one2one();

                    // Null 
                    // Set Null
                    from.C1C2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C2one2one);
                    from.C1C2one2one = to;
                    from.C1C2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C2one2one);
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.C1I1one2one);
                    Assert.AreEqual(null, to.C1WhereC1I1one2one);

                    // 1-1
                    from.C1I1one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1I1one2one);
                    Assert.AreEqual(from, to.C1WhereC1I1one2one);

                    // 0-0        
                    from.RemoveC1I1one2one();
                    from.RemoveC1I1one2one();
                    from.C1I1one2one = to;
                    from.C1I1one2one = null;
                    from.C1I1one2one = null;

                    mark();
                    Assert.AreEqual(null, from.C1I1one2one);
                    Assert.AreEqual(null, to.C1WhereC1I1one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C1one2one = to;
                    from.C1C1one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1C1one2one);
                    Assert.AreEqual(from, to.C1WhereC1C1one2one);

                    from.RemoveC1C1one2one();

                    // Same From / Different To
                    from.C1C1one2one = to;
                    from.C1C1one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.C1WhereC1C1one2one);
                    Assert.AreEqual(toAnother, from.C1C1one2one);
                    Assert.AreEqual(from, toAnother.C1WhereC1C1one2one);

                    // Different From / Different To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.C1C1one2one);
                    Assert.AreEqual(from, to.C1WhereC1C1one2one);
                    Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                    Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);

                    // Different From / Same To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = to;

                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);
                    Assert.AreEqual(to, fromAnother.C1C1one2one);
                    Assert.AreEqual(fromAnother, to.C1WhereC1C1one2one);

                    fromAnother.RemoveC1C1one2one();

                    // Null 
                    // Set Null
                    from.C1C1one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);
                    from.C1C1one2one = to;
                    from.C1C1one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.C1I2one2one);
                    Assert.AreEqual(null, to.C1WhereC1I2one2one);

                    // 1-1
                    from.C1I2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1I2one2one);
                    Assert.AreEqual(from, to.C1WhereC1I2one2one);

                    // 0-0        
                    from.RemoveC1I2one2one();
                    from.RemoveC1I2one2one();
                    from.C1I2one2one = to;
                    from.C1I2one2one = null;
                    from.C1I2one2one = null;

                    mark();
                    Assert.AreEqual(null, from.C1I2one2one);
                    Assert.AreEqual(null, to.C1WhereC1I2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1I2one2one = to;
                    from.C1I2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1I2one2one);
                    Assert.AreEqual(from, to.C1WhereC1I2one2one);

                    from.RemoveC1I2one2one();

                    // Same From / Different To
                    from.C1I2one2one = to;
                    from.C1I2one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.C1WhereC1I2one2one);
                    Assert.AreEqual(toAnother, from.C1I2one2one);
                    Assert.AreEqual(from, toAnother.C1WhereC1I2one2one);

                    // Different From / Different To
                    from.C1I2one2one = to;
                    fromAnother.C1I2one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.C1I2one2one);
                    Assert.AreEqual(from, to.C1WhereC1I2one2one);
                    Assert.AreEqual(toAnother, fromAnother.C1I2one2one);
                    Assert.AreEqual(fromAnother, toAnother.C1WhereC1I2one2one);

                    // Different From / Same To
                    from.C1I2one2one = to;
                    fromAnother.C1I2one2one = to;

                    mark();
                    Assert.AreEqual(null, from.C1I2one2one);
                    Assert.AreEqual(to, fromAnother.C1I2one2one);
                    Assert.AreEqual(fromAnother, to.C1WhereC1I2one2one);

                    fromAnother.RemoveC1I2one2one();

                    // Null 
                    // Set Null
                    from.C1I2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1I2one2one);
                    from.C1I2one2one = to;
                    from.C1I2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1I2one2one);
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.C3C4one2one);
                    Assert.AreEqual(null, to.C3WhereC3C4one2one);

                    // 1-1
                    from.C3C4one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C3C4one2one);
                    Assert.AreEqual(from, to.C3WhereC3C4one2one);

                    // 0-0        
                    from.RemoveC3C4one2one();
                    from.RemoveC3C4one2one();
                    from.C3C4one2one = to;
                    from.C3C4one2one = null;
                    from.C3C4one2one = null;

                    mark();
                    Assert.AreEqual(null, from.C3C4one2one);
                    Assert.AreEqual(null, to.C3WhereC3C4one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C3C4one2one = to;
                    from.C3C4one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C3C4one2one);
                    Assert.AreEqual(from, to.C3WhereC3C4one2one);

                    from.RemoveC3C4one2one();

                    // Same From / Different To
                    from.C3C4one2one = to;
                    from.C3C4one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.C3WhereC3C4one2one);
                    Assert.AreEqual(toAnother, from.C3C4one2one);
                    Assert.AreEqual(from, toAnother.C3WhereC3C4one2one);

                    // Different From / Different To
                    from.C3C4one2one = to;
                    fromAnother.C3C4one2one = toAnother;

                    this.Session.Commit();

                    mark();
                    Assert.AreEqual(to, from.C3C4one2one);
                    Assert.AreEqual(from, to.C3WhereC3C4one2one);
                    Assert.AreEqual(toAnother, fromAnother.C3C4one2one);
                    Assert.AreEqual(fromAnother, toAnother.C3WhereC3C4one2one);

                    // Different From / Same To
                    from.C3C4one2one = to;
                    fromAnother.C3C4one2one = to;

                    mark();
                    Assert.AreEqual(null, from.C3C4one2one);
                    Assert.AreEqual(to, fromAnother.C3C4one2one);
                    Assert.AreEqual(fromAnother, to.C3WhereC3C4one2one);

                    fromAnother.RemoveC3C4one2one();

                    // Null 
                    // Set Null
                    from.C3C4one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C3C4one2one);
                    from.C3C4one2one = to;
                    from.C3C4one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C3C4one2one);
                }
            }
        }

        [Test]
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
                        var from = C1.Create(Session);
                        var fromAnother = C1.Create(Session);

                        var to = C1.Create(Session);
                        var toAnother = C2.Create(Session);

                        // To 1 and back
                        // Get
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        // 1-1
                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);

                        // 0-0        
                        from.RemoveI1I12one2one();
                        from.RemoveI1I12one2one();
                        from.I1I12one2one = to;
                        from.I1I12one2one = null;
                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);

                        // 1-1
                        from.I1I12one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);

                        // 0-0        
                        from.RemoveI1I12one2one();
                        from.RemoveI1I12one2one();
                        from.I1I12one2one = to;
                        from.I1I12one2one = null;
                        from.I1I12one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        from.I1I12one2one = to;
                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);

                        from.RemoveI1I12one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;
                        from.I1I12one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);

                        from.RemoveI1I12one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        // Same From / Different To
                        // Get
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = toAnother;

                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(toAnother, from.I1I12one2one);
                        Assert.AreEqual(toAnother, from.I1I12one2one);
                        Assert.AreEqual(from, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(from, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        // Exist
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = toAnother;

                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        // Different From / Different To
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = toAnother;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = toAnother;

                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);

                        // Different From / Same To
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);

                        fromAnother.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(to, fromAnother.I1I12one2one);
                        Assert.AreEqual(to, fromAnother.I1I12one2one);
                        Assert.AreEqual(fromAnother, to.I1WhereI1I12one2one);
                        Assert.AreEqual(fromAnother, to.I1WhereI1I12one2one);

                        fromAnother.RemoveI1I12one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsTrue(from.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);

                        fromAnother.I1I12one2one = to;

                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);

                        fromAnother.RemoveI1I12one2one();

                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(from.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);

                        // Null 
                        // Set Null
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);

                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);

                        from.I1I12one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        Assert.AreEqual(to, from.I1I12one2one);

                        from.I1I12one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        Assert.AreEqual(null, from.I1I12one2one);
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
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        // 1-1
                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
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

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();

                        // 1-1
                        from.I1I12one2one = to;
                        mark();

                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
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

                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();

                        from.RemoveI1I12one2one();
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();
                        from.I1I12one2one = to;
                        mark();

                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.RemoveI1I12one2one();
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        // Same From / Different To
                        // Get
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = toAnother;
                        mark();

                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = toAnother;
                        mark();

                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        // Different From / Different To
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = toAnother;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(toAnother, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, toAnother.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = toAnother;
                        mark();

                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(toAnother.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = null;
                        mark();

                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(toAnother.ExistI1WhereI1I12one2one);
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

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(fromAnother, to.I1WhereI1I12one2one);
                        mark();

                        fromAnother.RemoveI1I12one2one();
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, fromAnother.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I12one2one);
                        mark();

                        // Exist
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.I1I12one2one = to;
                        mark();

                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I12one2one);
                        mark();

                        fromAnother.RemoveI1I12one2one();
                        mark();

                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(from.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(fromAnother.ExistI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I12one2one);
                        mark();

                        // Null 
                        // Set Null
                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();

                        // Get
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = to;
                        mark();

                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(to, from.I1I12one2one);
                        mark();

                        from.I1I12one2one = null;
                        mark();

                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                        Assert.AreEqual(null, from.I1I12one2one);
                        mark();
                    }
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.I1I1one2one);
                    Assert.AreEqual(null, to.I1WhereI1I1one2one);

                    // 1-1
                    from.I1I1one2one = to;

                    mark();
                    Assert.AreEqual(to, from.I1I1one2one);
                    Assert.AreEqual(from, to.I1WhereI1I1one2one);

                    // 0-0        
                    from.RemoveI1I1one2one();
                    from.RemoveI1I1one2one();
                    from.I1I1one2one = to;
                    from.I1I1one2one = null;
                    from.I1I1one2one = null;

                    mark();
                    Assert.AreEqual(null, from.I1I1one2one);
                    Assert.AreEqual(null, to.I1WhereI1I1one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.C1C1one2one = to;
                    from.C1C1one2one = to;

                    mark();
                    Assert.AreEqual(to, from.C1C1one2one);
                    Assert.AreEqual(from, to.C1WhereC1C1one2one);

                    from.RemoveC1C1one2one();

                    // Same From / Different To
                    from.C1C1one2one = to;
                    from.C1C1one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.C1WhereC1C1one2one);
                    Assert.AreEqual(toAnother, from.C1C1one2one);
                    Assert.AreEqual(from, toAnother.C1WhereC1C1one2one);

                    // Different From / Different To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.C1C1one2one);
                    Assert.AreEqual(from, to.C1WhereC1C1one2one);
                    Assert.AreEqual(toAnother, fromAnother.C1C1one2one);
                    Assert.AreEqual(fromAnother, toAnother.C1WhereC1C1one2one);

                    // Different From / Same To
                    from.C1C1one2one = to;
                    fromAnother.C1C1one2one = to;

                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);
                    Assert.AreEqual(to, fromAnother.C1C1one2one);
                    Assert.AreEqual(fromAnother, to.C1WhereC1C1one2one);

                    fromAnother.RemoveC1C1one2one();

                    // Null 
                    // Set Null
                    from.C1C1one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);
                    from.C1C1one2one = to;
                    from.C1C1one2one = null;
                    mark();
                    Assert.AreEqual(null, from.C1C1one2one);

                    // From - Middle - To
                    from = C1.Create(this.Session);
                    var middle = C1.Create(this.Session);
                    to = C1.Create(this.Session);

                    from.I1I1one2one = middle;
                    middle.I1I1one2one = to;
                    from.I1I1one2one = to;

                    mark();
                    Assert.IsNull(middle.I1WhereI1I1one2one);
                    Assert.IsNull(middle.I1I1one2one);
                }
            }
        }

        [Test]
        public void I1_I2one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    var from = C1.Create(Session);
                    var fromAnother = C1.Create(Session);

                    var to = C2.Create(Session);
                    var toAnother = C2.Create(Session);

                    // To 1 and back
                    mark();
                    Assert.AreEqual(null, from.I1I2one2one);
                    Assert.AreEqual(null, to.I1WhereI1I2one2one);

                    // 1-1
                    from.I1I2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.I1I2one2one);
                    Assert.AreEqual(from, to.I1WhereI1I2one2one);

                    // 0-0        
                    from.RemoveI1I2one2one();
                    from.RemoveI1I2one2one();
                    from.I1I2one2one = to;
                    from.I1I2one2one = null;
                    from.I1I2one2one = null;

                    mark();
                    Assert.AreEqual(null, from.I1I2one2one);
                    Assert.AreEqual(null, to.I1WhereI1I2one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.I1I2one2one = to;
                    from.I1I2one2one = to;

                    mark();
                    Assert.AreEqual(to, from.I1I2one2one);
                    Assert.AreEqual(from, to.I1WhereI1I2one2one);

                    from.RemoveI1I2one2one();

                    // Same From / Different To
                    from.I1I2one2one = to;
                    from.I1I2one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.I1WhereI1I2one2one);
                    Assert.AreEqual(toAnother, from.I1I2one2one);
                    Assert.AreEqual(from, toAnother.I1WhereI1I2one2one);

                    // Different From / Different To
                    from.I1I2one2one = to;
                    fromAnother.I1I2one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.I1I2one2one);
                    Assert.AreEqual(from, to.I1WhereI1I2one2one);
                    Assert.AreEqual(toAnother, fromAnother.I1I2one2one);
                    Assert.AreEqual(fromAnother, toAnother.I1WhereI1I2one2one);

                    // Different From / Same To
                    from.I1I2one2one = to;
                    fromAnother.I1I2one2one = to;

                    mark();
                    Assert.AreEqual(null, from.I1I2one2one);
                    Assert.AreEqual(to, fromAnother.I1I2one2one);
                    Assert.AreEqual(fromAnother, to.I1WhereI1I2one2one);

                    fromAnother.RemoveI1I2one2one();

                    // Null 
                    // Set Null
                    from.I1I2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.I1I2one2one);
                    from.I1I2one2one = to;
                    from.I1I2one2one = null;
                    mark();
                    Assert.AreEqual(null, from.I1I2one2one);
                }
            }
        }

        [Test]
        public void I1_I34one2one()
        {
            foreach (var init in this.Inits)
            {
                init();

                foreach (var mark in this.Markers)
                {
                    for (int i = 0; i < Settings.NumberOfRuns; i++)
                    {
                        var from = C1.Create(this.Session);
                        var fromAnother = C1.Create(this.Session);

                        var to = C3.Create(this.Session);
                        var toAnother = C4.Create(this.Session);

                        // To 1 and back
                        // Get
                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        // 1-1
                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);

                        // 0-0        
                        from.RemoveI1I34one2one();
                        from.RemoveI1I34one2one();
                        from.I1I34one2one = to;
                        from.I1I34one2one = null;
                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);

                        // 1-1
                        from.I1I34one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);

                        // 0-0        
                        from.RemoveI1I34one2one();
                        from.RemoveI1I34one2one();
                        from.I1I34one2one = to;
                        from.I1I34one2one = null;
                        from.I1I34one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);

                        // Multiplicity
                        // Same From / Same To
                        // Get
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        from.I1I34one2one = to;
                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);

                        from.RemoveI1I34one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;
                        from.I1I34one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);

                        from.RemoveI1I34one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        // Same From / Different To
                        // Get
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = toAnother;

                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(toAnother, from.I1I34one2one);
                        Assert.AreEqual(toAnother, from.I1I34one2one);
                        Assert.AreEqual(from, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(from, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        // Exist
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = toAnother;

                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        // Different From / Different To
                        // Get
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = toAnother;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I34one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I34one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I34one2one);
                        Assert.AreEqual(toAnother, fromAnother.I1I34one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(fromAnother, toAnother.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);
                        Assert.AreEqual(null, toAnother.I1WhereI1I34one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = toAnother;

                        mark();
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(toAnother.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = null;

                        mark();
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(toAnother.ExistI1WhereI1I34one2one);

                        // Different From / Same To
                        // Get
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);
                        Assert.AreEqual(from, to.I1WhereI1I34one2one);

                        fromAnother.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(to, fromAnother.I1I34one2one);
                        Assert.AreEqual(to, fromAnother.I1I34one2one);
                        Assert.AreEqual(fromAnother, to.I1WhereI1I34one2one);
                        Assert.AreEqual(fromAnother, to.I1WhereI1I34one2one);

                        fromAnother.RemoveI1I34one2one();

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, fromAnother.I1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);
                        Assert.AreEqual(null, to.I1WhereI1I34one2one);

                        // Exist
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsTrue(from.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);

                        fromAnother.I1I34one2one = to;

                        mark();
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(fromAnother.ExistI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);
                        Assert.IsTrue(to.ExistI1WhereI1I34one2one);

                        fromAnother.RemoveI1I34one2one();

                        mark();
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(from.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(fromAnother.ExistI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);
                        Assert.IsFalse(to.ExistI1WhereI1I34one2one);

                        // Null 
                        // Set Null
                        // Get
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);

                        // Get
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);

                        from.I1I34one2one = to;

                        mark();
                        Assert.AreEqual(to, from.I1I34one2one);
                        Assert.AreEqual(to, from.I1I34one2one);

                        from.I1I34one2one = null;

                        mark();
                        Assert.AreEqual(null, from.I1I34one2one);
                        Assert.AreEqual(null, from.I1I34one2one);
                    }
                }
            }
        }

        [Test]
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
                    Assert.AreEqual(null, from.I3I4one2one);
                    Assert.AreEqual(null, to.I3WhereI3I4one2one);

                    // 1-1
                    from.I3I4one2one = to;

                    mark();
                    Assert.AreEqual(to, from.I3I4one2one);
                    Assert.AreEqual(from, to.I3WhereI3I4one2one);

                    // 0-0        
                    from.RemoveI3I4one2one();
                    from.RemoveI3I4one2one();
                    from.I3I4one2one = to;
                    from.I3I4one2one = null;
                    from.I3I4one2one = null;

                    mark();
                    Assert.AreEqual(null, from.I3I4one2one);
                    Assert.AreEqual(null, to.I3WhereI3I4one2one);

                    // Multiplicity
                    // Same From / Same To
                    from.I3I4one2one = to;
                    from.I3I4one2one = to;

                    mark();
                    Assert.AreEqual(to, from.I3I4one2one);
                    Assert.AreEqual(from, to.I3WhereI3I4one2one);

                    from.RemoveI3I4one2one();

                    // Same From / Different To
                    from.I3I4one2one = to;
                    from.I3I4one2one = toAnother;

                    mark();
                    Assert.AreEqual(null, to.I3WhereI3I4one2one);
                    Assert.AreEqual(toAnother, from.I3I4one2one);
                    Assert.AreEqual(from, toAnother.I3WhereI3I4one2one);

                    // Different From / Different To
                    from.I3I4one2one = to;
                    fromAnother.I3I4one2one = toAnother;

                    mark();
                    Assert.AreEqual(to, from.I3I4one2one);
                    Assert.AreEqual(from, to.I3WhereI3I4one2one);
                    Assert.AreEqual(toAnother, fromAnother.I3I4one2one);
                    Assert.AreEqual(fromAnother, toAnother.I3WhereI3I4one2one);

                    // Different From / Same To
                    from.I3I4one2one = to;
                    fromAnother.I3I4one2one = to;

                    mark();
                    Assert.AreEqual(null, from.I3I4one2one);
                    Assert.AreEqual(to, fromAnother.I3I4one2one);
                    Assert.AreEqual(fromAnother, to.I3WhereI3I4one2one);

                    fromAnother.RemoveI3I4one2one();

                    // Null 
                    // Set Null
                    from.I3I4one2one = null;
                    mark();
                    Assert.AreEqual(null, from.I3I4one2one);
                    from.I3I4one2one = to;
                    from.I3I4one2one = null;
                    mark();
                    Assert.AreEqual(null, from.I3I4one2one);
                }
            }
        }

        [Test]
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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);

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

                    Assert.IsTrue(exceptionThrown);
                }
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodsTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
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
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class TreeTests : DomainTest
    {
        [Test]
        public void Resolve()
        {
            var c2A = new C2Builder(this.Session).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Session).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Session).WithC2AllorsString("c2C").Build();

            var c1A = new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1C2One2Many(c2A).Build();

            var c1B =
                new C1Builder(this.Session).WithC1AllorsString("c1B")
                    .WithC1C2One2Many(c2B)
                    .WithC1C2One2Many(c2C)
                    .Build();

            this.Session.Derive(true);

            var tree = new Tree(M.C1.ObjectType).Add(M.C1.C1C2One2Manies);

            var resolved = new HashSet<IObject>();
            tree.Resolve(c1A, resolved);

            resolved.Count.ShouldEqual(1);
            resolved.ShouldContain(c2A);

            resolved = new HashSet<IObject>();
            tree.Resolve(c1B, resolved);

            resolved.Count.ShouldEqual(2);
            resolved.ShouldContain(c2B);
            resolved.ShouldContain(c2C);
        }

        [Test]
        public void ResolveMultipleSubtree()
        {
            var c1A = new C1Builder(this.Session).WithC1AllorsString("c1A").Build();
            var c1B = new C1Builder(this.Session).WithC1AllorsString("c1B").Build();
            var c1C = new C1Builder(this.Session).WithC1AllorsString("c1C").Build();
            var c1D = new C1Builder(this.Session).WithC1AllorsString("c1D").Build();
            var c1E = new C1Builder(this.Session).WithC1AllorsString("c1E").Build();

            var c2A = new C2Builder(this.Session).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Session).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Session).WithC2AllorsString("c2C").Build();
            var c2D = new C2Builder(this.Session).WithC2AllorsString("c2D").Build();

            c1A.AddC1I12One2Many(c1C);
            c1B.AddC1I12One2Many(c1E);
            c1B.AddC1I12One2Many(c2A);
            c1B.AddC1I12One2Many(c2B);

            c1C.AddC1C1One2Many(c1D);
            c2A.AddC2C2One2Many(c2C);
            c2A.AddC2C2One2Many(c2D);

            this.Session.Derive(true);

            this.Session.Commit();

            var tree =
                new Tree(M.C1.ObjectType).Add(M.C1.C1I12One2Manies, new Tree(M.C1.ObjectType).Add(M.C1.C1C1One2Manies))
                    .Add(M.C1.C1I12One2Manies, new Tree(M.C2.ObjectType).Add(M.C2.C2C2One2Manies));

            var prefetchPolicy = tree.BuildPrefetchPolicy();

            var resolved = new HashSet<IObject>();

            this.Session.Prefetch(prefetchPolicy, c1A);
            tree.Resolve(c1A, resolved);

            resolved.Count.ShouldEqual(2);
            resolved.ShouldContain(c1C);
            resolved.ShouldContain(c1D);

            resolved = new HashSet<IObject>();

            this.Session.Prefetch(prefetchPolicy, c1B);
            tree.Resolve(c1B, resolved);

            resolved.Count.ShouldEqual(5);
            resolved.ShouldContain(c1E);
            resolved.ShouldContain(c2A);
            resolved.ShouldContain(c2B);
            resolved.ShouldContain(c2C);
            resolved.ShouldContain(c2D);
        }

        [Test]
        public void legal()
        {
            var tree = new Tree(M.C1.ObjectType)
                .Add(M.C1.C1C1Many2Manies);

            tree = new Tree(M.C1.ObjectType)
                .Add(M.I12.I12C2Many2Manies);

            tree = new Tree(M.I12.ObjectType)
                .Add(M.C1.C1C1Many2Manies);
        }

        [Test]
        public void Illegal()
        {
            {
                var exceptionThrown = false;

                try
                {
                    var tree = new Tree(M.C1.ObjectType)
                        .Add(M.C2.C2AllorsString);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                exceptionThrown.ShouldBeTrue();
            }
        }

        [Test]
        public void UnitTreeNodesDontHaveTreeNodes()
        {
            var tree = new Tree(M.C1.ObjectType)
                .Add(M.C1.C1AllorsString);

            var treeNode = tree.Nodes[0];

            treeNode.Nodes.ShouldBeNull();
        }

        [Test]
        public void Prefetch()
        {
            var tree = new Tree(M.C1.ObjectType);
            tree.BuildPrefetchPolicy();

            tree = new Tree(M.C1.ObjectType)
                .Add(M.C1.C1AllorsBinary);
            tree.BuildPrefetchPolicy();

            tree = new Tree(M.C1.ObjectType)
                .Add(M.C1.C1C1Many2Manies);
            tree.BuildPrefetchPolicy();
        }

    }
}

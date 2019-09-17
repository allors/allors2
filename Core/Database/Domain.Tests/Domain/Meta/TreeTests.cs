// <copyright file="TreeTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Tests
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public class TreeTests : DomainTest
    {
        [Fact]
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

            var tree = new[] { new TreeNode(M.C1.C1C2One2Manies) };

            var resolved = new HashSet<IObject>();
            tree.Resolve(c1A, resolved);

            Assert.Equal(1, resolved.Count);
            Assert.Contains(c2A, resolved);

            resolved = new HashSet<IObject>();
            tree.Resolve(c1B, resolved);

            Assert.Equal(2, resolved.Count);
            Assert.Contains(c2B, resolved);
            Assert.Contains(c2C, resolved);
        }

        [Fact]
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

            var tree = new[]
            {
                new TreeNode(M.C1.C1I12One2Manies)
                    .Add(M.C1.C1C1One2Manies),
                new TreeNode(M.C1.C1I12One2Manies)
                    .Add(M.C2.C2C2One2Manies),
            };

            var prefetchPolicy = tree.BuildPrefetchPolicy();

            var resolved = new HashSet<IObject>();

            this.Session.Prefetch(prefetchPolicy, c1A);
            tree.Resolve(c1A, resolved);

            Assert.Equal(2, resolved.Count);
            Assert.Contains(c1C, resolved);
            Assert.Contains(c1D, resolved);

            resolved = new HashSet<IObject>();

            this.Session.Prefetch(prefetchPolicy, c1B);
            tree.Resolve(c1B, resolved);

            Assert.Equal(5, resolved.Count);
            Assert.Contains(c1E, resolved);
            Assert.Contains(c2A, resolved);
            Assert.Contains(c2B, resolved);
            Assert.Contains(c2C, resolved);
            Assert.Contains(c2D, resolved);
        }

        [Fact]
        public void Legal()
        {
            new TreeNode(M.C1.C1C1Many2Manies).Add(M.C1.C1C2Many2Manies);

            new TreeNode(M.C1.C1C1Many2Manies).Add(M.I12.I12C2Many2Manies);

            new TreeNode(M.C1.C1I12Many2Manies).Add(M.C1.I12C2Many2Manies);
        }

        [Fact]
        public void Illegal()
        {
            {
                var exceptionThrown = false;

                try
                {
                    new TreeNode(M.C1.C1C1Many2Manies).Add(M.C2.C2C1Many2Manies);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void UnitTreeNodesDontHaveTreeNodes()
        {
            var treeNode = new TreeNode(M.C1.C1AllorsString);

            Assert.Null(treeNode.Nodes);
        }

        [Fact]
        public void Prefetch()
        {
            var tree = new TreeNode[0];
            tree.BuildPrefetchPolicy();

            tree = new[] { new TreeNode(M.C1.C1AllorsBinary) };
            tree.BuildPrefetchPolicy();

            tree = new[] { new TreeNode(M.C1.C1C1Many2Manies) };
            tree.BuildPrefetchPolicy();
        }
    }
}

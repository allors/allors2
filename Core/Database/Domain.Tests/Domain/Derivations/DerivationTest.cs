// <copyright file="DerivationTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Domain.NonLogging;

    using Xunit;

    public class DerivationTest : DomainTest
    {
        [Fact]
        public void Next()
        {
            var first = new FirstBuilder(this.Session).Build();

            this.Session.Derive(true);

            Assert.True(first.ExistIsDerived);
            Assert.True(first.Second.ExistIsDerived);
            Assert.True(first.Second.Third.ExistIsDerived);

            Assert.Equal(1, first.DerivationCount);
            Assert.Equal(1, first.Second.DerivationCount);
            Assert.Equal(1, first.Second.Third.DerivationCount);
        }

        [Fact]
        public void Dependency()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;

            this.Session.Commit();

            dependee.Counter = 10;

            this.Session.Derive(true);

            Assert.Equal(11, dependent.Counter);
            Assert.Equal(11, dependee.Counter);
        }

        [Fact]
        public void Subdependency()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();
            var subdependee = new SubdependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;
            dependee.Subdependee = subdependee;

            this.Session.Commit();

            subdependee.Subcounter = 10;

            this.Session.Derive(true);

            Assert.Equal(1, dependent.Counter);
            Assert.Equal(1, dependee.Counter);

            Assert.Equal(11, dependent.Subcounter);
            Assert.Equal(11, dependee.Subcounter);
            Assert.Equal(11, subdependee.Subcounter);
        }

        [Fact]
        public void Deleted()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;

            this.Session.Commit();

            dependee.DeleteDependent = true;

            this.Session.Derive(true);

            Assert.True(dependent.Strategy.IsDeleted);
            Assert.Equal(1, dependee.Counter);
        }

        [Fact]
        public void Force()
        {
            var first = new FirstBuilder(this.Session).Build();

            this.Session.Commit();

            this.Session.Derive(true);

            var derivation = new Derivation(this.Session);
            derivation.Derive(first);

            Assert.True(first.ExistIsDerived);
            Assert.True(first.Second.ExistIsDerived);
            Assert.True(first.Second.Third.ExistIsDerived);

            Assert.Equal(1, first.DerivationCount);
            Assert.Equal(1, first.Second.DerivationCount);
            Assert.Equal(1, first.Second.Third.DerivationCount);
        }
    }
}

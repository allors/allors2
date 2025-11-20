// <copyright file="ChangesTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ExtentTest type.</summary>

namespace Allors.Database.Adapters
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class ChangesTest : IDisposable
    {
        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void UnitRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c = this.Session.Create(MetaC3.Instance.ObjectType);
                this.Session.Commit();

                a = (C1)this.Session.Instantiate(a);
                var b = C2.Create(this.Session);
                this.Session.Instantiate(c);

                a.RemoveC1AllorsString();
                b.RemoveC2AllorsString();

                var changeSet = this.Session.Checkpoint();

                var associations = changeSet.Associations;
                var roles = changeSet.Roles;

                Assert.Empty(associations);
                Assert.Empty(roles);

                a.C1AllorsString = "a changed";
                b.C2AllorsString = "b changed";

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(2, associations.Count);
                Assert.Contains(a.Id, associations.ToArray());
                Assert.Contains(b.Id, associations.ToArray());

                Assert.Equal("a changed", a.C1AllorsString);
                Assert.Equal("b changed", b.C2AllorsString);

                Assert.Single(changeSet.GetRoleTypes(a.Id));
                Assert.Equal(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.Single(changeSet.GetRoleTypes(b.Id));
                Assert.Equal(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.True(associations.Contains(a.Id));
                Assert.True(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));
                Assert.False(roles.Contains(c.Id));

                a.C1AllorsString = "a changed";
                b.C2AllorsString = "b changed";

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Empty(associations);
                Assert.Empty(roles);

                a.C1AllorsString = "a changed again";
                b.C2AllorsString = "b changed again";

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(2, associations.Count);
                Assert.True(associations.Contains(a.Id));
                Assert.True(associations.Contains(a.Id));

                Assert.Single(changeSet.GetRoleTypes(a.Id));
                Assert.Equal(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.Single(changeSet.GetRoleTypes(b.Id));
                Assert.Equal(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.True(associations.Contains(a.Id));
                Assert.True(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));
                Assert.False(roles.Contains(c.Id));

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(0, associations.Count);
                Assert.Empty(changeSet.GetRoleTypes(a.Id));
                Assert.Empty(changeSet.GetRoleTypes(b.Id));

                Assert.False(associations.Contains(a.Id));
                Assert.False(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));
                Assert.False(roles.Contains(c.Id));

                a.RemoveC1AllorsString();
                b.RemoveC2AllorsString();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(2, associations.Count);
                Assert.True(associations.Contains(a.Id));
                Assert.True(associations.Contains(a.Id));

                Assert.Single(changeSet.GetRoleTypes(a.Id));
                Assert.Equal(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.Single(changeSet.GetRoleTypes(b.Id));
                Assert.Equal(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.True(associations.Contains(a.Id));
                Assert.True(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));
                Assert.False(roles.Contains(c.Id));

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(0, associations.Count);
                Assert.Empty(changeSet.GetRoleTypes(a.Id));
                Assert.Empty(changeSet.GetRoleTypes(b.Id));

                Assert.False(associations.Contains(a.Id));
                Assert.False(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));

                this.Session.Rollback();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(0, associations.Count);
                Assert.Empty(changeSet.GetRoleTypes(a.Id));
                Assert.Empty(changeSet.GetRoleTypes(b.Id));

                Assert.False(associations.Contains(a.Id));
                Assert.False(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));

                a.C1AllorsString = "a changed";

                this.Session.Commit();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.Equal(0, associations.Count);
                Assert.Empty(changeSet.GetRoleTypes(a.Id));
                Assert.Empty(changeSet.GetRoleTypes(b.Id));

                Assert.False(associations.Contains(a.Id));
                Assert.False(associations.Contains(b.Id));
                Assert.False(associations.Contains(c.Id));

                Assert.False(roles.Contains(a.Id));
                Assert.False(roles.Contains(b.Id));
            }
        }

        [Fact]
        public void One2OneRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c1b = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c2a = (C2)this.Session.Create(MetaC2.Instance.ObjectType);

                this.Session.Commit();

                c1a = (C1)this.Session.Instantiate(c1a);
                var c2b = C2.Create(this.Session);
                this.Session.Instantiate(c2a);

                var changes = this.Session.Checkpoint();

                c1a.C1C2one2one = null;

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2one2one();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2one2one = c2b;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                c1a.C1C2one2one = c2b;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2one2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Equal(2, roles.Length);
                Assert.Contains(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2one2one();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Strategy.ObjectId, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.C1C2one2one = c2a;

                this.Session.Rollback();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.C1C2one2one = c2a;

                this.Session.Commit();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1b.C1C2one2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Equal(2, associations.Length);
                Assert.Single(roles);
                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Single(changes.GetRoleTypes(c1b.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));
            }
        }

        [Fact]
        public void Many2OneRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c1b = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c2a = (C2)this.Session.Create(MetaC2.Instance.ObjectType);

                this.Session.Commit();

                c1a = (C1)this.Session.Instantiate(c1a);
                var c2b = C2.Create(this.Session);
                this.Session.Instantiate(c2a);

                var changes = this.Session.Checkpoint();

                c1a.C1C2many2one = null;

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2many2one();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2many2one = c2b;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                c1a.C1C2many2one = c2b;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2many2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Equal(2, roles.Length);
                Assert.Contains(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2many2one();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Strategy.ObjectId, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.C1C2many2one = c2a;

                this.Session.Rollback();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.C1C2many2one = c2a;

                this.Session.Commit();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1b.C1C2many2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Single(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Single(changes.GetRoleTypes(c1b.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));
            }
        }

        [Fact]
        public void One2ManyRoles()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c1b = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c2a = (C2)this.Session.Create(MetaC2.Instance.ObjectType);

                this.Session.Commit();

                c1a = (C1)this.Session.Instantiate(c1a);
                var c2b = C2.Create(this.Session);
                this.Session.Instantiate(c2a);

                c1a.C1C2one2manies = null;

                var changes = this.Session.Checkpoint();

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2one2manies();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2one2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.AddC1C2one2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                c1a.AddC1C2one2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2one2manies = new[] { c2b };

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.AddC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2one2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.AddC1C2one2many(c2a);

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.AddC1C2one2many(c2a);

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1b.AddC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Equal(2, associations.Length);
                Assert.Single(roles);
                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Single(changes.GetRoleTypes(c1b.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));
            }
        }

        [Fact]
        public void Many2ManyRoles()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c1b = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c2a = (C2)this.Session.Create(MetaC2.Instance.ObjectType);

                this.Session.Commit();

                c1a = (C1)this.Session.Instantiate(c1a);
                var c2b = C2.Create(this.Session);
                this.Session.Instantiate(c2a);

                c1a.C1C2many2manies = null;

                var changes = this.Session.Checkpoint();

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2many2manies();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.RemoveC1C2many2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.AddC1C2many2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                c1a.AddC1C2many2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.C1C2many2manies = new[] { c2b };

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                c1a.AddC1C2many2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2many2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2a.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.DoesNotContain(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.RemoveC1C2many2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Single(associations);
                Assert.Contains(c1a.Id, associations);

                Assert.Single(roles);
                Assert.Contains(c2b.Id, roles);

                Assert.Single(changes.GetRoleTypes(c1a.Id));
                Assert.Equal(MetaC1.Instance.C1C2many2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.Contains(c1a.Id, associations);
                Assert.DoesNotContain(c2b.Id, associations);
                Assert.DoesNotContain(c2a.Id, associations);

                Assert.DoesNotContain(c1a.Id, roles);
                Assert.Contains(c2b.Id, roles);
                Assert.DoesNotContain(c2a.Id, roles);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.AddC1C2many2many(c2a);

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1a.AddC1C2many2many(c2a);

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Empty(associations);
                Assert.Empty(roles);

                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));

                c1b.AddC1C2many2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.Equal(1, associations.Length);
                Assert.Single(roles);
                Assert.Empty(changes.GetRoleTypes(c1a.Id));
                Assert.Single(changes.GetRoleTypes(c1b.Id));
                Assert.Empty(changes.GetRoleTypes(c2b.Id));
                Assert.Empty(changes.GetRoleTypes(c2a.Id));
            }
        }

        [Fact]
        public void Delete()
        {
            foreach (var init in this.Inits)
            {
                init();

                var a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c = this.Session.Create(MetaC3.Instance.ObjectType);
                this.Session.Commit();

                a = (C1)this.Session.Instantiate(a);
                var b = C2.Create(this.Session);
                this.Session.Instantiate(c);

                a.Strategy.Delete();
                b.Strategy.Delete();

                var changes = this.Session.Checkpoint();

                Assert.Equal(2, changes.Deleted.Count());
                Assert.Contains(a.Id, changes.Deleted.ToArray());
                Assert.Contains(b.Id, changes.Deleted.ToArray());

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                Assert.Empty(changes.Deleted);

                a.Strategy.Delete();

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                Assert.Empty(changes.Deleted);
            }
        }

        [Fact]
        public void Create()
        {
            foreach (var init in this.Inits)
            {
                init();

                var a = (C1)this.Session.Create(MetaC1.Instance.ObjectType);
                var c = this.Session.Create(MetaC3.Instance.ObjectType);
                this.Session.Commit();

                a = (C1)this.Session.Instantiate(a);
                var b = C2.Create(this.Session);
                this.Session.Instantiate(c);

                var changes = this.Session.Checkpoint();

                Assert.Single(changes.Created);
                Assert.Contains(b.Id, changes.Created.ToArray());

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                Assert.Empty(changes.Created);

                b = C2.Create(this.Session);

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                Assert.Empty(changes.Created);
            }
        }
    }
}

//------------------------------------------------------------------------------------------------- 
// <copyright file="ChangesTest.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ExtentTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;
    using System.Linq;

    using Allors;

    using Allors.Domain;
    using Allors.Meta;
    using Adapters;

    using NUnit.Framework;

    public abstract class ChangesTest
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

                a.C1AllorsString = "a changed";
                b.C2AllorsString = "b changed";

                var changeSet = this.Session.Checkpoint();

                var associations = changeSet.Associations;
                var roles = changeSet.Roles;

                Assert.AreEqual(2, associations.Count);
                Assert.Contains(a.Id, associations.ToArray());
                Assert.Contains(b.Id, associations.ToArray());

                Assert.AreEqual("a changed", a.C1AllorsString);
                Assert.AreEqual("b changed", b.C2AllorsString);

                Assert.AreEqual(1, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.AreEqual(1, changeSet.GetRoleTypes(b.Id).Count());
                Assert.AreEqual(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.IsTrue(associations.Contains(a.Id));
                Assert.IsTrue(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));
                Assert.IsFalse(roles.Contains(c.Id));

                a.C1AllorsString = "a changed again";
                b.C2AllorsString = "b changed again";

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(2, associations.Count);
                Assert.IsTrue(associations.Contains(a.Id));
                Assert.IsTrue(associations.Contains(a.Id));

                Assert.AreEqual(1, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.AreEqual(1, changeSet.GetRoleTypes(b.Id).Count());
                Assert.AreEqual(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.IsTrue(associations.Contains(a.Id));
                Assert.IsTrue(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));
                Assert.IsFalse(roles.Contains(c.Id));

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(0, associations.Count);
                Assert.AreEqual(0, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(0, changeSet.GetRoleTypes(b.Id).Count());

                Assert.IsFalse(associations.Contains(a.Id));
                Assert.IsFalse(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));
                Assert.IsFalse(roles.Contains(c.Id));

                a.RemoveC1AllorsString();
                b.RemoveC2AllorsString();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(2, associations.Count);
                Assert.IsTrue(associations.Contains(a.Id));
                Assert.IsTrue(associations.Contains(a.Id));

                Assert.AreEqual(1, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1AllorsString, changeSet.GetRoleTypes(a.Id).First());

                Assert.AreEqual(1, changeSet.GetRoleTypes(b.Id).Count());
                Assert.AreEqual(MetaC2.Instance.C2AllorsString, changeSet.GetRoleTypes(b.Id).First());

                Assert.IsTrue(associations.Contains(a.Id));
                Assert.IsTrue(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));
                Assert.IsFalse(roles.Contains(c.Id));

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(0, associations.Count);
                Assert.AreEqual(0, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(0, changeSet.GetRoleTypes(b.Id).Count());

                Assert.IsFalse(associations.Contains(a.Id));
                Assert.IsFalse(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));

                this.Session.Rollback();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(0, associations.Count);
                Assert.AreEqual(0, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(0, changeSet.GetRoleTypes(b.Id).Count());

                Assert.IsFalse(associations.Contains(a.Id));
                Assert.IsFalse(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));

                a.C1AllorsString = "a changed";

                this.Session.Commit();

                changeSet = this.Session.Checkpoint();

                associations = changeSet.Associations;
                roles = changeSet.Roles;

                Assert.AreEqual(0, associations.Count);
                Assert.AreEqual(0, changeSet.GetRoleTypes(a.Id).Count());
                Assert.AreEqual(0, changeSet.GetRoleTypes(b.Id).Count());

                Assert.IsFalse(associations.Contains(a.Id));
                Assert.IsFalse(associations.Contains(b.Id));
                Assert.IsFalse(associations.Contains(c.Id));

                Assert.IsFalse(roles.Contains(a.Id));
                Assert.IsFalse(roles.Contains(b.Id));
            }
        }

        [Test]
        public void CompositeRole()
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

                c1a.C1C2one2one = c2b;

                var changes = this.Session.Checkpoint();

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2b.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsTrue(roles.Contains(c2b.Id));
                Assert.IsFalse(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.C1C2one2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(2, roles.Length);
                Assert.Contains(c2b.Id, roles);
                Assert.Contains(c2a.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsTrue(roles.Contains(c2b.Id));
                Assert.IsTrue(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.RemoveC1C2one2one();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Strategy.ObjectId, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2a.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2one, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsFalse(roles.Contains(c2b.Id));
                Assert.IsTrue(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.C1C2one2one = c2a;

                this.Session.Rollback();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.C1C2one2one = c2a;

                this.Session.Commit();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1b.C1C2one2one = c2a;

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(2, associations.Length);
                Assert.AreEqual(1, roles.Length);
                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(1, changes.GetRoleTypes(c1b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());
            }
        }

        [Test]
        public void CompositeRoles()
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

                c1a.AddC1C2one2many(c2b);

                var changes = this.Session.Checkpoint();

                var associations = changes.Associations.ToArray();
                var roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2b.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsTrue(roles.Contains(c2b.Id));
                Assert.IsFalse(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.AddC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2a.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsFalse(roles.Contains(c2b.Id));
                Assert.IsTrue(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.RemoveC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2a.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsFalse(roles.Contains(c2b.Id));
                Assert.IsTrue(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.RemoveC1C2one2many(c2b);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(1, associations.Length);
                Assert.Contains(c1a.Id, associations);

                Assert.AreEqual(1, roles.Length);
                Assert.Contains(c2b.Id, roles);

                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(MetaC1.Instance.C1C2one2manies, changes.GetRoleTypes(c1a.Id).First());

                Assert.IsTrue(associations.Contains(c1a.Id));
                Assert.IsFalse(associations.Contains(c2b.Id));
                Assert.IsFalse(associations.Contains(c2a.Id));

                Assert.IsFalse(roles.Contains(c1a.Id));
                Assert.IsTrue(roles.Contains(c2b.Id));
                Assert.IsFalse(roles.Contains(c2a.Id));

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);
                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.AddC1C2one2many(c2a);

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);

                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1a.AddC1C2one2many(c2a);

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(0, associations.Length);
                Assert.AreEqual(0, roles.Length);

                Assert.AreEqual(0, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());

                c1b.AddC1C2one2many(c2a);

                changes = this.Session.Checkpoint();

                associations = changes.Associations.ToArray();
                roles = changes.Roles.ToArray();

                Assert.AreEqual(2, associations.Length);
                Assert.AreEqual(1, roles.Length);
                Assert.AreEqual(1, changes.GetRoleTypes(c1a.Id).Count());
                Assert.AreEqual(1, changes.GetRoleTypes(c1b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2b.Id).Count());
                Assert.AreEqual(0, changes.GetRoleTypes(c2a.Id).Count());
            }
        }

        [Test]
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

                Assert.AreEqual(2, changes.Deleted.Count());
                Assert.Contains(a.Id, changes.Deleted.ToArray());
                Assert.Contains(b.Id, changes.Deleted.ToArray());

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                Assert.AreEqual(0, changes.Deleted.Count());

                a.Strategy.Delete();

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                Assert.AreEqual(0, changes.Deleted.Count());
            }
        }

        [Test]
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

                Assert.AreEqual(1, changes.Created.Count());
                Assert.Contains(b.Id, changes.Created.ToArray());

                this.Session.Rollback();

                changes = this.Session.Checkpoint();

                Assert.AreEqual(0, changes.Created.Count());

                b = C2.Create(this.Session);

                this.Session.Commit();

                changes = this.Session.Checkpoint();

                Assert.AreEqual(0, changes.Created.Count());
            }
        }
    }
}
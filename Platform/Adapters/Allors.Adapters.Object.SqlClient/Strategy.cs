
// <copyright file="Strategy.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsStrategySql type.
// </summary>

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Adapters;

    using Meta;

    public class Strategy : IStrategy
    {
        private IObject allorsObject;
        private Roles roles;

        internal Strategy(Reference reference)
        {
            this.Reference = reference;
            this.ObjectId = reference.ObjectId;
        }

        ISession IStrategy.Session => this.Reference.Session;

        public Session Session => this.Reference.Session;

        public IClass Class
        {
            get
            {
                if (!this.Reference.Exists)
                {
                    throw new Exception("Object that had  " + this.Reference.Class.Name + " with id " + this.ObjectId + " does not exist");
                }

                return this.Reference.Class;
            }
        }

        public long ObjectId { get; }

        public long ObjectVersion => this.Reference.Version;

        public bool IsDeleted => !this.Reference.Exists;

        public bool IsNewInSession => this.Reference.IsNew;

        internal Roles Roles => this.roles ?? (this.roles = this.Reference.Session.State.GetOrCreateRoles(this.Reference));

        internal Reference Reference { get; }

        public IObject GetObject() => this.allorsObject ?? (this.allorsObject = this.Reference.Session.Database.ObjectFactory.Create(this));

        public virtual void Delete()
        {
            this.AssertExist();

            foreach (var roleType in this.Class.RoleTypes)
            {
                if (roleType.ObjectType.IsComposite)
                {
                    this.RemoveRole(roleType.RelationType);
                }
            }

            foreach (var associationType in this.Class.AssociationTypes)
            {
                var relationType = associationType.RelationType;
                var roleType = relationType.RoleType;

                if (associationType.IsMany)
                {
                    foreach (var association in this.Session.GetAssociations(this, associationType))
                    {
                        var associationStrategy = this.Session.State.GetOrCreateReferenceForExistingObject(association, this.Session).Strategy;
                        if (roleType.IsMany)
                        {
                            associationStrategy.RemoveCompositeRole(relationType, this.GetObject());
                        }
                        else
                        {
                            associationStrategy.RemoveCompositeRole(relationType);
                        }
                    }
                }
                else
                {
                    var association = this.GetCompositeAssociation(relationType);
                    if (association != null)
                    {
                        if (roleType.IsMany)
                        {
                            association.Strategy.RemoveCompositeRole(roleType.RelationType, this.GetObject());
                        }
                        else
                        {
                            association.Strategy.RemoveCompositeRole(roleType.RelationType);
                        }
                    }
                }
            }

            this.Session.Commands.DeleteObject(this);
            this.Reference.Exists = false;

            this.Session.State.ChangeSet.OnDeleted(this.ObjectId);
        }

        public virtual bool ExistRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType.IsUnit)
            {
                return this.ExistUnitRole(relationType);
            }

            return relationType.RoleType.IsMany
                ? this.ExistCompositeRoles(relationType)
                : this.ExistCompositeRole(relationType);
        }

        public virtual object GetRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(relationType);
            }

            return relationType.RoleType.IsMany
                       ? (object)this.GetCompositeRoles(relationType)
                       : this.GetCompositeRole(relationType);
        }

        public virtual void SetRole(IRelationType relationType, object value)
        {
            if (relationType.RoleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(relationType, value);
            }
            else
            {
                if (relationType.RoleType.IsMany)
                {
                    var roleExtent = value as Allors.Extent ?? ((ICollection<IObject>)value).ToArray();
                    this.SetCompositeRoles(relationType, roleExtent);
                }
                else
                {
                    this.SetCompositeRole(relationType, (IObject)value);
                }
            }
        }

        public virtual void RemoveRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType.IsUnit)
            {
                this.RemoveUnitRole(relationType);
            }
            else
            {
                if (relationType.RoleType.IsMany)
                {
                    this.RemoveCompositeRoles(relationType);
                }
                else
                {
                    this.RemoveCompositeRole(relationType);
                }
            }
        }

        public virtual bool ExistUnitRole(IRelationType relationType) => this.GetUnitRole(relationType) != null;

        public virtual object GetUnitRole(IRelationType relationType)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            return this.Roles.GetUnitRole(roleType);
        }

        public virtual void SetUnitRole(IRelationType relationType, object role)
        {
            this.AssertExist();

            RoleAssertions.UnitRoleChecks(this, relationType.RoleType);

            if (role != null)
            {
                role = relationType.RoleType.Normalize(role);
            }

            var oldUnit = this.GetUnitRole(relationType);
            if (!Equals(oldUnit, role))
            {
                this.Roles.SetUnitRole(relationType.RoleType, role);
            }
        }

        public virtual void RemoveUnitRole(IRelationType relationType) => this.SetUnitRole(relationType, null);

        public virtual bool ExistCompositeRole(IRelationType relationType) => this.GetCompositeRole(relationType) != null;

        public virtual IObject GetCompositeRole(IRelationType relationType)
        {
            this.AssertExist();

            var role = this.Roles.GetCompositeRole(relationType.RoleType);
            return (role == null) ? null : this.Session.State.GetOrCreateReferenceForExistingObject(role.Value, this.Session).Strategy.GetObject();
        }

        public virtual void SetCompositeRole(IRelationType relationType, IObject newRoleObject)
        {
            if (newRoleObject == null)
            {
                this.RemoveCompositeRole(relationType);
                return;
            }

            this.AssertExist();

            RoleAssertions.CompositeRoleChecks(this, relationType.RoleType, newRoleObject);

            var newRoleObjectId = (Strategy)newRoleObject.Strategy;
            this.Roles.SetCompositeRole(relationType.RoleType, newRoleObjectId);
        }

        public virtual void RemoveCompositeRole(IRelationType relationType)
        {
            this.AssertExist();

            RoleAssertions.CompositeRoleChecks(this, relationType.RoleType);

            this.Roles.RemoveCompositeRole(relationType.RoleType);
        }

        public virtual bool ExistCompositeRoles(IRelationType relationType) => this.GetCompositeRoles(relationType).Count != 0;

        public virtual Allors.Extent GetCompositeRoles(IRelationType relationType)
        {
            this.AssertExist();

            return new ExtentRoles(this, relationType.RoleType);
        }

        public virtual void AddCompositeRole(IRelationType relationType, IObject roleObject)
        {
            this.AssertExist();

            if (roleObject != null)
            {
                RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, roleObject);

                var role = (Strategy)roleObject.Strategy;
                this.Roles.AddCompositeRole(relationType.RoleType, role);
            }
        }

        public virtual void RemoveCompositeRole(IRelationType relationType, IObject roleObject)
        {
            this.AssertExist();

            if (roleObject != null)
            {
                RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, roleObject);

                var role = (Strategy)roleObject.Strategy;
                this.Roles.RemoveCompositeRole(relationType.RoleType, role);
            }
        }

        public virtual void SetCompositeRoles(IRelationType relationType, Allors.Extent roleObjects)
        {
            if (roleObjects == null || roleObjects.Count == 0)
            {
                this.RemoveCompositeRoles(relationType);
            }
            else
            {
                this.AssertExist();

                // TODO: use CompositeRoles
                var previousRoles = new List<long>(this.Roles.GetCompositesRole(relationType.RoleType));
                var newRoles = new HashSet<long>();

                foreach (IObject roleObject in roleObjects)
                {
                    if (roleObject != null)
                    {
                        RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, roleObject);
                        var role = (Strategy)roleObject.Strategy;

                        if (!previousRoles.Contains(role.ObjectId))
                        {
                            this.Roles.AddCompositeRole(relationType.RoleType, role);
                        }

                        newRoles.Add(role.ObjectId);
                    }
                }

                foreach (var previousRole in previousRoles)
                {
                    if (!newRoles.Contains(previousRole))
                    {
                        this.Roles.RemoveCompositeRole(relationType.RoleType, this.Session.State.GetOrCreateReferenceForExistingObject(previousRole, this.Session).Strategy);
                    }
                }
            }
        }

        public virtual void RemoveCompositeRoles(IRelationType relationType)
        {
            this.AssertExist();

            RoleAssertions.CompositeRoleChecks(this, relationType.RoleType);

            var previousRoles = this.Roles.GetCompositesRole(relationType.RoleType);

            foreach (var previousRole in previousRoles)
            {
                this.Roles.RemoveCompositeRole(relationType.RoleType, this.Session.State.GetOrCreateReferenceForExistingObject(previousRole, this.Session).Strategy);
            }
        }

        public virtual bool ExistAssociation(IRelationType relationType) => relationType.AssociationType.IsMany ? this.ExistCompositeAssociations(relationType) : this.ExistCompositeAssociation(relationType);

        public virtual object GetAssociation(IRelationType relationType) => relationType.AssociationType.IsMany ? (object)this.GetCompositeAssociations(relationType) : this.GetCompositeAssociation(relationType);

        public virtual bool ExistCompositeAssociation(IRelationType relationType) => this.GetCompositeAssociation(relationType) != null;

        public virtual IObject GetCompositeAssociation(IRelationType relationType)
        {
            this.AssertExist();

            var association = this.Session.GetAssociation(this, relationType.AssociationType);

            return association?.Strategy.GetObject();
        }

        public virtual bool ExistCompositeAssociations(IRelationType relationType) => this.GetCompositeAssociations(relationType).Count != 0;

        public virtual Allors.Extent GetCompositeAssociations(IRelationType relationType)
        {
            this.AssertExist();
            return new ExtentAssociations(this, relationType.AssociationType);
        }

        public override string ToString() => "[" + this.Class + ":" + this.ObjectId + "]";

        internal virtual void Release() => this.roles = null;

        internal int ExtentRolesGetCount(IRoleType roleType)
        {
            this.AssertExist();

            return this.Roles.ExtentCount(roleType);
        }

        internal IObject ExtentRolesFirst(IRoleType roleType)
        {
            this.AssertExist();

            return this.Roles.ExtentFirst(this.Session, roleType);
        }

        internal void ExtentRolesCopyTo(IRoleType roleType, Array array, int index) => this.Roles.ExtentCopyTo(this.Session, roleType, array, index);

        internal int ExtentIndexOf(IRoleType roleType, IObject value)
        {
            var i = 0;
            foreach (var oid in this.Roles.GetCompositesRole(roleType))
            {
                if (oid.Equals(value.Id))
                {
                    return i;
                }
                ++i;
            }

            return -1;
        }

        internal IObject ExtentGetItem(IRoleType roleType, int index)
        {
            var i = 0;
            foreach (var oid in this.Roles.GetCompositesRole(roleType))
            {
                if (i == index)
                {
                    return this.Session.State.GetOrCreateReferenceForExistingObject(oid, this.Session).Strategy.GetObject();
                }
                ++i;
            }

            return null;
        }

        internal bool ExtentRolesContains(IRoleType roleType, IObject value) => this.Roles.ExtentContains(roleType, value.Id);

        internal virtual long[] ExtentGetCompositeAssociations(IAssociationType associationType)
        {
            this.AssertExist();

            return this.Session.GetAssociations(this, associationType);
        }

        protected virtual void AssertExist()
        {
            if (!this.Reference.Exists)
            {
                throw new Exception("Object of class " + this.Class.Name + " with id " + this.ObjectId + " does not exist");
            }
        }
    }
}

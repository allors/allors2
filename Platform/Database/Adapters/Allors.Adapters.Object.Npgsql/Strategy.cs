// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Strategy.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
//   Defines the AllorsStrategySql type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Adapters.Database.Npgsql;

    using Meta;

    public class Strategy : IStrategy
    {
        private readonly Reference reference;
        private readonly long objectId;

        private IObject allorsObject;
        private Roles roles;

        public Strategy(Reference reference)
        {
            this.reference = reference;
            this.objectId = reference.ObjectId;
        }

        public ISession Session
        {
            get { return this.reference.Session; }
        }

        public ISession DatabaseSession
        {
            get
            {
                return this.reference.Session;
            }
        }

        public IClass Class
        {
            get { return this.reference.ObjectType; }
        }

        public long ObjectId
        {
            get { return this.objectId; }
        }
        
        public long ObjectVersion => this.reference.Version;

        public bool IsDeleted
        {
            get
            {
                return !this.reference.Exists;
            }
        }

        public bool IsNewInSession
        {
            get
            {
                return this.reference.IsNew;
            }
        }

        public bool IsNewInWorkspace
        {
            get
            {
                return false;
            }
        }

        public DatabaseSession SqlSession
        {
            get { return this.reference.Session; }
        }

        public Roles Roles
        {
            get
            {
                return this.roles ?? (this.roles = this.reference.Session.GetOrCreateRoles(this.reference));
            }
        }

        internal Reference Reference
        {
            get
            {
                return this.reference;
            }
        }

        public IObject GetObject()
        {
            return this.allorsObject ?? (this.allorsObject = this.reference.Session.Database.ObjectFactory.Create(this));
        }

        public virtual void Delete()
        {
            this.AssertExist();

            foreach (var roleType in this.Class.RoleTypes)
            {
                if (roleType.ObjectType is IComposite)
                {
                    this.RemoveRole(roleType.RelationType);
                }
            }

            foreach (var associationType in this.Class.AssociationTypes)
            {
                var roleType = associationType.RoleType;

                if (associationType.IsMany)
                {
                    foreach (var association in this.SqlSession.GetAssociations(this, associationType))
                    {
                        var associationStrategy = this.SqlSession.GetOrCreateAssociationForExistingObject(association).Strategy;
                        if (roleType.IsMany)
                        {
                            associationStrategy.RemoveCompositeRole(roleType.RelationType, this.GetObject()); 
                        }
                        else
                        {
                            associationStrategy.RemoveCompositeRole(roleType.RelationType);
                        }
                    }
                }
                else
                {
                    var association = this.GetCompositeAssociation(associationType.RelationType);
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

            this.SqlSession.SessionCommands.DeleteObject(this);
            this.reference.Exists = false;

            this.SqlSession.SqlChangeSet.OnDeleted(this.ObjectId);
        }

        public virtual bool ExistRole(IRelationType relationType)
        {
            var roleType = relationType.RoleType;
            if (roleType.ObjectType is IUnit)
            {
                return this.ExistUnitRole(relationType);
            }

            if (roleType.IsMany)
            {
                return this.ExistCompositeRoles(relationType);
            }

            return this.ExistCompositeRole(relationType);
        }

        public virtual object GetRole(IRelationType relationType)
        {
            var roleType = relationType.RoleType;
            if (roleType.ObjectType is IUnit)
            {
                return this.GetUnitRole(relationType);
            }

            if (roleType.IsMany)
            {
                return this.GetCompositeRoles(relationType);
            }

            return this.GetCompositeRole(relationType);
        }

        public virtual void SetRole(IRelationType relationType, object value)
        {
            var roleType = relationType.RoleType;

            if (roleType.ObjectType is IUnit)
            {
                this.SetUnitRole(relationType, value);
            }
            else
            {
                if (roleType.IsMany)
                {
                    var roleExtent = value as Allors.Extent;
                    if (roleExtent == null)
                    {
                        var roleList = new ArrayList((ICollection)value);
                        roleExtent = (IObject[])roleList.ToArray(typeof(IObject));
                    }

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
            var roleType = relationType.RoleType;
            if (roleType.ObjectType is IUnit)
            {
                this.RemoveUnitRole(relationType);
            }
            else
            {
                if (roleType.IsMany)
                {
                    this.RemoveCompositeRoles(relationType);
                }
                else
                {
                    this.RemoveCompositeRole(relationType);
                }
            }
        }

        public virtual bool ExistUnitRole(IRelationType relationType)
        {
            return this.GetUnitRole(relationType) != null;
        }

        public virtual object GetUnitRole(IRelationType relationType)
        {
            this.AssertExist();

            return this.Roles.GetUnitRole(relationType.RoleType);
        }

        public virtual void SetUnitRole(IRelationType relationType, object role)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            RoleAssertions.UnitRoleChecks(this, roleType);

            if (role != null)
            {
                role = roleType.Normalize(role);
            }

            var oldUnit = this.GetUnitRole(relationType);
            if (!Equals(oldUnit, role))
            {
                this.Roles.SetUnitRole(roleType, role);
            }
        }

        public virtual void RemoveUnitRole(IRelationType relationType)
        {
            this.SetUnitRole(relationType, null);
        }

        public virtual bool ExistCompositeRole(IRelationType relationType)
        {
            return this.GetCompositeRole(relationType) != null;
        }

        public virtual IObject GetCompositeRole(IRelationType relationType)
        {
            this.AssertExist();
            var roleType = relationType.RoleType;
            var role = this.Roles.GetCompositeRole(roleType);
            return (role == null) ? null : this.SqlSession.GetOrCreateAssociationForExistingObject(role.Value).Strategy.GetObject();
        }

        public virtual void SetCompositeRole(IRelationType relationType, IObject newRoleObject)
        {
            var roleType = relationType.RoleType;
            
            if (newRoleObject == null)
            {
                this.RemoveCompositeRole(relationType);
                return;
            }

            this.AssertExist();

            RoleAssertions.CompositeRoleChecks(this, roleType, newRoleObject);

            var newRoleObjectId = (Strategy)newRoleObject.Strategy;

            this.Roles.SetCompositeRole(roleType, newRoleObjectId);
        }

        public virtual void RemoveCompositeRole(IRelationType relationType)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            RoleAssertions.CompositeRoleChecks(this, roleType);

            this.Roles.RemoveCompositeRole(roleType);
        }

        public virtual bool ExistCompositeRoles(IRelationType relationType)
        {
            return this.GetCompositeRoles(relationType).Count != 0;
        }

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
                var roleType = relationType.RoleType;

                RoleAssertions.CompositeRolesChecks(this, roleType, roleObject);

                var role = (Strategy)roleObject.Strategy;
                
                this.Roles.AddCompositeRole(roleType, role);
            }
        }

        public virtual void RemoveCompositeRole(IRelationType relationType, IObject roleObject)
        {
            this.AssertExist();
            
            if (roleObject != null)
            {
                var roleType = relationType.RoleType;
                RoleAssertions.CompositeRolesChecks(this, roleType, roleObject);
                
                var role = (Strategy)roleObject.Strategy;

                this.Roles.RemoveCompositeRole(roleType, role);
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
                var roleType = relationType.RoleType;
                
                // TODO: use CompositeRoles
                var previousRoles = new List<long>(this.Roles.GetCompositeRoles(roleType));
                var newRoles = new HashSet<long>();

                foreach (IObject roleObject in roleObjects)
                {
                    if (roleObject != null)
                    {
                        RoleAssertions.CompositeRolesChecks(this, roleType, roleObject);
                        var role = (Strategy)roleObject.Strategy;

                        if (!previousRoles.Contains(role.ObjectId))
                        {
                            this.Roles.AddCompositeRole(roleType, role);
                        }

                        newRoles.Add(role.ObjectId);
                    }
                }

                foreach (var previousRole in previousRoles)
                {
                    if (!newRoles.Contains(previousRole))
                    {
                        this.Roles.RemoveCompositeRole(roleType, this.SqlSession.GetOrCreateAssociationForExistingObject(previousRole).Strategy);
                    }
                }
            }
        }

        public virtual void RemoveCompositeRoles(IRelationType relationType)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            RoleAssertions.CompositeRoleChecks(this, roleType);

            var previousRoles = this.Roles.GetCompositeRoles(roleType);

            foreach (var previousRole in previousRoles)
            {
                this.Roles.RemoveCompositeRole(roleType, this.SqlSession.GetOrCreateAssociationForExistingObject(previousRole).Strategy);
            }
        }

        public virtual bool ExistAssociation(IRelationType relationType)
        {
            var associationType = relationType.AssociationType;
            if (associationType.IsMany)
            {
                return this.ExistCompositeAssociations(relationType);
            }

            return this.ExistCompositeAssociation(relationType);
        }

        public virtual object GetAssociation(IRelationType relationType)
        {
            var associationType = relationType.AssociationType;
            if (associationType.IsMany)
            {
                return this.GetCompositeAssociations(relationType);
            }

            return this.GetCompositeAssociation(relationType);
        }

        public virtual bool ExistCompositeAssociation(IRelationType relationType)
        {
            return this.GetCompositeAssociation(relationType) != null;
        }

        public virtual IObject GetCompositeAssociation(IRelationType relationType)
        {
            this.AssertExist();

            var associationType = relationType.AssociationType;
            var association = this.SqlSession.GetAssociation(this, associationType);

            return association == null ? null : association.Strategy.GetObject();
        }

        public virtual bool ExistCompositeAssociations(IRelationType relationType)
        {
            return this.GetCompositeAssociations(relationType).Count != 0;
        }

        public virtual Allors.Extent GetCompositeAssociations(IRelationType relationType)
        {
            this.AssertExist();
            var associationType = relationType.AssociationType;
            return new ExtentAssociations(this, associationType);
        }

        public override string ToString()
        {
            return "[" + this.Class + ":" + this.ObjectId + "]";
        }

        public virtual void Release()
        {
            this.roles = null;
        }

        internal int ExtentRolesGetCount(IRelationType relationType)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            return this.Roles.ExtentCount(roleType);
        }

        internal IObject ExtentRolesFirst(IRelationType relationType)
        {
            this.AssertExist();

            var roleType = relationType.RoleType;
            return this.Roles.ExtentFirst(this.SqlSession, roleType);
        }

        internal void ExtentRolesCopyTo(IRelationType relationType, Array array, int index)
        {
            var roleType = relationType.RoleType;
            this.Roles.ExtentCopyTo(this.SqlSession, roleType, array, index);
        }

        internal int ExtentIndexOf(IRelationType relationType, IObject value)
        {
            var roleType = relationType.RoleType;

            var i = 0;
            foreach (var oid in this.Roles.GetCompositeRoles(roleType))
            {
                if (oid.Equals(value.Id))
                {
                    return i;
                }
                ++i;
            }

            return -1;
        }

        internal IObject ExtentGetItem(IRelationType relationType, int index)
        {
            var roleType = relationType.RoleType;

            var i = 0;
            foreach (var oid in this.Roles.GetCompositeRoles(roleType))
            {
                if (i == index)
                {
                    return this.SqlSession.GetOrCreateAssociationForExistingObject(oid).Strategy.GetObject();
                }
                ++i;
            }

            return null;
        }

        internal bool ExtentRolesContains(IRelationType relationType, IObject value)
        {
            var roleType = relationType.RoleType;
            return this.Roles.ExtentContains(roleType, value.Id);
        }

        internal virtual long[] ExtentGetCompositeAssociations(IRelationType relationType)
        {
            this.AssertExist();

            var associationType = relationType.AssociationType;
            return this.SqlSession.GetAssociations(this, associationType);
        }

        protected virtual void AssertExist()
        {
            if (!this.reference.Exists)
            {
                throw new Exception("Object of class " + this.Class.Name + " with id " + this.ObjectId + " does not exist");
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Strategy.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;
    using Adapters;

    public class Strategy : IStrategy
    {
        private readonly Session session;
        private readonly ObjectId objectId;
        private IObject domainObject;
       
        internal Strategy(Session session, ObjectId objectId)
        {
            this.session = session;
            this.objectId = objectId;
        }

        ISession IStrategy.Session 
        {
            get
            {
                return this.session;
            }
        }
        
        public ObjectId ObjectId
        {
            get
            {
                return this.objectId;
            }
        }

        public ObjectVersion ObjectVersion
        {
            get
            {
                this.AssertNotDeleted();
                var cacheId = this.session.GetCacheId(objectId);
                return new ObjectVersionLong(cacheId);
            }
        }

        public bool IsDeleted 
        {
            get
            {
                return this.session.IsDeleted(this.objectId);
            }
        }

        public bool IsNewInSession 
        {
            get
            {
                return this.session.IsNew(this.ObjectId);
            }
        }

        public IClass Class
        {
            get
            {
                this.AssertNotDeleted();
                return this.session.GetObjectType(this.ObjectId);
            }
        }

        internal IClass UncheckedObjectType
        {
            get
            {
                return this.session.GetObjectType(this.ObjectId);
            }
        }

        public IObject GetObject()
        {
            this.AssertNotDeleted();
            return this.domainObject ?? (this.domainObject = this.session.Database.ObjectFactory.Create(this));
        }

        public void Delete()
        {
            this.AssertNotDeleted();

            foreach (var roleType in this.UncheckedObjectType.RoleTypes)
            {
                if (roleType.ObjectType is IUnit)
                {
                    this.session.RemoveUnitRole(this.objectId, roleType);
                }
                else
                {
                    if (roleType.IsMany)
                    {
                        switch (roleType.RelationType.Multiplicity)
                        {
                            case Multiplicity.OneToMany:
                                this.session.RemoveCompositeRolesOneToMany(this.objectId, roleType);
                                break;
                            case Multiplicity.ManyToMany:
                                this.session.RemoveCompositeRolesManyToMany(this.objectId, roleType);
                                break;
                        }
                    }
                    else
                    {
                        switch (roleType.RelationType.Multiplicity)
                        {
                            case Multiplicity.OneToOne:
                                this.session.RemoveCompositeRoleOneToOne(this.objectId, roleType);
                                break;
                            case Multiplicity.ManyToOne:
                                this.session.RemoveCompositeRoleManyToOne(this.objectId, roleType);
                                break;
                        }
                    }
                }
            }

            foreach (var associationType in this.UncheckedObjectType.AssociationTypes)
            {
                var roleType = associationType.RoleType;

                if (associationType.IsMany)
                {
                    switch (associationType.RelationType.Multiplicity)
                    {
                        case Multiplicity.ManyToOne:
                            foreach (var association in this.session.GetCompositeAssociationsManyToOne(this.objectId, associationType))
                            {
                                this.session.RemoveCompositeRoleManyToOne(association, roleType);
                            }

                            break;
                        case Multiplicity.ManyToMany:
                            foreach (var association in this.session.GetCompositeAssociationsManyToMany(this.objectId, associationType))
                            {
                                this.session.RemoveCompositeRoleManyToMany(association, roleType, this.objectId);
                            }

                            break;
                    }
                }
                else
                {
                    ObjectId association;
                    switch (associationType.RelationType.Multiplicity)
                    {
                        case Multiplicity.OneToOne:
                            association = this.session.GetCompositeAssociationOneToOne(this.objectId, associationType);
                            if (association != null)
                            {
                                this.session.RemoveCompositeRoleOneToOne(association, roleType);
                            }

                            break;
                        case Multiplicity.OneToMany:
                            association = this.session.GetCompositeAssociationOneToMany(this.objectId, associationType);
                            if (association != null)
                            {
                                this.session.RemoveCompositeRoleOneToMany(association, roleType, this.objectId);
                            }

                            break;
                    }
                }
            }

            this.session.Delete(this.objectId);
        }

        public virtual bool ExistRole(IRoleType roleType)
        {
            if (roleType.ObjectType is IUnit)
            {
                return this.ExistUnitRole(roleType);
            }

            if (roleType.IsMany)
            {
                return this.ExistCompositeRoles(roleType);
            }

            return this.ExistCompositeRole(roleType);
        }

        public virtual object GetRole(IRoleType roleType)
        {
            if (roleType.ObjectType is IUnit)
            {
                return this.GetUnitRole(roleType);
            }

            if (roleType.IsMany)
            {
                return this.GetCompositeRoles(roleType);
            }

            return this.GetCompositeRole(roleType);
        }

        public virtual void SetRole(IRoleType roleType, object value)
        {
            if (roleType.ObjectType is IUnit)
            {
                this.SetUnitRole(roleType, value);
            }
            else
            {
                if (roleType.IsMany)
                {
                    var roleExtent = value as Extent;
                    if (roleExtent == null)
                    {
                        // TODO: Use Linq
                        var roleList = new ArrayList((ICollection)value);
                        roleExtent = (IObject[])roleList.ToArray(typeof(IObject));
                    }

                    this.SetCompositeRoles(roleType, roleExtent);
                }
                else
                {
                    this.SetCompositeRole(roleType, (IObject)value);
                }
            }
        }

        public virtual void RemoveRole(IRoleType roleType)
        {
            if (roleType.ObjectType is IUnit)
            {
                this.RemoveUnitRole(roleType);
            }
            else
            {
                if (roleType.IsMany)
                {
                    this.RemoveCompositeRoles(roleType);
                }
                else
                {
                    this.RemoveCompositeRole(roleType);
                }
            }
        }
        
        public bool ExistUnitRole(IRoleType roleType)
        {
            this.AssertNotDeleted();

            return this.session.ExistUnitRole(this.objectId, roleType);
        }

        public object GetUnitRole(IRoleType roleType)
        {
            this.AssertNotDeleted();

            return this.session.GetUnitRole(this.ObjectId, roleType);
        }

        public void SetUnitRole(IRoleType roleType, object unit)
        {
            if (unit == null)
            {
                this.RemoveUnitRole(roleType);
                return;
            }

            this.AssertNotDeleted();

            RoleAssertions.UnitRoleChecks(this, roleType);
            unit = roleType.Normalize(unit);
            this.session.SetUnitRole(this.objectId, roleType, unit);
        }

        public void RemoveUnitRole(IRoleType roleType)
        {
            this.AssertNotDeleted();
            
            this.session.RemoveUnitRole(this.objectId, roleType);
        }

        public bool ExistCompositeRole(IRoleType roleType)
        {
            this.AssertNotDeleted();
            
            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    return this.session.ExistCompositeRoleOneToOne(this.objectId, roleType);
                case Multiplicity.ManyToOne:
                    return this.session.ExistCompositeRoleManyToOne(this.objectId, roleType);
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public IObject GetCompositeRole(IRoleType roleType)
        {
            this.AssertNotDeleted();
            
            ObjectId role;
            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    role = this.session.GetCompositeRoleOneToOne(this.objectId, roleType);
                    break;
                case Multiplicity.ManyToOne:
                    role = this.session.GetCompositeRoleManyToOne(this.objectId, roleType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }

            return role != null ? this.session.CreateStrategyForExistingObject(role).GetObject() : null;
        }

        public void SetCompositeRole(IRoleType roleType, IObject role)
        {
            this.AssertNotDeleted();

            if (role == null)
            {
                this.RemoveCompositeRole(roleType);
                return;
            }

            RoleAssertions.CompositeRoleChecks(this, roleType, role);

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    this.session.SetCompositeRoleOneToOne(this.objectId, roleType, role.Id);
                    return;
                case Multiplicity.ManyToOne:
                    this.session.SetCompositeRoleManyToOne(this.objectId, roleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public void RemoveCompositeRole(IRoleType roleType)
        {
            this.AssertNotDeleted();
            RoleAssertions.CompositeRoleChecks(this, roleType);

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    this.session.RemoveCompositeRoleOneToOne(this.objectId, roleType);
                    return;
                case Multiplicity.ManyToOne:
                    this.session.RemoveCompositeRoleManyToOne(this.objectId, roleType);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public bool ExistCompositeRoles(IRoleType roleType)
        {
            this.AssertNotDeleted();

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    return this.session.ExistCompositeRoleOneToMany(this.objectId, roleType);
                case Multiplicity.ManyToMany:
                    return this.session.ExistCompositeRoleManyToMany(this.objectId, roleType);
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public Extent GetCompositeRoles(IRoleType roleType)
        {
            this.AssertNotDeleted();

            return new AllorsExtentFilteredSql(this.session, this, roleType);
        }

        public void AddCompositeRole(IRoleType roleType, IObject role)
        {
            if (role == null)
            {
                return;
            }

            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, roleType, role);

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.AddCompositeRoleOneToMany(this.objectId, roleType, role.Id);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.AddCompositeRoleManyToMany(this.objectId, roleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public void RemoveCompositeRole(IRoleType roleType, IObject role)
        {
            if (role == null)
            {
                return;
            }

            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, roleType, role);

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.RemoveCompositeRoleOneToMany(this.objectId, roleType, role.Id);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.RemoveCompositeRoleManyToMany(this.objectId, roleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public void SetCompositeRoles(IRoleType roleType, Extent roles)
        {
            if (roles == null || roles.Count == 0)
            {
                this.RemoveCompositeRoles(roleType);
                return;
            }

            this.AssertNotDeleted();

            var roleObjectIds = new List<ObjectId>();
            foreach (IObject role in roles)
            {
                if (role != null)
                {
                    RoleAssertions.CompositeRolesChecks(this, roleType, role);
                    roleObjectIds.Add(role.Id);
                }
            }

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.SetCompositeRoleOneToMany(this.objectId, roleType, roleObjectIds.ToArray());
                    return;
                case Multiplicity.ManyToMany:
                    this.session.SetCompositeRoleManyToMany(this.objectId, roleType, roleObjectIds.ToArray());
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public void RemoveCompositeRoles(IRoleType roleType)
        {
            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, roleType);

            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.RemoveCompositeRolesOneToMany(this.objectId, roleType);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.RemoveCompositeRolesManyToMany(this.objectId, roleType);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }
        }

        public virtual bool ExistAssociation(IAssociationType associationType)
        {
            this.AssertNotDeleted();
            
            if (associationType.IsMany)
            {
                return this.ExistCompositeAssociations(associationType);
            }

            return this.ExistCompositeAssociation(associationType);
        }

        public virtual object GetAssociation(IAssociationType associationType)
        {
            this.AssertNotDeleted();

            if (associationType.IsMany)
            {
                return this.GetCompositeAssociations(associationType);
            }

            return this.GetCompositeAssociation(associationType);
        }

        public bool ExistCompositeAssociation(IAssociationType associationType)
        {
            this.AssertNotDeleted();

            switch (associationType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    return this.session.ExistCompositeAssociationOneToOne(this.objectId, associationType);
                case Multiplicity.OneToMany:
                    return this.session.ExistCompositeAssociationOneToMany(this.objectId, associationType);
                default:
                    throw new Exception("Unsupported multiplicity " + associationType.RelationType.Multiplicity);
            }
        }

        public IObject GetCompositeAssociation(IAssociationType associationType)
        {
            this.AssertNotDeleted();

            ObjectId association;
            switch (associationType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    association = this.session.GetCompositeAssociationOneToOne(this.objectId, associationType);
                    break;
                case Multiplicity.OneToMany:
                    association = this.session.GetCompositeAssociationOneToMany(this.objectId, associationType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + associationType.RelationType.Multiplicity);
            }

            return association != null ? new Strategy(this.session, association).GetObject() : null;
        }

        public bool ExistCompositeAssociations(IAssociationType associationType)
        {
            this.AssertNotDeleted();

            switch (associationType.RelationType.Multiplicity)
            {
                case Multiplicity.ManyToOne:
                    return this.session.ExistCompositeAssociationsManyToOne(this.objectId, associationType);
                case Multiplicity.ManyToMany:
                    return this.session.ExistCompositeAssociationsManyToMany(this.objectId, associationType);
                default:
                    throw new Exception("Unsupported multiplicity " + associationType.RelationType.Multiplicity);
            }
        }

        public Extent GetCompositeAssociations(IAssociationType associationType)
        {
            this.AssertNotDeleted();

            return new AllorsExtentFilteredSql(this.session, this, associationType);
        }

        public ObjectId[] ExtentGetCompositeRoles(IRoleType roleType)
        {
            ObjectId[] roles;
            switch (roleType.RelationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    roles = this.session.GetCompositeRolesOneToMany(this.objectId, roleType);
                    break;
                case Multiplicity.ManyToMany:
                    roles = this.session.GetCompositeRolesManyToMany(this.objectId, roleType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + roleType.RelationType.Multiplicity);
            }

            return roles;
        }

        public ObjectId[] ExtentGetCompositeAssociations(IAssociationType associationType)
        {
            ObjectId[] associations;
            switch (associationType.RelationType.Multiplicity)
            {
                case Multiplicity.ManyToOne:
                    associations = this.session.GetCompositeAssociationsManyToOne(this.objectId, associationType);
                    break;
                case Multiplicity.ManyToMany:
                    associations = this.session.GetCompositeAssociationsManyToMany(this.objectId, associationType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + associationType.RelationType.Multiplicity);
            }

            return associations;
        }

        private void AssertNotDeleted()
        {
            if (this.session.IsDeleted(this.ObjectId))
            {
                throw new Exception("Object with id " + this.ObjectId + " has been deleted.");
            }
        }
    }
}
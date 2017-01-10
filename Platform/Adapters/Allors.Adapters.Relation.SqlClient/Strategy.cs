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

namespace Allors.Adapters.Relation.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Adapters;

    using Allors.Meta;

    public class Strategy : IStrategy
    {
        private readonly Session session;
        private readonly long objectId;
        private IObject domainObject;
       
        internal Strategy(Session session, long objectId)
        {
            this.session = session;
            this.objectId = objectId;
        }

        ISession IStrategy.Session => this.session;

        public long ObjectId => this.objectId;

        public long ObjectVersion {
            get
            {
                this.AssertNotDeleted();
                var cacheId = this.session.GetCacheId(objectId);
                return cacheId;
            }
        }

        public bool IsDeleted => this.session.IsDeleted(this.objectId);

        public bool IsNewInSession => this.session.IsNew(this.ObjectId);

        public IClass Class
        {
            get
            {
                this.AssertNotDeleted();
                return this.session.GetObjectType(this.ObjectId);
            }
        }

        internal IClass UncheckedObjectType => this.session.GetObjectType(this.ObjectId);

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
                    long? association;
                    switch (associationType.RelationType.Multiplicity)
                    {
                        case Multiplicity.OneToOne:
                            association = this.session.GetCompositeAssociationOneToOne(this.objectId, associationType);
                            if (association != null)
                            {
                                this.session.RemoveCompositeRoleOneToOne(association.Value, roleType);
                            }

                            break;
                        case Multiplicity.OneToMany:
                            association = this.session.GetCompositeAssociationOneToMany(this.objectId, associationType);
                            if (association != null)
                            {
                                this.session.RemoveCompositeRoleOneToMany(association.Value, roleType, this.objectId);
                            }

                            break;
                    }
                }
            }

            this.session.Delete(this.objectId);
        }

        public virtual bool ExistRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                return this.ExistUnitRole(relationType);
            }

            if (relationType.RoleType.IsMany)
            {
                return this.ExistCompositeRoles(relationType);
            }

            return this.ExistCompositeRole(relationType);
        }

        public virtual object GetRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                return this.GetUnitRole(relationType);
            }

            if (relationType.RoleType.IsMany)
            {
                return this.GetCompositeRoles(relationType);
            }

            return this.GetCompositeRole(relationType);
        }

        public virtual void SetRole(IRelationType relationType, object value)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                this.SetUnitRole(relationType, value);
            }
            else
            {
                if (relationType.RoleType.IsMany)
                {
                    var roleExtent = value as Extent;
                    if (roleExtent == null)
                    {
                        // TODO: Use Linq
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
            if (relationType.RoleType.ObjectType is IUnit)
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
        
        public bool ExistUnitRole(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return this.session.ExistUnitRole(this.objectId, relationType.RoleType);
        }

        public object GetUnitRole(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return this.session.GetUnitRole(this.ObjectId, relationType.RoleType);
        }

        public void SetUnitRole(IRelationType relationType, object unit)
        {
            if (unit == null)
            {
                this.RemoveUnitRole(relationType);
                return;
            }

            this.AssertNotDeleted();

            RoleAssertions.UnitRoleChecks(this, relationType.RoleType);
            unit = relationType.RoleType.Normalize(unit);
            this.session.SetUnitRole(this.objectId, relationType.RoleType, unit);
        }

        public void RemoveUnitRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            
            this.session.RemoveUnitRole(this.objectId, relationType.RoleType);
        }

        public bool ExistCompositeRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            
            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    return this.session.ExistCompositeRoleOneToOne(this.objectId, relationType.RoleType);
                case Multiplicity.ManyToOne:
                    return this.session.ExistCompositeRoleManyToOne(this.objectId, relationType.RoleType);
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public IObject GetCompositeRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            
            long? role;
            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    role = this.session.GetCompositeRoleOneToOne(this.objectId, relationType.RoleType);
                    break;
                case Multiplicity.ManyToOne:
                    role = this.session.GetCompositeRoleManyToOne(this.objectId, relationType.RoleType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }

            return role != null ? new Strategy(this.session, role.Value).GetObject() : null;
        }

        public void SetCompositeRole(IRelationType relationType, IObject role)
        {
            this.AssertNotDeleted();

            if (role == null)
            {
                this.RemoveCompositeRole(relationType);
                return;
            }

            RoleAssertions.CompositeRoleChecks(this, relationType.RoleType, role);

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    this.session.SetCompositeRoleOneToOne(this.objectId, relationType.RoleType, role.Id);
                    return;
                case Multiplicity.ManyToOne:
                    this.session.SetCompositeRoleManyToOne(this.objectId, relationType.RoleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public void RemoveCompositeRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            RoleAssertions.CompositeRoleChecks(this, relationType.RoleType);

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    this.session.RemoveCompositeRoleOneToOne(this.objectId, relationType.RoleType);
                    return;
                case Multiplicity.ManyToOne:
                    this.session.RemoveCompositeRoleManyToOne(this.objectId, relationType.RoleType);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public bool ExistCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    return this.session.ExistCompositeRoleOneToMany(this.objectId, relationType.RoleType);
                case Multiplicity.ManyToMany:
                    return this.session.ExistCompositeRoleManyToMany(this.objectId, relationType.RoleType);
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public Extent GetCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return new AllorsExtentFilteredSql(this.session, this, relationType.RoleType);
        }

        public void AddCompositeRole(IRelationType relationType, IObject role)
        {
            if (role == null)
            {
                return;
            }

            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, role);

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.AddCompositeRoleOneToMany(this.objectId, relationType.RoleType, role.Id);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.AddCompositeRoleManyToMany(this.objectId, relationType.RoleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public void RemoveCompositeRole(IRelationType relationType, IObject role)
        {
            if (role == null)
            {
                return;
            }

            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, role);

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.RemoveCompositeRoleOneToMany(this.objectId, relationType.RoleType, role.Id);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.RemoveCompositeRoleManyToMany(this.objectId, relationType.RoleType, role.Id);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public void SetCompositeRoles(IRelationType relationType, Extent roles)
        {
            if (roles == null || roles.Count == 0)
            {
                this.RemoveCompositeRoles(relationType);
                return;
            }

            this.AssertNotDeleted();

            var roleObjectIds = new List<long>();
            foreach (IObject role in roles)
            {
                if (role != null)
                {
                    RoleAssertions.CompositeRolesChecks(this, relationType.RoleType, role);
                    roleObjectIds.Add(role.Id);
                }
            }

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.SetCompositeRoleOneToMany(this.objectId, relationType.RoleType, roleObjectIds.ToArray());
                    return;
                case Multiplicity.ManyToMany:
                    this.session.SetCompositeRoleManyToMany(this.objectId, relationType.RoleType, roleObjectIds.ToArray());
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public void RemoveCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();
            RoleAssertions.CompositeRolesChecks(this, relationType.RoleType);

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    this.session.RemoveCompositeRolesOneToMany(this.objectId, relationType.RoleType);
                    return;
                case Multiplicity.ManyToMany:
                    this.session.RemoveCompositeRolesManyToMany(this.objectId, relationType.RoleType);
                    return;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public virtual bool ExistAssociation(IRelationType relationType)
        {
            this.AssertNotDeleted();
            
            if (relationType.AssociationType.IsMany)
            {
                return this.ExistCompositeAssociations(relationType);
            }

            return this.ExistCompositeAssociation(relationType);
        }

        public virtual object GetAssociation(IRelationType relationType)
        {
            this.AssertNotDeleted();

            if (relationType.AssociationType.IsMany)
            {
                return this.GetCompositeAssociations(relationType);
            }

            return this.GetCompositeAssociation(relationType);
        }

        public bool ExistCompositeAssociation(IRelationType relationType)
        {
            this.AssertNotDeleted();

            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    return this.session.ExistCompositeAssociationOneToOne(this.objectId, relationType.AssociationType);
                case Multiplicity.OneToMany:
                    return this.session.ExistCompositeAssociationOneToMany(this.objectId, relationType.AssociationType);
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public IObject GetCompositeAssociation(IRelationType relationType)
        {
            this.AssertNotDeleted();

            long? association;
            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToOne:
                    association = this.session.GetCompositeAssociationOneToOne(this.objectId, relationType.AssociationType);
                    break;
                case Multiplicity.OneToMany:
                    association = this.session.GetCompositeAssociationOneToMany(this.objectId, relationType.AssociationType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }

            return association != null ? new Strategy(this.session, association.Value).GetObject() : null;
        }

        public bool ExistCompositeAssociations(IRelationType relationType)
        {
            this.AssertNotDeleted();

            switch (relationType.Multiplicity)
            {
                case Multiplicity.ManyToOne:
                    return this.session.ExistCompositeAssociationsManyToOne(this.objectId, relationType.AssociationType);
                case Multiplicity.ManyToMany:
                    return this.session.ExistCompositeAssociationsManyToMany(this.objectId, relationType.AssociationType);
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }
        }

        public Extent GetCompositeAssociations(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return new AllorsExtentFilteredSql(this.session, this, relationType.AssociationType);
        }

        public long[] ExtentGetCompositeRoles(IRelationType relationType)
        {
            long[] roles;
            switch (relationType.Multiplicity)
            {
                case Multiplicity.OneToMany:
                    roles = this.session.GetCompositeRolesOneToMany(this.objectId, relationType.RoleType);
                    break;
                case Multiplicity.ManyToMany:
                    roles = this.session.GetCompositeRolesManyToMany(this.objectId, relationType.RoleType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
            }

            return roles;
        }

        public long[] ExtentGetCompositeAssociations(IRelationType relationType)
        {
            long[] associations;
            switch (relationType.Multiplicity)
            {
                case Multiplicity.ManyToOne:
                    associations = this.session.GetCompositeAssociationsManyToOne(this.objectId, relationType.AssociationType);
                    break;
                case Multiplicity.ManyToMany:
                    associations = this.session.GetCompositeAssociationsManyToMany(this.objectId, relationType.AssociationType);
                    break;
                default:
                    throw new Exception("Unsupported multiplicity " + relationType.Multiplicity);
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
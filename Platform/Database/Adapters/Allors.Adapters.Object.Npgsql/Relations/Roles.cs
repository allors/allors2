// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Roles.cs" company="Allors bvba">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Collections.Generic;

    using Allors.Adapters.Database.Caching;
    using Allors.Adapters.Database.Npgsql;
    using Allors.Meta;

    public class Roles
    {
        public readonly Reference Reference;

        private ICachedObject cachedObject;

        private Dictionary<IRoleType, object> originalRoleByRoleType;

        private Dictionary<IRoleType, object> modifiedRoleByRoleType;
        private Dictionary<IRoleType, CompositeRoles> modifiedRolesByRoleType;

        private HashSet<IRoleType> requireFlushRoles;

        public Roles(Reference reference)
        {
            this.Reference = reference;
        }

        public ICachedObject CachedObject
        {
            get
            {
                if (this.cachedObject == null && !this.Reference.IsNew)
                {
                    var cache = this.Reference.Session.Database.Cache;
                    this.cachedObject = cache.GetOrCreateCachedObject(this.Reference.ObjectType, this.Reference.ObjectId, this.Reference.Version);
                }

                return this.cachedObject;
            }
        }

        public Dictionary<IRoleType, object> OriginalRoleByRoleType
        {
            get
            {
                return this.originalRoleByRoleType ?? (this.originalRoleByRoleType = new Dictionary<IRoleType, object>());
            }
        }

        public Dictionary<IRoleType, object> ModifiedRoleByRoleType
        {
            get
            {
                return this.modifiedRoleByRoleType ?? (this.modifiedRoleByRoleType = new Dictionary<IRoleType, object>());
            }
        }

        public Dictionary<IRoleType, CompositeRoles> ModifiedRolesByRoleType
        {
            get
            {
                return this.modifiedRolesByRoleType ?? (this.modifiedRolesByRoleType = new Dictionary<IRoleType, CompositeRoles>());
            }
        }

        public HashSet<IRoleType> RequireFlushRoles
        {
            get
            {
                return this.requireFlushRoles ?? (this.requireFlushRoles = new HashSet<IRoleType>());
            }
        }

        public object GetUnitRole(IRoleType roleType)
        {
            object role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        this.Reference.Session.SessionCommands.GetUnitRoles(this);
                        this.cachedObject.TryGetValue(roleType, out role);
                    }
                }
            }

            return role;
        }

        public void SetUnitRole(IRoleType roleType, object role)
        {
            this.Reference.Session.SqlChangeSet.OnChangingUnitRole(this.Reference.ObjectId, roleType);

            this.SetOriginal(roleType, role);

            this.ModifiedRoleByRoleType[roleType] = role;
            this.RequireFlushRoles.Add(roleType);

            this.Reference.Session.RequireFlush(this.Reference, this);
        }

        public virtual long? GetCompositeRole(IRoleType roleType)
        {
            object role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        this.Reference.Session.SessionCommands.GetCompositeRole(this, roleType);
                        this.cachedObject.TryGetValue(roleType, out role);
                    }
                }
            }

            return (long?)role;
        }

        public void SetCompositeRole(IRoleType roleType, Strategy newRoleStrategy)
        {
            var previousRole = this.GetCompositeRole(roleType);
            var newRole = newRoleStrategy?.Reference.ObjectId;

            this.Reference.Session.SqlChangeSet.OnChangingCompositeRole(this.Reference.ObjectId, roleType, previousRole, newRole);

            if (newRole != null && !newRole.Equals(previousRole))
            {
                if (roleType.AssociationType.IsOne)
                {
                    if (previousRole != null)
                    {
                        var previousRoleStrategy = this.Reference.Session.GetOrCreateAssociationForExistingObject(previousRole.Value).Strategy;
                        var previousAssociation = previousRoleStrategy.GetCompositeAssociation(roleType.RelationType);
                        if (previousAssociation != null)
                        {
                            previousAssociation.Strategy.RemoveCompositeRole(roleType.RelationType);
                        }
                    }

                    var newRoleAssociation = newRoleStrategy.GetCompositeAssociation(roleType.RelationType);
                    if (newRoleAssociation != null && !newRoleAssociation.Id.Equals(this.Reference.ObjectId))
                    {
                        newRoleAssociation.Strategy.RemoveCompositeRole(roleType.RelationType);
                    }

                    this.Reference.Session.SetAssociation(this.Reference, newRoleStrategy, roleType.AssociationType);
                }
                else
                {
                    if (previousRole != null)
                    {
                        var previousRoleReference = this.Reference.Session.GetOrCreateAssociationForExistingObject(previousRole.Value);
                        this.Reference.Session.RemoveAssociation(this.Reference, previousRoleReference, roleType.AssociationType);
                    }
                }
            }

            if (previousRole != null && !previousRole.Equals(newRole))
            {
                this.Reference.Session.TriggerFlush(previousRole.Value, roleType.AssociationType);
            }

            if ((newRole == null && previousRole != null) ||
                (newRole != null && !newRole.Equals(previousRole)))
            {
                this.SetOriginal(roleType, newRole);
                this.ModifiedRoleByRoleType[roleType] = newRole;
                this.AddRequiresFlushRoleType(roleType);
                this.Reference.Session.RequireFlush(this.Reference, this);

                if (newRole != null)
                {
                    if (roleType.AssociationType.IsMany)
                    {
                        this.Reference.Session.AddAssociation(this.Reference, newRoleStrategy.Reference, roleType.AssociationType);
                        this.Reference.Session.TriggerFlush(newRole.Value, roleType.AssociationType);
                    }
                }
            }
        }

        public void RemoveCompositeRole(IRoleType roleType)
        {
            var currentRole = this.GetCompositeRole(roleType);
            if (currentRole != null)
            {
                var currentRoleStrategy = this.Reference.Session.GetOrCreateAssociationForExistingObject(currentRole.Value).Strategy;

                this.Reference.Session.SqlChangeSet.OnChangingCompositeRole(this.Reference.ObjectId, roleType, currentRoleStrategy?.ObjectId, null);

                if (roleType.AssociationType.IsOne)
                {
                    this.Reference.Session.SetAssociation(null, currentRoleStrategy, roleType.AssociationType);
                }
                else
                {
                    this.Reference.Session.RemoveAssociation(this.Reference, currentRoleStrategy.Reference, roleType.AssociationType);
                    this.Reference.Session.TriggerFlush(currentRole.Value, roleType.AssociationType);
                }

                this.SetOriginal(roleType, null);
                this.ModifiedRoleByRoleType[roleType] = null;
                this.AddRequiresFlushRoleType(roleType);
                this.Reference.Session.RequireFlush(this.Reference, this);
            }
        }

        public virtual IEnumerable<long> GetCompositeRoles(IRoleType roleType)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                return compositeRoles.ObjectIds;
            }

            return this.GetNonModifiedCompositeRoles(roleType);
        }

        public void AddCompositeRole(IRoleType roleType, Strategy role)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType == null || !this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                compositeRoles = new CompositeRoles(this.GetCompositeRoles(roleType));
                this.ModifiedRolesByRoleType[roleType] = compositeRoles;
            }

            this.Reference.Session.SqlChangeSet.OnChangingCompositesRole(this.Reference.ObjectId, roleType, role);

            if (!compositeRoles.Contains(role.ObjectId))
            {
                compositeRoles.Add(role.ObjectId);

                if (roleType.AssociationType.IsOne)
                {
                    var previousAssociationObject = role.GetCompositeAssociation(roleType.RelationType);
                    var previousAssociation = previousAssociationObject != null ? (Strategy)previousAssociationObject.Strategy : null;
                    if (previousAssociation != null && !previousAssociation.ObjectId.Equals(this.Reference.ObjectId))
                    {
                        this.Reference.Session.SqlChangeSet.OnChangingCompositesRole(previousAssociation.ObjectId, roleType, null);

                        previousAssociation.RemoveCompositeRole(roleType.RelationType, role.GetObject());
                    }

                    this.Reference.Session.SetAssociation(this.Reference, role, roleType.AssociationType);
                }
                else
                {
                    this.Reference.Session.AddAssociation(this.Reference, role.Reference, roleType.AssociationType);
                    this.Reference.Session.TriggerFlush(role.ObjectId, roleType.AssociationType);
                }

                this.AddRequiresFlushRoleType(roleType);
                this.Reference.Session.RequireFlush(this.Reference, this);
            }
        }

        public void RemoveCompositeRole(IRoleType roleType, Strategy role)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType == null || !this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                compositeRoles = new CompositeRoles(this.GetCompositeRoles(roleType));
                this.ModifiedRolesByRoleType[roleType] = compositeRoles;
            }

            if (compositeRoles.Contains(role.ObjectId))
            {
                this.Reference.Session.SqlChangeSet.OnChangingCompositesRole(this.Reference.ObjectId, roleType, role);

                compositeRoles.Remove(role.ObjectId);

                if (roleType.AssociationType.IsOne)
                {
                    this.Reference.Session.SetAssociation(null, role, roleType.AssociationType);
                }
                else
                {
                    this.Reference.Session.RemoveAssociation(this.Reference, role.Reference, roleType.AssociationType);
                    this.Reference.Session.TriggerFlush(role.ObjectId, roleType.AssociationType);
                }

                this.AddRequiresFlushRoleType(roleType);
                this.Reference.Session.RequireFlush(this.Reference, this);
            }
        }

        public void AddRequiresFlushRoleType(IRoleType roleType)
        {
            if (this.requireFlushRoles == null)
            {
                this.requireFlushRoles = new HashSet<IRoleType>();
            }

            this.requireFlushRoles.Add(roleType);
        }

        public void Flush(IFlush flush)
        {
            IRoleType unitRole = null;
            List<IRoleType> unitRoles = null;
            foreach (var flushRole in this.RequireFlushRoles)
            {
                if (flushRole.ObjectType is IUnit)
                {
                    if (unitRole == null)
                    {
                        unitRole = flushRole;
                    }
                    else
                    {
                        if (unitRoles == null)
                        {
                            unitRoles = new List<IRoleType> { unitRole };
                        }

                        unitRoles.Add(flushRole);
                    }
                }
                else
                {
                    if (flushRole.IsOne)
                    {
                        var role = this.GetCompositeRole(flushRole);
                        if (role != null)
                        {
                            flush.SetCompositeRole(this.Reference, flushRole, role.Value);
                        }
                        else
                        {
                            flush.ClearCompositeAndCompositesRole(this.Reference, flushRole);
                        }
                    }
                    else
                    {
                        var roles = this.ModifiedRolesByRoleType[flushRole];
                        roles.Flush(flush, this, flushRole);                        
                    }
                }
            }

            if (unitRoles != null)
            {
                unitRoles.Sort();
                flush.SetUnitRoles(this, unitRoles);
            }
            else if (unitRole != null)
            {
                flush.SetUnitRole(this.Reference, unitRole, this.GetUnitRole(unitRole));
            }

            this.requireFlushRoles = null;
        }

        public int ExtentCount(IRoleType roleType)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                return compositeRoles.Count;
            }

            return this.GetNonModifiedCompositeRoles(roleType).Length;
        }

        public IObject ExtentFirst(DatabaseSession session, IRoleType roleType)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                var objectId = compositeRoles.First;
                return objectId == null ? null : session.GetOrCreateAssociationForExistingObject(objectId.Value).Strategy.GetObject();
            }

            var nonModifiedCompositeRoles = this.GetNonModifiedCompositeRoles(roleType);
            if (nonModifiedCompositeRoles.Length > 0)
            {
                return session.GetOrCreateAssociationForExistingObject(nonModifiedCompositeRoles[0]).Strategy.GetObject();
            }

            return null;
        }

        public void ExtentCopyTo(DatabaseSession session, IRoleType roleType, Array array, int index)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                var i = 0;
                foreach (var objectId in compositeRoles.ObjectIds)
                {
                    array.SetValue(session.GetOrCreateAssociationForExistingObject(objectId).Strategy.GetObject(), index + i);
                    ++i;
                }

                return;
            }

            var nonModifiedCompositeRoles = this.GetNonModifiedCompositeRoles(roleType);
            for (var i = 0; i < nonModifiedCompositeRoles.Length; i++)
            {
                var objectId = nonModifiedCompositeRoles[i];
                array.SetValue(session.GetOrCreateAssociationForExistingObject(objectId).Strategy.GetObject(), index + i);
            }
        }

        public bool ExtentContains(IRoleType roleType, long objectId)
        {
            CompositeRoles compositeRoles;
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out compositeRoles))
            {
                return compositeRoles.Contains(objectId);
            }

            return Array.IndexOf(this.GetNonModifiedCompositeRoles(roleType), objectId) >= 0;
        }

        private long[] GetNonModifiedCompositeRoles(IRoleType roleType)
        {
            if (!this.Reference.IsNew)
            {
                object roleOut;
                if (this.CachedObject != null && this.CachedObject.TryGetValue(roleType, out roleOut))
                {
                    return (long[])roleOut;
                }

                this.Reference.Session.SessionCommands.GetCompositesRole(this, roleType);
                this.cachedObject.TryGetValue(roleType, out roleOut);
                var role = (long[])roleOut;
                return role;
            }

            return Database.EmptyObjectIds;
        }

        private void SetOriginal(IRoleType roleType, object role)
        {
            if (!this.OriginalRoleByRoleType.ContainsKey(roleType))
            {
                this.OriginalRoleByRoleType[roleType] = role;
            }
        }
    }
}
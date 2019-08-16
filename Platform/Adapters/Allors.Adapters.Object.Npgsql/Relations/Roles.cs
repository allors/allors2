// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Roles.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Adapters.Object.Npgsql.Caching;

    using Allors;
    using Allors.Meta;

    public sealed class Roles
    {
        internal readonly Reference Reference;

        private ICachedObject cachedObject;

        private Dictionary<IRoleType, object> originalRoleByRoleType;
        private Dictionary<IRoleType, object> modifiedRoleByRoleType;
        private Dictionary<IRoleType, CompositesRole> modifiedCompositesRoleByRoleType;

        private HashSet<IRoleType> requireFlushRoles;

        internal Roles(Reference reference)
        {
            this.Reference = reference;
        }

        internal ICachedObject CachedObject
        {
            get
            {
                if (this.cachedObject == null && !this.Reference.IsNew)
                {
                    var cache = this.Reference.Session.Database.Cache;
                    this.cachedObject = cache.GetOrCreateCachedObject(this.Reference.Class, this.Reference.ObjectId, this.Reference.Version);
                }

                return this.cachedObject;
            }
        }

        internal Dictionary<IRoleType, object> ModifiedRoleByRoleType =>
            this.modifiedRoleByRoleType ?? (this.modifiedRoleByRoleType = new Dictionary<IRoleType, object>());

        private Dictionary<IRoleType, object> OriginalRoleByRoleType =>
            this.originalRoleByRoleType ?? (this.originalRoleByRoleType = new Dictionary<IRoleType, object>());

        private HashSet<IRoleType> RequireFlushRoles =>
            this.requireFlushRoles ?? (this.requireFlushRoles = new HashSet<IRoleType>());

        private Dictionary<IRoleType, CompositesRole> ModifiedRolesByRoleType =>
            this.modifiedCompositesRoleByRoleType ?? (this.modifiedCompositesRoleByRoleType = new Dictionary<IRoleType, CompositesRole>());

        internal bool TryGetUnitRole(IRoleType roleType, out object role)
        {
            role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal object GetUnitRole(IRoleType roleType)
        {
            object role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        this.Reference.Session.Commands.GetUnitRoles(this);
                        this.cachedObject.TryGetValue(roleType, out role);
                    }
                }
            }

            return role;
        }

        internal void SetUnitRole(IRoleType roleType, object role)
        {
            this.Reference.Session.State.ChangeSet.OnChangingUnitRole(this.Reference.ObjectId, roleType);

            this.SetOriginal(roleType, role);

            this.ModifiedRoleByRoleType[roleType] = role;
            this.RequireFlushRoles.Add(roleType);

            this.Reference.Session.RequireFlush(this.Reference, this);
        }

        internal bool TryGetCompositeRole(IRoleType roleType, out long? roleId)
        {
            roleId = null;

            object role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        return false;
                    }
                }
            }

            roleId = (long?)role;
            return true;
        }

        internal long? GetCompositeRole(IRoleType roleType)
        {
            object role = null;
            if (this.modifiedRoleByRoleType == null || !this.modifiedRoleByRoleType.TryGetValue(roleType, out role))
            {
                if (this.CachedObject == null || !this.CachedObject.TryGetValue(roleType, out role))
                {
                    if (!this.Reference.IsNew)
                    {
                        this.Reference.Session.Commands.GetCompositeRole(this, roleType);
                        this.cachedObject.TryGetValue(roleType, out role);
                    }
                }
            }

            return (long?)role;
        }

        internal void SetCompositeRole(IRoleType roleType, Strategy newRoleStrategy)
        {
            var previousRole = this.GetCompositeRole(roleType);
            var newRole = newRoleStrategy?.Reference.ObjectId;

            if (newRole != null && !newRole.Equals(previousRole))
            {
                this.Reference.Session.State.ChangeSet.OnChangingCompositeRole(this.Reference.ObjectId, roleType, previousRole, newRole);

                if (roleType.AssociationType.IsOne)
                {
                    if (previousRole != null)
                    {
                        var previousRoleStrategy = this.Reference.Session.State.GetOrCreateReferenceForExistingObject(previousRole.Value, this.Reference.Session).Strategy;
                        var previousAssociation = previousRoleStrategy.GetCompositeAssociation(roleType.RelationType);
                        previousAssociation?.Strategy.RemoveCompositeRole(roleType.RelationType);
                    }

                    var newRoleAssociation = newRoleStrategy.GetCompositeAssociation(roleType.RelationType);
                    if (newRoleAssociation != null && !newRoleAssociation.Id.Equals(this.Reference.ObjectId))
                    {
                        this.Reference.Session.State.ChangeSet.OnChangingCompositeRole(newRoleAssociation.Id, roleType, previousRole, null);

                        newRoleAssociation.Strategy.RemoveCompositeRole(roleType.RelationType);
                    }

                    this.Reference.Session.SetAssociation(this.Reference, newRoleStrategy, roleType.AssociationType);
                }
                else
                {
                    if (previousRole != null)
                    {
                        var previousRoleReference = this.Reference.Session.State.GetOrCreateReferenceForExistingObject(previousRole.Value, this.Reference.Session);
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

        internal void RemoveCompositeRole(IRoleType roleType)
        {
            var currentRole = this.GetCompositeRole(roleType);
            if (currentRole != null)
            {
                var currentRoleStrategy = this.Reference.Session.State.GetOrCreateReferenceForExistingObject(currentRole.Value, this.Reference.Session).Strategy;

                this.Reference.Session.State.ChangeSet.OnChangingCompositeRole(this.Reference.ObjectId, roleType, currentRoleStrategy?.ObjectId, null);

                if (roleType.AssociationType.IsOne)
                {
                    this.Reference.Session.SetAssociation(null, currentRoleStrategy, roleType.AssociationType);
                }
                else
                {
                    this.Reference.Session.RemoveAssociation(this.Reference, currentRoleStrategy?.Reference, roleType.AssociationType);
                    this.Reference.Session.TriggerFlush(currentRole.Value, roleType.AssociationType);
                }

                this.SetOriginal(roleType, null);
                this.ModifiedRoleByRoleType[roleType] = null;
                this.AddRequiresFlushRoleType(roleType);
                this.Reference.Session.RequireFlush(this.Reference, this);
            }
        }

        internal bool TryGetCompositesRole(IRoleType roleType, out IEnumerable<long> roleIds)
        {
            roleIds = null;

            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                roleIds = compositesRole.ObjectIds;
            }

            return false;
        }

        internal IEnumerable<long> GetCompositesRole(IRoleType roleType)
        {
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                return compositesRole.ObjectIds;
            }

            return this.GetNonModifiedCompositeRoles(roleType);
        }

        internal void AddCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this.ModifiedRolesByRoleType == null || !this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                compositesRole = new CompositesRole(this.GetCompositesRole(roleType));
                this.ModifiedRolesByRoleType[roleType] = compositesRole;
            }

            this.Reference.Session.State.ChangeSet.OnChangingCompositesRole(this.Reference.ObjectId, roleType, role);

            if (!compositesRole.Contains(role.ObjectId))
            {
                compositesRole.Add(role.ObjectId);

                if (roleType.AssociationType.IsOne)
                {
                    var previousAssociationObject = role.GetCompositeAssociation(roleType.RelationType);
                    var previousAssociation = (Strategy)previousAssociationObject?.Strategy;
                    if (previousAssociation != null && !previousAssociation.ObjectId.Equals(this.Reference.ObjectId))
                    {
                        this.Reference.Session.State.ChangeSet.OnChangingCompositesRole(previousAssociation.ObjectId, roleType, null);

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

        internal void RemoveCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this.ModifiedRolesByRoleType == null || !this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                compositesRole = new CompositesRole(this.GetCompositesRole(roleType));
                this.ModifiedRolesByRoleType[roleType] = compositesRole;
            }

            if (compositesRole.Contains(role.ObjectId))
            {
                this.Reference.Session.State.ChangeSet.OnChangingCompositesRole(this.Reference.ObjectId, roleType, role);

                compositesRole.Remove(role.ObjectId);

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

        internal void Flush(Flush flush)
        {
            IRoleType unitRole = null;
            List<IRoleType> unitRoles = null;
            foreach (var flushRole in this.RequireFlushRoles)
            {
                if (flushRole.ObjectType.IsUnit)
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
                this.Reference.Session.Commands.SetUnitRoles(this, unitRoles);
            }
            else if (unitRole != null)
            {
                flush.SetUnitRole(this.Reference, unitRole, this.GetUnitRole(unitRole));
            }

            this.requireFlushRoles = null;
        }

        internal int ExtentCount(IRoleType roleType)
        {
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                return compositesRole.Count;
            }

            return this.GetNonModifiedCompositeRoles(roleType).Length;
        }

        internal IObject ExtentFirst(Session session, IRoleType roleType)
        {
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                var objectId = compositesRole.First;
                return objectId == null ? null : session.State.GetOrCreateReferenceForExistingObject(objectId.Value, session).Strategy.GetObject();
            }

            var nonModifiedCompositeRoles = this.GetNonModifiedCompositeRoles(roleType);
            if (nonModifiedCompositeRoles.Length > 0)
            {
                return session.State.GetOrCreateReferenceForExistingObject(nonModifiedCompositeRoles[0], session).Strategy.GetObject();
            }

            return null;
        }

        internal void ExtentCopyTo(Session session, IRoleType roleType, Array array, int index)
        {
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                var i = 0;
                foreach (var objectId in compositesRole.ObjectIds)
                {
                    array.SetValue(session.State.GetOrCreateReferenceForExistingObject(objectId, session).Strategy.GetObject(), index + i);
                    ++i;
                }

                return;
            }

            var nonModifiedCompositeRoles = this.GetNonModifiedCompositeRoles(roleType);
            for (var i = 0; i < nonModifiedCompositeRoles.Length; i++)
            {
                var objectId = nonModifiedCompositeRoles[i];
                array.SetValue(session.State.GetOrCreateReferenceForExistingObject(objectId, session).Strategy.GetObject(), index + i);
            }
        }

        internal bool ExtentContains(IRoleType roleType, long objectId)
        {
            if (this.ModifiedRolesByRoleType != null && this.ModifiedRolesByRoleType.TryGetValue(roleType, out var compositesRole))
            {
                return compositesRole.Contains(objectId);
            }

            return Array.IndexOf(this.GetNonModifiedCompositeRoles(roleType), objectId) >= 0;
        }

        private void AddRequiresFlushRoleType(IRoleType roleType)
        {
            if (this.requireFlushRoles == null)
            {
                this.requireFlushRoles = new HashSet<IRoleType>();
            }

            this.requireFlushRoles.Add(roleType);
        }

        private long[] GetNonModifiedCompositeRoles(IRoleType roleType)
        {
            if (!this.Reference.IsNew)
            {
                if (this.CachedObject != null && this.CachedObject.TryGetValue(roleType, out var roleOut))
                {
                    return (long[])roleOut;
                }

                this.Reference.Session.Commands.GetCompositesRole(this, roleType);
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

        private class CompositesRole
        {
            private readonly HashSet<long> baseline;
            private HashSet<long> original;
            private HashSet<long> added;
            private HashSet<long> removed;

            internal CompositesRole(IEnumerable<long> compositeRoles)
            {
                this.baseline = new HashSet<long>(compositeRoles);
            }

            internal HashSet<long> ObjectIds
            {
                get
                {
                    if ((this.removed == null || this.removed.Count == 0) && (this.added == null || this.added.Count == 0))
                    {
                        return this.baseline;
                    }

                    var merged = new HashSet<long>(this.baseline);
                    if (this.removed != null && this.removed.Count > 0)
                    {
                        merged.ExceptWith(this.removed);
                    }

                    if (this.added != null && this.added.Count > 0)
                    {
                        merged.UnionWith(this.added);
                    }

                    return merged;
                }
            }

            internal int Count
            {
                get
                {
                    var addedCount = this.added?.Count ?? 0;
                    var removedCount = this.removed?.Count ?? 0;

                    return this.baseline.Count + addedCount - removedCount;
                }
            }

            internal long? First
            {
                get
                {
                    if (this.removed == null || this.removed.Count == 0)
                    {
                        if (this.baseline.Count > 0)
                        {
                            return this.baseline.First();
                        }

                        if (this.added != null && this.added.Count > 0)
                        {
                            return this.added.First();
                        }

                        return null;
                    }

                    var roles = this.ObjectIds;
                    if (roles.Count > 0)
                    {
                        return roles.First();
                    }

                    return null;
                }
            }

            internal bool Contains(long objectId)
            {
                if (this.removed != null && this.removed.Contains(objectId))
                {
                    return false;
                }

                return this.baseline.Contains(objectId) || (this.added != null && this.added.Contains(objectId));
            }

            internal void Add(long objectId)
            {
                if (this.original == null)
                {
                    this.original = new HashSet<long>(this.baseline);
                }

                if (this.removed != null && this.removed.Contains(objectId))
                {
                    this.removed.Remove(objectId);
                    return;
                }

                if (!this.baseline.Contains(objectId))
                {
                    if (this.added == null)
                    {
                        this.added = new HashSet<long>();
                    }

                    this.added.Add(objectId);
                }
            }

            internal void Remove(long objectId)
            {
                if (this.original == null)
                {
                    this.original = new HashSet<long>(this.baseline);
                }

                if (this.added != null && this.added.Contains(objectId))
                {
                    this.added.Remove(objectId);
                    return;
                }

                if (this.baseline.Contains(objectId))
                {
                    if (this.removed == null)
                    {
                        this.removed = new HashSet<long>();
                    }

                    this.removed.Add(objectId);
                }
            }

            internal void Flush(Flush flush, Roles roles, IRoleType roleType)
            {
                if (this.Count == 0)
                {
                    flush.ClearCompositeAndCompositesRole(roles.Reference, roleType);
                }
                else
                {
                    if (this.added != null && this.added.Count > 0)
                    {
                        flush.AddCompositeRole(roles.Reference, roleType, this.added);
                    }

                    if (this.removed != null && this.removed.Count > 0)
                    {
                        flush.RemoveCompositeRole(roles.Reference, roleType, this.removed);
                    }
                }

                if (this.added != null)
                {
                    this.baseline.UnionWith(this.added);
                }

                if (this.removed != null)
                {
                    this.baseline.ExceptWith(this.removed);
                }

                this.added = null;
                this.removed = null;
            }
        }
    }
}
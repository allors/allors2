// <copyright file="Prefetcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Meta;

    internal class Prefetcher
    {
        private Dictionary<IClass, ICommand> prefetchUnitRolesByClass;
        private Dictionary<IRoleType, ICommand> prefetchCompositeRoleByRoleType;
        private Dictionary<IRoleType, ICommand> prefetchCompositesRoleByRoleType;
        private Dictionary<IAssociationType, ICommand> prefetchCompositeAssociationByAssociationType;

        public Prefetcher(Transaction transaction) => this.Transaction = transaction;

        public Transaction Transaction { get; }

        public Database Database => this.Transaction.Database;

        private Dictionary<IClass, ICommand> PrefetchUnitRolesByClass => this.prefetchUnitRolesByClass ??= new Dictionary<IClass, ICommand>();

        private Dictionary<IRoleType, ICommand> PrefetchCompositeRoleByRoleType => this.prefetchCompositeRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

        private Dictionary<IRoleType, ICommand> PrefetchCompositesRoleByRoleType => this.prefetchCompositesRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

        private Dictionary<IAssociationType, ICommand> PrefetchCompositeAssociationByAssociationType => this.prefetchCompositeAssociationByAssociationType ??= new Dictionary<IAssociationType, ICommand>();

        internal HashSet<Reference> GetReferencesForPrefetching(IEnumerable<long> objectIds)
        {
            var references = new HashSet<Reference>();

            HashSet<long> referencesToInstantiate = null;
            foreach (var objectId in objectIds)
            {
                this.Transaction.State.ReferenceByObjectId.TryGetValue(objectId, out var reference);
                if (reference != null && reference.ExistsKnown && !reference.IsUnknownVersion)
                {
                    if (reference.Exists && !reference.IsNew)
                    {
                        references.Add(reference);
                    }
                }
                else
                {
                    if (referencesToInstantiate == null)
                    {
                        referencesToInstantiate = new HashSet<long>();
                    }

                    referencesToInstantiate.Add(objectId);
                }
            }

            if (referencesToInstantiate != null)
            {
                // TODO: Remove dependency from Prefetcher to Commands
                var newReferences = this.Transaction.Commands.InstantiateReferences(referencesToInstantiate);
                references.UnionWith(newReferences);
            }

            return references;
        }

        internal void ResetCommands()
        {
            this.prefetchUnitRolesByClass = null;
            this.prefetchCompositeRoleByRoleType = null;
            this.prefetchCompositesRoleByRoleType = null;
            this.prefetchCompositeAssociationByAssociationType = null;
        }

        internal void PrefetchUnitRoles(IClass @class, HashSet<Reference> associations, IRoleType anyRoleType)
        {
            var references = this.FilterForPrefetchRoles(associations, anyRoleType);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchUnitRolesByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForPrefetchUnitRolesByClass[@class];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchUnitRolesByClass[@class] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            using (var reader = command.ExecuteReader())
            {
                var sortedUnitRoles = this.Database.GetSortedUnitRolesByObjectType(@class);
                var cache = this.Database.Cache;

                while (reader.Read())
                {
                    var associatoinId = reader.GetInt64(0);
                    var associationReference = this.Transaction.State.ReferenceByObjectId[associatoinId];

                    Strategy modifiedRoles = null;
                    this.Transaction.State.ModifiedRolesByReference?.TryGetValue(associationReference, out modifiedRoles);

                    var cachedObject = cache.GetOrCreateCachedObject(@class, associatoinId, associationReference.Version);

                    for (var i = 0; i < sortedUnitRoles.Length; i++)
                    {
                        var roleType = sortedUnitRoles[i];

                        var index = i + 1;
                        object unit = null;
                        if (!reader.IsDBNull(index))
                        {
                            switch (((IUnit)roleType.ObjectType).Tag)
                            {
                                case UnitTags.String:
                                    unit = reader.GetString(index);
                                    break;

                                case UnitTags.Integer:
                                    unit = reader.GetInt32(index);
                                    break;

                                case UnitTags.Float:
                                    unit = reader.GetDouble(index);
                                    break;

                                case UnitTags.Decimal:
                                    unit = reader.GetDecimal(index);
                                    break;

                                case UnitTags.DateTime:
                                    var dateTime = reader.GetDateTime(index);
                                    if (dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
                                    {
                                        unit = dateTime;
                                    }
                                    else
                                    {
                                        unit = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
                                    }

                                    break;

                                case UnitTags.Boolean:
                                    unit = reader.GetBoolean(index);
                                    break;

                                case UnitTags.Unique:
                                    unit = reader.GetGuid(index);
                                    break;

                                case UnitTags.Binary:
                                    unit = (byte[])reader.GetValue(index);
                                    break;

                                default:
                                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.Name);
                            }
                        }

                        if (modifiedRoles?.EnsureModifiedRoleByRoleType.ContainsKey(roleType) != true)
                        {
                            cachedObject.SetValue(roleType, unit);
                        }
                    }
                }
            }
        }

        internal void PrefetchCompositeRoleObjectTable(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchRoles(associations, roleType) : this.FilterForPrefetchCompositeRoles(associations, roleType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = this.Database.Mapping.ProcedureNameForPrefetchRoleByRelationType[roleType.RelationType];
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeRoleByRoleType[roleType] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            using (var reader = command.ExecuteReader())
            {
                var cache = this.Database.Cache;

                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Transaction.State.ReferenceByObjectId[associationId];

                    var cachedObject = cache.GetOrCreateCachedObject(associationReference.Class, associationId, associationReference.Version);

                    var roleIdValue = reader[1];

                    if (roleIdValue == null || roleIdValue == DBNull.Value)
                    {
                        cachedObject.SetValue(roleType, null);
                    }
                    else
                    {
                        var roleId = (long)roleIdValue;
                        cachedObject.SetValue(roleType, roleId);

                        nestedObjectIds?.Add(roleId);
                        if (nestedObjectIds == null)
                        {
                            leafs.Add(roleId);
                        }
                    }
                }
            }
        }

        internal void PrefetchCompositeRoleRelationTable(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchRoles(associations, roleType) : this.FilterForPrefetchCompositeRoles(associations, roleType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForPrefetchRoleByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeRoleByRoleType[roleType] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            var roleByAssociation = new Dictionary<Reference, long>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Transaction.State.ReferenceByObjectId[associationId];
                    var roleId = reader.GetInt64(1);
                    roleByAssociation.Add(associationReference, roleId);
                }
            }

            var cache = this.Database.Cache;
            foreach (var reference in references)
            {
                var cachedObject = cache.GetOrCreateCachedObject(reference.Class, reference.ObjectId, reference.Version);

                if (roleByAssociation.TryGetValue(reference, out var roleId))
                {
                    cachedObject.SetValue(roleType, roleId);
                    nestedObjectIds?.Add(roleId);
                    if (nestedObjectIds == null)
                    {
                        leafs.Add(roleId);
                    }
                }
                else
                {
                    cachedObject.SetValue(roleType, null);
                }
            }
        }

        internal void PrefetchCompositesRoleObjectTable(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchRoles(associations, roleType) : this.FilterForPrefetchCompositesRoles(associations, roleType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForPrefetchRoleByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositesRoleByRoleType[roleType] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            var rolesByAssociation = new Dictionary<Reference, List<long>>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Transaction.State.ReferenceByObjectId[associationId];

                    var roleIdValue = reader[1];
                    if (roleIdValue == null || roleIdValue == DBNull.Value)
                    {
                        rolesByAssociation[associationReference] = null;
                    }
                    else
                    {
                        var objectId = (long)roleIdValue;
                        if (!rolesByAssociation.TryGetValue(associationReference, out var roleIds))
                        {
                            roleIds = new List<long>();
                            rolesByAssociation[associationReference] = roleIds;
                        }

                        roleIds.Add(objectId);
                    }
                }
            }

            var cache = this.Database.Cache;
            foreach (var dictionaryEntry in rolesByAssociation)
            {
                var association = dictionaryEntry.Key;
                var roleIds = dictionaryEntry.Value;

                var cachedObject = cache.GetOrCreateCachedObject(association.Class, association.ObjectId, association.Version);
                cachedObject.SetValue(roleType, roleIds?.ToArray() ?? Array.Empty<long>());

                if (roleIds != null)
                {
                    nestedObjectIds?.UnionWith(roleIds);
                    if (nestedObjectIds == null)
                    {
                        leafs.UnionWith(roleIds);
                    }
                }
            }
        }

        internal void PrefetchCompositesRoleRelationTable(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchRoles(associations, roleType) : this.FilterForPrefetchCompositesRoles(associations, roleType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForPrefetchRoleByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositesRoleByRoleType[roleType] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            var rolesByAssociation = new Dictionary<Reference, List<long>>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Transaction.State.ReferenceByObjectId[associationId];

                    var roleId = reader.GetInt64(1);
                    if (!rolesByAssociation.TryGetValue(associationReference, out var roleIds))
                    {
                        roleIds = new List<long>();
                        rolesByAssociation[associationReference] = roleIds;
                    }

                    roleIds.Add(roleId);
                }
            }

            var cache = this.Database.Cache;
            foreach (var reference in references)
            {
                Strategy modifiedRoles = null;
                this.Transaction.State.ModifiedRolesByReference?.TryGetValue(reference, out modifiedRoles);

                if (modifiedRoles == null || !modifiedRoles.EnsureModifiedRoleByRoleType.ContainsKey(roleType))
                {
                    var cachedObject = cache.GetOrCreateCachedObject(reference.Class, reference.ObjectId, reference.Version);

                    if (rolesByAssociation.TryGetValue(reference, out var roleIds))
                    {
                        cachedObject.SetValue(roleType, roleIds.ToArray());

                        nestedObjectIds?.UnionWith(roleIds);
                        if (nestedObjectIds == null)
                        {
                            leafs.UnionWith(roleIds);
                        }
                    }
                    else
                    {
                        cachedObject.SetValue(roleType, Array.Empty<long>());
                    }
                }
            }
        }

        internal void PrefetchCompositeAssociationObjectTable(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchAssociations(roles, associationType) : this.FilterForPrefetchCompositeAssociations(roles, associationType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForPrefetchAssociationByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var role = this.Transaction.State.ReferenceByObjectId[roleId];

                    var associationByRole = this.Transaction.State.GetAssociationByRole(associationType);
                    if (!associationByRole.ContainsKey(role))
                    {
                        var associationIdValue = reader[0];
                        Reference association = null;
                        if (associationIdValue != null && associationIdValue != DBNull.Value)
                        {
                            var associationId = (long)associationIdValue;
                            association = associationType.ObjectType.ExistExclusiveDatabaseClass ?
                                              this.Transaction.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveDatabaseClass, associationId, this.Transaction) :
                                              this.Transaction.State.GetOrCreateReferenceForExistingObject(associationId, this.Transaction);

                            nestedObjectIds?.Add(association.ObjectId);
                            if (nestedObjectIds == null)
                            {
                                leafs.Add(associationId);
                            }
                        }

                        associationByRole[role] = association;

                        this.Transaction.State.FlushConditionally(roleId, associationType);
                    }
                }
            }
        }

        internal void PrefetchCompositeAssociationRelationTable(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchAssociations(roles, associationType) : this.FilterForPrefetchCompositesAssociations(roles, associationType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForPrefetchAssociationByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }

            command.ObjectTableParameter(roles.Select(v => v.ObjectId));

            var prefetchedAssociationByRole = new Dictionary<Reference, long>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Transaction.State.ReferenceByObjectId[roleId];
                    var associationId = reader.GetInt64(0);
                    prefetchedAssociationByRole.Add(roleReference, associationId);
                }
            }

            var associationByRole = this.Transaction.State.GetAssociationByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationByRole.ContainsKey(role))
                {
                    Reference association = null;

                    if (prefetchedAssociationByRole.TryGetValue(role, out var associationId))
                    {
                        association = associationType.ObjectType.ExistExclusiveDatabaseClass ?
                                          this.Transaction.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveDatabaseClass, associationId, this.Transaction) :
                                          this.Transaction.State.GetOrCreateReferenceForExistingObject(associationId, this.Transaction);

                        nestedObjectIds?.Add(associationId);
                        if (nestedObjectIds == null)
                        {
                            leafs.Add(associationId);
                        }
                    }

                    associationByRole[role] = association;

                    this.Transaction.State.FlushConditionally(role.ObjectId, associationType);
                }
            }
        }

        internal void PrefetchCompositesAssociationObjectTable(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchAssociations(roles, associationType) : this.FilterForPrefetchCompositeAssociations(roles, associationType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForPrefetchAssociationByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }

            command.ObjectTableParameter(roles.Select(v => v.ObjectId));

            var prefetchedAssociationByRole = new Dictionary<Reference, List<long>>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Transaction.State.ReferenceByObjectId[roleId];

                    var associationIdValue = reader[0];
                    if (associationIdValue != null && associationIdValue != DBNull.Value)
                    {
                        if (!prefetchedAssociationByRole.TryGetValue(roleReference, out var associations))
                        {
                            associations = new List<long>();
                            prefetchedAssociationByRole.Add(roleReference, associations);
                        }

                        var associationId = (long)associationIdValue;
                        associations.Add(associationId);

                        if (associationType.ObjectType.ExistExclusiveDatabaseClass)
                        {
                            this.Transaction.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveDatabaseClass, associationId, this.Transaction);
                        }
                        else
                        {
                            this.Transaction.State.GetOrCreateReferenceForExistingObject(associationId, this.Transaction);
                        }
                    }
                }
            }

            var associationsByRole = this.Transaction.State.GetAssociationsByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationsByRole.ContainsKey(role))
                {
                    if (!prefetchedAssociationByRole.TryGetValue(role, out var associationIds))
                    {
                        associationsByRole[role] = Array.Empty<long>();
                    }
                    else
                    {
                        associationsByRole[role] = associationIds.ToArray();

                        nestedObjectIds?.UnionWith(associationIds);
                        if (nestedObjectIds == null)
                        {
                            leafs.UnionWith(associationIds);
                        }
                    }

                    this.Transaction.State.FlushConditionally(role.ObjectId, associationType);
                }
            }
        }

        internal void PrefetchCompositesAssociationRelationTable(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds, HashSet<long> leafs)
        {
            var references = nestedObjectIds == null ? this.FilterForPrefetchAssociations(roles, associationType) : this.FilterForPrefetchCompositeAssociations(roles, associationType, nestedObjectIds);
            if (references.Count == 0)
            {
                return;
            }

            if (!this.PrefetchCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForPrefetchAssociationByRelationType[roleType.RelationType];
                command = this.Transaction.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }

            command.ObjectTableParameter(roles.Select(v => v.ObjectId));

            var prefetchedAssociations = new HashSet<long>();

            var prefetchedAssociationByRole = new Dictionary<Reference, List<long>>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Transaction.State.ReferenceByObjectId[roleId];

                    if (!prefetchedAssociationByRole.TryGetValue(roleReference, out var associations))
                    {
                        associations = new List<long>();
                        prefetchedAssociationByRole.Add(roleReference, associations);
                    }

                    var associationId = reader.GetInt64(0);
                    associations.Add(associationId);
                    prefetchedAssociations.Add(associationId);
                }
            }

            foreach (var associationId in prefetchedAssociations)
            {
                if (associationType.ObjectType.ExistExclusiveDatabaseClass)
                {
                    this.Transaction.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveDatabaseClass, associationId, this.Transaction);
                }
                else
                {
                    this.Transaction.State.GetOrCreateReferenceForExistingObject(associationId, this.Transaction);
                }
            }

            var associationsByRole = this.Transaction.State.GetAssociationsByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationsByRole.ContainsKey(role))
                {
                    if (!prefetchedAssociationByRole.TryGetValue(role, out var associationIds))
                    {
                        associationsByRole[role] = Array.Empty<long>();
                    }
                    else
                    {
                        associationsByRole[role] = associationIds.ToArray();

                        nestedObjectIds?.UnionWith(associationIds);
                        if (nestedObjectIds == null)
                        {
                            leafs.UnionWith(associationIds);
                        }
                    }

                    this.Transaction.State.FlushConditionally(role.ObjectId, associationType);
                }
            }
        }

        private List<Reference> FilterForPrefetchRoles(HashSet<Reference> associations, IRoleType roleType)
        {
            var references = new List<Reference>();

            var cache = this.Database.Cache;

            foreach (var association in associations)
            {
                if (this.Transaction.State.ModifiedRolesByReference != null &&
                    this.Transaction.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.PrefetchTryGetUnitRole(roleType))
                {
                    continue;
                }

                if (!association.IsUnknownVersion)
                {
                    var cacheObject = cache.GetOrCreateCachedObject(association.Class, association.ObjectId, association.Version);
                    if (cacheObject.TryGetValue(roleType, out var role))
                    {
                        continue;
                    }
                }

                references.Add(association);
            }

            return references;
        }

        private List<Reference> FilterForPrefetchCompositeRoles(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjects)
        {
            var references = new List<Reference>();

            var cache = this.Database.Cache;

            foreach (var association in associations)
            {
                if (this.Transaction.State.ModifiedRolesByReference != null &&
                    this.Transaction.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.PrefetchTryGetCompositeRole(roleType, out var modifiedRole))
                {
                    if (modifiedRole != null)
                    {
                        nestedObjects.Add(modifiedRole.Value);
                    }

                    continue;
                }

                if (!association.IsUnknownVersion)
                {
                    var cacheObject = cache.GetOrCreateCachedObject(association.Class, association.ObjectId, association.Version);

                    if (cacheObject.TryGetValue(roleType, out var role))
                    {
                        if (role != null)
                        {
                            nestedObjects.Add((long)role);
                        }

                        continue;
                    }
                }

                references.Add(association);
            }

            return references;
        }

        private List<Reference> FilterForPrefetchCompositesRoles(HashSet<Reference> associations, IRoleType roleType, HashSet<long> nestedObjects)
        {
            var references = new List<Reference>();

            var cache = this.Database.Cache;

            foreach (var association in associations)
            {
                if (this.Transaction.State.ModifiedRolesByReference != null &&
                    this.Transaction.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.PrefetchTryGetCompositesRole(roleType, out var modifiedRole))
                {
                    nestedObjects.UnionWith(modifiedRole);
                    continue;
                }

                if (!association.IsUnknownVersion)
                {
                    var cacheObject = cache.GetOrCreateCachedObject(association.Class, association.ObjectId, association.Version);
                    if (cacheObject.TryGetValue(roleType, out var cachedRole))
                    {
                        nestedObjects.UnionWith((long[])cachedRole);
                        continue;
                    }
                }

                references.Add(association);
            }

            return references;
        }

        private HashSet<Reference> FilterForPrefetchAssociations(HashSet<Reference> roles, IAssociationType associationType)
        {
            if (!this.Transaction.State.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                return roles;
            }

            return new HashSet<Reference>(roles.Where(role => !associationByRole.ContainsKey(role)));
        }

        private HashSet<Reference> FilterForPrefetchCompositeAssociations(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds)
        {
            if (!this.Transaction.State.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                return roles;
            }

            var references = new HashSet<Reference>();
            foreach (var role in roles)
            {
                if (associationByRole.TryGetValue(role, out var association))
                {
                    nestedObjectIds.Add(association.ObjectId);
                    continue;
                }

                references.Add(role);
            }

            return references;
        }

        private HashSet<Reference> FilterForPrefetchCompositesAssociations(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds)
        {
            if (!this.Transaction.State.AssociationsByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                return roles;
            }

            var references = new HashSet<Reference>();
            foreach (var role in roles)
            {
                if (associationByRole.TryGetValue(role, out var association))
                {
                    nestedObjectIds.UnionWith(association);
                    continue;
                }

                references.Add(role);
            }

            return references;
        }
    }
}

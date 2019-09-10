// <copyright file="Prefetcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Session type.</summary>

namespace Allors.Adapters.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    using Allors.Meta;

    internal class Prefetcher
    {
        private static readonly long[] EmptyObjectIds = { };

        private Dictionary<IClass, Command> prefetchUnitRolesByClass;
        private Dictionary<IRoleType, Command> prefetchCompositeRoleByRoleType;
        private Dictionary<IRoleType, Command> prefetchCompositesRoleByRoleType;
        private Dictionary<IAssociationType, Command> prefetchCompositeAssociationByAssociationType;

        public Prefetcher(Session session) => this.Session = session;

        public Session Session { get; }

        public Database Database => this.Session.Database;

        private Dictionary<IClass, Command> PrefetchUnitRolesByClass => this.prefetchUnitRolesByClass ?? (this.prefetchUnitRolesByClass = new Dictionary<IClass, Command>());

        private Dictionary<IRoleType, Command> PrefetchCompositeRoleByRoleType => this.prefetchCompositeRoleByRoleType ?? (this.prefetchCompositeRoleByRoleType = new Dictionary<IRoleType, Command>());

        private Dictionary<IRoleType, Command> PrefetchCompositesRoleByRoleType => this.prefetchCompositesRoleByRoleType ?? (this.prefetchCompositesRoleByRoleType = new Dictionary<IRoleType, Command>());

        private Dictionary<IAssociationType, Command> PrefetchCompositeAssociationByAssociationType => this.prefetchCompositeAssociationByAssociationType ?? (this.prefetchCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Command>());

        internal HashSet<Reference> GetReferencesForPrefetching(IEnumerable<long> objectIds)
        {
            var references = new HashSet<Reference>();

            HashSet<long> referencesToInstantiate = null;
            foreach (var objectId in objectIds)
            {
                this.Session.State.ReferenceByObjectId.TryGetValue(objectId, out var reference);
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
                var newReferences = this.Session.Commands.InstantiateReferences(referencesToInstantiate);
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

                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchUnitRolesByClass[@class] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            using (DbDataReader reader = command.ExecuteReader())
            {
                var sortedUnitRoles = this.Database.GetSortedUnitRolesByObjectType(@class);
                var cache = this.Database.Cache;

                while (reader.Read())
                {
                    var associatoinId = reader.GetInt64(0);
                    var associationReference = this.Session.State.ReferenceByObjectId[associatoinId];

                    Roles modifiedRoles = null;
                    this.Session.State.ModifiedRolesByReference?.TryGetValue(associationReference, out modifiedRoles);

                    var cachedObject = cache.GetOrCreateCachedObject(@class, associatoinId, associationReference.Version);

                    for (var i = 0; i < sortedUnitRoles.Length; i++)
                    {
                        var roleType = sortedUnitRoles[i];

                        var index = i + 1;
                        object unit = null;
                        if (!reader.IsDBNull(index))
                        {
                            var unitTypeTag = ((IUnit)roleType.ObjectType).UnitTag;
                            switch (unitTypeTag)
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
                                    var byteArray = (byte[])reader.GetValue(index);
                                    unit = byteArray;
                                    break;

                                default:
                                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.Name);
                            }
                        }

                        if (modifiedRoles == null || !modifiedRoles.ModifiedRoleByRoleType.ContainsKey(roleType))
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = this.Database.Mapping.ProcedureNameForPrefetchRoleByRelationType[roleType.RelationType];
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            using (DbDataReader reader = command.ExecuteReader())
            {
                var cache = this.Database.Cache;

                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Session.State.ReferenceByObjectId[associationId];

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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var roleByAssociation = new Dictionary<Reference, long>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Session.State.ReferenceByObjectId[associationId];
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchCompositesRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var rolesByAssociation = new Dictionary<Reference, List<long>>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Session.State.ReferenceByObjectId[associationId];

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
                cachedObject.SetValue(roleType, roleIds?.ToArray() ?? EmptyObjectIds);

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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchCompositesRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var rolesByAssociation = new Dictionary<Reference, List<long>>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var associationId = reader.GetInt64(0);
                    var associationReference = this.Session.State.ReferenceByObjectId[associationId];

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
                Roles modifiedRoles = null;
                this.Session.State.ModifiedRolesByReference?.TryGetValue(reference, out modifiedRoles);

                if (modifiedRoles == null || !modifiedRoles.ModifiedRoleByRoleType.ContainsKey(roleType))
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
                        cachedObject.SetValue(roleType, EmptyObjectIds);
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(references);
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var role = this.Session.State.ReferenceByObjectId[roleId];

                    var associationByRole = this.Session.State.GetAssociationByRole(associationType);
                    if (!associationByRole.ContainsKey(role))
                    {
                        var associationIdValue = reader[0];
                        Reference association = null;
                        if (associationIdValue != null && associationIdValue != DBNull.Value)
                        {
                            var associationId = (long)associationIdValue;
                            association = associationType.ObjectType.ExistExclusiveClass ?
                                              this.Session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, associationId, this.Session) :
                                              this.Session.State.GetOrCreateReferenceForExistingObject(associationId, this.Session);

                            nestedObjectIds?.Add(association.ObjectId);
                            if (nestedObjectIds == null)
                            {
                                leafs.Add(associationId);
                            }
                        }

                        associationByRole[role] = association;

                        this.Session.FlushConditionally(roleId, associationType);
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(roles);
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var prefetchedAssociationByRole = new Dictionary<Reference, long>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Session.State.ReferenceByObjectId[roleId];
                    var associationId = reader.GetInt64(0);
                    prefetchedAssociationByRole.Add(roleReference, associationId);
                }
            }

            var associationByRole = this.Session.State.GetAssociationByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationByRole.ContainsKey(role))
                {
                    Reference association = null;

                    if (prefetchedAssociationByRole.TryGetValue(role, out var associationId))
                    {
                        association = associationType.ObjectType.ExistExclusiveClass ?
                                          this.Session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, associationId, this.Session) :
                                          this.Session.State.GetOrCreateReferenceForExistingObject(associationId, this.Session);

                        nestedObjectIds?.Add(associationId);
                        if (nestedObjectIds == null)
                        {
                            leafs.Add(associationId);
                        }
                    }

                    associationByRole[role] = association;

                    this.Session.FlushConditionally(role.ObjectId, associationType);
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(roles);
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var prefetchedAssociationByRole = new Dictionary<Reference, List<long>>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Session.State.ReferenceByObjectId[roleId];

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

                        if (associationType.ObjectType.ExistExclusiveClass)
                        {
                            this.Session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, associationId, this.Session);
                        }
                        else
                        {
                            this.Session.State.GetOrCreateReferenceForExistingObject(associationId, this.Session);
                        }
                    }
                }
            }

            var associationsByRole = this.Session.State.GetAssociationsByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationsByRole.ContainsKey(role))
                {
                    if (!prefetchedAssociationByRole.TryGetValue(role, out var associationIds))
                    {
                        associationsByRole[role] = EmptyObjectIds;
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

                    this.Session.FlushConditionally(role.ObjectId, associationType);
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
                command = this.Session.Connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(roles);
                this.prefetchCompositeAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
            }

            var prefetchedAssociations = new HashSet<long>();

            var prefetchedAssociationByRole = new Dictionary<Reference, List<long>>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var roleId = reader.GetInt64(1);
                    var roleReference = this.Session.State.ReferenceByObjectId[roleId];

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
                if (associationType.ObjectType.ExistExclusiveClass)
                {
                    this.Session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, associationId, this.Session);
                }
                else
                {
                    this.Session.State.GetOrCreateReferenceForExistingObject(associationId, this.Session);
                }
            }

            var associationsByRole = this.Session.State.GetAssociationsByRole(associationType);
            foreach (var role in roles)
            {
                if (!associationsByRole.ContainsKey(role))
                {
                    if (!prefetchedAssociationByRole.TryGetValue(role, out var associationIds))
                    {
                        associationsByRole[role] = EmptyObjectIds;
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

                    this.Session.FlushConditionally(role.ObjectId, associationType);
                }
            }
        }

        private List<Reference> FilterForPrefetchRoles(HashSet<Reference> associations, IRoleType roleType)
        {
            var references = new List<Reference>();

            var cache = this.Database.Cache;

            foreach (var association in associations)
            {
                if (this.Session.State.ModifiedRolesByReference != null &&
                    this.Session.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.TryGetUnitRole(roleType, out var modifiedRole))
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
                if (this.Session.State.ModifiedRolesByReference != null &&
                    this.Session.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.TryGetCompositeRole(roleType, out var modifiedRole))
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
                if (this.Session.State.ModifiedRolesByReference != null &&
                    this.Session.State.ModifiedRolesByReference.TryGetValue(association, out var roles) &&
                    roles.TryGetCompositesRole(roleType, out var modifiedRole))
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
            if (!this.Session.State.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                return roles;
            }

            return new HashSet<Reference>(roles.Where(role => !associationByRole.ContainsKey(role)));
        }

        private HashSet<Reference> FilterForPrefetchCompositeAssociations(HashSet<Reference> roles, IAssociationType associationType, HashSet<long> nestedObjectIds)
        {
            if (!this.Session.State.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
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
            if (!this.Session.State.AssociationsByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
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

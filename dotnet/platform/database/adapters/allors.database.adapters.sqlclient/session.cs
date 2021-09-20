// <copyright file="Session.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Session type.</summary>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class Session : ISession
    {
        private static readonly IObject[] EmptyObjects = { };
        private bool busyCommittingOrRollingBack;

        private Dictionary<string, object> properties;

        internal Session(Database database, Connection connection)
        {
            var serviceScopeFactory = database.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this.ServiceProvider = scope.ServiceProvider;

            this.Database = database;
            this.Connection = connection;

            this.State = new State();

            this.Prefetcher = new Prefetcher(this);
            this.Commands = new Commands(this, connection);
        }

        public IServiceProvider ServiceProvider { get; }

        public Connection Connection { get; }

        public Commands Commands { get; }

        public State State { get; }

        IDatabase ISession.Database => this.Database;

        public Database Database { get; }

        private Prefetcher Prefetcher { get; }

        public object this[string name]
        {
            get
            {
                if (this.properties == null)
                {
                    return null;
                }

                this.properties.TryGetValue(name, out var value);
                return value;
            }

            set
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.properties.Remove(name);
                }
                else
                {
                    this.properties[name] = value;
                }
            }
        }

        public IObject Create(IClass objectType)
        {
            if (!objectType.IsClass)
            {
                throw new ArgumentException("Can not create non concrete composite type " + objectType);
            }

            var reference = this.Commands.CreateObject(objectType);
            this.State.ReferenceByObjectId[reference.ObjectId] = reference;

            this.Database.Cache.SetObjectType(reference.ObjectId, objectType);

            this.State.ChangeSet.OnCreated(reference.ObjectId);

            return reference.Strategy.GetObject();
        }

        public IObject[] Create(IClass objectType, int count)
        {
            if (!objectType.IsClass)
            {
                throw new ArgumentException("Can not create non concrete composite type " + objectType);
            }

            var references = this.Commands.CreateObjects(objectType, count);

            var arrayType = this.Database.ObjectFactory.GetTypeForObjectType(objectType);
            var domainObjects = (IObject[])Array.CreateInstance(arrayType, count);

            for (var i = 0; i < references.Count; i++)
            {
                var reference = references[i];
                this.State.ReferenceByObjectId[reference.ObjectId] = reference;

                domainObjects[i] = reference.Strategy.GetObject();

                this.State.ChangeSet.OnCreated(reference.ObjectId);
            }

            return domainObjects;
        }

        public IObject Instantiate(IObject obj) => this.Instantiate(obj.Strategy.ObjectId);

        public IObject Instantiate(string objectId)
        {
            if (long.TryParse(objectId, out var id))
            {
                return this.Instantiate(id);
            }
;

            return null;
        }

        public IObject Instantiate(long objectId)
        {
            var strategy = this.InstantiateStrategy(objectId);
            return strategy?.GetObject();
        }

        public IStrategy InstantiateStrategy(long objectId)
        {
            if (!this.State.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                reference = this.Commands.InstantiateObject(objectId);
                if (reference != null)
                {
                    this.State.ReferenceByObjectId[objectId] = reference;
                }
            }

            if (reference == null || !reference.Exists)
            {
                return null;
            }

            return reference.Strategy;
        }

        public IObject[] Instantiate(IEnumerable<string> objectIdStrings) => objectIdStrings != null ? this.Instantiate(objectIdStrings.Select(long.Parse)) : EmptyObjects;

        public IObject[] Instantiate(IEnumerable<IObject> objects) => objects != null ? this.Instantiate(objects.Select(v => v.Id)) : EmptyObjects;

        public IObject[] Instantiate(IEnumerable<long> objectIds)
        {
            if (objectIds == null)
            {
                return EmptyObjects;
            }

            var objectIdArray = objectIds.ToArray();

            if (objectIdArray.Length == 0)
            {
                return EmptyObjects;
            }

            var references = new List<Reference>(objectIdArray.Length);

            List<long> nonCachedObjectIds = null;
            foreach (var objectId in objectIdArray)
            {
                if (!this.State.ReferenceByObjectId.TryGetValue(objectId, out var reference))
                {
                    if (nonCachedObjectIds == null)
                    {
                        nonCachedObjectIds = new List<long>();
                    }

                    nonCachedObjectIds.Add(objectId);
                }
                else
                {
                    if (!reference.Strategy.IsDeleted)
                    {
                        references.Add(reference);
                    }
                }
            }

            if (nonCachedObjectIds != null)
            {
                var nonCachedReferences = this.Commands.InstantiateReferences(nonCachedObjectIds);
                references.AddRange(nonCachedReferences);

                // Return objects in the same order as objectIds
                var referenceById = references.ToDictionary(v => v.ObjectId);
                references = new List<Reference>();
                foreach (var objectId in objectIdArray)
                {
                    if (referenceById.TryGetValue(objectId, out var reference))
                    {
                        references.Add(reference);
                    }
                }
            }

            var allorsObjects = new IObject[references.Count];
            for (var i = 0; i < allorsObjects.Length; i++)
            {
                allorsObjects[i] = references[i].Strategy.GetObject();
            }

            return allorsObjects;
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects) => this.Prefetch(prefetchPolicy, objects.Select(x => x.Strategy.ObjectId));

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IObject> objects) => this.Prefetch(prefetchPolicy, objects.Select(x => x.Strategy.ObjectId));

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IStrategy> strategies) => this.Prefetch(prefetchPolicy, strategies.Select(v => v.ObjectId));

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<string> objectIdStrings) => this.Prefetch(prefetchPolicy, objectIdStrings.Select(long.Parse));

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<long> objectIds)
        {
            var references = this.Prefetcher.GetReferencesForPrefetching(objectIds);

            if (references.Count != 0)
            {
                this.Flush();

                var prefetcher = new Prefetch(this.Prefetcher, prefetchPolicy, references);
                prefetcher.Execute();
            }
        }

        public IChangeSet Checkpoint()
        {
            try
            {
                return this.State.ChangeSet;
            }
            finally
            {
                this.State.ChangeSet = new ChangeSet();
            }
        }

        public Extent<T> Extent<T>() where T : IObject => this.Extent((IComposite)this.Database.ObjectFactory.GetObjectTypeForType(typeof(T)));

        public Allors.Extent Extent(IComposite type) => new ExtentFiltered(this, type);

        public Allors.Extent Union(Allors.Extent firstOperand, Allors.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Union);

        public Allors.Extent Intersect(Allors.Extent firstOperand, Allors.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Intersect);

        public Allors.Extent Except(Allors.Extent firstOperand, Allors.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Except);

        public void Commit()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.State.ReferenceByObjectId.Keys);
                    this.Flush();

                    var changed = this.State.ModifiedRolesByReference?.Select(dictionaryEntry => dictionaryEntry.Key.ObjectId).ToArray() ?? Array.Empty<long>();
                    if (changed.Length > 0)
                    {
                        this.Commands.UpdateVersion(changed);
                    }

                    this.Connection.Commit();

                    this.State.ModifiedRolesByReference = null;

                    var referencesWithStrategy = new HashSet<Reference>();
                    foreach (var reference in this.State.ReferenceByObjectId.Values)
                    {
                        reference.Commit(referencesWithStrategy);
                    }

                    this.State.ExistingObjectIdsWithoutReference = new HashSet<long>();
                    this.State.ReferencesWithoutVersions = referencesWithStrategy;

                    this.State.ReferenceByObjectId = new Dictionary<long, Reference>();
                    foreach (var reference in referencesWithStrategy)
                    {
                        this.State.ReferenceByObjectId[reference.ObjectId] = reference;
                    }

                    this.State.AssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
                    this.State.AssociationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

                    this.State.ChangeSet = new ChangeSet();

                    this.Database.Cache.OnCommit(accessed, changed);

                    this.Prefetcher.ResetCommands();
                    this.Commands.ResetCommands();
                }
                finally
                {
                    this.busyCommittingOrRollingBack = false;
                }
            }
        }

        public void Rollback()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.State.ReferenceByObjectId.Keys);

                    this.Connection.Rollback();

                    var referencesWithStrategy = new HashSet<Reference>();
                    foreach (var reference in this.State.ReferenceByObjectId.Values)
                    {
                        reference.Rollback(referencesWithStrategy);
                    }

                    this.State.ExistingObjectIdsWithoutReference = new HashSet<long>();
                    this.State.ReferencesWithoutVersions = referencesWithStrategy;

                    this.State.ReferenceByObjectId = new Dictionary<long, Reference>();
                    foreach (var reference in referencesWithStrategy)
                    {
                        this.State.ReferenceByObjectId[reference.ObjectId] = reference;
                    }

                    this.State.UnflushedRolesByReference = null;
                    this.State.ModifiedRolesByReference = null;
                    this.State.TriggersFlushRolesByAssociationType = null;

                    this.State.AssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
                    this.State.AssociationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

                    this.State.ChangeSet = new ChangeSet();

                    this.Database.Cache.OnRollback(accessed);

                    this.Prefetcher.ResetCommands();
                    this.Commands.ResetCommands();
                }
                finally
                {
                    this.busyCommittingOrRollingBack = false;
                }
            }
        }

        public void Dispose() => this.Rollback();

        public T Create<T>() where T : IObject
        {
            var objectType = (IClass)this.Database.ObjectFactory.GetObjectTypeForType(typeof(T));
            return (T)this.Create(objectType);
        }

        public override string ToString() => "Session[id=" + this.GetHashCode() + "] " + this.Database;

        internal Reference GetAssociation(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationByRole = this.State.GetAssociationByRole(associationType);

            if (!associationByRole.TryGetValue(roleStrategy.Reference, out var association))
            {
                this.FlushConditionally(roleStrategy.ObjectId, associationType);
                association = this.Commands.GetCompositeAssociation(roleStrategy.Reference, associationType);
                associationByRole[roleStrategy.Reference] = association;
            }

            return association;
        }

        internal void SetAssociation(Reference previousAssociation, Strategy roleStrategy, IAssociationType associationType)
        {
            var associationByRole = this.State.GetAssociationByRole(associationType);
            associationByRole[roleStrategy.Reference] = previousAssociation;
        }

        internal Reference[] GetOrCreateReferencesForExistingObjects(IEnumerable<long> objectIds) => this.State.GetOrCreateReferencesForExistingObjects(objectIds, this);

        internal long[] GetAssociations(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            if (!associationsByRole.TryGetValue(roleStrategy.Reference, out var associations))
            {
                this.FlushConditionally(roleStrategy.ObjectId, associationType);
                associations = this.Commands.GetCompositesAssociation(roleStrategy, associationType);
                associationsByRole[roleStrategy.Reference] = associations;
            }

            return associations;
        }

        internal void AddAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            if (associationsByRole.TryGetValue(role, out var associations))
            {
                associationsByRole[role] = associations.Add(association.ObjectId);
            }
        }

        internal void RemoveAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            if (associationsByRole.TryGetValue(role, out var associations))
            {
                associationsByRole[role] = associations.Remove(association.ObjectId);
            }
        }

        internal void Flush()
        {
            if (this.State.UnflushedRolesByReference != null)
            {
                var flush = new Flush(this, this.State.UnflushedRolesByReference);
                flush.Execute();

                this.State.UnflushedRolesByReference = null;
                this.State.TriggersFlushRolesByAssociationType = null;
            }
        }

        internal void AddReferenceWithoutVersionOrExistsKnown(Reference reference) => this.State.ReferencesWithoutVersions.Add(reference);

        internal void GetVersionAndExists()
        {
            if (this.State.ReferencesWithoutVersions.Count > 0)
            {
                var versionByObjectId = this.Commands.GetVersions(this.State.ReferencesWithoutVersions);
                foreach (var association in this.State.ReferencesWithoutVersions)
                {
                    if (versionByObjectId.TryGetValue(association.ObjectId, out var version))
                    {
                        association.Version = version;
                        association.Exists = true;
                    }
                    else
                    {
                        association.Exists = false;
                    }
                }

                this.State.ReferencesWithoutVersions = new HashSet<Reference>();
            }
        }

        internal void RequireFlush(Reference association, Roles roles)
        {
            if (this.State.UnflushedRolesByReference == null)
            {
                this.State.UnflushedRolesByReference = new Dictionary<Reference, Roles>();
            }

            this.State.UnflushedRolesByReference[association] = roles;

            if (this.State.ModifiedRolesByReference == null)
            {
                this.State.ModifiedRolesByReference = new Dictionary<Reference, Roles>();
            }

            this.State.ModifiedRolesByReference[association] = roles;
        }

        internal void TriggerFlush(long role, IAssociationType associationType)
        {
            if (this.State.TriggersFlushRolesByAssociationType == null)
            {
                this.State.TriggersFlushRolesByAssociationType = new Dictionary<IAssociationType, HashSet<long>>();
            }

            if (!this.State.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out var associations))
            {
                associations = new HashSet<long>();
                this.State.TriggersFlushRolesByAssociationType[associationType] = associations;
            }

            associations.Add(role);
        }

        internal void FlushConditionally(long roleId, IAssociationType associationType)
        {
            if (this.State.TriggersFlushRolesByAssociationType != null)
            {
                if (this.State.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out var roles))
                {
                    if (roles.Contains(roleId))
                    {
                        this.Flush();
                    }
                }
            }
        }

        internal void InstantiateReferences(IEnumerable<long> objectIds)
        {
            var forceEvaluation = this.Commands.InstantiateReferences(objectIds).ToArray();
        }
    }
}

// <copyright file="Transaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Transaction type.</summary>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

    public sealed class Transaction : ITransaction
    {
        private bool busyCommittingOrRollingBack;

        private Dictionary<string, object> properties;

        internal Transaction(Database database, IConnection connection, ITransactionServices scope)
        {
            this.Database = database;
            this.Connection = connection;
            this.Services = scope;

            this.State = new State(this);

            this.Prefetcher = new Prefetcher(this);
            this.Commands = new Commands(this, connection);

            this.Services.OnInit(this);
        }

        public IConnection Connection { get; }

        public Commands Commands { get; }

        public State State { get; }

        IDatabase ITransaction.Database => this.Database;

        public ITransactionServices Services { get; }

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

            this.State.ChangeLog.OnCreated(reference.Strategy);

            return reference.Strategy.GetObject();
        }

        public IObject[] Create(IClass objectType, int count)
        {
            if (!objectType.IsClass)
            {
                throw new ArgumentException("Can not create non concrete composite type " + objectType);
            }

            var references = this.Commands.CreateObjects(objectType, count);

            var arrayType = this.Database.ObjectFactory.GetType(objectType);
            var domainObjects = (IObject[])Array.CreateInstance(arrayType, count);

            for (var i = 0; i < references.Count; i++)
            {
                var reference = references[i];
                this.State.ReferenceByObjectId[reference.ObjectId] = reference;

                domainObjects[i] = reference.Strategy.GetObject();

                this.State.ChangeLog.OnCreated(reference.Strategy);
            }

            return domainObjects;
        }

        public IObject Instantiate(IObject obj) => this.Instantiate(obj.Strategy.ObjectId);

        public IObject Instantiate(string objectId) => long.TryParse(objectId, out var id) ? this.Instantiate(id) : null;

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

        public IObject[] Instantiate(IEnumerable<string> objectIdStrings) => objectIdStrings != null ? this.Instantiate(objectIdStrings.Select(long.Parse)) : Array.Empty<IObject>();

        public IObject[] Instantiate(IEnumerable<IObject> objects) => objects != null ? this.Instantiate(objects.Select(v => v.Id)) : Array.Empty<IObject>();

        public IObject[] Instantiate(IEnumerable<long> objectIds)
        {
            IObject[] emptyObjects = Array.Empty<IObject>();
            if (objectIds == null)
            {
                return emptyObjects;
            }

            var objectIdArray = objectIds.ToArray();

            if (objectIdArray.Length == 0)
            {
                return emptyObjects;
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
                else if (!reference.Strategy.IsDeleted)
                {
                    references.Add(reference);
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
                this.State.Flush();

                var prefetcher = new Prefetch(this.Prefetcher, prefetchPolicy, references);
                prefetcher.Execute();
            }
        }

        public IChangeSet Checkpoint()
        {
            try
            {
                return this.State.ChangeLog.Checkpoint();
            }
            finally
            {
                this.State.ChangeLog.Reset();
            }
        }

        public Extent<T> Extent<T>() where T : IObject => this.Extent((IComposite)this.Database.ObjectFactory.GetObjectType(typeof(T)));

        public Allors.Database.Extent Extent(IComposite type) => new ExtentFiltered(this, type);

        public Allors.Database.Extent Union(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Union);

        public Allors.Database.Extent Intersect(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Intersect);

        public Allors.Database.Extent Except(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand) => new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Except);

        public void Commit()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.State.ReferenceByObjectId.Keys);
                    this.State.Flush();

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

                    this.State.ChangeLog.Reset();

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

                    this.State.ChangeLog.Reset();

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
            var objectType = (IClass)this.Database.ObjectFactory.GetObjectType(typeof(T));
            return (T)this.Create(objectType);
        }

        public override string ToString() => "Transaction[id=" + this.GetHashCode() + "] " + this.Database;

        internal Reference[] GetOrCreateReferencesForExistingObjects(IEnumerable<long> objectIds) => this.State.GetOrCreateReferencesForExistingObjects(objectIds, this);

        internal long[] GetAssociations(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            if (!associationsByRole.TryGetValue(roleStrategy.Reference, out var associations))
            {
                this.State.FlushConditionally(roleStrategy.ObjectId, associationType);
                associations = this.Commands.GetCompositesAssociation(roleStrategy, associationType);
                associationsByRole[roleStrategy.Reference] = associations;
            }

            return associations;
        }

        internal void RemoveAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            if (associationsByRole.TryGetValue(role, out var associations))
            {
                associationsByRole[role] = associations.Remove(association.ObjectId);
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

        internal void InstantiateReferences(IEnumerable<long> objectIds)
        {
            var forceEvaluation = this.Commands.InstantiateReferences(objectIds).ToArray();
        }
    }
}

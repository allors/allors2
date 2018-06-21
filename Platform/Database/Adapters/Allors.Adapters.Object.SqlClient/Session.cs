//------------------------------------------------------------------------------------------------- 
// <copyright file="Session.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the Session type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class Session : ISession
    {
        private static readonly IObject[] EmptyObjects = { };

        private readonly Database database;

        private bool busyCommittingOrRollingBack;

        private Dictionary<string, object> properties;

        internal Session(Database database, Connection connection)
        {
            var serviceScopeFactory = database.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this.ServiceProvider = scope.ServiceProvider;

            this.database = database;
            this.Connection = connection;

            this.State = new State();

            this.Prefetcher = new Prefetcher(this);
            this.Commands = new Commands(this, connection);
        }

        public IServiceProvider ServiceProvider { get; }

        public Connection Connection { get; }

        public Commands Commands { get; }

        public State State { get; }

        private Prefetcher Prefetcher { get; }

        IDatabase ISession.Database => this.database;

        public Database Database => this.database;

        public object this[string name]
        {
            get
            {
                if (this.properties == null)
                {
                    return null;
                }

                object value;
                this.properties.TryGetValue(name, out value);
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

        public IObject Insert(IClass @class, string objectIdString)
        {
            var objectId = long.Parse(objectIdString);
            var insertedObject = this.Insert(@class, objectId);

            this.State.ChangeSet.OnCreated(objectId);

            return insertedObject;
        }

        public IObject Insert(IClass @class, long objectId)
        {
            if (this.State.ReferenceByObjectId.ContainsKey(objectId))
            {
                var oldStrategy = this.State.ReferenceByObjectId[objectId].Strategy;
                if (!oldStrategy.IsDeleted)
                {
                    throw new Exception("Duplicate object id " + objectId);
                }
            }

            var reference = this.Commands.InsertObject(@class, objectId);
            this.State.ReferenceByObjectId[objectId] = reference;
            var insertedObject = reference.Strategy.GetObject();

            this.State.ChangeSet.OnCreated(objectId);

            return insertedObject;
        }

        public IObject Instantiate(IObject obj)
        {
            return this.Instantiate(obj.Strategy.ObjectId);
        }

        public IObject Instantiate(string objectId)
        {
            long id;
            if (long.TryParse(objectId, out id))
            {
                return this.Instantiate(id);
            };

            return null;
        }

        public IObject Instantiate(long objectId)
        {
            var strategy = this.InstantiateStrategy(objectId);
            return strategy?.GetObject();
        }

        public IStrategy InstantiateStrategy(long objectId)
        {
            Reference reference;
            if (!this.State.ReferenceByObjectId.TryGetValue(objectId, out reference))
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

        public IObject[] Instantiate(string[] objectIdStrings)
        {
            if (objectIdStrings == null || objectIdStrings.Length == 0)
            {
                return EmptyObjects;
            }

            var objectIds = new long[objectIdStrings.Length];
            for (var i = 0; i < objectIdStrings.Length; i++)
            {
                objectIds[i] = long.Parse(objectIdStrings[i]);
            }

            return this.Instantiate(objectIds);
        }

        public IObject[] Instantiate(IObject[] objects)
        {
            if (objects == null || objects.Length == 0)
            {
                return EmptyObjects;
            }

            var objectIds = new long[objects.Length];
            for (var i = 0; i < objects.Length; i++)
            {
                objectIds[i] = objects[i].Strategy.ObjectId;
            }

            return this.Instantiate(objectIds);
        }

        public IObject[] Instantiate(long[] objectIds)
        {
            if (objectIds == null || objectIds.Length == 0)
            {
                return EmptyObjects;
            }

            var references = new List<Reference>(objectIds.Length);

            List<long> nonCachedObjectIds = null;
            foreach (var objectId in objectIds)
            {
                Reference reference;
                if (!this.State.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
                foreach (var objectId in objectIds)
                {
                    if (referenceById.TryGetValue(objectId, out Reference reference))
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

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects)
        {
            var objectIds = new HashSet<long>(objects.Select(x => x.Strategy.ObjectId));
            var references = this.Prefetcher.GetReferencesForPrefetching(objectIds);

            if (references.Count != 0)
            {
                this.Flush();

                var prefetcher = new Prefetch(this.Prefetcher, prefetchPolicy, references);
                prefetcher.Execute();
            }
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IStrategy[] strategies)
        {
            var objectIds = new HashSet<long>(strategies.Select(x => x.ObjectId));
            var references = this.Prefetcher.GetReferencesForPrefetching(objectIds);

            if (references.Count != 0)
            {
                this.Flush();

                var prefetcher = new Prefetch(this.Prefetcher, prefetchPolicy, references);
                prefetcher.Execute();
            }
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params string[] objectIdStrings)
        {
            var objectIds = new HashSet<long>(objectIdStrings.Select(v => long.Parse(v)));
            var references = this.Prefetcher.GetReferencesForPrefetching(objectIds);

            if (references.Count != 0)
            {
                this.Flush();

                var prefetcher = new Prefetch(this.Prefetcher, prefetchPolicy, references);
                prefetcher.Execute();
            }
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params long[] objectIds)
        {
            var references = this.Prefetcher.GetReferencesForPrefetching(new HashSet<long>(objectIds));

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

        public Extent<T> Extent<T>() where T : IObject
        {
            return this.Extent((IComposite)this.Database.ObjectFactory.GetObjectTypeForType(typeof(T)));
        }

        public Allors.Extent Extent(IComposite type)
        {
            return new ExtentFiltered(this, type);
        }

        public Allors.Extent Union(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Union);
        }

        public Allors.Extent Intersect(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Intersect);
        }

        public Allors.Extent Except(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Except);
        }

        public void Commit()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.State.ReferenceByObjectId.Keys);
                    var changed = Database.EmptyObjectIds;

                    this.Flush();

                    if (this.State.ModifiedRolesByReference != null)
                    {
                        this.Commands.UpdateVersion();

                        changed = this.State.ModifiedRolesByReference
                            .Select(dictionaryEntry => dictionaryEntry.Key.ObjectId).ToArray();
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

                    this.busyCommittingOrRollingBack = false;
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

        public void Dispose()
        {
            this.Rollback();
        }

        public T Create<T>() where T : IObject
        {
            var objectType = (IClass)this.Database.ObjectFactory.GetObjectTypeForType(typeof(T));
            return (T)this.Create(objectType);
        }

        public override string ToString()
        {
            return "Session[id=" + this.GetHashCode() + "] " + this.Database;
        }

        internal Reference GetAssociation(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationByRole = this.State.GetAssociationByRole(associationType);

            Reference association;
            if (!associationByRole.TryGetValue(roleStrategy.Reference, out association))
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

        internal Reference[] GetOrCreateReferencesForExistingObjects(IEnumerable<long> objectIds)
        {
            return this.State.GetOrCreateReferencesForExistingObjects(objectIds, this);
        }

        internal long[] GetAssociations(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            long[] associations;
            if (!associationsByRole.TryGetValue(roleStrategy.Reference, out associations))
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

            long[] associations;
            if (associationsByRole.TryGetValue(role, out associations))
            {
                associationsByRole[role] = associations.Add(association.ObjectId);
            }
        }

        internal void RemoveAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.State.GetAssociationsByRole(associationType);

            long[] associations;
            if (associationsByRole.TryGetValue(role, out associations))
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

        internal void AddReferenceWithoutVersionOrExistsKnown(Reference reference)
        {
            this.State.ReferencesWithoutVersions.Add(reference);
        }

        internal void GetVersionAndExists()
        {
            if (this.State.ReferencesWithoutVersions.Count > 0)
            {
                var versionByObjectId = this.Commands.GetVersions(this.State.ReferencesWithoutVersions);
                foreach (var association in this.State.ReferencesWithoutVersions)
                {
                    long version;
                    if (versionByObjectId.TryGetValue(association.ObjectId, out version))
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

            HashSet<long> associations;
            if (!this.State.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out associations))
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
                HashSet<long> roles;
                if (this.State.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out roles))
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
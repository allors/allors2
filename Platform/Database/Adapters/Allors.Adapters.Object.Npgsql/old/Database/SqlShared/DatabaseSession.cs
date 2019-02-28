// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSession.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors;
    using Allors.Meta;

    using Microsoft.Extensions.DependencyInjection;

    public abstract class DatabaseSession : ISession
    {
        private static readonly IObject[] EmptyObjects = { };

        private ChangeSet changeSet;

        private Dictionary<Reference, Roles> modifiedRolesByReference;
        private Dictionary<Reference, Roles> unflushedRolesByReference;
        private Dictionary<IAssociationType, HashSet<long>> triggersFlushRolesByAssociationType;

        private Dictionary<long, Reference> referenceByObjectId;
        private HashSet<Reference> referencesWithoutCacheId;

        private Dictionary<IAssociationType, Dictionary<Reference, Reference>> associationByRoleByAssociationType;
        private Dictionary<IAssociationType, Dictionary<Reference, long[]>> associationsByRoleByAssociationType;
        
        private bool busyCommittingOrRollingBack;

        private Dictionary<string, object> properties;

        protected DatabaseSession(Database database)
        {
            var serviceScopeFactory = database.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this.ServiceProvider = scope.ServiceProvider;

            this.referenceByObjectId = new Dictionary<long, Reference>();
            this.referencesWithoutCacheId = new HashSet<Reference>();

            this.associationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
            this.associationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

            this.changeSet = new ChangeSet();
        }
        
        public IServiceProvider ServiceProvider { get; }

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

        public ChangeSet SqlChangeSet
        {
            get
            {
                return this.changeSet;
            }
        }

        public IChangeSet Changes
        {
            get
            {
                return this.SqlChangeSet;
            }
        }

        public abstract Allors.IDatabase Database { get; }

        public abstract Database SqlDatabase { get; }

        public abstract SessionCommands SessionCommands { get; }

        public Schema Schema
        {
            get { return this.SqlDatabase.Schema; }
        }

        public virtual IObject Create(IClass objectType)
        {
            var strategyReference = this.SessionCommands.CreateObjectCommand.Execute(objectType);
            this.referenceByObjectId[strategyReference.ObjectId] = strategyReference;

            this.SqlDatabase.Cache.SetObjectType(strategyReference.ObjectId, objectType);

            this.changeSet.OnCreated(strategyReference.ObjectId);

            return strategyReference.Strategy.GetObject();
        }
        
        public virtual IObject[] Create(IClass objectType, int count)
        {
            var strategyReferences = this.SessionCommands.CreateObjectsCommand.Execute(objectType, count);

            var arrayType = this.SqlDatabase.ObjectFactory.GetTypeForObjectType(objectType);
            var domainObjects = (IObject[])Array.CreateInstance(arrayType, count);

            for (var i = 0; i < strategyReferences.Count; i++)
            {
                var strategyReference = strategyReferences[i];
                this.referenceByObjectId[strategyReference.ObjectId] = strategyReference;

                domainObjects[i] = strategyReference.Strategy.GetObject();

                this.changeSet.OnCreated(strategyReference.ObjectId);
            }

            return domainObjects;
        }

        public virtual IObject Insert(IClass domainType, string objectIdString)
        {
            var objectId = long.Parse(objectIdString);
            var insertedObject = this.Insert(domainType, objectId);

            this.changeSet.OnCreated(objectId);

            return insertedObject;
        }

        public virtual IObject Insert(IClass domainType, long objectId)
        {
            if (this.referenceByObjectId.ContainsKey(objectId))
            {
                var oldStrategy = this.referenceByObjectId[objectId].Strategy;
                if (!oldStrategy.IsDeleted)
                {
                    throw new Exception("Duplicate object id " + objectId);
                }
            }

            var strategyReference = this.SessionCommands.InsertObjectCommand.Execute(domainType, objectId);
            this.referenceByObjectId[objectId] = strategyReference;
            var insertedObject = strategyReference.Strategy.GetObject();

            this.changeSet.OnCreated(objectId);

            return insertedObject;
        }

        public virtual IObject Instantiate(IObject obj)
        {
            return this.Instantiate(obj.Strategy.ObjectId);
        }

        public virtual IObject Instantiate(string objectId)
        {
            var id = long.Parse(objectId);
            return this.Instantiate(id);
        }

        public virtual IObject Instantiate(long objectId)
        {
            var strategyReference = this.InstantiateSqlStrategy(objectId);
            if (strategyReference == null)
            {
                return null;
            }

            return strategyReference.Strategy.GetObject();
        }

        public virtual IStrategy InstantiateStrategy(long objectId)
        {
            var strategyReference = this.InstantiateSqlStrategy(objectId);

            if (strategyReference == null)
            {
                return null;
            }

            return strategyReference.Strategy;
        }

        public IObject[] Instantiate(IEnumerable<string> objectIdStrings)
        {
            return objectIdStrings != null ? this.Instantiate(objectIdStrings.Select(long.Parse)) : EmptyObjects;
        }

        public IObject[] Instantiate(IEnumerable<IObject> objects)
        {
            return objects != null ? this.Instantiate(objects.Select(v => v.Id)) : EmptyObjects;
        }

        public virtual IObject[] Instantiate(IEnumerable<long> objectIds)
        {
            var objectIdArray = objectIds.ToArray();
            var references = new List<Reference>(objectIdArray.Length);

            var nonCachedObjectIds = new List<long>();
            foreach (var objectId in objectIdArray)
            {
                if (!this.referenceByObjectId.TryGetValue(objectId, out var reference))
                {
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

            if (nonCachedObjectIds.Count > 0)
            {
                var nonCachedReferences = this.SessionCommands.InstantiateObjectsCommand.Execute(nonCachedObjectIds);
                references.AddRange(nonCachedReferences);

                var objectByObjectId = references.ToDictionary(strategyReference => strategyReference.ObjectId, strategyReference => strategyReference.Strategy.GetObject());

                var allorsObjects = new List<IObject>();
                foreach (var objectId in objectIdArray)
                {
                    if (objectByObjectId.TryGetValue(objectId, out var allorsObject))
                    {
                        allorsObjects.Add(allorsObject);
                    }
                }

                return allorsObjects.ToArray();
            }
            else
            {
                var allorsObjects = new IObject[references.Count];
                for (var i = 0; i < allorsObjects.Length; i++)
                {
                    allorsObjects[i] = references[i].Strategy.GetObject();
                }

                return allorsObjects;
            }
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects)
        {
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<string> objectIds)
        {
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<long> objectIds)
        {
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IStrategy> strategies)
        {
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IObject> objects)
        {
        }

        public IChangeSet Checkpoint()
        {
            try
            {
                return this.changeSet;
            }
            finally
            {
                this.changeSet = new ChangeSet();
            }
        }

        public virtual Extent<T> Extent<T>() where T : IObject
        {
            var compositeType = this.SqlDatabase.ObjectFactory.GetObjectTypeForType(typeof(T)) as IComposite;

            if (compositeType == null)
            {
                throw new Exception("type should be a CompositeType");
            }

            return this.Extent(compositeType);
        }
        
        public virtual Allors.Extent Extent(IComposite type)
        {
            return new ExtentFiltered(this, type);
        }

        public virtual Allors.Extent Union(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Union);
        }

        public virtual Allors.Extent Intersect(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Intersect);
        }

        public virtual Allors.Extent Except(Allors.Extent firstOperand, Allors.Extent secondOperand)
        {
            return new ExtentOperation(((Extent)firstOperand).ContainedInExtent, ((Extent)secondOperand).ContainedInExtent, ExtentOperations.Except);
        }

        public virtual void Commit()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.referenceByObjectId.Keys);
                    var changed = new long[0];

                    this.Flush();

                    if (this.modifiedRolesByReference != null)
                    {
                        this.UpdateCacheIds();

                        changed = this.modifiedRolesByReference.Select(dictionaryEntry => dictionaryEntry.Key.ObjectId).ToArray();
                    }

                    this.SqlCommit();

                    this.modifiedRolesByReference = null;

                    var referencesWithStrategy = new HashSet<Reference>();
                    foreach (var reference in this.referenceByObjectId.Values)
                    {
                        reference.Commit(referencesWithStrategy);
                    }

                    this.referencesWithoutCacheId = referencesWithStrategy;

                    this.referenceByObjectId = new Dictionary<long, Reference>();
                    foreach (var reference in referencesWithStrategy)
                    {
                        this.referenceByObjectId[reference.ObjectId] = reference;
                    }

                    this.associationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
                    this.associationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

                    this.changeSet = new ChangeSet();

                    this.busyCommittingOrRollingBack = false;
                    
                    this.SqlDatabase.Cache.OnCommit(accessed, changed);
                }
                finally
                {
                    this.busyCommittingOrRollingBack = false;
                }
            }
        }

        public virtual void Rollback()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    var accessed = new List<long>(this.referenceByObjectId.Keys);

                    this.SqlRollback();

                    var referencesWithStrategy = new HashSet<Reference>();
                    foreach (var reference in this.referenceByObjectId.Values)
                    {
                        reference.Rollback(referencesWithStrategy);
                    }

                    this.referencesWithoutCacheId = referencesWithStrategy;

                    this.referenceByObjectId = new Dictionary<long, Reference>();
                    foreach (var reference in referencesWithStrategy)
                    {
                        this.referenceByObjectId[reference.ObjectId] = reference;
                    }

                    this.unflushedRolesByReference = null;
                    this.modifiedRolesByReference = null;
                    this.triggersFlushRolesByAssociationType = null;

                    this.associationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
                    this.associationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

                    this.changeSet = new ChangeSet();

                    this.SqlDatabase.Cache.OnRollback(accessed);
                }
                finally
                {
                    this.busyCommittingOrRollingBack = false;
                }
            }
        }

        public virtual void Dispose()
        {
            this.Rollback();
        }

        public virtual T Create<T>() where T : IObject
        {
            var objectType = this.SqlDatabase.ObjectFactory.GetObjectTypeForType(typeof(T));
            var @class = objectType as IClass;
            if (@class == null)
            {
                throw new Exception("IObjectType is not a Class");
            }

            return (T)this.Create(@class);
        }

        public virtual Reference GetAssociation(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationByRole = this.GetAssociationByRole(associationType);

            Reference association;
            if (!associationByRole.TryGetValue(roleStrategy.Reference, out association))
            {
                this.FlushConditionally(roleStrategy, associationType);
                association = this.SessionCommands.GetCompositeAssociationCommand.Execute(roleStrategy.Reference, associationType);
                associationByRole[roleStrategy.Reference] = association;
            }

            return association;
        }

        public void SetAssociation(Reference previousAssociation, Strategy roleStrategy, IAssociationType associationType)
        {
            var associationByRole = this.GetAssociationByRole(associationType);
            associationByRole[roleStrategy.Reference] = previousAssociation;
        }

        public virtual long[] GetAssociations(Strategy roleStrategy, IAssociationType associationType)
        {
            var associationsByRole = this.GetAssociationsByRole(associationType);

            long[] associations;
            if (!associationsByRole.TryGetValue(roleStrategy.Reference, out associations))
            {
                this.FlushConditionally(roleStrategy, associationType);
                associations = this.SessionCommands.GetCompositeAssociationsCommand.Execute(roleStrategy, associationType);
                associationsByRole[roleStrategy.Reference] = associations;
            }

            return associations;
        }

        public void AddAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.GetAssociationsByRole(associationType);

            long[] associations;
            if (associationsByRole.TryGetValue(role, out associations))
            {
                var newAssociations = new long[associations.Length + 1];
                associations.CopyTo(newAssociations, 0);
                newAssociations[newAssociations.Length - 1] = association.ObjectId;
                associationsByRole[role] = newAssociations;
            }
        }

        public void RemoveAssociation(Reference association, Reference role, IAssociationType associationType)
        {
            var associationsByRole = this.GetAssociationsByRole(associationType);

            long[] associations;
            if (associationsByRole.TryGetValue(role, out associations))
            {
                var associationList = new List<long>(associations);
                associationList.Remove(association.ObjectId);
                associations = associationList.ToArray();
                associationsByRole[role] = associations;
            }
        }

        public virtual Reference[] GetOrCreateAssociationsForExistingObjects(IEnumerable<long> objectIds)
        {
            return objectIds.Select(this.GetOrCreateAssociationForExistingObject).ToArray();
        }

        public virtual Reference GetOrCreateAssociationForExistingObject(long objectId)
        {
            Reference association;
            if (!this.referenceByObjectId.TryGetValue(objectId, out association))
            {
                var objectType = this.SqlDatabase.Cache.GetObjectType(objectId);
                if (objectType == null)
                {
                    objectType = this.SessionCommands.GetObjectType.Execute(objectId);
                    this.SqlDatabase.Cache.SetObjectType(objectId, objectType);
                }

                var @class = objectType as IClass;
                if (@class == null)
                {
                    throw new Exception("IObjectType should be a class");
                }

                association = this.CreateReference(@class, objectId, false);
                this.referenceByObjectId[objectId] = association;
                this.referencesWithoutCacheId.Add(association);
            }

            return association;
        }

        public virtual Reference GetOrCreateAssociationForExistingObject(IClass objectType, long objectId)
        {
            Reference association;
            if (!this.referenceByObjectId.TryGetValue(objectId, out association))
            {
                association = this.CreateReference(objectType, objectId, false);
                this.referenceByObjectId[objectId] = association;
                this.referencesWithoutCacheId.Add(association);
            }

            return association;
        }

        public virtual Reference GetOrCreateAssociationForExistingObject(IClass objectType, long objectId, int cacheId)
        {
            Reference association;
            if (!this.referenceByObjectId.TryGetValue(objectId, out association))
            {
                association = this.CreateReference(objectType, objectId, cacheId);
                this.referenceByObjectId[objectId] = association;
            }

            return association;
        }

        public virtual Reference CreateAssociationForNewObject(IClass objectType, long objectId)
        {
            var strategyReference = this.CreateReference(objectType, objectId, true);
            this.referenceByObjectId[objectId] = strategyReference;
            return strategyReference;
        }

        public virtual Roles GetOrCreateRoles(Reference reference)
        {
            if (this.modifiedRolesByReference != null)
            {
                Roles roles;
                if (this.modifiedRolesByReference.TryGetValue(reference, out roles))
                {
                    return roles;
                }
            }

            return new Roles(reference);
        }

        public override string ToString()
        {
            return "Session[id=" + this.GetHashCode() + "] " + this.Database;
        }

        public abstract Command CreateCommand(string commandText);

        public virtual void Flush()
        {
            if (this.unflushedRolesByReference != null)
            {
                var flush = this.CreateFlush(this.unflushedRolesByReference);
                flush.Execute();
            }

            this.unflushedRolesByReference = null;
            this.triggersFlushRolesByAssociationType = null;
        }

        public virtual void FlushConditionally(Strategy strategy, IAssociationType associationType)
        {
            if (this.triggersFlushRolesByAssociationType != null)
            {
                HashSet<long> roles;
                if (this.triggersFlushRolesByAssociationType.TryGetValue(associationType, out roles))
                {
                    if (roles.Contains(strategy.ObjectId))
                    {
                        this.Flush();
                    }
                }
            }
        }

        internal void AddReferenceWithoutCacheId(Reference reference)
        {
            this.referencesWithoutCacheId.Add(reference);
        }

        protected internal virtual void GetCacheIdsAndExists()
        {
            var cacheIdByObjectId = this.SessionCommands.GetCacheIdsCommand.Execute(this.referencesWithoutCacheId);
            foreach (var association in this.referencesWithoutCacheId)
            {
                int cacheId;
                if (cacheIdByObjectId.TryGetValue(association.ObjectId, out cacheId))
                {
                    association.Version = cacheId;
                    association.Exists = true;
                }
                else
                {
                    association.Exists = false;
                }
            }

            this.referencesWithoutCacheId = new HashSet<Reference>();
        }

        protected internal virtual void RequireFlush(Reference association, Roles roles)
        {
            if (this.unflushedRolesByReference == null)
            {
                this.unflushedRolesByReference = new Dictionary<Reference, Roles>();
            }

            this.unflushedRolesByReference[association] = roles;

            if (this.modifiedRolesByReference == null)
            {
                this.modifiedRolesByReference = new Dictionary<Reference, Roles>();
            }
            
            this.modifiedRolesByReference[association] = roles;
        }

        protected internal virtual void TriggerFlush(long role, IAssociationType associationType)
        {
            if (this.triggersFlushRolesByAssociationType == null)
            {
                this.triggersFlushRolesByAssociationType = new Dictionary<IAssociationType, HashSet<long>>();    
            }

            HashSet<long> associations;
            if (!this.triggersFlushRolesByAssociationType.TryGetValue(associationType, out associations))
            {
                associations = new HashSet<long>();
                this.triggersFlushRolesByAssociationType[associationType] = associations;
            }

            associations.Add(role);
        }

        protected abstract IFlush CreateFlush(Dictionary<Reference, Roles> unsyncedRolesByReference);

        protected virtual void UpdateCacheIds()
        {
            this.SessionCommands.UpdateCacheIdsCommand.Execute(this.modifiedRolesByReference);
        }

        protected abstract void SqlCommit();

        protected abstract void SqlRollback();

        protected virtual Reference CreateReference(IClass objectType, long objectId, bool isNew)
        {
            return new Reference(this, objectType, objectId, isNew);
        }

        protected virtual Reference CreateReference(IClass objectType, long objectId, int cacheId)
        {
            return new Reference(this, objectType, objectId, cacheId);
        }

        private Reference InstantiateSqlStrategy(long objectId)
        {
            Reference strategyReference;
            if (!this.referenceByObjectId.TryGetValue(objectId, out strategyReference))
            {
                strategyReference = this.SessionCommands.InstantiateObjectCommand.Execute(objectId);
                if (strategyReference != null)
                {
                    this.referenceByObjectId[objectId] = strategyReference;
                }
            }

            if (strategyReference == null || !strategyReference.Exists)
            {
                return null;
            }

            return strategyReference;
        }

        private Dictionary<Reference, Reference> GetAssociationByRole(IAssociationType associationType)
        {
            Dictionary<Reference, Reference> associationByRole;
            if (!this.associationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<Reference, Reference>();
                this.associationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        private Dictionary<Reference, long[]> GetAssociationsByRole(IAssociationType associationType)
        {
            Dictionary<Reference, long[]> associationsByRole;
            if (!this.associationsByRoleByAssociationType.TryGetValue(associationType, out associationsByRole))
            {
                associationsByRole = new Dictionary<Reference, long[]>();
                this.associationsByRoleByAssociationType[associationType] = associationsByRole;
            }

            return associationsByRole;
        }
    }
}
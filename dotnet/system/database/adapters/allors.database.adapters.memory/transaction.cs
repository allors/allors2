// <copyright file="Transaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Meta;
    using Version = Allors.Version;

    public class Transaction : ITransaction
    {
        private static readonly HashSet<Strategy> EmptyStrategies = new HashSet<Strategy>();

        private readonly Dictionary<IObjectType, IObjectType[]> concreteClassesByObjectType;
        private bool busyCommittingOrRollingBack;

        private Dictionary<long, Strategy> strategyByObjectId;
        private Dictionary<IObjectType, HashSet<Strategy>> strategiesByObjectType;

        private long currentId;

        internal Transaction(Database database, ITransactionServices scope)
        {
            this.Database = database;
            this.Services = scope;

            this.busyCommittingOrRollingBack = false;

            this.concreteClassesByObjectType = new Dictionary<IObjectType, IObjectType[]>();

            this.ChangeLog = new ChangeLog();

            this.Reset();

            this.Services.OnInit(this);
        }

        public IDatabase Population => this.Database;

        IDatabase ITransaction.Database => this.Database;

        public ITransactionServices Services { get; }

        internal ChangeLog ChangeLog { get; private set; }

        internal Database Database { get; }

        public void Commit()
        {
            if (!this.busyCommittingOrRollingBack)
            {
                try
                {
                    this.busyCommittingOrRollingBack = true;

                    IList<Strategy> strategiesToDelete = null;
                    foreach (var dictionaryEntry in this.strategyByObjectId)
                    {
                        var strategy = dictionaryEntry.Value;

                        strategy.Commit();

                        if (strategy.IsDeleted)
                        {
                            strategiesToDelete ??= new List<Strategy>();
                            strategiesToDelete.Add(strategy);
                        }
                    }

                    if (strategiesToDelete != null)
                    {
                        foreach (var strategy in strategiesToDelete)
                        {
                            this.strategyByObjectId.Remove(strategy.ObjectId);

                            if (this.strategiesByObjectType.TryGetValue(strategy.UncheckedObjectType, out var strategies))
                            {
                                strategies.Remove(strategy);
                            }
                        }
                    }

                    this.ChangeLog = new ChangeLog();
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

                    foreach (var strategy in new List<Strategy>(this.strategyByObjectId.Values))
                    {
                        strategy.Rollback();
                        if (strategy.IsDeleted)
                        {
                            this.strategyByObjectId.Remove(strategy.ObjectId);

                            if (this.strategiesByObjectType.TryGetValue(strategy.UncheckedObjectType, out var strategies))
                            {
                                strategies.Remove(strategy);
                            }
                        }
                    }

                    this.ChangeLog = new ChangeLog();
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
            var objectType = this.Database.ObjectFactory.GetObjectType(typeof(T));

            if (!(objectType is IClass @class))
            {
                throw new Exception("IObjectType should be a class");
            }

            return (T)this.Create(@class);
        }

        public IObject[] Create(IClass objectType, int count)
        {
            var arrayType = this.Database.ObjectFactory.GetType(objectType);
            var allorsObjects = (IObject[])Array.CreateInstance(arrayType, count);
            for (var i = 0; i < count; i++)
            {
                allorsObjects[i] = this.Create(objectType);
            }

            return allorsObjects;
        }

        public IObject Instantiate(string objectIdString) => long.TryParse(objectIdString, out var id) ? this.Instantiate(id) : null;

        public IObject Instantiate(IObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            return this.Instantiate(obj.Strategy.ObjectId);
        }

        public IObject Instantiate(long objectId)
        {
            var strategy = this.InstantiateMemoryStrategy(objectId);
            return strategy?.GetObject();
        }

        public IStrategy InstantiateStrategy(long objectId) => this.InstantiateMemoryStrategy(objectId);

        public IObject[] Instantiate(IEnumerable<string> objectIdStrings) => objectIdStrings != null ? this.Instantiate(objectIdStrings.Select(long.Parse)) : Array.Empty<IObject>();

        public IObject[] Instantiate(IEnumerable<IObject> objects) => objects != null ? this.Instantiate(objects.Select(v => v.Id)) : Array.Empty<IObject>();

        public IObject[] Instantiate(IEnumerable<long> objectIds) => objectIds != null ? objectIds.Select(v => this.InstantiateMemoryStrategy(v)?.GetObject()).Where(v => v != null).ToArray() : Array.Empty<IObject>();

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects)
        {
            // nop
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<string> objectIds)
        {
            // nop
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<long> objectIds)
        {
            // nop
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IStrategy> strategies)
        {
            // nop
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IObject> objects)
        {
            // nop
        }

        public IChangeSet Checkpoint()
        {
            try
            {
                return this.ChangeLog.Checkpoint();
            }
            finally
            {
                this.ChangeLog = new ChangeLog();
            }
        }

        public Extent<T> Extent<T>() where T : IObject
        {
            if (!(this.Database.ObjectFactory.GetObjectType(typeof(T)) is IComposite compositeType))
            {
                throw new Exception("type should be a CompositeType");
            }

            return this.Extent(compositeType);
        }

        public virtual Allors.Database.Extent Extent(IComposite objectType) => new ExtentFiltered(this, objectType);

        public virtual Allors.Database.Extent Union(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand) => new ExtentOperation(this, (Extent)firstOperand, (Extent)secondOperand, ExtentOperationType.Union);

        public virtual Allors.Database.Extent Intersect(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand) => new ExtentOperation(this, (Extent)firstOperand, (Extent)secondOperand, ExtentOperationType.Intersect);

        public virtual Allors.Database.Extent Except(Allors.Database.Extent firstOperand, Allors.Database.Extent secondOperand)
        {
            var firstExtent = (Extent)firstOperand;
            var secondExtent = (Extent)secondOperand;

            return new ExtentOperation(this, firstExtent, secondExtent, ExtentOperationType.Except);
        }

        public virtual IObject Create(IClass objectType)
        {
            var strategy = new Strategy(this, objectType, ++this.currentId, Version.DatabaseInitial);
            this.AddStrategy(strategy);

            this.ChangeLog.OnCreated(strategy);

            return strategy.GetObject();
        }

        internal void Init() => this.Reset();

        internal Type GetTypeForObjectType(IObjectType objectType) => this.Database.ObjectFactory.GetType(objectType);

        internal virtual Strategy InsertStrategy(IClass objectType, long objectId, long objectVersion)
        {
            var strategy = this.GetStrategy(objectId);
            if (strategy != null)
            {
                throw new Exception("Duplicate id error");
            }

            if (this.currentId < objectId)
            {
                this.currentId = objectId;
            }

            strategy = new Strategy(this, objectType, objectId, objectVersion);
            this.AddStrategy(strategy);

            this.ChangeLog.OnCreated(strategy);

            return strategy;
        }

        internal virtual Strategy InstantiateMemoryStrategy(long objectId) => this.GetStrategy(objectId);

        internal Strategy GetStrategy(IObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            return this.GetStrategy(obj.Id);
        }

        internal Strategy GetStrategy(long objectId)
        {
            if (!this.strategyByObjectId.TryGetValue(objectId, out var strategy))
            {
                return null;
            }

            return strategy.IsDeleted ? null : strategy;
        }

        internal void AddStrategy(Strategy strategy)
        {
            this.strategyByObjectId.Add(strategy.ObjectId, strategy);

            if (!this.strategiesByObjectType.TryGetValue(strategy.UncheckedObjectType, out var strategies))
            {
                strategies = new HashSet<Strategy>();
                this.strategiesByObjectType.Add(strategy.UncheckedObjectType, strategies);
            }

            strategies.Add(strategy);
        }

        internal virtual HashSet<Strategy> GetStrategiesForExtentIncludingDeleted(IObjectType type)
        {
            if (!this.concreteClassesByObjectType.TryGetValue(type, out var concreteClasses))
            {
                var sortedClassAndSubclassList = new List<IObjectType>();

                if (type is IClass)
                {
                    sortedClassAndSubclassList.Add(type);
                }

                if (type is IInterface)
                {
                    foreach (var subClass in ((IInterface)type).DatabaseClasses)
                    {
                        sortedClassAndSubclassList.Add(subClass);
                    }
                }

                concreteClasses = sortedClassAndSubclassList.ToArray();

                this.concreteClassesByObjectType[type] = concreteClasses;
            }

            switch (concreteClasses.Length)
            {
                case 0:
                    return EmptyStrategies;

                case 1:
                {
                    var objectType = concreteClasses[0];
                    if (this.strategiesByObjectType.TryGetValue(objectType, out var strategies))
                    {
                        return strategies;
                    }

                    return EmptyStrategies;
                }

                default:
                {
                    var strategies = new HashSet<Strategy>();

                    foreach (var objectType in concreteClasses)
                    {
                        if (this.strategiesByObjectType.TryGetValue(objectType, out var objectTypeStrategies))
                        {
                            strategies.UnionWith(objectTypeStrategies);
                        }
                    }

                    return strategies;
                }
            }
        }

        internal void Save(XmlWriter writer)
        {
            var sortedNonDeletedStrategiesByObjectType = new Dictionary<IObjectType, List<Strategy>>();
            foreach (var dictionaryEntry in this.strategyByObjectId)
            {
                var strategy = dictionaryEntry.Value;
                if (!strategy.IsDeleted)
                {
                    var objectType = strategy.UncheckedObjectType;

                    if (!sortedNonDeletedStrategiesByObjectType.TryGetValue(objectType, out var sortedNonDeletedStrategies))
                    {
                        sortedNonDeletedStrategies = new List<Strategy>();
                        sortedNonDeletedStrategiesByObjectType[objectType] = sortedNonDeletedStrategies;
                    }

                    sortedNonDeletedStrategies.Add(strategy);
                }
            }

            foreach (var dictionaryEntry in sortedNonDeletedStrategiesByObjectType)
            {
                var sortedNonDeletedStrategies = dictionaryEntry.Value;
                sortedNonDeletedStrategies.Sort(new Strategy.ObjectIdComparer());
            }

            var save = new Save(this, writer, sortedNonDeletedStrategiesByObjectType);
            save.Execute();
        }

        private void Reset()
        {
            // Strategies
            this.strategyByObjectId = new Dictionary<long, Strategy>();
            this.strategiesByObjectType = new Dictionary<IObjectType, HashSet<Strategy>>();
        }
    }
}

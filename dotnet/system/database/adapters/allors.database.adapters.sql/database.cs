// <copyright file="Database.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using Allors;
    using Caching;
    using Meta;
    using Ranges;

    public abstract class Database : IDatabase
    {
        public static readonly IsolationLevel DefaultIsolationLevel = System.Data.IsolationLevel.Snapshot;

        private readonly Dictionary<IObjectType, HashSet<IObjectType>> concreteClassesByObjectType;

        private readonly Dictionary<IObjectType, IRoleType[]> sortedUnitRolesByObjectType;

        private ICacheFactory cacheFactory;

        protected Database(IDatabaseServices state, Configuration configuration)
        {
            this.Services = state;
            if (this.Services == null)
            {
                throw new Exception("Services is missing");
            }

            this.ObjectFactory = configuration.ObjectFactory;
            if (!this.ObjectFactory.MetaPopulation.IsValid)
            {
                throw new ArgumentException("Domain is invalid");
            }

            this.MetaPopulation = this.ObjectFactory.MetaPopulation;

            this.ConnectionString = configuration.ConnectionString;

            this.concreteClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();

            this.CommandTimeout = configuration.CommandTimeout;
            this.IsolationLevel = configuration.IsolationLevel;

            this.sortedUnitRolesByObjectType = new Dictionary<IObjectType, IRoleType[]>();

            this.CacheFactory = configuration.CacheFactory;
            this.Cache = this.CacheFactory.CreateCache();

            this.SchemaName = (configuration.SchemaName ?? "allors").ToLowerInvariant();
        }

        public abstract event ObjectNotLoadedEventHandler ObjectNotLoaded;

        public abstract event RelationNotLoadedEventHandler RelationNotLoaded;

        public IDatabaseServices Services { get; }

        public abstract IConnectionFactory ConnectionFactory
        {
            get;
            set;
        }

        public abstract IConnectionFactory ManagementConnectionFactory
        {
            get;
            set;
        }

        public ICacheFactory CacheFactory
        {
            get => this.cacheFactory;

            set => this.cacheFactory = value ?? (this.cacheFactory = new DefaultCacheFactory());
        }

        public abstract string Id { get; }

        public string SchemaName { get; }

        public IObjectFactory ObjectFactory { get; }

        public IMetaPopulation MetaPopulation { get; }

        public bool IsShared => true;

        public abstract bool IsValid
        {
            get;
        }

        public string ConnectionString { get; set; }

        protected internal ICache Cache { get; }

        public int? CommandTimeout { get; }

        public IsolationLevel? IsolationLevel { get; }

        public abstract Mapping Mapping
        {
            get;
        }

        internal IRanges<long> Ranges = new DefaultStructRanges<long>();

        public ITransaction CreateTransaction()
        {
            var connection = this.ConnectionFactory.Create();
            return this.CreateTransaction(connection);
        }

        public ITransaction CreateTransaction(IConnection connection)
        {
            if (!this.IsValid)
            {
                throw new Exception(this.ValidationMessage);
            }

            return new Transaction(this, connection, this.Services.CreateTransactionServices());
        }

        public abstract string ValidationMessage { get; }

        public abstract void Init();

        public abstract void Load(XmlReader reader);

        public abstract void Save(XmlWriter writer);

        public override string ToString() => "Population[driver=Sql, type=Connected, id=" + this.GetHashCode() + "]";

        ITransaction IDatabase.CreateTransaction() => this.CreateTransaction();

        public bool ContainsClass(IObjectType container, IObjectType containee)
        {
            if (container.IsClass)
            {
                return container.Equals(containee);
            }

            if (!this.concreteClassesByObjectType.TryGetValue(container, out var concreteClasses))
            {
                concreteClasses = new HashSet<IObjectType>(((IInterface)container).DatabaseClasses);
                this.concreteClassesByObjectType[container] = concreteClasses;
            }

            return concreteClasses.Contains(containee);
        }


        internal Type GetDomainType(IObjectType objectType) => this.ObjectFactory.GetType(objectType);

        public IRoleType[] GetSortedUnitRolesByObjectType(IObjectType objectType)
        {
            if (!this.sortedUnitRolesByObjectType.TryGetValue(objectType, out var sortedUnitRoles))
            {
                var sortedUnitRoleList = new List<IRoleType>(((IComposite)objectType).DatabaseRoleTypes.Where(r => r.ObjectType.IsUnit));
                sortedUnitRoleList.Sort();
                sortedUnitRoles = sortedUnitRoleList.ToArray();
                this.sortedUnitRolesByObjectType[objectType] = sortedUnitRoles;
            }

            return sortedUnitRoles;
        }
    }
}

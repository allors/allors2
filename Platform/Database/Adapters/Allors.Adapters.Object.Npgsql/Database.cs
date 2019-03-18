// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using global::Npgsql;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Adapters.Object.Npgsql.Caching;

    using Allors;
    using Allors.Adapters.Schema;
    using Allors.Meta;

    using NpgsqlTypes;

    public class Database : IDatabase
    {
        public static readonly long[] EmptyObjectIds = { };

        private readonly object lockObject = new object();

        private readonly IObjectFactory objectFactory;

        private readonly Dictionary<IObjectType, HashSet<IObjectType>> concreteClassesByObjectType;

        private readonly Dictionary<IObjectType, IRoleType[]> sortedUnitRolesByObjectType;

        private Mapping mapping;

        private bool? isValid;

        private string validationMessage;

        private IConnectionFactory connectionFactory;

        private IConnectionFactory managementConnectionFactory;

        private ICacheFactory cacheFactory;

        public Database(IServiceProvider serviceProvider, Configuration configuration)
        {
            this.ServiceProvider = serviceProvider;
            this.objectFactory = configuration.ObjectFactory;
            if (!this.objectFactory.MetaPopulation.IsValid)
            {
                throw new ArgumentException("Domain is invalid");
            }

            this.ConnectionString = configuration.ConnectionString;
            this.ConnectionFactory = configuration.ConnectionFactory;
            this.ManagementConnectionFactory = configuration.ManagementConnectionFactory;

            this.concreteClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();

            this.CommandTimeout = configuration.CommandTimeout;
            this.IsolationLevel = configuration.IsolationLevel;

            this.Serializable = configuration.Serializable;

            this.sortedUnitRolesByObjectType = new Dictionary<IObjectType, IRoleType[]>();

            this.CacheFactory = configuration.CacheFactory;
            this.Cache = this.CacheFactory.CreateCache();

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(this.ConnectionString);
            var applicationName = connectionStringBuilder.ApplicationName?.Trim();
            if (!string.IsNullOrWhiteSpace(applicationName))
            {
                this.Id = applicationName;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(connectionStringBuilder.Database))
                {
                    this.Id = connectionStringBuilder.Database.ToLowerInvariant();
                }
                else
                {
                    using (var connection = new NpgsqlConnection(this.ConnectionString))
                    {
                        connection.Open();
                        this.Id = connection.Database.ToLowerInvariant();
                    }
                }
            }

            this.SchemaName = (configuration.SchemaName ?? "allors").ToLowerInvariant();
        }

        public event ObjectNotLoadedEventHandler ObjectNotLoaded;

        public event RelationNotLoadedEventHandler RelationNotLoaded;
        
        public IServiceProvider ServiceProvider { get; }

        public IConnectionFactory ConnectionFactory
        {
            get => this.connectionFactory ?? (this.connectionFactory = new DefaultConnectionFactory());

            set => this.connectionFactory = value;
        }

        public IConnectionFactory ManagementConnectionFactory
        {
            get => this.managementConnectionFactory ?? (this.managementConnectionFactory = new DefaultConnectionFactory());

            set => this.managementConnectionFactory = value;
        }

        public ICacheFactory CacheFactory
        {
            get => this.cacheFactory;

            set => this.cacheFactory = value ?? (this.cacheFactory = new DefaultCacheFactory());
        }

        public string Id { get; }

        public string SchemaName { get; }

        public IObjectFactory ObjectFactory => this.objectFactory;

        public IMetaPopulation MetaPopulation => this.objectFactory.MetaPopulation;

        public IDatabase Serializable { get; }

        public bool IsDatabase => true;

        public bool IsWorkspace => false;

        public bool IsShared => true;

        public bool IsValid
        {
            get
            {
                if (!this.isValid.HasValue)
                {
                    lock (this.lockObject)
                    {
                        if (!this.isValid.HasValue)
                        {
                            var validate = this.Validate();
                            this.validationMessage = validate.Message;
                            return validate.IsValid;
                        }
                    }
                }

                return this.isValid.Value;
            }
        }

        internal string ConnectionString { get; set; }

        internal string AscendingSortAppendix => null;

        internal string DescendingSortAppendix => null;

        internal string Except => "EXCEPT";

        internal ICache Cache { get; }

        internal int CommandTimeout { get; }

        internal IsolationLevel IsolationLevel { get; }

        internal Mapping Mapping
        {
            get
            {
                if (this.ObjectFactory.MetaPopulation != null)
                {
                    if (this.mapping == null)
                    {
                        this.mapping = new Mapping(this);
                    }
                }

                return this.mapping;
            }
        }

        public ISession CreateSession()
        {
            var connection = this.ConnectionFactory.Create(this);
            return this.CreateSession(connection);
        }

        public ISession CreateSession(Connection connection)
        {
            if (!this.IsValid)
            {
                throw new Exception(this.validationMessage);
            }

            return new Session(this, connection);
        }

        public void Init()
        {
            try
            {
                new Initialization(this).Execute();
            }
            finally
            {
                this.ResetSchema();
                this.Cache.Invalidate();
            }
        }

        public void Load(XmlReader reader)
        {
            lock (this)
            {
                this.Init();

                using (var connection = new NpgsqlConnection(this.ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        var load = new Load(this, connection, this.ObjectNotLoaded, this.RelationNotLoaded);
                        load.Execute(reader);

                        connection.Close();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            connection.Close();
                        }
                        finally
                        {
                            this.Init();
                            throw e;
                        }
                    }
                }
            }
        }

        public void Save(XmlWriter writer)
        {
            lock (this)
            {
                var session = new ManagementSession(this, this.ManagementConnectionFactory);
                try
                {
                    var save = new Save(this, writer);
                    save.Execute(session);
                }
                finally
                {
                    session.Rollback();
                }
            }
        }

        public override string ToString()
        {
            return "Population[driver=Sql, type=Connected, id=" + this.GetHashCode() + "]";
        }

        public Validation Validate()
        {
            var validateResult = new Validation(this);
            this.isValid = validateResult.IsValid;
            return validateResult;
        }

        ISession IDatabase.CreateSession()
        {
            return this.CreateSession();
        }

        internal bool ContainsConcreteClass(IObjectType container, IObjectType containee)
        {
            if (container.IsClass)
            {
                return container.Equals(containee);
            }

            HashSet<IObjectType> concreteClasses;
            if (!this.concreteClassesByObjectType.TryGetValue(container, out concreteClasses))
            {
                concreteClasses = new HashSet<IObjectType>(((IInterface)container).Subclasses);
                this.concreteClassesByObjectType[container] = concreteClasses;
            }

            return concreteClasses.Contains(containee);
        }
        
        internal Type GetDomainType(IObjectType objectType)
        {
            return this.ObjectFactory.GetTypeForObjectType(objectType);
        }

        internal IRoleType[] GetSortedUnitRolesByObjectType(IObjectType objectType)
        {
            if (!this.sortedUnitRolesByObjectType.TryGetValue(objectType, out var sortedUnitRoles))
            {
                var sortedUnitRoleList = new List<IRoleType>(((IComposite)objectType).RoleTypes.Where(r => r.ObjectType.IsUnit));
                sortedUnitRoleList.Sort();
                sortedUnitRoles = sortedUnitRoleList.ToArray();
                this.sortedUnitRolesByObjectType[objectType] = sortedUnitRoles;
            }

            return sortedUnitRoles;
        }

        internal NpgsqlDbType GetNpgsqlDbType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.UnitTag)
            {
                case UnitTags.String:
                    if (roleType.Size == -1 || roleType.Size > 4000)
                    {
                        return NpgsqlDbType.Text;
                    }

                    return NpgsqlDbType.Varchar;
                case UnitTags.Integer:
                    return NpgsqlDbType.Integer;
                case UnitTags.Decimal:
                    return NpgsqlDbType.Numeric;
                case UnitTags.Float:
                    return NpgsqlDbType.Double;
                case UnitTags.Boolean:
                    return NpgsqlDbType.Boolean;
                case UnitTags.DateTime:
                    return NpgsqlDbType.Timestamp;
                case UnitTags.Unique:
                    return NpgsqlDbType.Uuid;
                case UnitTags.Binary:
                    return NpgsqlDbType.Bytea;
                default:
                    throw new Exception("!UNKNOWN VALUE TYPE!");
            }
        }

        internal string GetSqlType(NpgsqlDbType type)
        {
            switch (type)
            {
                case NpgsqlDbType.Varchar:
                    return "VARCHAR";
                case NpgsqlDbType.Text:
                    return "TEXT";
                case NpgsqlDbType.Integer:
                    return "INTEGER";
                case NpgsqlDbType.Bigint:
                    return "BIGINT";
                case NpgsqlDbType.Numeric:
                    return "NUMERIC";
                case NpgsqlDbType.Double:
                    return "DOUBLE PRECISION";
                case NpgsqlDbType.Boolean:
                    return "BOOLEAN";
                case NpgsqlDbType.Date:
                    return "DATE";
                case NpgsqlDbType.Timestamp:
                    return "TIMESTAMP";
                case NpgsqlDbType.Uuid:
                    return "UUID";
                case NpgsqlDbType.Bytea:
                    return "BYTEA";
                default:
                    return "!UNKNOWN VALUE TYPE!";
            }
        }

        private void ResetSchema()
        {
            this.mapping = null;
        }
    }
}
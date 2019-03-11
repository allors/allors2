// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Database.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using Allors.Adapters.Database.Caching;
    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    using NpgsqlTypes;

    public class Database : Allors.IDatabase
    {
        public static readonly long[] EmptyObjectIds = { };

        private const string ConnectionStringsKey = "allors";

        private readonly string id;

        private readonly Dictionary<IObjectType, IRoleType[]> sortedUnitRolesByObjectType;

        private readonly IObjectFactory objectFactory;

        private readonly Dictionary<IObjectType, HashSet<IObjectType>> concreteClassesByObjectType;

        protected Dictionary<string, object> Properties;


        public Database(IServiceProvider serviceProvider, Configuration configuration)
        {
            this.objectFactory = configuration.ObjectFactory;
            this.concreteClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();

            this.ServiceProvider = serviceProvider;
            this.Serializable = configuration.Serializable;

            this.ConnectionString = configuration.ConnectionString;
            this.CommandTimeout = configuration.CommandTimeout;
            this.IsolationLevel = configuration.IsolationLevel;

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(this.ConnectionString);
            var applicationName = connectionStringBuilder.ApplicationName?.Trim();
            if (!string.IsNullOrWhiteSpace(applicationName))
            {
                this.id = applicationName;
            }
            else
            {
                using (var connection = new NpgsqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    this.id = connection.Database?.ToLowerInvariant() ?? this.ConnectionString;
                }
            }

            this.sortedUnitRolesByObjectType = new Dictionary<IObjectType, IRoleType[]>();

            this.Cache = configuration.CacheFactory.CreateCache(this);
        }

        public Schema NpgsqlSchema
        {
            get
            {
                if (this.ObjectFactory.MetaPopulation != null)
                {
                    if (this.schema == null)
                    {
                        this.schema = new Schema(this);
                    }
                }

                return this.schema;
            }
        }

        public IObjectFactory ObjectFactory => this.objectFactory;

        public IMetaPopulation MetaPopulation => this.objectFactory.MetaPopulation;

        public object this[string name]
        {
            get
            {
                if (this.Properties == null)
                {
                    return null;
                }

                object value;
                this.Properties.TryGetValue(name, out value);
                return value;
            }

            set
            {
                if (this.Properties == null)
                {
                    this.Properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.Properties.Remove(name);
                }
                else
                {
                    this.Properties[name] = value;
                }
            }
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

        public event ObjectNotLoadedEventHandler ObjectNotLoaded;

        public event RelationNotLoadedEventHandler RelationNotLoaded;

        public Allors.IDatabase Serializable { get; }

        public IServiceProvider ServiceProvider { get; }

        public bool IsDatabase => true;

        public bool IsWorkspace => false;

        public string Id => this.id;

        public bool IsShared => true;

        public virtual string AscendingSortAppendix => null;

        public virtual string DescendingSortAppendix => null;

        public virtual string Except => "EXCEPT";
        
        public ICache Cache { get; }

        public string ConnectionString { get; }

        public int CommandTimeout { get; }

        public IsolationLevel IsolationLevel { get; }

        public ISession CreateSession()
        {
            return this.CreateDatabaseSession();
        }

        public void Recover()
        {
            if (!ObjectFactory.MetaPopulation.IsValid)
            {
                throw new ArgumentException("Domain is invalid");
            }

            var session = this.CreateManagementSession();
            try
            {
                this.DropIndexes(session);
                this.DropTriggers(session);
                this.DropProcedures(session);
                this.DropFunctions(session);
                this.DropUserDefinedTypes(session);

                this.CreateUserDefinedTypes(session);
                this.CreateFunctions(session);
                this.CreateProcedures(session);
                this.CreateTriggers(session);
                this.CreateIndexes(session);

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
            finally
            {
                this.ResetSchema();
                this.Cache.Invalidate();
            }
        }

        ISession Allors.IDatabase.CreateSession()
        {
            return this.CreateDatabaseSession();
        }

        public virtual ISession CreateDatabaseSession()
        {
            if (Schema.SchemaValidationErrors.HasErrors)
            {
                var errors = new StringBuilder();
                foreach (var error in Schema.SchemaValidationErrors.Errors)
                {
                    errors.Append("\n");
                    errors.Append(error.Message);
                }

                throw new SchemaValidationException(Schema.SchemaValidationErrors, "Database schema is not compatible with domain.\nUpgrade manually or use Save & Load.\n" + errors);
            }

            var session = this.CreateSqlSession();

            return session;
        }

        public void Init()
        {
            this.Init(true);
        }

        public void Init(bool allowTruncate)
        {
            if (!ObjectFactory.MetaPopulation.IsValid)
            {
                throw new ArgumentException("Domain is invalid");
            }

            var session = this.CreateManagementSession();
            try
            {
                if (allowTruncate && !this.Schema.SchemaValidationErrors.HasErrors)
                {
                    this.TruncateTables(session);
                }
                else
                {
                    this.DropTriggers(session);
                    this.DropProcedures(session);
                    this.DropFunctions(session);
                    this.DropUserDefinedTypes(session);
                    this.ResetSequence(session);
                    this.DropTables(session);
                    this.CreateTables(session);
                    this.CreateUserDefinedTypes(session);
                    this.CreateFunctions(session);
                    this.CreateProcedures(session);
                    this.CreateTriggers(session);
                    this.CreateIndexes(session);
                }

                session.Commit();
            }
            catch
            {
                try
                {
                    session.Rollback();
                }
                catch
                {
                }

                throw;
            }
            finally
            {
                this.Properties = null;
                this.ResetSchema();
                this.Cache.Invalidate();
            }
        }

        public void Load(XmlReader reader)
        {
            this.Init();

            var session = this.CreateManagementSession();
            try
            {
                var load = this.CreateLoad(this.ObjectNotLoaded, this.RelationNotLoaded, reader);
                load.Execute(session);
                session.Commit();
            }
            catch
            {
                session.Rollback();
                this.Init();
                throw;
            }
        }

        public void Save(XmlWriter writer)
        {
            var session = this.CreateManagementSession();
            try
            {
                var save = this.CreateSave(writer);
                save.Execute(session);
            }
            finally
            {
                session.Rollback();
            }
        }

        public override string ToString()
        {
            return "Population[driver=Sql, type=Connected, id=" + this.GetHashCode() + "]";
        }

        public Type GetDomainType(IObjectType objectType)
        {
            return this.ObjectFactory.GetTypeForObjectType(objectType);
        }

        public IRoleType[] GetSortedUnitRolesByObjectType(IComposite objectType)
        {
            IRoleType[] sortedUnitRoles;
            if (!this.sortedUnitRolesByObjectType.TryGetValue(objectType, out sortedUnitRoles))
            {
                var sortedUnitRoleList = new List<IRoleType>(objectType.RoleTypes.Where(roleType => roleType.ObjectType is IUnit));
                sortedUnitRoleList.Sort();
                sortedUnitRoles = sortedUnitRoleList.ToArray();
                this.sortedUnitRolesByObjectType[objectType] = sortedUnitRoles;
            }

            return sortedUnitRoles;
        }
        
        protected virtual void TruncateTables(ManagementSession session)
        {
            this.ResetSequence(session);

            foreach (SchemaTable table in Schema)
            {
                switch (table.Kind)
                {
                    case SchemaTableKind.System:
                    case SchemaTableKind.Object:
                    case SchemaTableKind.Relation:
                        this.TruncateTables(session, table);
                        break;
                }
            }
        }
        
        protected virtual void CreateUserDefinedTypes(ManagementSession session)
        {
        }

        protected virtual void CreateFunctions(ManagementSession session)
        {
        }

        protected virtual void CreateIndexes(ManagementSession session)
        {
            foreach (SchemaTable table in Schema)
            {
                foreach (SchemaColumn column in table)
                {
                    if (column.IndexType != SchemaIndexType.None)
                    {
                        this.CreateIndex(session, table, column);
                    }
                }
            }
        }

        protected virtual void DropIndexes(ManagementSession session)
        {
            foreach (SchemaTable table in Schema)
            {
                foreach (SchemaColumn column in table)
                {
                    if (column.IndexType != SchemaIndexType.None)
                    {
                        this.DropIndex(session, table, column);
                    }
                }
            }
        }

        protected virtual void CreateProcedures(ManagementSession session)
        {
            foreach (var procedure in Schema.Procedures)
            {
                this.CreateProcedure(session, procedure);
            }
        }

        protected virtual void CreateTables(ManagementSession session)
        {
            foreach (SchemaTable table in Schema)
            {
                this.CreateTable(session, table);
            }
        }

        protected virtual void CreateTriggers(ManagementSession session)
        {
        }

        protected virtual void DropFunctions(ManagementSession session)
        {
        }

        protected virtual void DropTables(ManagementSession session)
        {
            foreach (SchemaTable table in Schema)
            {
                this.DropTable(session, table);
            }
        }

        protected virtual void DropTriggers(ManagementSession session)
        {
        }









        private Schema schema;

        public Sql.Schema Schema => this.NpgsqlSchema;

        protected string IdentityType => "bigserial";

        public void ResetSchema()
        {
            this.schema = null;
        }

        public DbConnection CreateDbConnection()
        {
            return new NpgsqlConnection(this.ConnectionString);
        }

        public void DropIndex(ManagementSession session, SchemaTable table, SchemaColumn column)
        {
            var indexName = Sql.Schema.AllorsPrefix + table.Name + "_" + column.Name;

            try
            {
                var sql = new StringBuilder();
                sql.Append("DROP INDEX IF EXISTS " + indexName);
                session.ExecuteSql(sql.ToString());
            }
            catch (Exception e)
            {
                throw new Exception("Could not drop index " + indexName, e);
            }
        }

        internal object[] CreateObjectTable(IEnumerable<long> objectIds)
        {
            return objectIds.Cast<object>().ToArray();
        }

        internal object[] CreateObjectTable(IEnumerable<Reference> strategies)
        {
            return strategies.Select(strategy => strategy.ObjectId).Cast<object>().ToArray();
        }

        internal object[] CreateObjectTable(Dictionary<Reference, Roles> rolesByReference)
        {
            return rolesByReference
                .Select(dictionaryEntry => dictionaryEntry.Key)
                .Select(strategy => strategy.ObjectId)
                .Cast<object>()
                .ToArray();
        }

        internal object[] CreateAssociationTable(IEnumerable<CompositeRelation> relations)
        {
            return relations.Select(relation => relation.Association).Cast<object>().ToArray();
        }
        
        internal object[] CreateAssociationTable(IEnumerable<UnitRelation> relations)
        {
            return relations.Select(relation => relation.Association).Cast<object>().ToArray(); 
        }
        
        internal object[] CreateRoleTable(IEnumerable<CompositeRelation> relations)
        {
            return relations.Select(relation => relation.Role).Cast<object>().ToArray();
        }

        internal object[] CreateRoleTable(IEnumerable<UnitRelation> relations)
        {
            var roleArray = relations.Select(relation => relation.Role).ToArray();
            return roleArray;
        }
        
        internal string GetSqlType(SchemaColumn column)
        {
            if (column.IsIdentity)
            {
                return this.IdentityType;
            }

            switch (column.DbType)
            {
                case DbType.String:
                    if (column.Size == -1)
                    {
                        return "text";
                    }

                    return "VARCHAR(" + column.Size + ") ";
                case DbType.Int32:
                    return "INTEGER ";
                case DbType.Int64:
                    return "BIGINT  ";
                case DbType.Decimal:
                    return "NUMERIC(" + column.Precision + "," + column.Scale + ") ";
                case DbType.Double:
                    return "DOUBLE PRECISION ";
                case DbType.Boolean:
                    return "BOOLEAN ";
                case DbType.Date:
                case DbType.DateTime:
                    return "TIMESTAMP ";
                case DbType.Guid:
                    return "UUID ";
                case DbType.Binary:
                    return "BYTEA ";
                default:
                    return "!UNKNOWN VALUE TYPE!";
            }
        }

        internal string GetSqlType(DbType type)
        {
            switch (type)
            {
                case DbType.String:
                    return "VARCHAR";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.Int64:
                    return "BIGINT";
                case DbType.Decimal:
                    return "NUMERIC";
                case DbType.Double:
                    return "DOUBLE PRECISION";
                case DbType.Boolean:
                    return "BOOLEAN";
                case DbType.Date:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Guid:
                    return "UUID";
                case DbType.Binary:
                    return "BYTEA";
                default:
                    return "!UNKNOWN VALUE TYPE!";
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

        internal ManagementSession CreateNpgsqlManagementSession()
        {
            return new ManagementSession(this);
        }

        protected internal ManagementSession CreateManagementSession()
        {
            return this.CreateNpgsqlManagementSession();
        }

        protected ISession CreateSqlSession()
        {
            return new DatabaseSession(this);
        }

        protected void ResetSequence(ManagementSession session)
        {
            //var sql = new StringBuilder();
            //sql.Append("DROP SEQUENCE IF EXISTS " + this.Schema.Objects.Name + "_SEQ");
            //session.ExecuteSql(sql.ToString());

            //sql = new StringBuilder();
            //sql.Append("CREATE SEQUENCE " + this.Schema.Objects.Name + "_SEQ");
            //session.ExecuteSql(sql.ToString());
        }

        protected void DropUserDefinedTypes(ManagementSession session)
        {
        }

        protected void DropTable(ManagementSession session, SchemaTable schemaTable)
        {
            var sql = new StringBuilder();
            sql.Append(
@"DROP TABLE IF EXISTS " + schemaTable + " CASCADE;");
            session.ExecuteSql(sql.ToString());
        }

        protected void CreateTable(ManagementSession session, SchemaTable schemaTable)
        {
            var keys = new List<SchemaColumn>();

            var sql = new StringBuilder();
            sql.Append("CREATE TABLE " + schemaTable.StatementName);
            sql.Append("(");
            foreach (SchemaColumn column in schemaTable)
            {
                sql.Append(column.StatementName + " " + this.GetSqlType(column));
                if (column.IsKey)
                {
                    keys.Add(column);
                }

                sql.Append(",\n");
            }

            sql.Append("CONSTRAINT " + schemaTable.Name + "_pk PRIMARY KEY ( ");
            for (int i = 0; i < keys.Count; i++)
            {
                if (i > 0)
                {
                    sql.Append(", ");
                }

                sql.Append(keys[i].Name);
            }

            sql.Append(" )\n");
            sql.Append(")");
            session.ExecuteSql(sql.ToString());
        }

        protected void DropProcedures(ManagementSession session)
        {
        }

        protected void CreateProcedure(ManagementSession session, SchemaProcedure storedProcedure)
        {
            session.ExecuteSql(storedProcedure.Definition);
        }

        protected void CreateIndex(ManagementSession session, SchemaTable table, SchemaColumn column)
        {
            var indexName = Sql.Schema.AllorsPrefix + table.Name + "_" + column.Name;

            if (column.IndexType == SchemaIndexType.Single)
            {
                var sql = new StringBuilder();
                sql.Append("CREATE INDEX " + indexName + "\n");
                sql.Append("ON " + table + " (" + column + ")");
                session.ExecuteSql(sql.ToString());
            }
            else
            {
                var sql = new StringBuilder();
                sql.Append("CREATE INDEX " + indexName + "\n");
                sql.Append("ON " + table + " (" + column + ", " + table.FirstKeyColumn + ")");
                session.ExecuteSql(sql.ToString());
            }
        }

        protected void TruncateTables(ManagementSession session, SchemaTable table)
        {
            var sql = new StringBuilder();
            sql.Append("TRUNCATE TABLE " + table.StatementName);
            session.ExecuteSql(sql.ToString());
        }

        protected Sql.Load CreateLoad(ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded, System.Xml.XmlReader reader)
        {
            return new Load(this, objectNotLoaded, relationNotLoaded, reader);
        }

        protected Sql.Save CreateSave(System.Xml.XmlWriter writer)
        {
            return new Save(this, writer);
        }
    }
}
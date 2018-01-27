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

    using Allors.Adapters.Database.Sql;

    using global::Npgsql;

    using NpgsqlTypes;

    public abstract class Database : Sql.Database
    {
        private readonly CommandFactories commandFactories;

        protected Database(Configuration configuration)
            : base(configuration)
        {
            this.commandFactories = new CommandFactories(this);
        }

        public CommandFactories NpgsqlCommandFactories
        {
            get
            {
                return this.commandFactories;
            }
        }

        public override Sql.Schema Schema
        {
            get
            {
                return this.NpgsqlSchema;
            }
        }

        public abstract Schema NpgsqlSchema { get; }

        public override Sql.CommandFactories CommandFactories
        {
            get { return this.commandFactories; }
        }

        protected abstract string IdentityType { get; }

        public override DbConnection CreateDbConnection()
        {
            return new NpgsqlConnection(this.ConnectionString);
        }

        public override void DropIndex(Sql.ManagementSession session, SchemaTable table, SchemaColumn column)
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
            return objectIds.Select(objectId => objectId.Value).ToArray();
        }

        internal object[] CreateObjectTable(IEnumerable<Reference> strategies)
        {
            return strategies.Select(strategy => strategy.ObjectId.Value).ToArray();
        }

        internal object[] CreateObjectTable(Dictionary<Reference, Roles> rolesByReference)
        {
            return rolesByReference
                .Select(dictionaryEntry => dictionaryEntry.Key)
                .Select(strategy => strategy.ObjectId.Value)
                .ToArray();
        }

        internal object[] CreateAssociationTable(IEnumerable<CompositeRelation> relations)
        {
            return relations.Select(relation => relation.Association.Value).ToArray();
        }
        
        internal object[] CreateAssociationTable(IEnumerable<UnitRelation> relations)
        {
            return relations.Select(relation => relation.Association.Value).ToArray(); 
        }
        
        internal object[] CreateRoleTable(IEnumerable<CompositeRelation> relations)
        {
            return relations.Select(relation => relation.Role.Value).ToArray();
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
                    return "DATE ";
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
                    return "DATE";
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

        protected internal override Sql.ManagementSession CreateManagementSession()
        {
            return this.CreateNpgsqlManagementSession();
        }

        protected override ISession CreateSqlSession()
        {
            return new DatabaseSession(this);
        }

        protected override void ResetSequence(Sql.ManagementSession session)
        {
            //var sql = new StringBuilder();
            //sql.Append("DROP SEQUENCE IF EXISTS " + this.Schema.Objects.Name + "_SEQ");
            //session.ExecuteSql(sql.ToString());

            //sql = new StringBuilder();
            //sql.Append("CREATE SEQUENCE " + this.Schema.Objects.Name + "_SEQ");
            //session.ExecuteSql(sql.ToString());
        }

        protected override void DropUserDefinedTypes(Sql.ManagementSession session)
        {
        }

        protected override void DropTable(Sql.ManagementSession session, SchemaTable schemaTable)
        {
            var sql = new StringBuilder();
            sql.Append(
@"DROP TABLE IF EXISTS " + schemaTable + " CASCADE;");
            session.ExecuteSql(sql.ToString());
        }

        protected override void CreateTable(Sql.ManagementSession session, SchemaTable schemaTable)
        {
            SchemaColumn identity = null;
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

        protected override void DropProcedures(Sql.ManagementSession session)
        {
        }

        protected override void CreateProcedure(Sql.ManagementSession session, SchemaProcedure storedProcedure)
        {
            session.ExecuteSql(storedProcedure.Definition);
        }

        protected override void CreateIndex(Sql.ManagementSession session, SchemaTable table, SchemaColumn column)
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

        protected override void TruncateTables(Sql.ManagementSession session, SchemaTable table)
        {
            var sql = new StringBuilder();
            sql.Append("TRUNCATE TABLE " + table.StatementName);
            session.ExecuteSql(sql.ToString());
        }

        protected override Sql.Load CreateLoad(ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded, System.Xml.XmlReader reader)
        {
            return new Load(this, objectNotLoaded, relationNotLoaded, reader);
        }

        protected override Sql.Save CreateSave(System.Xml.XmlWriter writer)
        {
            return new Save(this, writer);
        }
    }
}
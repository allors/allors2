// <copyright file="Initialization.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Text;
    using Meta;
    using global::Npgsql;

    public class Initialization
    {
        private readonly Database database;
        private readonly Mapping mapping;

        private Validation validation;

        internal Initialization(Database database)
        {
            this.database = database;
            this.mapping = (Mapping)database.Mapping;
        }

        internal void Execute()
        {
            this.validation = new Validation(this.database);

            if (this.validation.IsValid)
            {
                this.TruncateTables();
            }
            else
            {
                this.CreateSchema();

                this.DropProcedures();

                this.DropTables();

                this.CreateTables();

                this.CreateProcedures();

                this.CreateIndeces();
            }
        }

        private void CreateSchema()
        {
            if (!this.validation.Schema.Exists)
            {
                // CREATE SCHEMA must be in its own batch
                using (var connection = new NpgsqlConnection(this.database.ConnectionString))
                {
                    connection.Open();
                    try
                    {
                        var cmdText = @"
CREATE SCHEMA " + this.database.SchemaName;
                        using (var command = new NpgsqlCommand(cmdText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void DropProcedures()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    foreach (var name in this.validation.Schema.ProcedureByName.Keys)
                    {
                        using (var command = new NpgsqlCommand("DROP FUNCTION " + name, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void TruncateTables()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    this.TruncateTable(connection, this.mapping.TableNameForObjects);

                    foreach (var @class in this.mapping.Database.MetaPopulation.DatabaseClasses)
                    {
                        var tableName = this.mapping.TableNameForObjectByClass[@class];

                        this.TruncateTable(connection, tableName);
                    }

                    foreach (var relationType in this.mapping.Database.MetaPopulation.DatabaseRelationTypes)
                    {
                        var associationType = relationType.AssociationType;
                        var roleType = relationType.RoleType;

                        if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                        {
                            var tableName = this.mapping.TableNameForRelationByRelationType[relationType];
                            this.TruncateTable(connection, tableName);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void DropTables()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    this.DropTable(connection, this.mapping.TableNameForObjects);

                    foreach (var @class in this.mapping.Database.MetaPopulation.DatabaseClasses)
                    {
                        var tableName = this.mapping.TableNameForObjectByClass[@class];

                        this.DropTable(connection, tableName);
                    }

                    foreach (var relationType in this.mapping.Database.MetaPopulation.DatabaseRelationTypes)
                    {
                        var associationType = relationType.AssociationType;
                        var roleType = relationType.RoleType;

                        if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                        {
                            var tableName = this.mapping.TableNameForRelationByRelationType[relationType];
                            this.DropTable(connection, tableName);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CreateTables()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    {
                        var sql = new StringBuilder();
                        sql.Append("CREATE TABLE " + this.mapping.TableNameForObjects + "\n");
                        sql.Append("(\n");
                        sql.Append(Mapping.ColumnNameForObject + " BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,\n");
                        sql.Append(Mapping.ColumnNameForClass + " " + Mapping.SqlTypeForClass + ",\n");
                        sql.Append(Mapping.ColumnNameForVersion + " " + Mapping.SqlTypeForVersion + "\n");
                        sql.Append(")\n");

                        using (var command = new NpgsqlCommand(sql.ToString(), connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    foreach (var @class in this.mapping.Database.MetaPopulation.DatabaseClasses)
                    {
                        var tableName = this.mapping.TableNameForObjectByClass[@class];

                        var sql = new StringBuilder();
                        sql.Append("CREATE TABLE " + tableName + "\n");
                        sql.Append("(\n");
                        sql.Append(Mapping.ColumnNameForObject + " " + Mapping.SqlTypeForObject + " PRIMARY KEY,\n");
                        sql.Append(Mapping.ColumnNameForClass + " " + Mapping.SqlTypeForClass);

                        foreach (var associationType in @class.DatabaseAssociationTypes)
                        {
                            var relationType = associationType.RelationType;
                            var roleType = relationType.RoleType;
                            if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && roleType.IsMany)
                            {
                                sql.Append(",\n" + this.mapping.ColumnNameByRelationType[relationType] + " " + Mapping.SqlTypeForObject);
                            }
                        }

                        foreach (var roleType in @class.DatabaseRoleTypes)
                        {
                            var relationType = roleType.RelationType;
                            var associationType3 = relationType.AssociationType;
                            if (roleType.ObjectType.IsUnit)
                            {
                                sql.Append(",\n" + this.mapping.ColumnNameByRelationType[relationType] + " " + this.mapping.GetSqlType(roleType));
                            }
                            else if (!(associationType3.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && !roleType.IsMany)
                            {
                                sql.Append(",\n" + this.mapping.ColumnNameByRelationType[relationType] + " " + Mapping.SqlTypeForObject);
                            }
                        }

                        sql.Append(")\n");

                        using (var command = new NpgsqlCommand(sql.ToString(), connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    foreach (var relationType in this.mapping.Database.MetaPopulation.DatabaseRelationTypes)
                    {
                        var associationType = relationType.AssociationType;
                        var roleType = relationType.RoleType;

                        if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                        {
                            var tableName = this.mapping.TableNameForRelationByRelationType[relationType];

                            var primaryKeyName = $"pk_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";

                            var sql =
$@"CREATE TABLE {tableName}(
    {Mapping.ColumnNameForAssociation} {Mapping.SqlTypeForObject},
    {Mapping.ColumnNameForRole} {Mapping.SqlTypeForObject},
    {(relationType.RoleType.IsOne
          ? $"CONSTRAINT {primaryKeyName} PRIMARY KEY ({Mapping.ColumnNameForAssociation})\n"
          : $"CONSTRAINT {primaryKeyName} PRIMARY KEY ({Mapping.ColumnNameForAssociation}, {Mapping.ColumnNameForRole})\n")}

)";

                            using (var command = new NpgsqlCommand(sql, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CreateProcedures()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    foreach (var dictionaryEntry in this.mapping.ProcedureDefinitionByName)
                    {
                        var definition = dictionaryEntry.Value;
                        using (var command = new NpgsqlCommand(definition, connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CreateIndeces()
        {
            using (var connection = new NpgsqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                try
                {
                    foreach (var @class in this.mapping.Database.MetaPopulation.DatabaseClasses)
                    {
                        var tableName = this.mapping.TableNameForObjectByClass[@class];
                        foreach (var associationType in @class.DatabaseAssociationTypes)
                        {
                            var relationType = associationType.RelationType;
                            if (relationType.IsIndexed)
                            {
                                var roleType = relationType.RoleType;

                                if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && roleType.IsMany)
                                {
                                    var indexName = "idx_" + @class.Name.ToLowerInvariant() + "_" + relationType.AssociationType.SingularFullName.ToLowerInvariant();
                                    this.CreateIndex(connection, indexName, relationType, tableName);
                                }
                            }
                        }

                        foreach (var roleType in @class.DatabaseRoleTypes)
                        {
                            var relationType = roleType.RelationType;
                            if (relationType.IsIndexed)
                            {
                                if (roleType.ObjectType.IsUnit)
                                {
                                    var unit = (IUnit)roleType.ObjectType;
                                    if (unit.IsString || unit.IsBinary)
                                    {
                                        if (roleType.Size == -1 || roleType.Size > 4000)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                var associationType = relationType.AssociationType;
                                if (roleType.ObjectType.IsUnit)
                                {
                                    var indexName = "idx_" + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
                                    this.CreateIndex(connection, indexName, relationType, tableName);
                                }
                                else if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && !roleType.IsMany)
                                {
                                    var indexName = "idx_" + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
                                    this.CreateIndex(connection, indexName, relationType, tableName);
                                }
                            }
                        }
                    }

                    foreach (var relationType in this.mapping.Database.MetaPopulation.DatabaseRelationTypes)
                    {
                        if (relationType.IsIndexed)
                        {
                            var associationType = relationType.AssociationType;
                            var roleType = relationType.RoleType;
                            if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                            {
                                var tableName = this.mapping.TableNameForRelationByRelationType[relationType];
                                var indexName = "idx_" + relationType.RoleType.SingularFullName.ToLowerInvariant() + "_" + Mapping.ColumnNameForRole.ToLowerInvariant();
                                var sql = new StringBuilder();
                                sql.Append("CREATE INDEX " + indexName + "\n");
                                sql.Append("ON " + tableName + " (" + Mapping.ColumnNameForRole + ")");
                                using (var command = new NpgsqlCommand(sql.ToString(), connection))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void TruncateTable(NpgsqlConnection connection, string tableName)
        {
            var cmdText = @"TRUNCATE TABLE " + tableName + @";";
            using (var command = new NpgsqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void DropTable(NpgsqlConnection connection, string tableName)
        {
            if (!this.validation.MissingTableNames.Contains(tableName))
            {
                using (var command = new NpgsqlCommand("DROP TABLE " + tableName, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void CreateIndex(NpgsqlConnection connection, string indexName, IRelationType relationType, string tableName)
        {
            var sql = new StringBuilder();
            sql.Append("CREATE INDEX " + indexName + "\n");
            sql.Append("ON " + tableName + " (" + this.mapping.ColumnNameByRelationType[relationType] + ")");
            using (var command = new NpgsqlCommand(sql.ToString(), connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}

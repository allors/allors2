// <copyright file="Schema.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections.Generic;
    using global::Npgsql;

    using NpgsqlTypes;

    public class Schema
    {
        public readonly bool Exists;

        public Schema(Database database)
        {
            var mapping = (Mapping)database.Mapping;

            this.Database = database;
            this.TableByName = new Dictionary<string, SchemaTable>();
            this.ProcedureByName = new Dictionary<string, SchemaProcedure>();
            this.IndexByIndexNameByTableName = new Dictionary<string, Dictionary<string, SchemaIndex>>();

            using (var connection = new NpgsqlConnection(database.ConnectionString))
            {
                connection.Open();
                try
                {
                    // Schema
                    var cmdText = @"
SELECT  count(schema_name)
FROM    information_schema.schemata
WHERE   schema_name = @schemaName";
                    using (var command = new NpgsqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add("@schemaName", NpgsqlDbType.Varchar).Value = database.SchemaName;
                        var schemaCount = (long)command.ExecuteScalar();
                        this.Exists = schemaCount != 0;
                    }

                    // Tables
                    cmdText = @"
SELECT T.table_name,
       C.column_name,
       C.data_type,
       C.character_maximum_length,
       C.numeric_precision,
       C.numeric_scale
FROM information_schema.tables AS T
FULL OUTER JOIN information_schema.columns AS C
ON T.table_name = C.table_name
WHERE T.table_type = 'BASE TABLE'
AND T.table_schema = @tableSchema
AND C.table_schema = @tableSchema";

                    using (var command = new NpgsqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add("@tableSchema", NpgsqlDbType.Varchar).Value = database.SchemaName;
                        using (var reader = command.ExecuteReader())
                        {
                            var tableNameOrdinal = reader.GetOrdinal("table_name");
                            var columnNameOrdinal = reader.GetOrdinal("column_name");
                            var dataTypeOrdinal = reader.GetOrdinal("data_type");
                            var characterMaximumLengthOrdinal = reader.GetOrdinal("character_maximum_length");
                            var numericPrecisionOrdinal = reader.GetOrdinal("numeric_precision");
                            var numericScaleOrdinal = reader.GetOrdinal("numeric_scale");

                            while (reader.Read())
                            {
                                var tableName = reader.GetString(tableNameOrdinal);
                                var columnName = reader.GetString(columnNameOrdinal);
                                var dataType = reader.GetString(dataTypeOrdinal).Trim().ToLowerInvariant();
                                var characterMaximumLength = reader.IsDBNull(characterMaximumLengthOrdinal) ? (int?)null : Convert.ToInt32(reader.GetValue(characterMaximumLengthOrdinal));
                                var numericPrecision = reader.IsDBNull(numericPrecisionOrdinal) ? (int?)null : Convert.ToInt32(reader.GetValue(numericPrecisionOrdinal));
                                var numericScale = reader.IsDBNull(numericScaleOrdinal) ? (int?)null : Convert.ToInt32(reader.GetValue(numericScaleOrdinal));

                                tableName = tableName.Trim().ToLowerInvariant();
                                tableName = mapping.NormalizeName(tableName);
                                var fullyQualifiedTableName = database.SchemaName + "." + tableName;

                                columnName = mapping.NormalizeName(columnName);

                                if (!this.TableByName.TryGetValue(fullyQualifiedTableName, out var table))
                                {
                                    table = new SchemaTable(this, fullyQualifiedTableName);
                                    this.TableByName[fullyQualifiedTableName] = table;
                                }

                                if (!reader.IsDBNull(columnNameOrdinal))
                                {
                                    var column = new SchemaTableColumn(table, columnName, dataType, characterMaximumLength, numericPrecision, numericScale);
                                    table.ColumnByLowercaseColumnName[column.LowercaseName] = column;
                                }
                            }
                        }
                    }

                    // Procedures
                    cmdText = @"
SELECT routine_name, routine_definition
FROM information_schema.routines
WHERE routine_schema = @routineSchema";

                    using (var command = new NpgsqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add("@routineSchema", NpgsqlDbType.Varchar).Value = database.SchemaName;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var routineNameValue = reader["routine_name"];
                                var routineDefinitionValue = reader["routine_definition"];

                                var routineName = (string)routineNameValue;
                                var routineDefinition = routineDefinitionValue != DBNull.Value ? (string)routineDefinitionValue : string.Empty;

                                var lowercaseRoutineName = routineName.Trim().ToLowerInvariant();
                                var fullyQualifiedName = database.SchemaName + "." + lowercaseRoutineName;
                                this.ProcedureByName[fullyQualifiedName] = new SchemaProcedure(routineName, routineDefinition);
                            }
                        }
                    }

                    // // Indeces
                    //                    cmdText = @"
                    // SELECT	o.name AS table_name,
                    // i.name AS index_name
                    // FROM
                    // sys.indexes i
                    // INNER JOIN sys.objects o ON i.object_id = o.object_id
                    // INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
                    // WHERE
                    // i.name IS NOT NULL
                    // AND o.type = 'U'
                    // AND i.type = 2
                    // AND s.name = @tableSchema";

                    // using (var command = new NpgsqlCommand(cmdText, connection))
                    //                    {
                    //                        command.Parameters.Add("@tableSchema", NpgsqlDbType.Varchar).Value = database.SchemaName;
                    //                        using (var reader = command.ExecuteReader())
                    //                        {
                    //                            var tableNameOrdinal = reader.GetOrdinal("table_name");
                    //                            var indexNameOrdinal = reader.GetOrdinal("index_name");

                    // while (reader.Read())
                    //                            {
                    //                                var tableName = reader.GetString(tableNameOrdinal);
                    //                                var indexName = reader.GetString(indexNameOrdinal);

                    // tableName = tableName.Trim().ToLowerInvariant();
                    //                                indexName = indexName.Trim().ToLowerInvariant();

                    // Dictionary<string, SchemaIndex> indexByLowercaseIndexName;
                    //                                if (!this.indexByIndexNameByTableName.TryGetValue(tableName, out indexByLowercaseIndexName))
                    //                                {
                    //                                    indexByLowercaseIndexName = new Dictionary<string, SchemaIndex>();
                    //                                    this.indexByIndexNameByTableName[tableName] = indexByLowercaseIndexName;
                    //                                }

                    // SchemaIndex index;
                    //                                if (!indexByLowercaseIndexName.TryGetValue(indexName, out index))
                    //                                {
                    //                                    index = new SchemaIndex(this, indexName);
                    //                                    indexByLowercaseIndexName[indexName] = index;
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Database Database { get; }

        public Dictionary<string, SchemaTable> TableByName { get; }

        public Dictionary<string, SchemaProcedure> ProcedureByName { get; }

        public Dictionary<string, Dictionary<string, SchemaIndex>> IndexByIndexNameByTableName { get; }

        public SchemaTable GetTable(string tableName)
        {
            this.TableByName.TryGetValue(tableName.ToLowerInvariant(), out var table);
            return table;
        }

        public SchemaProcedure GetProcedure(string procedureName)
        {
            this.ProcedureByName.TryGetValue(procedureName, out var procedure);
            return procedure;
        }

        public SchemaIndex GetIndex(string tableName, string indexName)
        {
            if (this.IndexByIndexNameByTableName.TryGetValue(tableName.ToLowerInvariant(), out var indexByLowercaseIndexName))
            {
                indexByLowercaseIndexName.TryGetValue(indexName.ToLowerInvariant(), out var index);
                return index;
            }

            return null;
        }
    }
}

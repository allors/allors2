// <copyright file="Validation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.SqlClient
{
    using System.Collections.Generic;
    using System.Text;

    public class Validation
    {
        public readonly HashSet<string> MissingTableNames;
        public readonly HashSet<SchemaTable> InvalidTables;

        public readonly HashSet<string> MissingTableTypeNames;
        public readonly HashSet<SchemaTableType> InvalidTableTypes;

        public readonly HashSet<string> MissingProcedureNames;
        public readonly HashSet<SchemaProcedure> InvalidProcedures;

        private readonly Mapping mapping;

        public Validation(Database database)
        {
            this.Database = database;
            this.mapping = database.Mapping;
            this.Schema = new Schema(database);

            this.MissingTableNames = new HashSet<string>();
            this.InvalidTables = new HashSet<SchemaTable>();

            this.MissingTableTypeNames = new HashSet<string>();
            this.InvalidTableTypes = new HashSet<SchemaTableType>();

            this.MissingProcedureNames = new HashSet<string>();
            this.InvalidProcedures = new HashSet<SchemaProcedure>();

            this.Validate();

            this.IsValid =
                this.MissingTableNames.Count == 0 &
                this.InvalidTables.Count == 0 &
                this.MissingTableTypeNames.Count == 0 &
                this.InvalidTableTypes.Count == 0 &
                this.MissingProcedureNames.Count == 0 &
                this.InvalidProcedures.Count == 0;
        }

        public bool IsValid { get; }

        public Database Database { get; }

        public Schema Schema { get; }

        public string Message
        {
            get
            {
                var message = new StringBuilder();

                if (this.MissingTableNames.Count > 0)
                {
                    message.Append("Missing Tables:\n");
                    foreach (var missingTable in this.MissingTableNames)
                    {
                        message.Append("- " + missingTable + ":\n");
                    }
                }

                if (this.InvalidTables.Count > 0)
                {
                    message.Append("Invalid Tables:\n");
                    foreach (var invalidTable in this.InvalidTables)
                    {
                        message.Append("- " + invalidTable.Name + ":\n");
                    }
                }

                if (this.MissingTableTypeNames.Count > 0)
                {
                    message.Append("Missing Table Types:\n");
                    foreach (var missingTableType in this.MissingTableTypeNames)
                    {
                        message.Append("- " + missingTableType + ":\n");
                    }
                }

                if (this.InvalidTableTypes.Count > 0)
                {
                    message.Append("Invalid Table Types:\n");
                    foreach (var invalidTableType in this.InvalidTableTypes)
                    {
                        message.Append("- " + invalidTableType.Name + ":\n");
                    }
                }

                if (this.MissingProcedureNames.Count > 0)
                {
                    message.Append("Missing Procedures:\n");
                    foreach (var missingProcedure in this.MissingProcedureNames)
                    {
                        message.Append("- " + missingProcedure + ":\n");
                    }
                }

                if (this.InvalidProcedures.Count > 0)
                {
                    message.Append("Invalid Procedures:\n");
                    foreach (var invalidProcedure in this.InvalidProcedures)
                    {
                        message.Append("- " + invalidProcedure.Name + ":\n");
                    }
                }

                return message.ToString();
            }
        }

        private void Validate()
        {
            // Objects Table
            var objectsTable = this.Schema.GetTable(this.Database.Mapping.TableNameForObjects);
            if (objectsTable == null)
            {
                this.MissingTableNames.Add(this.mapping.TableNameForObjects);
            }
            else
            {
                if (objectsTable.ColumnByLowercaseColumnName.Count != 3)
                {
                    this.InvalidTables.Add(objectsTable);
                }

                this.ValidateColumn(objectsTable, Mapping.ColumnNameForObject, Mapping.SqlTypeForObject);
                this.ValidateColumn(objectsTable, Mapping.ColumnNameForClass, Mapping.SqlTypeForClass);
                this.ValidateColumn(objectsTable, Mapping.ColumnNameForVersion, Mapping.SqlTypeForVersion);
            }

            // Object Tables
            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                var tableName = this.mapping.TableNameForObjectByClass[@class];
                var table = this.Schema.GetTable(tableName);

                if (table == null)
                {
                    this.MissingTableNames.Add(tableName);
                }
                else
                {
                    this.ValidateColumn(table, Mapping.ColumnNameForObject, Mapping.SqlTypeForObject);
                    this.ValidateColumn(table, Mapping.ColumnNameForClass, Mapping.SqlTypeForClass);

                    foreach (var associationType in @class.AssociationTypes)
                    {
                        var relationType = associationType.RelationType;
                        var roleType = relationType.RoleType;
                        if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses
                            && roleType.IsMany)
                        {
                            this.ValidateColumn(
                                table,
                                this.Database.Mapping.ColumnNameByRelationType[relationType],
                                Mapping.SqlTypeForObject);
                        }
                    }

                    foreach (var roleType in @class.RoleTypes)
                    {
                        var relationType = roleType.RelationType;
                        var associationType = relationType.AssociationType;
                        if (roleType.ObjectType.IsUnit)
                        {
                            this.ValidateColumn(
                                table,
                                this.Database.Mapping.ColumnNameByRelationType[relationType],
                                this.Database.Mapping.GetSqlType(relationType.RoleType));
                        }
                        else
                        {
                            if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses
                                && !roleType.IsMany)
                            {
                                this.ValidateColumn(
                                    table,
                                    this.Database.Mapping.ColumnNameByRelationType[relationType],
                                    Mapping.SqlTypeForObject);
                            }
                        }
                    }
                }
            }

            // Relation Tables
            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (!roleType.ObjectType.IsUnit &&
                    ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    var tableName = this.mapping.TableNameForRelationByRelationType[relationType];
                    var table = this.Schema.GetTable(tableName);

                    if (table == null)
                    {
                        this.MissingTableNames.Add(tableName);
                    }
                    else
                    {
                        if (table.ColumnByLowercaseColumnName.Count != 2)
                        {
                            this.InvalidTables.Add(table);
                        }

                        this.ValidateColumn(table, Mapping.ColumnNameForAssociation, Mapping.SqlTypeForObject);

                        var roleSqlType = relationType.RoleType.ObjectType.IsComposite ? Mapping.SqlTypeForObject : this.mapping.GetSqlType(relationType.RoleType);
                        this.ValidateColumn(table, Mapping.ColumnNameForRole, roleSqlType);
                    }
                }
            }

            // TableTypes
            {
                // Object TableType
                var tableType = this.Schema.GetTableType(this.mapping.TableTypeNameForObject);
                if (tableType == null)
                {
                    this.MissingTableTypeNames.Add(this.mapping.TableTypeNameForObject);
                }
                else
                {
                    if (tableType.ColumnByLowercaseColumnName.Count != 1)
                    {
                        this.InvalidTableTypes.Add(tableType);
                    }

                    this.ValidateColumn(tableType, this.mapping.TableTypeColumnNameForObject, Mapping.SqlTypeForObject);
                }
            }

            this.ValidateTableType(this.mapping.TableTypeNameForCompositeRelation, Mapping.SqlTypeForObject);
            this.ValidateTableType(this.mapping.TableTypeNameForStringRelation, "nvarchar(max)");
            this.ValidateTableType(this.mapping.TableTypeNameForIntegerRelation, "int");
            this.ValidateTableType(this.mapping.TableTypeNameForFloatRelation, "float");
            this.ValidateTableType(this.mapping.TableTypeNameForDateTimeRelation, "datetime2");
            this.ValidateTableType(this.mapping.TableTypeNameForBooleanRelation, "bit");
            this.ValidateTableType(this.mapping.TableTypeNameForUniqueRelation, "uniqueidentifier");
            this.ValidateTableType(this.mapping.TableTypeNameForBinaryRelation, "varbinary(max)");
            this.ValidateTableType(this.mapping.TableTypeNameForBinaryRelation, "varbinary(max)");

            // Decimal TableType
            foreach (var precisionEntry in this.mapping.TableTypeNameForDecimalRelationByScaleByPrecision)
            {
                foreach (var scaleEntry in precisionEntry.Value)
                {
                    var name = scaleEntry.Value;
                    var precision = precisionEntry.Key;
                    var scale = scaleEntry.Key;

                    this.ValidateTableType(name, "decimal(" + precision + "," + scale + ")");
                }
            }

            // Procedures Tables
            foreach (var dictionaryEntry in this.mapping.ProcedureDefinitionByName)
            {
                var procedureName = dictionaryEntry.Key;
                var procedureDefinition = dictionaryEntry.Value;

                var procedure = this.Schema.GetProcedure(procedureName);

                if (procedure == null)
                {
                    this.MissingProcedureNames.Add(procedureName);
                }
                else
                {
                    if (!procedure.IsDefinitionCompatible(procedureDefinition))
                    {
                        this.InvalidProcedures.Add(procedure);
                    }
                }
            }

            // TODO: Primary Keys and Indeces
        }

        private void ValidateTableType(string name, string columnType)
        {
            var tableType = this.Schema.GetTableType(name);
            if (tableType == null)
            {
                this.MissingTableTypeNames.Add(name);
            }
            else
            {
                if (tableType.ColumnByLowercaseColumnName.Count != 2)
                {
                    this.InvalidTableTypes.Add(tableType);
                }

                this.ValidateColumn(tableType, this.mapping.TableTypeColumnNameForAssociation, Mapping.SqlTypeForObject);
                this.ValidateColumn(tableType, this.mapping.TableTypeColumnNameForRole, columnType);
            }
        }

        private void ValidateColumn(SchemaTable table, string columnName, string sqlType)
        {
            var objectColumn = table.GetColumn(columnName);

            if (objectColumn == null || !objectColumn.SqlType.Equals(sqlType))
            {
                this.InvalidTables.Add(table);
            }
        }

        private void ValidateColumn(SchemaTableType tableType, string columnName, string sqlType)
        {
            var objectColumn = tableType.GetColumn(columnName);

            if (objectColumn == null || !objectColumn.SqlType.Equals(sqlType))
            {
                this.InvalidTableTypes.Add(tableType);
            }
        }
    }
}

namespace Allors.Adapters.Relation.SqlClient
{
    using System.Collections.Generic;

    public class Validation
    {
        public readonly HashSet<string> MissingTableNames;
        public readonly Dictionary<Table, HashSet<string>> MissingColumnNamesByTable;

        public readonly HashSet<Table> InvalidTables;
        public readonly HashSet<TableColumn> InvalidColumns; 

        private readonly Database database;
        private readonly Schema schema;

        private readonly bool success;

        public Validation(Database database)
        {
            this.database = database;
            this.schema = new Schema(database);
            
            this.MissingTableNames = new HashSet<string>();
            this.MissingColumnNamesByTable = new Dictionary<Table, HashSet<string>>();
            
            this.InvalidTables = new HashSet<Table>();
            this.InvalidColumns = new HashSet<TableColumn>();

            this.Validate();

            this.success = this.MissingTableNames.Count == 0 &
                           this.MissingColumnNamesByTable.Count == 0 &
                           this.InvalidTables.Count == 0 & 
                           this.InvalidColumns.Count == 0;
        }

        public bool Success
        {
            get
            {
                return this.success;
            }
        }

        public Database Database
        {
            get
            {
                return this.database;
            }
        }

        public Schema Schema
        {
            get
            {
                return this.schema;
            }
        }

        private void Validate()
        {
            var mapping = this.Database.Mapping;

            // Objects Table
            var objectsTable = this.Schema.GetTable(Mapping.TableNameForObjects);
            if (objectsTable == null)
            {
                this.MissingTableNames.Add(Mapping.TableNameForObjects);
            }
            else
            {
                if (objectsTable.ColumnByLowercaseColumnName.Count != 3)
                {
                    this.InvalidTables.Add(objectsTable);
                }

                this.ValidateColumn(objectsTable, Mapping.ColumnNameForObject, this.Database.Mapping.SqlTypeForObject);
                this.ValidateColumn(objectsTable, Mapping.ColumnNameForType, Mapping.SqlTypeForType);
                this.ValidateColumn(objectsTable, Mapping.ColumnNameForCache, Mapping.SqlTypeForCache);
            }
            
            // Relations
            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var tableName = mapping.GetTableName(relationType);
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

                    this.ValidateColumn(table, Mapping.ColumnNameForAssociation, this.Database.Mapping.SqlTypeForObject);
                    this.ValidateColumn(table, Mapping.ColumnNameForRole, this.Database.Mapping.GetSqlType(relationType.RoleType));
                }
            }
        }

        private void ValidateColumn(Table table, string columnName, string sqlType)
        {
            var objectColumn = table.GetColumn(columnName);

            if (objectColumn == null)
            {
                this.AddMissingColumnName(table, columnName);
            }
            else
            {
                if (!objectColumn.SqlType.Equals(sqlType))
                {
                    this.InvalidColumns.Add(objectColumn);
                }
            }
        }

        private void AddMissingColumnName(Table table, string columnName)
        {
            HashSet<string> missingColumnNames;
            if (!this.MissingColumnNamesByTable.TryGetValue(table, out missingColumnNames))
            {
                missingColumnNames = new HashSet<string>();
                this.MissingColumnNamesByTable[table] = missingColumnNames;
            }

            missingColumnNames.Add(columnName);
        }
    }
}
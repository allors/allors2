namespace Allors.Adapters.Relation.SQLite
{
    using System.Collections.Generic;

    public class Validation
    {
        public readonly HashSet<string> MissingTableNames;

        private readonly Database database;
        private readonly Schema schema;

        private readonly bool success;

        public Validation(Database database)
        {
            this.database = database;
            this.schema = new Schema(database);
            
            this.MissingTableNames = new HashSet<string>();

            this.Validate();

            this.success = this.MissingTableNames.Count == 0;
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
            if (!this.Schema.LowercaseTableNames.Contains(Mapping.TableNameForObjects))
            {
                this.MissingTableNames.Add(Mapping.TableNameForObjects);
            }
            
            // Relations
            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var tableName = mapping.GetTableName(relationType);
                if (!this.Schema.LowercaseTableNames.Contains(tableName))
                {
                    this.MissingTableNames.Add(Mapping.TableNameForObjects);
                }
            }
        }
    }
}
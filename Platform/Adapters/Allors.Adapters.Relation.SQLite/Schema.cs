namespace Allors.Adapters.Relation.SQLite
{
    using System.Collections.Generic;
    using System.Data.SQLite;

    public class Schema
    {
        private readonly HashSet<string> lowercaseTableNames;

        public Schema(Database database)
        {
            this.lowercaseTableNames = new HashSet<string>();

            using (var connection = new SQLiteConnection(database.ConnectionString))
            {
                connection.Open();
                try
                {
                    // Objects
                    var cmdText = @"SELECT name FROM sqlite_master WHERE type='table';";

                    using (var command = new SQLiteCommand(cmdText, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tableName = (string)reader["name"];
                                var lowercaseTableName = tableName.Trim().ToLowerInvariant();
                                this.lowercaseTableNames.Add(lowercaseTableName);
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

        public HashSet<string> LowercaseTableNames
        {
            get
            {
                return this.lowercaseTableNames;
            }
        }
    }
}
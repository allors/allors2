// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initialization.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Adapters.Relation.SQLite
{
    using System.Data.SQLite;
    using System.Text;

    using Allors.Meta;

    internal class Initialization 
    {
        private readonly Mapping mapping;
        private readonly Schema schema;
        private readonly bool autoIncrement;

        internal Initialization(Mapping mapping, Schema schema, bool autoIncrement)
        {
            this.mapping = mapping;
            this.schema = schema;
            this.autoIncrement = autoIncrement;
        }

        internal void Execute()
        {
            var cmdText = new StringBuilder();

            // Objects
            cmdText.Append(@"
DROP TABLE IF EXISTS " + Mapping.TableNameForObjects + @";
CREATE TABLE " + Mapping.TableNameForObjects + @"
(
" + Mapping.ColumnNameForObject + @" " + this.mapping.SqlTypeForId + @" PRIMARY KEY " + (this.autoIncrement ? "AUTOINCREMENT" : string.Empty) + @",
" + Mapping.ColumnNameForType + @" " + Mapping.SqlTypeForType + @",
" + Mapping.ColumnNameForCache + @" " + Mapping.SqlTypeForCache + @"
);
");

            // Relations
            foreach (var relationType in this.mapping.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.RoleType;
                var roleType = relationType.RoleType;

                var tableName = this.mapping.GetTableName(relationType);
                var sqlTypeForRole = this.mapping.GetSqlType(roleType);

                if (this.schema.LowercaseTableNames.Contains(tableName))
                {
                    cmdText.Append(@"
DELETE FROM " + tableName + @";
");
                }
                else
                {
                    var primaryKeys = Mapping.ColumnNameForAssociation;
                    if (roleType.ObjectType is IComposite)
                    {
                        if (associationType.IsMany && roleType.IsMany)
                        {
                            primaryKeys = Mapping.ColumnNameForAssociation + @" , " + Mapping.ColumnNameForRole;
                        }
                        else if (roleType.IsMany)
                        {
                            primaryKeys = Mapping.ColumnNameForRole;
                        }
                    }

                    cmdText.Append(@"
CREATE TABLE " + tableName + @"
(
" + Mapping.ColumnNameForAssociation + @" " + this.mapping.SqlTypeForId + @",
" + Mapping.ColumnNameForRole + @" " + sqlTypeForRole + @",
PRIMARY KEY ( " + primaryKeys + @")
);
");
                }
            }

            using (var connection = new SQLiteConnection(this.mapping.Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    using (var command = new SQLiteCommand(cmdText.ToString(), connection))
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
}
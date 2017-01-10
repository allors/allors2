// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mapping.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;

    using Allors.Meta;

    public class Mapping
    {
        public const string ParamPrefix = "@";

        public const string TableNameForObjects = "_o";

        public const string ColumnNameForObject = "o";
        public const string ColumnNameForType = "t";
        public const string ColumnNameForCache = "c";
        public const string ColumnNameForAssociation = "a";
        public const string ColumnNameForRole = "r";

        public const string ParameterNameForObject = ParamPrefix + "o";
        public const string ParameterNameForType = ParamPrefix + "t";
        public const string ParameterNameForCache = ParamPrefix + "c";
        public const string ParameterNameForAssociation = ParamPrefix + "a";
        public const string ParameterNameForRole = ParamPrefix + "r";

        public const string SqlTypeForType = "blob";
        public const string SqlTypeForCache = "integer";

        public const DbType DbTypeForId = DbType.Int64;
        public const DbType DbTypeForType = DbType.Guid;
        public const DbType DbTypeForCache = DbType.Int64;

        private readonly Database database;

        private readonly string sqlTypeForId;

        private readonly Dictionary<IRelationType, string> tableNameByRelationType;
        private readonly Dictionary<IRoleType, string> sqlTypeByRoleType;
        private readonly Dictionary<IRoleType, DbType> dbTypeByRoleType;

        public Mapping(Database database)
        {
            this.database = database;

            if (!this.database.MetaPopulation.IsValid)
            {
                throw new Exception("MetaPopulation is invalid.");
            }

            this.sqlTypeForId = "integer";

            this.tableNameByRelationType = new Dictionary<IRelationType, string>();
            this.sqlTypeByRoleType = new Dictionary<IRoleType, string>();
            this.dbTypeByRoleType = new Dictionary<IRoleType, DbType>();

            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var tableName = "_" + relationType.Id.ToString("N");
                DbType dbType;
                string sqlType;

                var roleType = relationType.RoleType;
                var unit = roleType.ObjectType as IUnit;
                if (unit != null)
                {
                    switch (unit.UnitTag)
                    {
                        case UnitTags.AllorsBinary:
                            tableName = tableName + "_binary";
                            dbType = DbType.Binary;
                            sqlType = "blob";
                            
                            break;

                        case UnitTags.AllorsBoolean:
                            tableName = tableName + "_boolean";
                            dbType = DbType.Boolean;
                            sqlType = "integer";
                            break;

                        case UnitTags.AllorsDateTime:
                            tableName = tableName + "_datetime";
                            dbType = DbType.String;
                            sqlType = "text";
                            break;

                        case UnitTags.AllorsDecimal:
                            tableName = tableName + "_decimal";
                            dbType = DbType.Decimal;
                            sqlType = "text";
                            break;

                        case UnitTags.AllorsFloat:
                            tableName = tableName + "_float";
                            dbType = DbType.Double;
                            sqlType = "real";
                            break;

                        case UnitTags.AllorsInteger:
                            tableName = tableName + "_integer";
                            dbType = DbType.Int32;
                            sqlType = "integer";
                            break;

                        case UnitTags.AllorsString:
                            tableName = tableName + "_string";
                            dbType = DbType.String;
                            sqlType = "text";

                            break;

                        case UnitTags.AllorsUnique:
                            tableName = tableName + "_unique";
                            dbType = DbType.Guid;
                            sqlType = "text";
                            break;

                        default:
                            throw new NotSupportedException("Unit " + unit + "is not supported.");
                    }
                }
                else
                {
                    if (relationType.AssociationType.IsOne)
                    {
                        if (roleType.IsOne)
                        {
                            tableName = tableName + "_11";
                        }
                        else
                        {
                            tableName = tableName + "_1m";
                        }
                    }
                    else
                    {
                        if (roleType.IsOne)
                        {
                            tableName = tableName + "_m1";
                        }
                        else
                        {
                            tableName = tableName + "_mm";
                        }
                    }


                    dbType = DbTypeForId;
                    sqlType = this.SqlTypeForId;
                }

                this.tableNameByRelationType[relationType] = tableName.ToLowerInvariant();
                this.dbTypeByRoleType[roleType] = dbType;
                this.sqlTypeByRoleType[roleType] = sqlType;
            }
        }

        public string SqlTypeForId
        {
            get
            {
                return this.sqlTypeForId;
            }
        }

        public Database Database
        {
            get
            {
                return this.database;
            }
        }

        public string GetTableName(IAssociationType associationType)
        {
            return this.GetTableName(associationType.RelationType);
        }

        public string GetTableName(IRoleType roleType)
        {
            return this.GetTableName(roleType.RelationType);
        }

        public string GetTableName(IRelationType relationType)
        {
            string tableName;
            this.tableNameByRelationType.TryGetValue(relationType, out tableName);
            return tableName;
        }
        
        public DbType GetSqlDbType(IRoleType roleType)
        {
            DbType dbType;
            this.dbTypeByRoleType.TryGetValue(roleType, out dbType);
            return dbType;
        }

        public string GetSqlType(IRoleType roleType)
        {
            string sqlType;
            this.sqlTypeByRoleType.TryGetValue(roleType, out sqlType);
            return sqlType;
        }
    }
}
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

namespace Allors.Adapters.Relation.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Allors.Meta;

    using Microsoft.SqlServer.Server;

    public class Mapping
    {
        public const string ParamPrefix = "@";

        public const string TableNameForObjects = "_o";
        public const string ColumnNameForObject = "o";
        public const string ColumnNameForType = "t";
        public const string ColumnNameForCache = "c";
        public const string ColumnNameForAssociation = "a";
        public const string ColumnNameForRole = "r";

        public const string TableTypeNameForObjects = "_t_o";
        public const string TableTypeColumnNameForObject = "_o";
        public const string TableTypeNameForCompositeRelations = "_t_c";
        public const string TableTypeColumnNameForAssociation = "_a";
        public const string TableTypeColumnNameForRole = "_r";

        public const string ProcedureNameForCreateObject = "_co";
        public const string ProcedureNameForCreateObjects = "_cos";
        public const string ProcedureNameForInsertObject = "_io";
        public const string ProcedureNameForFetchObjects = "_fo";
        public const string ProcedureNameForDeleteObjects = "_fd";
        public const string ProcedureNameForUpdateCacheIds = "_uc";

        public const string ParameterNameForCount = ParamPrefix + "cnt";
        public const string ParameterNameForObject = ParamPrefix + "o";
        public const string ParameterNameForObjectTable = ParamPrefix + "ot";
        public const string ParameterNameForType = ParamPrefix + "t";
        public const string ParameterNameForCache = ParamPrefix + "c";
        public const string ParameterNameForAssociation = ParamPrefix + "a";
        public const string ParameterNameForRole = ParamPrefix + "r";
        public const string ParameterNameForRelationTable = ParamPrefix + "rt";

        public const string SqlTypeForType = "uniqueidentifier";
        public const string SqlTypeForCache = "bigint";
        public const string SqlTypeForCount = "int";

        public const SqlDbType SqlDbTypeForType = SqlDbType.UniqueIdentifier;
        public const SqlDbType SqlDbTypeForCache = SqlDbType.Int;

        private readonly Database database;

        private readonly string sqlTypeForObject;

        private readonly Dictionary<IRelationType, string> tableNameByRelationType;
        private readonly Dictionary<IRoleType, string> sqlTypeByRoleType;
        private readonly Dictionary<IRoleType, SqlDbType> sqlDbTypeByRoleType;

        private readonly Dictionary<IRelationType, string> tableTypeNameByRelationType;
        private readonly Dictionary<IRelationType, string> tableTypeSqlTypeByRelationType;

        private readonly Dictionary<IRoleType, SqlMetaData[]> sqlMetaDataByRoleType;

        private readonly Dictionary<IRelationType, string> procedureNameForGetRoleByRelationType;
        private readonly Dictionary<IRelationType, string> procedureNameForGetAssociationByRelationType;
        private readonly Dictionary<IRelationType, string> procedureNameForSetRoleByRelationType;
        private readonly Dictionary<IRelationType, string> procedureNameForAddRoleByRelationType;
        private readonly Dictionary<IRelationType, string> procedureNameForRemoveRoleByRelationType;
        private readonly Dictionary<IRelationType, string> procedureNameForDeleteRoleByRelationType;

        public Mapping(Database database)
        {
            this.database = database;
            this.sqlTypeForObject = "bigint";
            this.SqlDbTypeForObject = SqlDbType.BigInt;

            if (!this.database.MetaPopulation.IsValid)
            {
                throw new Exception("MetaPopulation is invalid.");
            }

            this.tableNameByRelationType = new Dictionary<IRelationType, string>();
            this.sqlTypeByRoleType = new Dictionary<IRoleType, string>();
            this.sqlDbTypeByRoleType = new Dictionary<IRoleType, SqlDbType>();

            this.tableTypeNameByRelationType = new Dictionary<IRelationType, string>();
            this.tableTypeSqlTypeByRelationType = new Dictionary<IRelationType, string>();

            this.sqlMetaDataByRoleType = new Dictionary<IRoleType, SqlMetaData[]>();
            this.SqlMetaDataForObject = new SqlMetaData(TableTypeColumnNameForObject, this.SqlDbTypeForObject);
            
            this.procedureNameForGetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForGetAssociationByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForSetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForAddRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForRemoveRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForDeleteRoleByRelationType = new Dictionary<IRelationType, string>();

            var sqlMetaDataBySqlType = new Dictionary<string, SqlMetaData[]>();
            var sqlMetaDataForCompositeRelation = new[]
            {
                new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                new SqlMetaData(TableTypeColumnNameForRole, this.SqlDbTypeForObject)
            };

            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var tableName = "_" + relationType.Id.ToString("N");
                SqlDbType sqlDbType;
                string sqlType;

                var sqlMetaData = sqlMetaDataForCompositeRelation;

                this.procedureNameForGetRoleByRelationType.Add(relationType, "_gr_" + relationType.Id.ToString("N").ToLowerInvariant());
                this.procedureNameForGetAssociationByRelationType.Add(relationType, "_ga_" + relationType.Id.ToString("N").ToLowerInvariant());
                this.procedureNameForSetRoleByRelationType.Add(relationType, "_sr_" + relationType.Id.ToString("N").ToLowerInvariant());
                this.procedureNameForAddRoleByRelationType.Add(relationType, "_ar_" + relationType.Id.ToString("N").ToLowerInvariant());
                this.procedureNameForRemoveRoleByRelationType.Add(relationType, "_rr_" + relationType.Id.ToString("N").ToLowerInvariant());
                this.procedureNameForDeleteRoleByRelationType.Add(relationType, "_dr_" + relationType.Id.ToString("N").ToLowerInvariant());

                var tableTypeName = TableTypeNameForCompositeRelations;
                var tableTypeSqlType = this.sqlTypeForObject;
                
                var roleType = relationType.RoleType;
                var unit = roleType.ObjectType as IUnit;
                if (unit != null)
                {
                    switch (unit.UnitTag)
                    {
                        case UnitTags.Binary:
                            sqlDbType = SqlDbType.VarBinary;
                            if (roleType.Size != -1 && roleType.Size <= 8000)
                            {
                                tableName = tableName + "_binary_" + roleType.Size;
                                sqlType = "varbinary(" + roleType.Size + ")";
                            }
                            else
                            {
                                tableName = tableName + "_binary";
                                sqlType = "varbinary(MAX)";
                            }
                            
                            tableTypeName = "_t_bi";
                            tableTypeSqlType = "varbinary(MAX)";

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType, -1)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];

                            break;

                        case UnitTags.Boolean:
                            tableName = tableName + "_boolean";
                            sqlDbType = SqlDbType.Bit;
                            sqlType = "bit";

                            tableTypeName = "_t_bo";
                            tableTypeSqlType = sqlType;

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.DateTime:
                            tableName = tableName + "_datetime";
                            sqlDbType = SqlDbType.DateTime2;
                            sqlType = "datetime2";

                            tableTypeName = "_t_da";
                            tableTypeSqlType = sqlType;

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType)
                                    };
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.Decimal:
                            tableName = tableName + "_decimal_" + roleType.Precision + "_" + roleType.Scale;
                            sqlDbType = SqlDbType.Decimal;
                            sqlType = "decimal(" + roleType.Precision + "," + roleType.Scale + ")";

                            tableTypeName = "_t_de_" + roleType.Precision + "_" + roleType.Scale;
                            tableTypeSqlType = sqlType;

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType, (byte)roleType.Precision.Value, (byte)roleType.Scale.Value)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.Float:
                            tableName = tableName + "_float";
                            sqlDbType = SqlDbType.Float;
                            sqlType = "float";

                            tableTypeName = "_t_f";
                            tableTypeSqlType = sqlType;

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.Integer:
                            tableName = tableName + "_integer";
                            sqlDbType = SqlDbType.Int;
                            sqlType = "int";

                            tableTypeName = "_t_i";
                            tableTypeSqlType = sqlType;
                            
                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.String:
                            sqlDbType = SqlDbType.NVarChar;
                            if (roleType.Size != -1 && roleType.Size <= 4000)
                            {
                                tableName = tableName + "_string_" + roleType.Size;
                                sqlType = "nvarchar(" + roleType.Size + ")";
                            }
                            else
                            {
                                tableName = tableName + "_string";
                                sqlType = "nvarchar(MAX)";
                            }

                            tableTypeName = "_t_s";
                            tableTypeSqlType = "nvarchar(MAX)";

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType, -1)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
                            break;

                        case UnitTags.Unique:
                            tableName = tableName + "_unique";
                            sqlDbType = SqlDbType.UniqueIdentifier;
                            sqlType = "uniqueidentifier";

                            tableTypeName = "_t_u";
                            tableTypeSqlType = sqlType;

                            if (!sqlMetaDataBySqlType.ContainsKey(tableTypeSqlType))
                            {
                                sqlMetaDataBySqlType[tableTypeSqlType] = new[]
                                    {
                                        new SqlMetaData(TableTypeColumnNameForAssociation, this.SqlDbTypeForObject), 
                                        new SqlMetaData(TableTypeColumnNameForRole, sqlDbType)
                                    };    
                            }

                            sqlMetaData = sqlMetaDataBySqlType[tableTypeSqlType];
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
                            tableName = tableName + "_n1";
                        }
                        else
                        {
                            tableName = tableName + "_nm";
                        }
                    }

                    sqlDbType = this.SqlDbTypeForObject;
                    sqlType = this.SqlTypeForObject;
                }

                this.tableNameByRelationType[relationType] = tableName.ToLowerInvariant();
                this.sqlDbTypeByRoleType[roleType] = sqlDbType;
                this.sqlTypeByRoleType[roleType] = sqlType;

                this.tableTypeNameByRelationType[relationType] = tableTypeName.ToLowerInvariant();
                this.tableTypeSqlTypeByRelationType[relationType] = tableTypeSqlType.ToLowerInvariant();

                this.sqlMetaDataByRoleType[roleType] = sqlMetaData;
            }
        }

        public string SqlTypeForObject => this.sqlTypeForObject;

        public SqlDbType SqlDbTypeForObject { get; }

        public Database Database => this.database;

        public SqlMetaData SqlMetaDataForObject { get; }

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
        
        public SqlDbType GetSqlDbType(IRoleType roleType)
        {
            SqlDbType sqlDbType;
            this.sqlDbTypeByRoleType.TryGetValue(roleType, out sqlDbType);
            return sqlDbType;
        }

        public string GetSqlType(IRoleType roleType)
        {
            string sqlType;
            this.sqlTypeByRoleType.TryGetValue(roleType, out sqlType);
            return sqlType;
        }

        public string GetTableTypeName(IRelationType relationType)
        {
            string tableTypeName;
            this.tableTypeNameByRelationType.TryGetValue(relationType, out tableTypeName);
            return tableTypeName;
        }

        public string GetTableTypeSqlType(IRelationType relationType)
        {
            string tableTypeSqlType;
            this.tableTypeSqlTypeByRelationType.TryGetValue(relationType, out tableTypeSqlType);
            return tableTypeSqlType;
        }

        public SqlMetaData[] GetSqlMetaData(IRoleType roleType)
        {
            SqlMetaData[] sqlMetaData;
            this.sqlMetaDataByRoleType.TryGetValue(roleType, out sqlMetaData);
            return sqlMetaData;
        }

        public string GetProcedureNameForGetRole(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForGetRoleByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }

        public string GetProcedureNameForGetAssociation(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForGetAssociationByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }

        public string GetProcedureNameForSetRole(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForSetRoleByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }

        public string GetProcedureNameForAddRole(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForAddRoleByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }

        public string GetProcedureNameForRemoveRole(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForRemoveRoleByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }

        public string GetProcedureNameForDeleteRole(IRelationType relationType)
        {
            string procedureName;
            this.procedureNameForDeleteRoleByRelationType.TryGetValue(relationType, out procedureName);
            return procedureName;
        }
    }
}
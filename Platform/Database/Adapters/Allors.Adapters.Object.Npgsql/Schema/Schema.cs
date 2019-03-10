// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Schema.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.RegularExpressions;

    using Allors.Adapters.Database.Npgsql;
    using Allors.Meta;

    using NpgsqlTypes;

    public class Schema : IEnumerable<SchemaTable>
    {
        /// <summary>
        /// This prefix will be used for
        /// <ul>
        /// <li>System tables (e.g. OC)</li>
        /// <li>Indexes</li>
        /// <li>Parameters</li>
        /// </ul>
        /// in order to avoid naming conflictts with existing tables
        /// </summary>
        public const string AllorsPrefix = "_";

        public readonly SchemaArrayParameter ObjectArrayParam;
        public readonly SchemaArrayParameter CompositeRelationArrayParam;
        public readonly SchemaArrayParameter StringRelationArrayParam;
        public readonly SchemaArrayParameter StringMaxRelationArrayParam;
        public readonly SchemaArrayParameter IntegerRelationArrayParam;
        public readonly SchemaArrayParameter LongRelationArrayParam;
        public readonly SchemaArrayParameter DecimalRelationArrayParam;
        public readonly SchemaArrayParameter DoubleRelationArrayParam;
        public readonly SchemaArrayParameter BooleanRelationArrayParam;
        public readonly SchemaArrayParameter DateRelationArrayParam;
        public readonly SchemaArrayParameter DateTimeRelationArrayParam;
        public readonly SchemaArrayParameter UniqueRelationArrayParam;
        public readonly SchemaArrayParameter BinaryRelationArrayParam;

        public readonly Dictionary<int, Dictionary<int, string>> DecimalRelationTableByScaleByPrecision = new Dictionary<int, Dictionary<int, string>>();
        public readonly Dictionary<int, Dictionary<int, SchemaArrayParameter>> DecimalRelationTableParameterByScaleByPrecision = new Dictionary<int, Dictionary<int, SchemaArrayParameter>>();

        private SchemaValidationErrors schemaValidationErrors;
        private Dictionary<string, SchemaProcedure> procedureByName;


        public readonly string ParamInvocationFormat;
        public readonly string ParamFormat;

        private readonly Npgsql.Database database;

        private readonly string prefix;
        private readonly string postfix;

        private Dictionary<IRelationType, SchemaColumn> columnsByRelationType;
        private Dictionary<IRelationType, SchemaTable> tablesByRelationType;
        private Dictionary<IClass, SchemaTable> tableByClass;
        private Dictionary<string, SchemaTable> tablesByName;

        private SchemaColumn objectId;
        private SchemaColumn associationId;
        private SchemaColumn roleId;
        private SchemaColumn typeId;
        private SchemaColumn cacheId;

        private SchemaTable objects;
        private SchemaColumn objectsObjectId;
        private SchemaColumn objectsTypeId;
        private SchemaColumn objectsCacheId;

        private SchemaParameter countParam;
        private SchemaParameter matchRoleParam;

        private DbType cacheDbType;
        private DbType typeDbType;
        private DbType singletonDbType;
        private DbType versionDbType;

        public Schema(Npgsql.Database database)
        {
            this.database = database;

            this.ParamInvocationFormat = ":p_{0}";
            this.ParamFormat = "p_{0}";
            this.prefix = "\"";
            this.postfix = "\"";
            
            var objectDbType = NpgsqlDbType.Bigint;

            this.ObjectArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_o", objectDbType);
            this.CompositeRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", objectDbType);
            this.StringRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Varchar);
            this.StringMaxRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Text);
            this.IntegerRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Integer);
            this.LongRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Bigint);
            this.DecimalRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Numeric);
            this.DoubleRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Double);
            this.BooleanRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Boolean);
            this.DateRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Date);
            this.DateTimeRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Timestamp);
            this.UniqueRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Uuid);
            this.BinaryRelationArrayParam = new SchemaArrayParameter(this, AllorsPrefix + "arr_r", NpgsqlDbType.Bytea);

            foreach (var relationType in database.ObjectFactory.MetaPopulation.RelationTypes)
            {
                var roleType = relationType.RoleType;
                var unitType = roleType.ObjectType as IUnit;
                if (unitType != null && unitType.IsDecimal)
                {
                    var precision = roleType.Precision;
                    var scale = roleType.Scale;

                    var tableName = AllorsPrefix + "_DecimalR_" + precision + "_" + scale;

                    // table
                    Dictionary<int, string> decimalRelationTableByScale;
                    if (!this.DecimalRelationTableByScaleByPrecision.TryGetValue(precision.Value, out decimalRelationTableByScale))
                    {
                        decimalRelationTableByScale = new Dictionary<int, string>();
                        this.DecimalRelationTableByScaleByPrecision[precision.Value] = decimalRelationTableByScale;
                    }

                    if (!decimalRelationTableByScale.ContainsKey(scale.Value))
                    {
                        decimalRelationTableByScale[scale.Value] = tableName;
                    }

                    // param
                    Dictionary<int, SchemaArrayParameter> schemaTableParameterByScale;
                    if (!this.DecimalRelationTableParameterByScaleByPrecision.TryGetValue(precision.Value, out schemaTableParameterByScale))
                    {
                        schemaTableParameterByScale = new Dictionary<int, SchemaArrayParameter>();
                        this.DecimalRelationTableParameterByScaleByPrecision[precision.Value] = schemaTableParameterByScale;
                    }

                    if (!schemaTableParameterByScale.ContainsKey(scale.Value))
                    {
                        schemaTableParameterByScale[scale.Value] = new SchemaArrayParameter(this, AllorsPrefix + "p_r", NpgsqlDbType.Numeric);
                    }
                }
            }

            this.OnConstructed();
        }

        /// <summary>
        /// Gets the parameter to pass a count to.
        /// <example>
        /// Is used in CreateObjects to denote the amount of objects to create.
        /// </example>
        /// </summary>
        public SchemaParameter CountParam
        {
            get { return this.countParam; }
        }

        public SchemaParameter MatchRoleParam
        {
            get { return this.matchRoleParam; }
        }

        public SchemaColumn TypeId
        {
            get { return this.typeId; }
        }

        public SchemaColumn CacheId
        {
            get { return this.cacheId; }
        }

        public SchemaColumn AssociationId
        {
            get { return this.associationId; }
        }

        public SchemaColumn RoleId
        {
            get { return this.roleId; }
        }

        public SchemaColumn ObjectId
        {
            get { return this.objectId; }
        }

        public SchemaTable Objects
        {
            get { return this.objects; }
        }

        public SchemaColumn ObjectsObjectId
        {
            get { return this.objectsObjectId; }
        }

        public SchemaColumn ObjectsTypeId
        {
            get { return this.objectsTypeId; }
        }

        public SchemaColumn ObjectsCacheId
        {
            get { return this.objectsCacheId; }
        }

        public int SingletonValue
        {
            get
            {
                return 1;
            }
        }

        protected Database Database
        {
            get { return this.database; }
        }

        protected Dictionary<IRelationType, SchemaColumn> ColumnsByRelationType
        {
            get { return this.columnsByRelationType; }
        }

        protected Dictionary<string, SchemaTable> TablesByName
        {
            get { return this.tablesByName; }
        }

        protected Dictionary<IRelationType, SchemaTable> TablesByRelationType
        {
            get { return this.tablesByRelationType; }
        }

        protected Dictionary<IClass, SchemaTable> TableByClass
        {
            get { return this.tableByClass; }
        }

        private DbType TypeDbType
        {
            get { return this.typeDbType; }
        }

        private DbType CacheDbType
        {
            get { return this.cacheDbType; }
        }

        private DbType VersionDbType
        {
            get { return this.versionDbType; }
        }

        private DbType SingletonDbType
        {
            get { return this.singletonDbType; }
        }

        public SchemaTable this[string tableName]
        {
            get { return this.tablesByName[tableName.ToLowerInvariant()]; }
        }

        public static void AddError(SchemaValidationErrors schemaValidationErrors, SchemaTable table, SchemaValidationErrorKind kind)
        {
            schemaValidationErrors.AddTableError(table.ObjectType, table.RelationType, null, table.ToString(), null, kind, kind + ": " + table);
        }

        public static void AddError(SchemaValidationErrors schemaValidationErrors, SchemaTable table, SchemaColumn column, SchemaValidationErrorKind kind)
        {
            var roleType = column.RelationType == null ? null : column.RelationType.RoleType;
            schemaValidationErrors.AddTableError(null, null, roleType, table.ToString(), column.ToString(), kind, kind + ": " + table + "." + column);
        }

        public static void AddError(SchemaValidationErrors schemaValidationErrors, SchemaProcedure schemaProcedure, SchemaValidationErrorKind kind, string message)
        {
            schemaValidationErrors.AddProcedureError(schemaProcedure, kind, message);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<SchemaTable>)this).GetEnumerator();
        }

        IEnumerator<SchemaTable> IEnumerable<SchemaTable>.GetEnumerator()
        {
            return this.tablesByName.Values.GetEnumerator();
        }

        public SchemaColumn Column(IRelationType relationType)
        {
            return this.columnsByRelationType[relationType];
        }

        public SchemaColumn Column(IAssociationType association)
        {
            return this.columnsByRelationType[association.RelationType];
        }

        public SchemaColumn Column(IRoleType role)
        {
            return this.columnsByRelationType[role.RelationType];
        }

        public SchemaTable Table(IClass type)
        {
            return this.tableByClass[type];
        }

        public SchemaTable Table(IRelationType relationType)
        {
            return this.tablesByRelationType[relationType];
        }

        public SchemaTable Table(IAssociationType association)
        {
            return this.tablesByRelationType[association.RelationType];
        }

        public SchemaTable Table(IRoleType role)
        {
            return this.tablesByRelationType[role.RelationType];
        }

        public string EscapeIfReserved(string name)
        {
            if (ReservedWords.Names.Contains(name.ToLowerInvariant()))
            {
                return this.prefix + name + this.postfix;
            }

            return name;
        }

        protected virtual DbType GetDbType(IRoleType role)
        {
            var unitType = (IUnit)role.ObjectType;
            var unitTypeTag = unitType.UnitTag;
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    return DbType.String;
                case UnitTags.Integer:
                    return DbType.Int32;
                case UnitTags.Decimal:
                    return DbType.Decimal;
                case UnitTags.Float:
                    return DbType.Double;
                case UnitTags.Boolean:
                    return DbType.Boolean;
                case UnitTags.DateTime:
                    return DbType.DateTime;
                case UnitTags.Unique:
                    return DbType.Guid;
                case UnitTags.Binary:
                    return DbType.Binary;
                default:
                    throw new ArgumentException("Unkown unit type " + role.ObjectType);
            }
        }

        protected bool Contains(string tableName)
        {
            return this.tablesByName.ContainsKey(tableName.ToLowerInvariant());
        }

        private void CreateTablesFromMeta()
        {
            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (roleType.ObjectType is IComposite &&
                    ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    var column = new SchemaColumn(this, "R", this.ObjectDbType, false, true, relationType.IsIndexed ? SchemaIndexType.Combined : SchemaIndexType.None, relationType);
                    this.ColumnsByRelationType.Add(relationType, column);
                }
                else
                {
                    if (roleType.ObjectType is IUnit)
                    {
                        var column = new SchemaColumn(this, roleType.SingularPropertyName, this.GetDbType(roleType), false, false, relationType.IsIndexed ? SchemaIndexType.Single : SchemaIndexType.None, relationType, roleType.Size, roleType.Precision, roleType.Scale);
                        this.ColumnsByRelationType.Add(relationType, column);
                    }
                    else if (relationType.ExistExclusiveClasses)
                    {
                        if (roleType.IsOne)
                        {
                            var column = new SchemaColumn(this, roleType.SingularPropertyName, this.ObjectDbType, false, false, relationType.IsIndexed ? SchemaIndexType.Combined : SchemaIndexType.None, relationType);
                            this.ColumnsByRelationType.Add(relationType, column);
                        }
                        else
                        {
                            var column = new SchemaColumn(this, associationType.SingularFullName, this.ObjectDbType, false, false, relationType.IsIndexed ? SchemaIndexType.Combined : SchemaIndexType.None, relationType);
                            this.ColumnsByRelationType.Add(relationType, column);
                        }
                    }
                }
            }

            foreach (var objectType in this.Database.MetaPopulation.Composites)
            {
                var @class = objectType as IClass;
                if (@class != null)
                {
                    var schemaTable = new SchemaTable(this, SchemaTableKind.Object, objectType);
                    this.TablesByName.Add(schemaTable.Name, schemaTable);
                    this.TableByClass.Add(@class, schemaTable);

                    schemaTable.AddColumn(this.ObjectId);
                    schemaTable.AddColumn(this.TypeId);

                    var roleTypes = new List<IRoleType>();
                    var associationTypes = new List<IAssociationType>();

                    foreach (var roleType in @class.RoleTypes)
                    {
                        if (!roleTypes.Contains(roleType))
                        {
                            roleTypes.Add(roleType);
                        }
                    }

                    foreach (var associationType in @class.AssociationTypes)
                    {
                        if (!associationTypes.Contains(associationType))
                        {
                            associationTypes.Add(associationType);
                        }
                    }

                    foreach (var associationType in associationTypes)
                    {
                        var relationType = associationType.RelationType;
                        var roleType = relationType.RoleType;
                        if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && roleType.IsMany)
                        {
                            schemaTable.AddColumn(this.Column(relationType));
                        }
                    }

                    foreach (var roleType in roleTypes)
                    {
                        var relationType = roleType.RelationType;
                        var associationType = relationType.AssociationType;
                        if (roleType.ObjectType is IUnit)
                        {
                            schemaTable.AddColumn(this.Column(relationType));
                        }
                        else
                        {
                            if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && !roleType.IsMany)
                            {
                                schemaTable.AddColumn(this.Column(relationType));
                            }
                        }
                    }
                }
            }

            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (roleType.ObjectType is IComposite && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    var schemaTable = new SchemaTable(this, SchemaTableKind.Relation, relationType);
                    this.TablesByName.Add(schemaTable.Name, schemaTable);
                    this.TablesByRelationType.Add(relationType, schemaTable);

                    schemaTable.AddColumn(this.AssociationId);
                    schemaTable.AddColumn(this.Column(relationType));
                }
            }
        }

        public SchemaValidationErrors SchemaValidationErrors
        {
            get
            {
                if (this.schemaValidationErrors == null)
                {
                    this.schemaValidationErrors = new SchemaValidationErrors();

                    var session = this.database.CreateNpgsqlManagementSession();
                    try
                    {
                        var tableNames = new HashSet<string>();
                        lock (this.Database)
                        {
                            using (var command = session.CreateCommand("SELECT table_name FROM information_schema.tables"))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var tableName = reader.GetString(0).ToLowerInvariant();
                                        if (this.Contains(tableName))
                                        {
                                            tableNames.Add(tableName);
                                        }
                                    }
                                }
                            }
                        }

                        var dataRowViewByExistingColumnsByTable = new Dictionary<SchemaTable, Dictionary<SchemaColumn, SchemaExistingColumn>>();
                        lock (this.Database)
                        {
                            using (var command = session.CreateCommand(
@"SELECT    table_name, 
            column_name, 
            data_type, 
            character_maximum_length,
            character_octet_length, 
            numeric_precision, 
            numeric_scale, 
            datetime_precision 
FROM information_schema.columns"))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var tableName = reader.GetString(0).ToLowerInvariant();
                                        if (this.Contains(tableName))
                                        {
                                            var table = this[tableName];
                                            var columnName = reader.GetString(1).ToLowerInvariant();

                                            if (table.Contains(columnName))
                                            {
                                                var column = table[columnName];

                                                var existingColumn = new SchemaExistingColumn
                                                {
                                                    DataType = reader.GetString(2),
                                                    CharacterMaximumLength = reader.IsDBNull(3) ? 0 : (int)reader[3],
                                                    CharacterOctetLength = reader.IsDBNull(4) ? 0 : (int)reader[4],
                                                    NumericPrecision = reader.IsDBNull(5) ? 0 : int.Parse(reader[5].ToString()),
                                                    NumericScale = reader.IsDBNull(6) ? 0 : (int)reader[6],
                                                    DateTimePrecision = reader.IsDBNull(7) ? 0 : int.Parse(reader[7].ToString())
                                                };

                                                if (!dataRowViewByExistingColumnsByTable.ContainsKey(table))
                                                {
                                                    dataRowViewByExistingColumnsByTable.Add(table, new Dictionary<SchemaColumn, SchemaExistingColumn>());
                                                }

                                                dataRowViewByExistingColumnsByTable[table].Add(column, existingColumn);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        foreach (SchemaTable table in this)
                        {
                            if (tableNames.Contains(table.Name))
                            {
                                if (dataRowViewByExistingColumnsByTable.ContainsKey(table))
                                {
                                    var dataRowViewByExistingColumns = dataRowViewByExistingColumnsByTable[table];
                                    foreach (SchemaColumn column in table)
                                    {
                                        if (dataRowViewByExistingColumns.ContainsKey(column))
                                        {
                                            var existingColumn = dataRowViewByExistingColumns[column];
                                            if (column.RelationType != null)
                                            {
                                                var dataType = existingColumn.DataType.ToLower();

                                                if (column.RelationType.RoleType.ObjectType is IComposite)
                                                {
                                                    if (!dataType.Equals(this.NpgsqlDbType.ToString().ToLower()))
                                                    {
                                                        AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                    }
                                                }
                                                else
                                                {
                                                    var unitType = (IUnit)column.RelationType.RoleType.ObjectType;
                                                    var unitTypeTag = unitType.UnitTag;
                                                    switch (dataType)
                                                    {
                                                        case "varchar":
                                                        case "character varying":
                                                            if (unitTypeTag != UnitTags.String)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }
                                                            else
                                                            {
                                                                var size = existingColumn.CharacterMaximumLength;

                                                                if (column.RelationType.RoleType.Size > size || size == -1)
                                                                {
                                                                    AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                                }
                                                            }

                                                            break;

                                                        case "text":
                                                            if (unitTypeTag != UnitTags.String)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }
                                                            else
                                                            {
                                                                var size = existingColumn.CharacterMaximumLength;

                                                                if (column.RelationType.RoleType.Size != -1)
                                                                {
                                                                    AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                                }
                                                            }

                                                            break;

                                                        case "int4":
                                                        case "integer":
                                                            if (unitTypeTag != UnitTags.Integer)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }

                                                            break;

                                                        case "numeric":
                                                            if (unitTypeTag != UnitTags.Decimal)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }
                                                            else
                                                            {
                                                                var precision = existingColumn.NumericPrecision;
                                                                var scale = existingColumn.NumericScale;

                                                                var role = column.RelationType.RoleType;
                                                                if (role.Precision > precision)
                                                                {
                                                                    AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                                }

                                                                if (role.Scale > scale)
                                                                {
                                                                    AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                                }
                                                            }

                                                            break;

                                                        case "double precision":
                                                            if (unitTypeTag != UnitTags.Float)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }

                                                            break;

                                                        case "boolean":
                                                            if (unitTypeTag != UnitTags.Boolean)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }

                                                            break;

                                                        case "timestamp":
                                                            if (unitTypeTag != UnitTags.DateTime)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }

                                                            break;

                                                        case "uuid":
                                                            if (unitTypeTag != UnitTags.Unique)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }

                                                            break;

                                                        case "bytea":
                                                            if (unitTypeTag != UnitTags.Binary)
                                                            {
                                                                AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                            }
                                                            else
                                                            {
                                                                var size = existingColumn.CharacterOctetLength;

                                                                if (column.RelationType.RoleType.Size > size
                                                                    && size != -1)
                                                                {
                                                                    AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Incompatible);
                                                                }
                                                            }

                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            AddError(this.schemaValidationErrors, table, column, SchemaValidationErrorKind.Missing);
                                        }
                                    }
                                }
                                else
                                {
                                    AddError(this.schemaValidationErrors, table, SchemaValidationErrorKind.Missing);
                                }
                            }
                            else
                            {
                                AddError(this.schemaValidationErrors, table, SchemaValidationErrorKind.Missing);
                            }
                        }

                        var procedureDefinitionByName = new Dictionary<string, string>();
                        lock (this.Database)
                        {
                            using (var command = session.CreateCommand("SELECT ROUTINE_NAME, ROUTINE_DEFINITION FROM INFORMATION_SCHEMA.ROUTINES"))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var name = reader.GetString(0).ToLowerInvariant();
                                        var definition = reader.GetValue(1);

                                        if (!procedureDefinitionByName.ContainsKey(name)
                                            && definition != DBNull.Value)
                                        {
                                            procedureDefinitionByName.Add(name, (string)definition);
                                        }
                                    }
                                }
                            }
                        }

                        var strip = new Regex(@"\s+");

                        foreach (var procedure in this.Procedures)
                        {
                            string existingProcedureDefinition;
                            if (!procedureDefinitionByName.TryGetValue(procedure.Name.ToLowerInvariant(), out existingProcedureDefinition))
                            {
                                AddError(this.schemaValidationErrors, procedure, SchemaValidationErrorKind.Missing, "Procedure " + procedure.Name + " is missing.");
                            }
                            else
                            {
                                var strippedMeta = strip.Replace(procedure.Definition, string.Empty);
                                var strippedDatabase = strip.Replace(existingProcedureDefinition, string.Empty);
                                if (!strippedMeta.Contains(strippedDatabase))
                                {
                                    AddError(this.schemaValidationErrors, procedure, SchemaValidationErrorKind.Incompatible, "Procedure " + procedure.Name + " is incompatible.");
                                }
                            }
                        }
                    }
                    catch
                    {
                        this.schemaValidationErrors = null;
                        throw;
                    }
                    finally
                    {
                        session.Rollback();
                    }
                }

                return this.schemaValidationErrors;
            }
        }

        public IEnumerable<SchemaProcedure> Procedures
        {
            get
            {
                return this.procedureByName.Values;
            }
        }

        protected internal Npgsql.Database NpgsqlDatabase
        {
            get
            {
                return this.database;
            }
        }

        protected DbType ObjectDbType
        {
            get
            {
                return DbType.Int64;
            }
        }

        protected NpgsqlDbType NpgsqlDbType
        {
            get
            {
                return NpgsqlDbType.Bigint;
            }
        }

        public Sql.SchemaParameter CreateParameter(string name, DbType dbType)
        {
            return new SchemaParameter(this, name, dbType);
        }

        protected void OnConstructed()
        {
            this.procedureByName = new Dictionary<string, SchemaProcedure>();

            // Get Cache Ids
            var procedure = new SchemaProcedure { Name = AllorsPrefix + "GC" };
            procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS TABLE 
(
     __" + this.ObjectId + " " + this.database.GetSqlType(this.ObjectId) + @",
     __" + this.CacheId + " " + this.database.GetSqlType(this.CacheId) + @"
) 
AS $$
BEGIN
    RETURN QUERY
    SELECT " + this.ObjectId + ", " + this.CacheId + @"
    FROM " + this.Objects + @"
    WHERE " + this.ObjectId + " IN (SELECT * FROM unnest(" + this.ObjectArrayParam + @"));
END
$$ language plpgsql;
";

            this.procedureByName.Add(procedure.Name, procedure);

            // Update Cache Ids
            procedure = new SchemaProcedure { Name = AllorsPrefix + "UC" };
            procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + this.Objects + @"
    SET " + this.CacheId + " = " + this.CacheId + @" - 1
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

            this.procedureByName.Add(procedure.Name, procedure);

            // Reset Cache Ids
            procedure = new SchemaProcedure { Name = AllorsPrefix + "RC" };
            procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN
FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + this.Objects + @"
    SET " + this.CacheId + " = " + Reference.InitialVersion + @"
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

            this.procedureByName.Add(procedure.Name, procedure);

            // Instantiate Objects
            procedure = new SchemaProcedure { Name = AllorsPrefix + "IOS" };
            procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS TABLE 
(
     __" + this.ObjectId + " " + this.database.GetSqlType(this.ObjectId) + @",
     __" + this.TypeId + " " + this.database.GetSqlType(this.TypeId) + @",
     __" + this.CacheId + " " + this.database.GetSqlType(this.CacheId) + @"
) 
AS $$
BEGIN

RETURN QUERY
SELECT " + this.ObjectId + "," + this.TypeId + "," + this.CacheId + @"
FROM " + this.Objects + @"
WHERE " + this.ObjectId + @" IN ( SELECT * FROM unnest(" + this.ObjectArrayParam + @"));

END
$$ language plpgsql;
";

            this.procedureByName.Add(procedure.Name, procedure);

            foreach (var concreteComposite in this.Database.ObjectFactory.MetaPopulation.Classes)
            {
                var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(concreteComposite);

                if (sortedUnitRoleTypes.Length > 0)
                {
                    // Get Unit Roles
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GU_" + concreteComposite.Name };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.ObjectId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectId.Param + " " + this.database.GetSqlType(this.ObjectId) + @")
RETURNS TABLE 
(";
                    var first = true;
                    foreach (var role in sortedUnitRoleTypes)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            procedure.Definition += ", ";
                        }

                        procedure.Definition += "__" + this.ColumnsByRelationType[role.RelationType].Name + " " + this.database.GetSqlType(this.ColumnsByRelationType[role.RelationType]);
                    }

                    procedure.Definition += @") 
AS $$
BEGIN
    RETURN QUERY
    SELECT ";
                    first = true;
                    foreach (var role in sortedUnitRoleTypes)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            procedure.Definition += ", ";
                        }

                        procedure.Definition += this.ColumnsByRelationType[role.RelationType];
                    }

                    procedure.Definition += @"
    FROM " + this.Table(concreteComposite.ExclusiveClass) + @"
    WHERE " + this.ObjectId + "=" + this.ObjectId.Param + @";
END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);
                }
            }

            foreach (var dictionaryEntry in this.TableByClass)
            {
                var objectType = dictionaryEntry.Key;
                var table = dictionaryEntry.Value;

                // Load Objects
                procedure = new SchemaProcedure { Name = AllorsPrefix + "L_" + objectType.Name };
                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.TypeId) + ", " + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.TypeId.Param + @" " + this.database.GetSqlType(this.TypeId) + @", " + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN
FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    INSERT INTO " + table + " (" + this.TypeId + ", " + this.ObjectId + @")
    VALUES (" + this.TypeId.Param + @", " + this.ObjectArrayParam + @"[i]);

    END LOOP;

END
$$ language plpgsql;
";

                this.procedureByName.Add(procedure.Name, procedure);

                // Create Object
                procedure = new SchemaProcedure { Name = AllorsPrefix + "CO_" + objectType.Name };
                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.TypeId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.TypeId.Param + @" " + this.database.GetSqlType(this.TypeId) + @")
RETURNS " + this.database.GetSqlType(this.ObjectId) + @"
AS $$
DECLARE  " + this.ObjectId.Param + " " + this.database.GetSqlType(this.ObjectId) + @";
BEGIN

INSERT INTO " + this.Objects + " (" + this.TypeId + ", " + this.CacheId + @")
VALUES (" + this.TypeId.Param + ", " + Reference.InitialVersion + @")
RETURNING " + this.ObjectId + " INTO " + this.ObjectId.Param + @";

INSERT INTO " + table + " (" + this.ObjectId + "," + this.TypeId + @")
VALUES (" + this.ObjectId.Param + "," + this.TypeId.Param + @");

RETURN " + this.ObjectId.Param + @";

END
$$ language plpgsql;
";

                this.procedureByName.Add(procedure.Name, procedure);

                // Create Objects
                procedure = new SchemaProcedure { Name = AllorsPrefix + "COS_" + objectType.Name };
                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.TypeId) + ", " + this.database.GetSqlType(this.CountParam.DbType) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.TypeId.Param + @" " + this.database.GetSqlType(this.TypeId) + @", " + this.CountParam + @" " + this.database.GetSqlType(this.CountParam.DbType) + @")
RETURNS SETOF " + this.database.GetSqlType(this.ObjectId) + @"
AS $$
DECLARE ID integer; 
DECLARE COUNTER integer := 0;

BEGIN

WHILE COUNTER < " + this.CountParam + @" LOOP

    INSERT INTO " + this.Objects.StatementName + " (" + this.TypeId + ", " + this.CacheId + @")
    VALUES (" + this.TypeId.Param + ", " + Reference.InitialVersion + @" )
    RETURNING " + this.ObjectId + @" INTO ID;

    INSERT INTO " + this.Table(objectType.ExclusiveClass) + " (" + this.ObjectId + "," + this.TypeId + @")
    VALUES (ID," + this.TypeId.Param + @");
   
    COUNTER := COUNTER+1;

    RETURN NEXT ID;

    END LOOP;

END
$$ language plpgsql;
";

                this.procedureByName.Add(procedure.Name, procedure);

                // Insert Object
                procedure = new SchemaProcedure { Name = AllorsPrefix + "INS_" + objectType.Name };
                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.ObjectId) + ", " + this.database.GetSqlType(this.TypeId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectId.Param + " " + this.database.GetSqlType(this.ObjectId) + ", " + this.TypeId.Param + @" " + this.database.GetSqlType(this.TypeId) + @")
RETURNS BOOLEAN
AS $$
BEGIN

IF 
    EXISTS (SELECT " + this.ObjectId + @"
    FROM " + this.Table(objectType.ExclusiveClass) + @"
    WHERE " + this.ObjectId + "=" + this.ObjectId.Param + @")
THEN 
    RETURN FALSE;
ELSE
    INSERT INTO " + this.Objects + " (" + this.ObjectId + "," + this.TypeId + "," + this.CacheId + @")
    VALUES (" + this.ObjectId.Param + "," + this.TypeId.Param + ", " + Reference.InitialVersion + @");
    
    INSERT INTO " + this.Table(objectType.ExclusiveClass) + " (" + this.ObjectId + "," + this.TypeId + @")
    VALUES (" + this.ObjectId.Param + "," + this.TypeId.Param + @");
   
    IF currval('" + this.Objects + "_" + this.ObjectId + @"_seq') < " + this.ObjectId.Param + @"
    THEN
        PERFORM setval('" + this.Objects + "_" + this.ObjectId + @"_seq', " + this.ObjectId.Param + @");
    END IF;

    RETURN TRUE;
END IF;

END
$$ language plpgsql;
";

                this.procedureByName.Add(procedure.Name, procedure);

                foreach (SchemaColumn column in table)
                {
                    var relationType = column.RelationType;
                    if (relationType != null)
                    {
                        var roleType = relationType.RoleType;
                        var associationType = relationType.AssociationType;

                        if (relationType.RoleType.ObjectType is IUnit)
                        {
                            var unitType = (IUnit)relationType.RoleType.ObjectType;
                            var unitTypeTag = unitType.UnitTag;
                            switch (unitTypeTag)
                            {
                                case UnitTags.String:
                                    if (relationType.RoleType.Size == -1)
                                    {
                                        // Set MAX String Role
                                        procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                        procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.StringRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.StringRelationArrayParam + @" " + this.StringRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.StringRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    }
                                    else
                                    {
                                        // Set String Role
                                        procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                        procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.StringMaxRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.StringMaxRelationArrayParam + @" " + this.StringMaxRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.StringMaxRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    }

                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.Integer:
                                    // Set Integer Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.IntegerRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.IntegerRelationArrayParam + @" " + this.IntegerRelationArrayParam.TypeName + @")
RETURNS void
AS $$ 
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.IntegerRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.Decimal:
                                    // Set Decimal Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.DecimalRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.DecimalRelationArrayParam + @" " + this.DecimalRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.DecimalRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    this.procedureByName.Add(procedure.Name, procedure);

                                    break;

                                case UnitTags.Float:
                                    // Set Double Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.DoubleRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.DoubleRelationArrayParam + @" " + this.DoubleRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.DoubleRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.Boolean:
                                    // Set Boolean Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.BooleanRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.BooleanRelationArrayParam + @" " + this.BooleanRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.BooleanRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.DateTime:
                                    // Set DateTime Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.DateTimeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.DateTimeRelationArrayParam + @" " + this.DateTimeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.DateTimeRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.Unique:
                                    // Set Unique Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.UniqueRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.UniqueRelationArrayParam + @" " + this.UniqueRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.UniqueRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";
                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                case UnitTags.Binary:
                                    // Set Binary Role
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "SR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.BinaryRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.BinaryRelationArrayParam + @" " + this.BinaryRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.BinaryRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                    break;

                                default:
                                    throw new ArgumentException("Unknown Unit IObjectType: " + roleType.ObjectType.SingularName);
                            }
                        }
                        else
                        {
                            if (roleType.IsOne)
                            {
                                // Get IComposite Role (1-1 and *-1) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "GR_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.AssociationId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.AssociationId.Param + @" " + this.database.GetSqlType(this.AssociationId) + @")
RETURNS " + this.database.GetSqlType(this.RoleId) + @"
AS $$
DECLARE " + this.RoleId.Param + " " + this.database.GetSqlType(this.RoleId) + @";
BEGIN
    SELECT " + this.Column(roleType) + @"
    FROM " + table + @"
    WHERE " + this.ObjectId + "=" + this.AssociationId.Param + @"
    INTO " + this.RoleId.Param + @";

    RETURN " + this.RoleId.Param + @";
END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);

                                if (associationType.IsOne)
                                {
                                    // Get IComposite Association (1-1) [object table]
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GA_" + objectType.Name + "_" + associationType.SingularFullName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.RoleId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.RoleId.Param + @" " + this.database.GetSqlType(this.RoleId) + @")
RETURNS " + this.database.GetSqlType(this.AssociationId) + @"
AS $$
DECLARE " + this.AssociationId.Param + " " + this.database.GetSqlType(this.AssociationId) + @";
BEGIN
    SELECT " + this.ObjectId + @"
    FROM " + table + @"
    WHERE " + this.Column(roleType) + "=" + this.RoleId.Param + @"
    INTO " + this.AssociationId.Param + @";

    RETURN " + this.AssociationId.Param + @";
END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                }
                                else
                                {
                                    // Get IComposite Association (*-1) [object table]
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GA_" + objectType.Name + "_" + associationType.SingularFullName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.RoleId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.RoleId.Param + @" " + this.database.GetSqlType(this.RoleId) + @")
RETURNS SETOF " + this.database.GetSqlType(this.AssociationId) + @"
AS $$
BEGIN
    RETURN QUERY
    SELECT " + this.ObjectId + @"
    FROM " + table + @"
    WHERE " + this.Column(roleType) + "=" + this.RoleId.Param + @";
END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                }

                                // Set IComposite Role (1-1 and *-1) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "S_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + @" " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = " + this.CompositeRelationArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);

                                // Clear IComposite Role (1-1 and *-1) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "C_" + objectType.Name + "_" + roleType.SingularPropertyName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 
    UPDATE " + table + @"
    SET " + this.Column(roleType) + @" = null
    WHERE " + this.ObjectId + " IN (SELECT unnest(" + this.ObjectArrayParam + @"));
END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);
                            }
                            else
                            {
                                // Get Composites Role (1-*) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "GR_" + objectType.Name + "_" + associationType.SingularFullName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.AssociationId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.AssociationId.Param + @" " + this.database.GetSqlType(this.AssociationId) + @")
RETURNS SETOF " + this.database.GetSqlType(this.RoleId) + @"
AS $$
DECLARE " + this.RoleId.Param + " " + this.database.GetSqlType(this.RoleId) + @";
BEGIN
    RETURN QUERY
    SELECT " + this.ObjectId + @"
    FROM " + table + @"
    WHERE " + this.Column(associationType) + "=" + this.AssociationId.Param + @";
END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);

                                if (associationType.IsOne)
                                {
                                    // Get IComposite Association (1-*) [object table]
                                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GA_" + objectType.Name + "_" + associationType.SingularFullName };
                                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.RoleId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.RoleId.Param + @" " + this.database.GetSqlType(this.RoleId) + @")
RETURNS " + this.database.GetSqlType(this.AssociationId) + @"
AS $$
DECLARE " + this.AssociationId.Param + " " + this.database.GetSqlType(this.AssociationId) + @";
BEGIN
    SELECT " + this.Column(associationType) + @"
    FROM " + table + @"
    WHERE " + this.ObjectId + "=" + this.RoleId.Param + @"
    INTO " + this.AssociationId.Param + @";

    RETURN " + this.AssociationId.Param + @";
END
$$ language plpgsql;
";

                                    this.procedureByName.Add(procedure.Name, procedure);
                                }

                                // Add IComposite Role (1-*) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "A_" + objectType.Name + "_" + associationType.SingularFullName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + @" " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(associationType) + @" = " + this.ObjectArrayParam + @"[i]
    WHERE " + this.ObjectId + @" = " + this.CompositeRelationArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);

                                // Remove IComposite Role (1-*) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "R_" + objectType.Name + "_" + associationType.SingularFullName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + @" " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void

AS $$
BEGIN
FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.Column(associationType) + @" = null
    WHERE " + this.ObjectId + " = " + this.CompositeRelationArrayParam + @"[i]
    AND " + this.Column(associationType) + " = " + this.ObjectArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                                this.procedureByName.Add(procedure.Name, procedure);

                                // Clear Composites Role (1-*) [object table]
                                procedure = new SchemaProcedure { Name = AllorsPrefix + "C_" + objectType.Name + "_" + associationType.SingularFullName };
                                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN
    UPDATE " + table + @"
    SET " + this.Column(associationType) + @" = null
    WHERE " + this.Column(associationType) + " IN (SELECT * FROM unnest(" + this.ObjectArrayParam + @"));
END
$$ language plpgsql;
";
                                this.procedureByName.Add(procedure.Name, procedure);
                            }
                        }
                    }
                }
            }

            foreach (var dictionaryEntry in this.TablesByRelationType)
            {
                var relationType = dictionaryEntry.Key;
                var roleType = relationType.RoleType;
                var associationType = relationType.AssociationType;
                var table = dictionaryEntry.Value;

                if (roleType.IsMany)
                {
                    // Get Composites Role (1-* and *-*) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GR_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.AssociationId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.AssociationId.Param + @" " + this.database.GetSqlType(this.AssociationId) + @")
RETURNS SETOF " + this.database.GetSqlType(this.RoleId) + @"
AS $$
DECLARE " + this.RoleId.Param + " " + this.database.GetSqlType(this.RoleId) + @";
BEGIN
    RETURN QUERY
    SELECT " + this.RoleId + @"
    FROM " + this.Table(roleType) + @"
    WHERE " + this.AssociationId + "=" + this.AssociationId.Param + @";
END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);

                    // Add IComposite Role (1-* and *-*) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "A_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + @" " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    INSERT INTO " + table + " (" + this.AssociationId + "," + this.RoleId + @")
    VALUES (" + this.ObjectArrayParam + @"[i], " + this.CompositeRelationArrayParam + @"[i]);

    END LOOP;

END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);

                    // Remove IComposite Role (1-* and *-*) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "R_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + " " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    DELETE
    FROM " + table + @"
    WHERE
    " + this.AssociationId + " = " + this.ObjectArrayParam + @"[i]
    AND " + this.RoleId + " = " + this.CompositeRelationArrayParam + @"[i];

    END LOOP;

END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);
                }
                else
                {
                    // Get IComposite Role (1-1 and *-1) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GR_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.AssociationId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.AssociationId.Param + @" " + this.database.GetSqlType(this.AssociationId) + @")
RETURNS " + this.database.GetSqlType(this.RoleId) + @"
AS $$
DECLARE " + this.RoleId.Param + " " + this.database.GetSqlType(this.RoleId) + @";
BEGIN
    SELECT " + this.RoleId + @"
    FROM " + this.Table(roleType) + @"
    WHERE " + this.AssociationId + "=" + this.AssociationId.Param + @"
    INTO " + this.RoleId.Param + @";

    RETURN " + this.RoleId.Param + @";
END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);

                    // Set IComposite Role (1-1 and *-1) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "S_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + ", " + this.CompositeRelationArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @", " + this.CompositeRelationArrayParam + @" " + this.CompositeRelationArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN

FOR i IN array_lower(" + this.ObjectArrayParam + @", 1) .. array_upper(" + this.ObjectArrayParam + @", 1)
    LOOP

    UPDATE " + table + @"
    SET " + this.RoleId + @" = " + this.CompositeRelationArrayParam + @"[i]
    WHERE " + this.AssociationId + @" = " + this.ObjectArrayParam + @"[i];
    IF found THEN
        CONTINUE;
    END IF;

    INSERT INTO " + table + @"(" + this.AssociationId + "," + this.RoleId + @")
    VALUES (" + this.ObjectArrayParam + "[i], " + this.CompositeRelationArrayParam + @"[i]);

    END LOOP;

END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);
                }

                if (associationType.IsOne)
                {
                    // Get IComposite Association (1-1) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GA_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.RoleId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.RoleId.Param + @" " + this.database.GetSqlType(this.RoleId) + @")
RETURNS " + this.database.GetSqlType(this.AssociationId) + @"
AS $$
DECLARE " + this.AssociationId.Param + " " + this.database.GetSqlType(this.AssociationId) + @";
BEGIN
    SELECT " + this.AssociationId + @"
    FROM " + table + @"
    WHERE " + this.RoleId + "=" + this.RoleId.Param + @"
    INTO " + this.AssociationId.Param + @";

    RETURN " + this.AssociationId.Param + @";
END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);
                }
                else
                {
                    // Get IComposite Association (*-1) [relation table]
                    procedure = new SchemaProcedure { Name = AllorsPrefix + "GA_" + roleType.SingularFullName };
                    procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.database.GetSqlType(this.RoleId) + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.RoleId.Param + @" " + this.database.GetSqlType(this.RoleId) + @")
RETURNS SETOF " + this.database.GetSqlType(this.AssociationId) + @"
AS $$
BEGIN
    RETURN QUERY
    SELECT " + this.AssociationId + @"
    FROM " + table + @"
    WHERE " + this.RoleId + "=" + this.RoleId.Param + @";
END
$$ language plpgsql;
";

                    this.procedureByName.Add(procedure.Name, procedure);
                }

                // Clear IComposite Role (1-1 and *-1) [relation table]
                procedure = new SchemaProcedure { Name = AllorsPrefix + "C_" + roleType.SingularFullName };
                procedure.Definition =
@"DROP FUNCTION IF EXISTS " + procedure.Name + @"(" + this.ObjectArrayParam.TypeName + @");
CREATE FUNCTION " + procedure.Name + @"(" + this.ObjectArrayParam + @" " + this.ObjectArrayParam.TypeName + @")
RETURNS void
AS $$
BEGIN 
    DELETE
    FROM " + table + @" T
    WHERE T." + this.AssociationId + " IN ( SELECT * FROM unnest(" + this.ObjectArrayParam + @"));
END
$$ language plpgsql;
";

                this.procedureByName.Add(procedure.Name, procedure);
            }
        }

        public class SchemaExistingColumn
        {
            public string DataType { get; set; }

            public int CharacterMaximumLength { get; set; }

            public int CharacterOctetLength { get; set; }

            public int NumericPrecision { get; set; }

            public int NumericScale { get; set; }

            public int DateTimePrecision { get; set; }
        }
    }
}
// <copyright file="Mapping.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public class Mapping
    {
        public const string ParamFormat = "@{0}";

        public const string ColumnNameForObject = "o";
        public const string ColumnNameForClass = "c";
        public const string ColumnNameForVersion = "v";
        public const string ColumnNameForAssociation = "a";
        public const string ColumnNameForRole = "r";

        public const string SqlTypeForClass = "uniqueidentifier";
        public const string SqlTypeForObject = "bigint";
        public const string SqlTypeForVersion = "bigint";
        public const string SqlTypeForCount = "int";

        public const SqlDbType SqlDbTypeForClass = SqlDbType.UniqueIdentifier;
        public const SqlDbType SqlDbTypeForObject = SqlDbType.BigInt;
        public const SqlDbType SqlDbTypeForVersion = SqlDbType.BigInt;
        public const SqlDbType SqlDbTypeForCount = SqlDbType.Int;

        public readonly Dictionary<IRoleType, string> ParamNameByRoleType;

        internal static readonly string ParamNameForObject = string.Format(ParamFormat, ColumnNameForObject);
        internal static readonly string ParamNameForClass = string.Format(ParamFormat, ColumnNameForClass);
        internal static readonly string ParamNameForVersion = string.Format(ParamFormat, ColumnNameForVersion);
        internal static readonly string ParamNameForAssociation = string.Format(ParamFormat, ColumnNameForAssociation);
        internal static readonly string ParamNameForCompositeRole = string.Format(ParamFormat, ColumnNameForRole);
        internal static readonly string ParamNameForCount = string.Format(ParamFormat, "count");
        internal static readonly string ParamNameForTableType = string.Format(ParamFormat, "table");

        internal readonly string TableNameForObjects;

        internal readonly Dictionary<IClass, string> TableNameForObjectByClass;
        internal readonly Dictionary<IRelationType, string> ColumnNameByRelationType;
        internal readonly Dictionary<IRelationType, string> UnescapedColumnNameByRelationType;
        internal readonly Dictionary<IRelationType, string> TableNameForRelationByRelationType;

        internal readonly string TableTypeNameForObject;
        internal readonly string TableTypeNameForVersionedObject;

        internal readonly string TableTypeColumnNameForObject;
        internal readonly string TableTypeColumnNameForVersion;

        internal readonly string TableTypeNameForCompositeRelation;
        internal readonly string TableTypeNameForStringRelation;
        internal readonly string TableTypeNameForIntegerRelation;
        internal readonly string TableTypeNameForFloatRelation;
        internal readonly string TableTypeNameForBooleanRelation;
        internal readonly string TableTypeNameForDateTimeRelation;
        internal readonly string TableTypeNameForUniqueRelation;
        internal readonly string TableTypeNameForBinaryRelation;

        internal readonly string TableTypeNamePrefixForDecimalRelation;

        internal readonly string TableTypeColumnNameForAssociation;
        internal readonly string TableTypeColumnNameForRole;

        internal readonly Dictionary<int, Dictionary<int, string>> TableTypeNameForDecimalRelationByScaleByPrecision;

        internal readonly Dictionary<IClass, string> ProcedureNameForLoadObjectByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForCreateObjectByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForCreateObjectsByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForDeleteObjectByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForGetUnitRolesByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForPrefetchUnitRolesByClass;
        internal readonly Dictionary<IClass, Dictionary<IRelationType, string>> ProcedureNameForSetUnitRoleByRelationTypeByClass;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForGetRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForPrefetchRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForSetRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForAddRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForRemoveRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForClearRoleByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForGetAssociationByRelationType;
        internal readonly Dictionary<IRelationType, string> ProcedureNameForPrefetchAssociationByRelationType;
        internal string ProcedureNameForInstantiate;

        internal string ProcedureNameForGetVersion;
        internal string ProcedureNameForUpdateVersion;
        private const string ProcedurePrefixForInstantiate = "i";

        private const string ProcedurePrefixForGetVersion = "gv";
        private const string ProcedurePrefixForSetVersion = "sv";
        private const string ProcedurePrefixForUpdateVersion = "uv";

        private const string ProcedurePrefixForCreateObject = "co_";
        private const string ProcedurePrefixForCreateObjects = "cos_";
        private const string ProcedurePrefixForDeleteObject = "do_";
        private const string ProcedurePrefixForLoad = "l_";

        private const string ProcedurePrefixForGetUnits = "gu_";
        private const string ProcedurePrefixForPrefetchUnits = "pu_";

        private const string ProcedurePrefixForGetRole = "gc_";
        private const string ProcedurePrefixForPrefetchRole = "pc_";
        private const string ProcedurePrefixForSetRole = "sc_";
        private const string ProcedurePrefixForClearRole = "cc_";
        private const string ProcedurePrefixForAddRole = "ac_";
        private const string ProcedurePrefixForRemoveRole = "rc_";

        private const string ProcedurePrefixForGetAssociation = "ga_";
        private const string ProcedurePrefixForPrefetchAssociation = "pa_";

        public Mapping(Database database)
        {
            this.Database = database;

            // TableTypes
            // ----------
            this.TableTypeNameForObject = database.SchemaName + "." + "_t_o";
            this.TableTypeNameForVersionedObject = database.SchemaName + "." + "_t_vo";
            this.TableTypeNameForCompositeRelation = database.SchemaName + "." + "_t_c";
            this.TableTypeNameForStringRelation = database.SchemaName + "." + "_t_s";
            this.TableTypeNameForIntegerRelation = database.SchemaName + "." + "_t_i";
            this.TableTypeNameForFloatRelation = database.SchemaName + "." + "_t_f";
            this.TableTypeNameForBooleanRelation = database.SchemaName + "." + "_t_bo";
            this.TableTypeNameForDateTimeRelation = database.SchemaName + "." + "_t_da";
            this.TableTypeNameForUniqueRelation = database.SchemaName + "." + "_t_u";
            this.TableTypeNameForBinaryRelation = database.SchemaName + "." + "_t_bi";
            this.TableTypeNamePrefixForDecimalRelation = database.SchemaName + "." + "_t_de";

            this.TableTypeColumnNameForObject = "_o";
            this.TableTypeColumnNameForVersion = "_c";
            this.TableTypeColumnNameForAssociation = "_a";
            this.TableTypeColumnNameForRole = "_r";

            this.TableTypeNameForDecimalRelationByScaleByPrecision = new Dictionary<int, Dictionary<int, string>>();
            foreach (var relationType in database.MetaPopulation.RelationTypes)
            {
                var roleType = relationType.RoleType;
                if (roleType.ObjectType.IsUnit && ((IUnit)roleType.ObjectType).IsDecimal)
                {
                    var precision = roleType.Precision;
                    var scale = roleType.Scale;

                    var tableName = this.TableTypeNamePrefixForDecimalRelation + precision + "_" + scale;

                    if (!this.TableTypeNameForDecimalRelationByScaleByPrecision.TryGetValue(precision.Value, out var decimalRelationTableByScale))
                    {
                        decimalRelationTableByScale = new Dictionary<int, string>();
                        this.TableTypeNameForDecimalRelationByScaleByPrecision[precision.Value] = decimalRelationTableByScale;
                    }

                    if (!decimalRelationTableByScale.ContainsKey(scale.Value))
                    {
                        decimalRelationTableByScale[scale.Value] = tableName;
                    }
                }
            }

            this.TableTypeDefinitionByName = new Dictionary<string, string>
             {
                 {
                     this.TableTypeNameForObject,
                     $@"CREATE TYPE {this.TableTypeNameForObject} AS TABLE ({this.TableTypeColumnNameForObject} {SqlTypeForObject})"
                 },
                 {
                     this.TableTypeNameForVersionedObject,
                     $"CREATE TYPE {this.TableTypeNameForVersionedObject} AS TABLE ({this.TableTypeColumnNameForObject} {SqlTypeForObject}, {this.TableTypeColumnNameForVersion} {SqlTypeForVersion})"
                 },
                 {
                     this.TableTypeNameForCompositeRelation,
                     $@"CREATE TYPE {this.TableTypeNameForCompositeRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} {SqlTypeForObject})"
                 },
                 {
                     this.TableTypeNameForStringRelation,
                     $@"CREATE TYPE {this.TableTypeNameForStringRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} nvarchar(max))"
                 },
                 {
                     this.TableTypeNameForIntegerRelation,
                     $@"CREATE TYPE {this.TableTypeNameForIntegerRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} int)"
                 },
                 {
                     this.TableTypeNameForFloatRelation,
                     $@"CREATE TYPE {this.TableTypeNameForFloatRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} float)"
                 },
                 {
                     this.TableTypeNameForDateTimeRelation,
                     $@"CREATE TYPE {this.TableTypeNameForDateTimeRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} datetime2)"
                 },
                 {
                     this.TableTypeNameForBooleanRelation,
                     $@"CREATE TYPE {this.TableTypeNameForBooleanRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} bit)"
                 },
                 {
                     this.TableTypeNameForUniqueRelation,
                     $@"CREATE TYPE {this.TableTypeNameForUniqueRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} uniqueidentifier)"
                 },
                 {
                     this.TableTypeNameForBinaryRelation,
                     $@"CREATE TYPE {this.TableTypeNameForBinaryRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} varbinary(max))"
                 },
             };

            foreach (var precisionEntry in this.TableTypeNameForDecimalRelationByScaleByPrecision)
            {
                var precision = precisionEntry.Key;
                foreach (var scaleEntry in precisionEntry.Value)
                {
                    var scale = scaleEntry.Key;
                    var decimalRelationTable = scaleEntry.Value;

                    var sql = new StringBuilder();
                    sql.Append("CREATE TYPE " + decimalRelationTable + " AS TABLE\n");
                    sql.Append("(" + this.TableTypeColumnNameForAssociation + " " + SqlTypeForObject + ",\n");
                    sql.Append(this.TableTypeColumnNameForRole + " DECIMAL(" + precision + "," + scale + ") )\n");

                    this.TableTypeDefinitionByName.Add(decimalRelationTable, sql.ToString());
                }
            }

            // Tables
            // ------
            this.TableNameForObjects = database.SchemaName + "." + "_o";
            this.TableNameForObjectByClass = new Dictionary<IClass, string>();
            this.ColumnNameByRelationType = new Dictionary<IRelationType, string>();
            this.UnescapedColumnNameByRelationType = new Dictionary<IRelationType, string>();
            this.ParamNameByRoleType = new Dictionary<IRoleType, string>();

            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                this.TableNameForObjectByClass.Add(@class, this.Database.SchemaName + "." + this.NormalizeName(@class.SingularName));

                foreach (var associationType in @class.AssociationTypes)
                {
                    var relationType = associationType.RelationType;
                    var roleType = relationType.RoleType;
                    if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && roleType.IsMany)
                    {
                        this.ColumnNameByRelationType[relationType] = this.NormalizeName(associationType.SingularPropertyName);
                        this.UnescapedColumnNameByRelationType[relationType] = associationType.SingularPropertyName;
                    }
                }

                foreach (var roleType in @class.RoleTypes)
                {
                    var relationType = roleType.RelationType;
                    var associationType3 = relationType.AssociationType;
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.ColumnNameByRelationType[relationType] = this.NormalizeName(roleType.SingularName);
                        this.UnescapedColumnNameByRelationType[relationType] = roleType.SingularName;
                        this.ParamNameByRoleType[roleType] = string.Format(ParamFormat, roleType.SingularFullName);
                    }
                    else
                    {
                        if (!(associationType3.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && !roleType.IsMany)
                        {
                            this.ColumnNameByRelationType[relationType] = this.NormalizeName(roleType.SingularName);
                            this.UnescapedColumnNameByRelationType[relationType] = roleType.SingularName;
                        }
                    }
                }
            }

            this.TableNameForRelationByRelationType = new Dictionary<IRelationType, string>();

            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    this.TableNameForRelationByRelationType.Add(relationType, this.Database.SchemaName + "." + this.NormalizeName(relationType.RoleType.SingularFullName));
                }
            }

            // Procedures
            // ----------
            this.ProcedureDefinitionByName = new Dictionary<string, string>();

            this.ProcedureNameForLoadObjectByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForCreateObjectByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForCreateObjectsByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForDeleteObjectByClass = new Dictionary<IClass, string>();

            this.ProcedureNameForGetUnitRolesByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForPrefetchUnitRolesByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForSetUnitRoleByRelationTypeByClass = new Dictionary<IClass, Dictionary<IRelationType, string>>();

            this.ProcedureNameForGetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForPrefetchRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForSetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForAddRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForRemoveRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForClearRoleByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForGetAssociationByRelationType = new Dictionary<IRelationType, string>();
            this.ProcedureNameForPrefetchAssociationByRelationType = new Dictionary<IRelationType, string>();

            this.Instantiate();
            this.GetVersionIds();
            this.UpdateVersionIds();

            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                this.LoadObjects(@class);
                this.CreateObject(@class);
                this.CreateObjects(@class);
                this.DeleteObject(@class);

                if (this.Database.GetSortedUnitRolesByObjectType(@class).Length > 0)
                {
                    this.GetUnitRoles(@class);
                    this.PrefetchUnitRoles(@class);
                }

                foreach (var associationType in @class.AssociationTypes)
                {
                    if (!(associationType.IsMany && associationType.RoleType.IsMany) && associationType.RelationType.ExistExclusiveClasses && associationType.RoleType.IsMany)
                    {
                        this.GetCompositesRoleObjectTable(associationType, @class);
                        this.PrefetchCompositesRoleObjectTable(associationType, @class);

                        if (associationType.IsOne)
                        {
                            this.GetCompositeAssociationObjectTable(associationType, @class);
                            this.PrefetchCompositeAssociationObjectTable(associationType, @class);
                        }

                        this.AddCompositeRoleObjectTable(associationType, @class);
                        this.RemoveCompositeRoleObjectTable(associationType, @class);
                        this.ClearCompositeRoleObjectTable(associationType, @class);
                    }
                }

                foreach (var roleType in @class.RoleTypes)
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.SetUnitRoleType(roleType, @class);
                    }
                    else
                    {
                        if (!(roleType.AssociationType.IsMany && roleType.IsMany) && roleType.RelationType.ExistExclusiveClasses && roleType.IsOne)
                        {
                            this.GetCompositeRoleObjectTable(roleType, @class);
                            this.PrefetchCompositeRoleObjectTable(roleType, @class);

                            if (roleType.AssociationType.IsOne)
                            {
                                this.GetCompositeAssociationOne2OneObjectTable(roleType, @class);
                                this.PrefetchCompositeAssociationObjectTable(roleType, @class);
                            }
                            else
                            {
                                this.GetCompositesAssociationMany2OneObjectTable(roleType, @class);
                                this.PrefetchCompositesAssociationMany2OneObjectTable(roleType, @class);
                            }

                            this.SetCompositeRole(roleType, @class);
                            this.ClearCompositeRole(roleType, @class);
                        }
                    }
                }
            }

            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
            {
                if (!relationType.RoleType.ObjectType.IsUnit && ((relationType.AssociationType.IsMany && relationType.RoleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant());
                    this.ProcedureNameForClearRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + relationType.RoleType.SingularFullName.ToLowerInvariant());

                    if (relationType.RoleType.IsMany)
                    {
                        this.GetCompositesRoleRelationTable(relationType);
                        this.PrefetchCompositesRoleRelationTable(relationType);
                        this.AddCompositeRoleRelationTable(relationType);
                        this.RemoveCompositeRoleRelationTable(relationType);
                    }
                    else
                    {
                        this.GetCompositeRoleRelationTable(relationType);
                        this.PrefetchCompositeRoleRelationType(relationType);
                        this.SetCompositeRoleRelationType(relationType);
                    }

                    if (relationType.AssociationType.IsOne)
                    {
                        this.GetCompositeAssociationRelationTable(relationType);
                        this.PrefetchCompositeAssociationRelationTable(relationType);
                    }
                    else
                    {
                        this.GetCompositesAssociationRelationTable(relationType);
                        this.PrefetchCompositesAssociationRelationTable(relationType);
                    }

                    this.ClearCompositeRoleRelationTable(relationType);
                }
            }
        }

        public Dictionary<string, string> ProcedureDefinitionByName { get; }

        public Dictionary<string, string> TableTypeDefinitionByName { get; }

        protected internal Database Database { get; }

        public string GetTableTypeName(IRoleType roleType)
        {
            var unitTypeTag = ((IUnit)roleType.ObjectType).UnitTag;
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    return this.TableTypeNameForStringRelation;

                case UnitTags.Integer:
                    return this.TableTypeNameForIntegerRelation;

                case UnitTags.Float:
                    return this.TableTypeNameForFloatRelation;

                case UnitTags.Boolean:
                    return this.TableTypeNameForBooleanRelation;

                case UnitTags.DateTime:
                    return this.TableTypeNameForDateTimeRelation;

                case UnitTags.Unique:
                    return this.TableTypeNameForUniqueRelation;

                case UnitTags.Binary:
                    return this.TableTypeNameForBinaryRelation;

                case UnitTags.Decimal:
                    return this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value];

                default:
                    throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
            }
        }

        internal string NormalizeName(string name)
        {
            name = name.ToLowerInvariant();
            if (ReservedWords.Names.Contains(name))
            {
                return "[" + name + "]";
            }

            return name;
        }

        internal string GetSqlType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.UnitTag)
            {
                case UnitTags.String:
                    if (roleType.Size == -1 || roleType.Size > 4000)
                    {
                        return "nvarchar(max)";
                    }

                    return "nvarchar(" + roleType.Size + ")";

                case UnitTags.Integer:
                    return "int";

                case UnitTags.Decimal:
                    return "decimal(" + roleType.Precision + "," + roleType.Scale + ")";

                case UnitTags.Float:
                    return "float";

                case UnitTags.Boolean:
                    return "bit";

                case UnitTags.DateTime:
                    return "datetime2";

                case UnitTags.Unique:
                    return "uniqueidentifier";

                case UnitTags.Binary:
                    if (roleType.Size == -1 || roleType.Size > 8000)
                    {
                        return "varbinary(max)";
                    }

                    return "varbinary(" + roleType.Size + ")";

                default:
                    return "!UNKNOWN VALUE TYPE!";
            }
        }

        internal SqlDbType GetSqlDbType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.UnitTag)
            {
                case UnitTags.String:
                    return SqlDbType.NVarChar;

                case UnitTags.Integer:
                    return SqlDbType.Int;

                case UnitTags.Decimal:
                    return SqlDbType.Decimal;

                case UnitTags.Float:
                    return SqlDbType.Float;

                case UnitTags.Boolean:
                    return SqlDbType.Bit;

                case UnitTags.DateTime:
                    return SqlDbType.DateTime2;

                case UnitTags.Unique:
                    return SqlDbType.UniqueIdentifier;

                case UnitTags.Binary:
                    return SqlDbType.VarBinary;

                default:
                    throw new Exception("Unknown Unit Type");
            }
        }

        private void LoadObjects(IClass @class)
        {
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForLoad + @class.Name.ToLowerInvariant();
            this.ProcedureNameForLoadObjectByClass.Add(@class, name);

            // Load Objects
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForClass} {SqlTypeForClass},
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    INSERT INTO {table} ({ColumnNameForClass}, {ColumnNameForObject})
    SELECT {ParamNameForClass}, {this.TableTypeColumnNameForObject}
    FROM {ParamNameForTableType}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObject(IClass @class)
        {
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObject + @class.Name.ToLowerInvariant();
            this.ProcedureNameForCreateObjectByClass.Add(@class, name);

            // CreateObject
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForClass} {SqlTypeForClass}
AS
BEGIN
    DECLARE  {ParamNameForObject} AS {SqlTypeForObject}

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({ParamNameForClass}, {Reference.InitialVersion});

    SELECT {ParamNameForObject} = SCOPE_IDENTITY();

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({ParamNameForObject},{ParamNameForClass});

    SELECT {ParamNameForObject};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObjects(IClass @class)
        {
            var table = this.TableNameForObjectByClass[@class.ExclusiveClass];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObjects + @class.Name.ToLowerInvariant();
            this.ProcedureNameForCreateObjectsByClass.Add(@class, name);

            // CreateObjects
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForClass} {SqlTypeForClass},
    {ParamNameForCount} {SqlTypeForCount}
AS
BEGIN
    DECLARE @IDS TABLE (id INT);
    DECLARE @O INT, @COUNTER INT

    SET @COUNTER = 0
    WHILE @COUNTER < {ParamNameForCount}
        BEGIN

        INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
        VALUES ({ParamNameForClass}, {Reference.InitialVersion} );

        INSERT INTO @IDS(id)
        VALUES (SCOPE_IDENTITY());

        SET @COUNTER = @COUNTER+1;
        END

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    SELECT ID, {ParamNameForClass} FROM @IDS;

    SELECT id FROM @IDS;
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void DeleteObject(IClass @class)
        {
            var table = this.TableNameForObjectByClass[@class.ExclusiveClass];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForDeleteObject + @class.Name.ToLowerInvariant();
            this.ProcedureNameForDeleteObjectByClass.Add(@class, name);

            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForObject} {SqlTypeForObject}
AS
BEGIN
    DELETE FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject}={ParamNameForObject};

    DELETE FROM {table}
    WHERE {ColumnNameForObject}={ParamNameForObject};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var table = this.TableNameForObjectByClass[@class.ExclusiveClass];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetUnits + @class.Name.ToLowerInvariant();
            this.ProcedureNameForGetUnitRolesByClass.Add(@class, name);

            // Get Unit Roles
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForObject} AS {SqlTypeForObject}
AS
BEGIN
    SELECT {string.Join(", ", sortedUnitRoleTypes.Select(v => this.ColumnNameByRelationType[v.RelationType]))}
    FROM {table}
    WHERE {ColumnNameForObject}={ParamNameForObject}
END
";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var table = this.TableNameForObjectByClass[@class.ExclusiveClass];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchUnits + @class.Name.ToLowerInvariant();
            this.ProcedureNameForPrefetchUnitRolesByClass.Add(@class, name);

            // Prefetch Unit Roles
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {string.Join(", ", sortedUnitRoleTypes.Select(v => this.ColumnNameByRelationType[v.RelationType]))}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[relationType]}={ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[relationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForAssociation}
    FROM {table}
    INNER JOIN {ParamNameForTableType} AS r
    ON {ColumnNameForObject} = r.{this.TableTypeColumnNameForRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = null
    FROM {table}
    INNER JOIN {ParamNameForTableType} AS r
    ON
        {this.ColumnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForAssociation} AND
        {ColumnNameForObject} = r.{this.TableTypeColumnNameForRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForClearRoleByRelationType.Add(relationType, name);

            // Clear Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {this.TableNameForObjectByClass[@class]}
    SET {this.ColumnNameByRelationType[relationType]} = null
    FROM {this.TableNameForObjectByClass[@class]}
    INNER JOIN {ParamNameForTableType} AS a
    ON {this.ColumnNameByRelationType[relationType]} = a.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetUnitRoleType(IRoleType roleType, IClass @class)
        {
            if (!this.ProcedureNameForSetUnitRoleByRelationTypeByClass.TryGetValue(@class, out var procedureNameForSetUnitRoleByRelationType))
            {
                procedureNameForSetUnitRoleByRelationType = new Dictionary<IRelationType, string>();
                this.ProcedureNameForSetUnitRoleByRelationTypeByClass.Add(@class, procedureNameForSetUnitRoleByRelationType);
            }

            var relationType = roleType.RelationType;
            var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).UnitTag;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            procedureNameForSetUnitRoleByRelationType.Add(relationType, name);

            string tableTypeName;
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    tableTypeName = this.TableTypeNameForStringRelation;
                    break;

                case UnitTags.Integer:
                    tableTypeName = this.TableTypeNameForIntegerRelation;
                    break;

                case UnitTags.Float:
                    tableTypeName = this.TableTypeNameForFloatRelation;
                    break;

                case UnitTags.Decimal:
                    tableTypeName = this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value];
                    break;

                case UnitTags.Boolean:
                    tableTypeName = this.TableTypeNameForBooleanRelation;
                    break;

                case UnitTags.DateTime:
                    tableTypeName = this.TableTypeNameForDateTimeRelation;
                    break;

                case UnitTags.Unique:
                    tableTypeName = this.TableTypeNameForUniqueRelation;
                    break;

                case UnitTags.Binary:
                    tableTypeName = this.TableTypeNameForBinaryRelation;
                    break;

                default:
                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName);
            }

            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {tableTypeName} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForRole}
    FROM {table}
    INNER JOIN {ParamNameForTableType} AS r
    ON {ColumnNameForObject} = r.{this.TableTypeColumnNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeRoleObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT  {ColumnNameForObject}, {this.ColumnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationOne2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[relationType]}={ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IRoleType roleType, IClass @class)
        {
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchAssociationByRelationType.Add(roleType.RelationType, name);

            // Prefetch Composite Association (1-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {this.ColumnNameByRelationType[roleType.RelationType]}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[roleType.RelationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationMany2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[relationType]}={ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationMany2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (*-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {this.ColumnNameByRelationType[relationType]}
    FROM {table}
    WHERE {this.ColumnNameByRelationType[relationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetCompositeRole(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForSetRoleByRelationType.Add(relationType, name);

            // Set Composite Role (1-1 and *-1) [object table]
            var definition = @"CREATE PROCEDURE " + name + @"
    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
AS
BEGIN
    UPDATE " + table + @"
    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
    FROM " + table + @"
    INNER JOIN " + ParamNameForTableType + @" AS r
    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRole(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForClearRoleByRelationType.Add(relationType, name);

            // Clear Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = null
    FROM {table}
    INNER JOIN {ParamNameForTableType} AS a
    ON {ColumnNameForObject} = a.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    INSERT INTO {table} ({ColumnNameForAssociation},{ColumnNameForRole})
    SELECT {this.TableTypeColumnNameForAssociation}, {this.TableTypeColumnNameForRole}
    FROM {ParamNameForTableType}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    DELETE T
    FROM {table} T
    INNER JOIN {ParamNameForTableType} R
    ON T.{ColumnNameForAssociation} = R.{this.TableTypeColumnNameForAssociation}
    AND T.{ColumnNameForRole} = R.{this.TableTypeColumnNameForRole};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeRoleRelationTable(IRelationType relationType)
        {
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForRole}
    FROM {this.TableNameForRelationByRelationType[relationType]}
    WHERE {ColumnNameForAssociation}={ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";
            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForSetRoleByRelationType.Add(relationType, name);

            // Set Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    MERGE {table} T
    USING {ParamNameForTableType} AS r
    ON T.{ColumnNameForAssociation} = r.{this.TableTypeColumnNameForAssociation}

    WHEN MATCHED THEN
    UPDATE SET {ColumnNameForRole}= r.{this.TableTypeColumnNameForRole}

    WHEN NOT MATCHED THEN
    INSERT ({ColumnNameForAssociation},{ColumnNameForRole})
    VALUES (r.{this.TableTypeColumnNameForAssociation}, r.{this.TableTypeColumnNameForRole});
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.ProcedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (1-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.ProcedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (*-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
   {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.TableNameForRelationByRelationType[relationType];
            var name = this.ProcedureNameForClearRoleByRelationType[relationType];

            // Clear Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    DELETE T
    FROM {table} T
    INNER JOIN {ParamNameForTableType} A
    ON T.{ColumnNameForAssociation} = A.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void UpdateVersionIds()
        {
            this.ProcedureNameForUpdateVersion = this.Database.SchemaName + "." + ProcedurePrefixForUpdateVersion;

            // Update Version Ids
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForUpdateVersion}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {this.TableNameForObjects}
    SET {ColumnNameForVersion} = {ColumnNameForVersion} + 1
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN ( SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType});
END
";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForUpdateVersion, definition);
        }

        private void GetVersionIds()
        {
            this.ProcedureNameForGetVersion = this.Database.SchemaName + "." + ProcedurePrefixForGetVersion;

            // Get Version Ids
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForGetVersion}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);
        }

        private void Instantiate()
        {
            this.ProcedureNameForInstantiate = this.Database.SchemaName + "." + ProcedurePrefixForInstantiate;

            // Instantiate
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForInstantiate}
    {ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {ColumnNameForClass}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);
        }
    }
}

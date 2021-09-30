// <copyright file="Mapping.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Meta;

    [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
    public class Mapping : Sql.Mapping
    {
        public override string ParamInvocationFormat => ParamFormat;

        public override string ParamInvocationNameForObject => this.ParamNameForObject;
        public override string ParamInvocationNameForClass => this.ParamNameForClass;

        public override string TableNameForObjects { get; }
        public override IDictionary<IClass, string> TableNameForObjectByClass => this.tableNameForObjectByClass;

        public override IDictionary<IRelationType, string> ColumnNameByRelationType => this.columnNameByRelationType;

        public override IDictionary<IRoleType, string> ParamInvocationNameByRoleType => this.ParamNameByRoleType;

        public override IDictionary<IClass, string> ProcedureNameForDeleteObjectByClass => this.procedureNameForDeleteObjectByClass;

        public override IDictionary<IClass, string> ProcedureNameForCreateObjectsByClass => this.procedureNameForCreateObjectsByClass;

        public override IDictionary<IClass, string> ProcedureNameForGetUnitRolesByClass => this.procedureNameForGetUnitRolesByClass;

        public override IDictionary<IClass, IDictionary<IRelationType, string>> ProcedureNameForSetUnitRoleByRelationTypeByClass => this.procedureNameForSetUnitRoleByRelationTypeByClass;

        public override IDictionary<IRelationType, string> ProcedureNameForGetRoleByRelationType => this.procedureNameForGetRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForSetRoleByRelationType => this.procedureNameForSetRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForAddRoleByRelationType => this.procedureNameForAddRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForRemoveRoleByRelationType => this.procedureNameForRemoveRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForClearRoleByRelationType => this.procedureNameForClearRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForGetAssociationByRelationType => this.procedureNameForGetAssociationByRelationType;

        public override IDictionary<IClass, string> ProcedureNameForCreateObjectByClass => this.procedureNameForCreateObjectByClass;

        public override string ProcedureNameForInstantiate { get; }

        public override string ProcedureNameForGetVersion { get; }

        public override string ProcedureNameForUpdateVersion { get; }

        public override IDictionary<IClass, string> ProcedureNameForPrefetchUnitRolesByClass => this.procedureNameForPrefetchUnitRolesByClass;

        public override IDictionary<IRelationType, string> ProcedureNameForPrefetchRoleByRelationType => this.procedureNameForPrefetchRoleByRelationType;

        public override IDictionary<IRelationType, string> ProcedureNameForPrefetchAssociationByRelationType => this.procedureNameForPrefetchAssociationByRelationType;

        public override string StringCollation => "COLLATE Latin1_General_100_BIN2";
        public override string Ascending => "ASC";
        public override string Descending => "DESC";

        public override IDictionary<IRelationType, string> TableNameForRelationByRelationType => this.tableNameForRelationByRelationType;

        internal const string SqlTypeForClass = "uniqueidentifier";
        internal const string SqlTypeForObject = "bigint";
        internal const string SqlTypeForVersion = "bigint";

        internal const SqlDbType SqlDbTypeForClass = SqlDbType.UniqueIdentifier;
        internal const SqlDbType SqlDbTypeForObject = SqlDbType.BigInt;
        internal const SqlDbType SqlDbTypeForCount = SqlDbType.Int;

        internal string ParamNameForAssociation { get; }
        internal string ParamNameForCompositeRole { get; }
        internal string ParamNameForCount { get; }
        internal string ParamNameForTableType { get; }
        internal IDictionary<IRoleType, string> ParamNameByRoleType { get; }

        internal string TableTypeNameForObject { get; }
        private string TableTypeNameForVersionedObject { get; }

        internal string TableTypeColumnNameForObject { get; }
        private string TableTypeColumnNameForVersion { get; }

        internal string TableTypeNameForCompositeRelation { get; }
        internal string TableTypeNameForStringRelation { get; }
        internal string TableTypeNameForIntegerRelation { get; }
        internal string TableTypeNameForFloatRelation { get; }
        internal string TableTypeNameForBooleanRelation { get; }
        internal string TableTypeNameForDateTimeRelation { get; }
        internal string TableTypeNameForUniqueRelation { get; }
        internal string TableTypeNameForBinaryRelation { get; }
        private string TableTypeNamePrefixForDecimalRelation { get; }

        internal string TableTypeColumnNameForAssociation { get; }
        internal string TableTypeColumnNameForRole { get; }

        internal Dictionary<int, Dictionary<int, string>> TableTypeNameForDecimalRelationByScaleByPrecision { get; }

        private string ParamNameForObject { get; }
        private string ParamNameForClass { get; }

        private const string ParamFormat = "@{0}";

        private const string ProcedurePrefixForGetVersion = "gv";
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

        private readonly IDictionary<IRelationType, string> tableNameForRelationByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForPrefetchAssociationByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForPrefetchRoleByRelationType;
        private readonly IDictionary<IClass, string> procedureNameForPrefetchUnitRolesByClass;
        private readonly IDictionary<IClass, string> procedureNameForCreateObjectByClass;
        private readonly IDictionary<IRelationType, string> procedureNameForGetAssociationByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForClearRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForRemoveRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForAddRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForSetRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForGetRoleByRelationType;
        private readonly IDictionary<IClass, IDictionary<IRelationType, string>> procedureNameForSetUnitRoleByRelationTypeByClass;
        private readonly IDictionary<IClass, string> procedureNameForGetUnitRolesByClass;
        private readonly IDictionary<IClass, string> procedureNameForCreateObjectsByClass;
        private readonly IDictionary<IClass, string> procedureNameForDeleteObjectByClass;
        private readonly IDictionary<IRelationType, string> columnNameByRelationType;
        private readonly IDictionary<IClass, string> tableNameForObjectByClass;

        public Mapping(Database database)
        {
            this.Database = database;

            this.ProcedureNameForInstantiate = $"{this.Database.SchemaName}.i";
            this.ProcedureNameForGetVersion = $"{this.Database.SchemaName}.{ProcedurePrefixForGetVersion}";
            this.ProcedureNameForUpdateVersion = $"{this.Database.SchemaName}.{ProcedurePrefixForUpdateVersion}";

            this.ParamNameForAssociation = string.Format(ParamFormat, ColumnNameForAssociation);
            this.ParamNameForCompositeRole = string.Format(ParamFormat, ColumnNameForRole);
            this.ParamNameForCount = string.Format(ParamFormat, "count");
            this.ParamNameForTableType = string.Format(ParamFormat, "table");

            this.ParamNameForObject = string.Format(ParamFormat, ColumnNameForObject);
            this.ParamNameForClass = string.Format(ParamFormat, ColumnNameForClass);

            // TableTypes
            // ----------
            this.TableTypeNameForObject = $"{database.SchemaName}._t_o";
            this.TableTypeNameForVersionedObject = $"{database.SchemaName}._t_vo";
            this.TableTypeNameForCompositeRelation = $"{database.SchemaName}._t_c";
            this.TableTypeNameForStringRelation = $"{database.SchemaName}._t_s";
            this.TableTypeNameForIntegerRelation = $"{database.SchemaName}._t_i";
            this.TableTypeNameForFloatRelation = $"{database.SchemaName}._t_f";
            this.TableTypeNameForBooleanRelation = $"{database.SchemaName}._t_bo";
            this.TableTypeNameForDateTimeRelation = $"{database.SchemaName}._t_da";
            this.TableTypeNameForUniqueRelation = $"{database.SchemaName}._t_u";
            this.TableTypeNameForBinaryRelation = $"{database.SchemaName}._t_bi";
            this.TableTypeNamePrefixForDecimalRelation = $"{database.SchemaName}._t_de";

            this.TableTypeColumnNameForObject = "_o";
            this.TableTypeColumnNameForVersion = "_c";
            this.TableTypeColumnNameForAssociation = "_a";
            this.TableTypeColumnNameForRole = "_r";

            this.TableTypeNameForDecimalRelationByScaleByPrecision = new Dictionary<int, Dictionary<int, string>>();
            foreach (var relationType in database.MetaPopulation.DatabaseRelationTypes)
            {
                var roleType = relationType.RoleType;
                if (roleType.ObjectType.IsUnit && ((IUnit)roleType.ObjectType).IsDecimal)
                {
                    var precision = roleType.Precision.Value;
                    var scale = roleType.Scale.Value;

                    var tableName = $"{this.TableTypeNamePrefixForDecimalRelation}{precision}_{scale}";

                    if (!this.TableTypeNameForDecimalRelationByScaleByPrecision.TryGetValue(precision, out var decimalRelationTableByScale))
                    {
                        decimalRelationTableByScale = new Dictionary<int, string>();
                        this.TableTypeNameForDecimalRelationByScaleByPrecision[precision] = decimalRelationTableByScale;
                    }

                    if (!decimalRelationTableByScale.ContainsKey(scale))
                    {
                        decimalRelationTableByScale[scale] = tableName;
                    }
                }
            }

            this.TableTypeDefinitionByName = new Dictionary<string, string>
             {
                 {
                     this.TableTypeNameForObject,
                     $"CREATE TYPE {this.TableTypeNameForObject} AS TABLE ({this.TableTypeColumnNameForObject} {SqlTypeForObject})"
                 },
                 {
                     this.TableTypeNameForVersionedObject,
                     $"CREATE TYPE {this.TableTypeNameForVersionedObject} AS TABLE ({this.TableTypeColumnNameForObject} {SqlTypeForObject}, {this.TableTypeColumnNameForVersion} {SqlTypeForVersion})"
                 },
                 {
                     this.TableTypeNameForCompositeRelation,
                     $"CREATE TYPE {this.TableTypeNameForCompositeRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} {SqlTypeForObject})"
                 },
                 {
                     this.TableTypeNameForStringRelation,
                     $"CREATE TYPE {this.TableTypeNameForStringRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} nvarchar(max))"
                 },
                 {
                     this.TableTypeNameForIntegerRelation,
                     $"CREATE TYPE {this.TableTypeNameForIntegerRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} int)"
                 },
                 {
                     this.TableTypeNameForFloatRelation,
                     $"CREATE TYPE {this.TableTypeNameForFloatRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} float)"
                 },
                 {
                     this.TableTypeNameForDateTimeRelation,
                     $"CREATE TYPE {this.TableTypeNameForDateTimeRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} datetime2)"
                 },
                 {
                     this.TableTypeNameForBooleanRelation,
                     $"CREATE TYPE {this.TableTypeNameForBooleanRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} bit)"
                 },
                 {
                     this.TableTypeNameForUniqueRelation,
                     $"CREATE TYPE {this.TableTypeNameForUniqueRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} uniqueidentifier)"
                 },
                 {
                     this.TableTypeNameForBinaryRelation,
                     $"CREATE TYPE {this.TableTypeNameForBinaryRelation} AS TABLE ({this.TableTypeColumnNameForAssociation} {SqlTypeForObject}, {this.TableTypeColumnNameForRole} varbinary(max))"
                 },
             };

            foreach (var precisionEntry in this.TableTypeNameForDecimalRelationByScaleByPrecision)
            {
                var precision = precisionEntry.Key;
                foreach (var scaleEntry in precisionEntry.Value)
                {
                    var scale = scaleEntry.Key;
                    var decimalRelationTable = scaleEntry.Value;

                    var sql =
@$"CREATE TYPE {decimalRelationTable} AS TABLE
({this.TableTypeColumnNameForAssociation} {SqlTypeForObject},
{this.TableTypeColumnNameForRole} DECIMAL({precision},{scale}) )
";

                    this.TableTypeDefinitionByName.Add(decimalRelationTable, sql);
                }
            }

            // Tables
            // ------
            this.TableNameForObjects = $"{database.SchemaName}._o";
            this.tableNameForObjectByClass = new Dictionary<IClass, string>();
            this.columnNameByRelationType = new Dictionary<IRelationType, string>();
            this.ParamNameByRoleType = new Dictionary<IRoleType, string>();

            foreach (var @class in this.Database.MetaPopulation.DatabaseClasses)
            {
                this.tableNameForObjectByClass.Add(@class, $"{this.Database.SchemaName}.{this.NormalizeName(@class.SingularName)}");

                foreach (var associationType in @class.DatabaseAssociationTypes)
                {
                    var relationType = associationType.RelationType;
                    var roleType = relationType.RoleType;
                    if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && roleType.IsMany)
                    {
                        this.columnNameByRelationType[relationType] = this.NormalizeName(associationType.SingularName);
                    }
                }

                foreach (var roleType in @class.DatabaseRoleTypes)
                {
                    var relationType = roleType.RelationType;
                    var associationType3 = relationType.AssociationType;
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.columnNameByRelationType[relationType] = this.NormalizeName(roleType.SingularName);
                        this.ParamNameByRoleType[roleType] = string.Format(ParamFormat, roleType.SingularFullName);
                    }
                    else if (!(associationType3.IsMany && roleType.IsMany) && relationType.ExistExclusiveDatabaseClasses && !roleType.IsMany)
                    {
                        this.columnNameByRelationType[relationType] = this.NormalizeName(roleType.SingularName);
                    }
                }
            }

            this.tableNameForRelationByRelationType = new Dictionary<IRelationType, string>();

            foreach (var relationType in this.Database.MetaPopulation.DatabaseRelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                {
                    this.tableNameForRelationByRelationType.Add(relationType, $"{this.Database.SchemaName}.{this.NormalizeName(relationType.RoleType.SingularFullName)}");
                }
            }

            // Procedures
            // ----------
            this.ProcedureDefinitionByName = new Dictionary<string, string>();

            this.procedureNameForCreateObjectByClass = new Dictionary<IClass, string>();
            this.procedureNameForCreateObjectsByClass = new Dictionary<IClass, string>();
            this.procedureNameForDeleteObjectByClass = new Dictionary<IClass, string>();

            this.procedureNameForGetUnitRolesByClass = new Dictionary<IClass, string>();
            this.procedureNameForPrefetchUnitRolesByClass = new Dictionary<IClass, string>();
            this.procedureNameForSetUnitRoleByRelationTypeByClass = new Dictionary<IClass, IDictionary<IRelationType, string>>();

            this.procedureNameForGetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForPrefetchRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForSetRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForAddRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForRemoveRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForClearRoleByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForGetAssociationByRelationType = new Dictionary<IRelationType, string>();
            this.procedureNameForPrefetchAssociationByRelationType = new Dictionary<IRelationType, string>();

            this.Instantiate();
            this.GetVersionIds();
            this.UpdateVersionIds();

            foreach (var @class in this.Database.MetaPopulation.DatabaseClasses)
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

                foreach (var associationType in @class.DatabaseAssociationTypes)
                {
                    if (!(associationType.IsMany && associationType.RoleType.IsMany) && associationType.RelationType.ExistExclusiveDatabaseClasses && associationType.RoleType.IsMany)
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

                foreach (var roleType in @class.DatabaseRoleTypes)
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.SetUnitRoleType(roleType, @class);
                    }
                    else if (!(roleType.AssociationType.IsMany && roleType.IsMany) && roleType.RelationType.ExistExclusiveDatabaseClasses && roleType.IsOne)
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

            foreach (var relationType in this.Database.MetaPopulation.DatabaseRelationTypes)
            {
                if (!relationType.RoleType.ObjectType.IsUnit && ((relationType.AssociationType.IsMany && relationType.RoleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                {
                    this.procedureNameForPrefetchAssociationByRelationType.Add(relationType,
                        $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchAssociation}{relationType.RoleType.SingularFullName.ToLowerInvariant()}");
                    this.procedureNameForClearRoleByRelationType.Add(relationType,
                        $"{this.Database.SchemaName}.{ProcedurePrefixForClearRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}");

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
            var unitTypeTag = ((IUnit)roleType.ObjectType).Tag;
            return unitTypeTag switch
            {
                UnitTags.String => this.TableTypeNameForStringRelation,
                UnitTags.Integer => this.TableTypeNameForIntegerRelation,
                UnitTags.Float => this.TableTypeNameForFloatRelation,
                UnitTags.Boolean => this.TableTypeNameForBooleanRelation,
                UnitTags.DateTime => this.TableTypeNameForDateTimeRelation,
                UnitTags.Unique => this.TableTypeNameForUniqueRelation,
                UnitTags.Binary => this.TableTypeNameForBinaryRelation,
                UnitTags.Decimal => this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value],
                _ => throw new ArgumentException($"Unknown Unit ObjectType: {unitTypeTag}")
            };
        }

        public string NormalizeName(string name)
        {
            name = name.ToLowerInvariant();
            if (ReservedWords.Names.Contains(name))
            {
                return $"[{name}]";
            }

            return name;
        }

        internal string GetSqlType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.Tag)
            {
                case UnitTags.String:
                    if (roleType.Size == -1 || roleType.Size > 4000)
                    {
                        return "nvarchar(max)";
                    }

                    return $"nvarchar({roleType.Size})";

                case UnitTags.Integer:
                    return "int";

                case UnitTags.Decimal:
                    return $"decimal({roleType.Precision},{roleType.Scale})";

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

                    return $"varbinary({roleType.Size})";

                default:
                    return "!UNKNOWN VALUE TYPE!";
            }
        }

        internal SqlDbType GetSqlDbType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            return unit.Tag switch
            {
                UnitTags.String => SqlDbType.NVarChar,
                UnitTags.Integer => SqlDbType.Int,
                UnitTags.Decimal => SqlDbType.Decimal,
                UnitTags.Float => SqlDbType.Float,
                UnitTags.Boolean => SqlDbType.Bit,
                UnitTags.DateTime => SqlDbType.DateTime2,
                UnitTags.Unique => SqlDbType.UniqueIdentifier,
                UnitTags.Binary => SqlDbType.VarBinary,
                _ => throw new Exception("Unknown Unit Type")
            };
        }

        private void LoadObjects(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForLoad}{@class.Name.ToLowerInvariant()}";

            // Import Objects
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForClass} {SqlTypeForClass},
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    INSERT INTO {table} ({ColumnNameForClass}, {ColumnNameForObject})
    SELECT {this.ParamNameForClass}, {this.TableTypeColumnNameForObject}
    FROM {this.ParamNameForTableType}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObject(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForCreateObject}{@class.Name.ToLowerInvariant()}";
            this.procedureNameForCreateObjectByClass.Add(@class, name);

            // CreateObject
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForClass} {SqlTypeForClass}
AS
BEGIN
    DECLARE  {this.ParamNameForObject} AS {SqlTypeForObject}

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial});

    SELECT {this.ParamNameForObject} = SCOPE_IDENTITY();

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({this.ParamNameForObject},{this.ParamNameForClass});

    SELECT {this.ParamNameForObject};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObjects(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForCreateObjects}{@class.Name.ToLowerInvariant()}";
            this.procedureNameForCreateObjectsByClass.Add(@class, name);

            // CreateObjects
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForClass} {SqlTypeForClass},
    {this.ParamNameForCount} {"int"}
AS
BEGIN
    DECLARE @IDS TABLE (id INT);
    DECLARE @O INT, @COUNTER INT

    SET @COUNTER = 0
    WHILE @COUNTER < {this.ParamNameForCount}
        BEGIN

        INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
        VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial} );

        INSERT INTO @IDS(id)
        VALUES (SCOPE_IDENTITY());

        SET @COUNTER = @COUNTER+1;
        END

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    SELECT ID, {this.ParamNameForClass} FROM @IDS;

    SELECT id FROM @IDS;
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void DeleteObject(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForDeleteObject}{@class.Name.ToLowerInvariant()}";
            this.procedureNameForDeleteObjectByClass.Add(@class, name);

            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForObject} {SqlTypeForObject}
AS
BEGIN
    DELETE FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};

    DELETE FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var table = this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForGetUnits}{@class.Name.ToLowerInvariant()}";
            this.procedureNameForGetUnitRolesByClass.Add(@class, name);

            // Get Unit Roles
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForObject} AS {SqlTypeForObject}
AS
BEGIN
    SELECT {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRelationType[v.RelationType]))}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForObject}
END
";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var table = this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass];
            var name = $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchUnits}{@class.Name.ToLowerInvariant()}";
            this.procedureNameForPrefetchUnitRolesByClass.Add(@class, name);

            // Prefetch Unit Roles
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRelationType[v.RelationType]))}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetAssociation}{@class.Name.ToLowerInvariant()}_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchAssociation}{@class.Name.ToLowerInvariant()}_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForAddRole}{@class.Name.ToLowerInvariant()}_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForAssociation}
    FROM {table}
    INNER JOIN {this.ParamNameForTableType} AS r
    ON {ColumnNameForObject} = r.{this.TableTypeColumnNameForRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForRemoveRole}{@class.Name.ToLowerInvariant()}_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM {table}
    INNER JOIN {this.ParamNameForTableType} AS r
    ON
        {this.columnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForAssociation} AND
        {ColumnNameForObject} = r.{this.TableTypeColumnNameForRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleObjectTable(IAssociationType associationType, IClass @class)
        {
            var relationType = associationType.RelationType;
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForClearRole}{@class.Name.ToLowerInvariant()}_{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForClearRoleByRelationType.Add(relationType, name);

            // Clear Composites Role (1-*) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {this.tableNameForObjectByClass[@class]}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM {this.tableNameForObjectByClass[@class]}
    INNER JOIN {this.ParamNameForTableType} AS a
    ON {this.columnNameByRelationType[relationType]} = a.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetUnitRoleType(IRoleType roleType, IClass @class)
        {
            if (!this.procedureNameForSetUnitRoleByRelationTypeByClass.TryGetValue(@class, out var procedureNameForSetUnitRoleByRelationType))
            {
                procedureNameForSetUnitRoleByRelationType = new Dictionary<IRelationType, string>();
                this.procedureNameForSetUnitRoleByRelationTypeByClass.Add(@class, procedureNameForSetUnitRoleByRelationType);
            }

            var relationType = roleType.RelationType;
            var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).Tag;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForSetRole}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            procedureNameForSetUnitRoleByRelationType.Add(relationType, name);

            var tableTypeName = unitTypeTag switch
            {
                UnitTags.String => this.TableTypeNameForStringRelation,
                UnitTags.Integer => this.TableTypeNameForIntegerRelation,
                UnitTags.Float => this.TableTypeNameForFloatRelation,
                UnitTags.Decimal => this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value],
                UnitTags.Boolean => this.TableTypeNameForBooleanRelation,
                UnitTags.DateTime => this.TableTypeNameForDateTimeRelation,
                UnitTags.Unique => this.TableTypeNameForUniqueRelation,
                UnitTags.Binary => this.TableTypeNameForBinaryRelation,
                _ => throw new ArgumentException($"Unknown Unit ObjectType: {roleType.ObjectType.SingularName}")
            };

            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {tableTypeName} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForRole}
    FROM {table}
    INNER JOIN {this.ParamNameForTableType} AS r
    ON {ColumnNameForObject} = r.{this.TableTypeColumnNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeRoleObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetRole}{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchRole}{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT  {ColumnNameForObject}, {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationOne2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetAssociation}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IRoleType roleType, IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchAssociation}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchAssociationByRelationType.Add(roleType.RelationType, name);

            // Prefetch Composite Association (1-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {this.columnNameByRelationType[roleType.RelationType]}
    FROM {table}
    WHERE {this.columnNameByRelationType[roleType.RelationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationMany2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetAssociation}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationMany2OneObjectTable(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchAssociation}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (*-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetCompositeRole(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForSetRole}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForSetRoleByRelationType.Add(relationType, name);

            // Set Composite Role (1-1 and *-1) [object table]
            var definition = $@"CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = r.{this.TableTypeColumnNameForRole}
    FROM {table}
    INNER JOIN {this.ParamNameForTableType} AS r
    ON {ColumnNameForObject} = r.{this.TableTypeColumnNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRole(IRoleType roleType, IClass @class)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForClearRole}{@class.Name.ToLowerInvariant()}_{roleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForClearRoleByRelationType.Add(relationType, name);

            // Clear Composite Role (1-1 and *-1) [object table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM {table}
    INNER JOIN {this.ParamNameForTableType} AS a
    ON {ColumnNameForObject} = a.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForAddRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    INSERT INTO {table} ({ColumnNameForAssociation},{ColumnNameForRole})
    SELECT {this.TableTypeColumnNameForAssociation}, {this.TableTypeColumnNameForRole}
    FROM {this.ParamNameForTableType}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForRemoveRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-* and *-*) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    DELETE T
    FROM {table} T
    INNER JOIN {this.ParamNameForTableType} R
    ON T.{ColumnNameForAssociation} = R.{this.TableTypeColumnNameForAssociation}
    AND T.{ColumnNameForRole} = R.{this.TableTypeColumnNameForRole};
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeRoleRelationTable(IRelationType relationType)
        {
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForAssociation} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForRole}
    FROM {this.tableNameForRelationByRelationType[relationType]}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForPrefetchRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";
            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForSetRole}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForSetRoleByRelationType.Add(relationType, name);

            // Set Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForCompositeRelation} READONLY
AS
BEGIN
    MERGE {table} T
    USING {this.ParamNameForTableType} AS r
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
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetAssociation}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.procedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (1-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name =
                $"{this.Database.SchemaName}.{ProcedurePrefixForGetAssociation}{relationType.RoleType.SingularFullName.ToLowerInvariant()}";
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForCompositeRole} {SqlTypeForObject}
AS
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.procedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (*-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
   {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.procedureNameForClearRoleByRelationType[relationType];

            // Clear Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
CREATE PROCEDURE {name}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    DELETE T
    FROM {table} T
    INNER JOIN {this.ParamNameForTableType} A
    ON T.{ColumnNameForAssociation} = A.{this.TableTypeColumnNameForObject}
END";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void UpdateVersionIds()
        {
            // Update Version Ids
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForUpdateVersion}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    UPDATE {this.TableNameForObjects}
    SET {ColumnNameForVersion} = {ColumnNameForVersion} + 1
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN ( SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType});
END
";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForUpdateVersion, definition);
        }

        private void GetVersionIds()
        {
            // Get Version Ids
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForGetVersion}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);
        }

        private void Instantiate()
        {
            // Instantiate
            var definition = $@"
CREATE PROCEDURE {this.ProcedureNameForInstantiate}
    {this.ParamNameForTableType} {this.TableTypeNameForObject} READONLY
AS
BEGIN
    SELECT {ColumnNameForObject}, {ColumnNameForClass}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN (SELECT {this.TableTypeColumnNameForObject} FROM {this.ParamNameForTableType})
END";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);
        }
    }
}

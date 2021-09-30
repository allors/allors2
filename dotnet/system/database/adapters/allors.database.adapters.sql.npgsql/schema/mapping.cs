// <copyright file="Mapping.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using NpgsqlTypes;

    public class Mapping : Sql.Mapping
    {
        private readonly IDictionary<IRoleType, string> paramInvocationNameByRoleType;
        private readonly IDictionary<IClass, string> tableNameForObjectByClass;
        private readonly IDictionary<IRelationType, string> columnNameByRelationType;
        private readonly IDictionary<IRelationType, string> tableNameForRelationByRelationType;
        private readonly IDictionary<IClass, string> procedureNameForCreateObjectByClass;
        private readonly IDictionary<IClass, string> procedureNameForCreateObjectsByClass;
        private readonly IDictionary<IClass, string> procedureNameForDeleteObjectByClass;
        private readonly IDictionary<IClass, string> procedureNameForGetUnitRolesByClass;
        private readonly IDictionary<IClass, string> procedureNameForPrefetchUnitRolesByClass;
        private readonly IDictionary<IClass, IDictionary<IRelationType, string>> procedureNameForSetUnitRoleByRelationTypeByClass;
        private readonly IDictionary<IRelationType, string> procedureNameForGetRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForPrefetchRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForSetRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForAddRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForRemoveRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForClearRoleByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForGetAssociationByRelationType;
        private readonly IDictionary<IRelationType, string> procedureNameForPrefetchAssociationByRelationType;

        public override string ParamInvocationFormat => ParameterInvocationFormat;
        public override string ParamInvocationNameForObject { get; }
        public override string ParamInvocationNameForClass { get; }

        public override IDictionary<IRoleType, string> ParamInvocationNameByRoleType => this.paramInvocationNameByRoleType;

        public override string TableNameForObjects { get; }
        public override IDictionary<IClass, string> TableNameForObjectByClass => this.tableNameForObjectByClass;
        public override IDictionary<IRelationType, string> ColumnNameByRelationType => this.columnNameByRelationType;
        public override IDictionary<IRelationType, string> TableNameForRelationByRelationType => this.tableNameForRelationByRelationType;

        public override IDictionary<IClass, string> ProcedureNameForCreateObjectByClass => this.procedureNameForCreateObjectByClass;
        public override IDictionary<IClass, string> ProcedureNameForCreateObjectsByClass => this.procedureNameForCreateObjectsByClass;
        public override IDictionary<IClass, string> ProcedureNameForDeleteObjectByClass => this.procedureNameForDeleteObjectByClass;
        public override IDictionary<IClass, string> ProcedureNameForGetUnitRolesByClass => this.procedureNameForGetUnitRolesByClass;
        public override IDictionary<IClass, string> ProcedureNameForPrefetchUnitRolesByClass => this.procedureNameForPrefetchUnitRolesByClass;
        public override IDictionary<IClass, IDictionary<IRelationType, string>> ProcedureNameForSetUnitRoleByRelationTypeByClass => this.procedureNameForSetUnitRoleByRelationTypeByClass;
        public override IDictionary<IRelationType, string> ProcedureNameForGetRoleByRelationType => this.procedureNameForGetRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForPrefetchRoleByRelationType => this.procedureNameForPrefetchRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForSetRoleByRelationType => this.procedureNameForSetRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForAddRoleByRelationType => this.procedureNameForAddRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForRemoveRoleByRelationType => this.procedureNameForRemoveRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForClearRoleByRelationType => this.procedureNameForClearRoleByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForGetAssociationByRelationType => this.procedureNameForGetAssociationByRelationType;
        public override IDictionary<IRelationType, string> ProcedureNameForPrefetchAssociationByRelationType => this.procedureNameForPrefetchAssociationByRelationType;

        public override string StringCollation => string.Empty;
        public override string Ascending => "ASC NULLS FIRST";
        public override string Descending => "DESC NULLS LAST";

        public override string ProcedureNameForInstantiate { get; }
        public override string ProcedureNameForGetVersion { get; }
        public override string ProcedureNameForUpdateVersion { get; }

        internal const string SqlTypeForClass = "uuid";
        internal const string SqlTypeForObject = "bigint";
        internal const string SqlTypeForVersion = "bigint";
        private const string SqlTypeForCount = "integer";

        internal const NpgsqlDbType NpgsqlDbTypeForClass = NpgsqlDbType.Uuid;
        internal const NpgsqlDbType NpgsqlDbTypeForObject = NpgsqlDbType.Bigint;
        internal const NpgsqlDbType NpgsqlDbTypeForCount = NpgsqlDbType.Integer;

        internal MappingArrayParameter ObjectArrayParam { get; }
        private MappingArrayParameter CompositeRoleArrayParam { get; }
        internal MappingArrayParameter StringRoleArrayParam { get; }
        private MappingArrayParameter StringMaxRoleArrayParam { get; }
        private MappingArrayParameter IntegerRoleArrayParam { get; }
        private MappingArrayParameter DecimalRoleArrayParam { get; }
        private MappingArrayParameter DoubleRoleArrayParam { get; }
        private MappingArrayParameter BooleanRoleArrayParam { get; }
        private MappingArrayParameter DateTimeRoleArrayParam { get; }
        private MappingArrayParameter UniqueRoleArrayParam { get; }
        private MappingArrayParameter BinaryRoleArrayParam { get; }

        private string ParamNameForAssociation { get; }
        private string ParamNameForCompositeRole { get; }
        private string ParamNameForCount { get; }
        private string ParamNameForObject { get; }
        private string ParamNameForClass { get; }

        internal string ParamInvocationNameForAssociation { get; }
        internal string ParamInvocationNameForCompositeRole { get; }
        internal string ParamInvocationNameForCount { get; }

        private const string ProcedurePrefixForInstantiate = "i";
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

        internal const string ParameterFormat = "p_{0}";
        private const string ParameterInvocationFormat = ":p_{0}";

        public Mapping(Database database)
        {
            this.Database = database;

            this.ParamInvocationNameForObject = string.Format(ParameterInvocationFormat, ColumnNameForObject);
            this.ParamInvocationNameForClass = string.Format(ParameterInvocationFormat, ColumnNameForClass);

            this.ProcedureNameForInstantiate = this.Database.SchemaName + "." + ProcedurePrefixForInstantiate;
            this.ProcedureNameForGetVersion = this.Database.SchemaName + "." + ProcedurePrefixForGetVersion;
            this.ProcedureNameForUpdateVersion = this.Database.SchemaName + "." + ProcedurePrefixForUpdateVersion;

            this.ParamNameForAssociation = string.Format(ParameterFormat, ColumnNameForAssociation);
            this.ParamNameForCompositeRole = string.Format(ParameterFormat, ColumnNameForRole);
            this.ParamNameForCount = string.Format(ParameterFormat, "count");
            this.ParamNameForObject = string.Format(ParameterFormat, ColumnNameForObject);
            this.ParamNameForClass = string.Format(ParameterFormat, ColumnNameForClass);

            this.ParamInvocationNameForAssociation = string.Format(ParameterInvocationFormat, ColumnNameForAssociation);
            this.ParamInvocationNameForCompositeRole = string.Format(ParameterInvocationFormat, ColumnNameForRole);
            this.ParamInvocationNameForCount = string.Format(ParameterInvocationFormat, "count");

            this.ObjectArrayParam = new MappingArrayParameter(database, this, "arr_o", NpgsqlDbType.Bigint);
            this.CompositeRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Bigint);
            this.StringRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Varchar);
            this.StringMaxRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Text);
            this.IntegerRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Integer);
            this.DecimalRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Numeric);
            this.DoubleRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Double);
            this.BooleanRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Boolean);
            this.DateTimeRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Timestamp);
            this.UniqueRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Uuid);
            this.BinaryRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Bytea);

            // Tables
            // ------
            this.TableNameForObjects = database.SchemaName + "._o";
            this.tableNameForObjectByClass = new Dictionary<IClass, string>();
            this.columnNameByRelationType = new Dictionary<IRelationType, string>();
            this.paramInvocationNameByRoleType = new Dictionary<IRoleType, string>();

            foreach (var @class in this.Database.MetaPopulation.DatabaseClasses)
            {
                this.tableNameForObjectByClass.Add(@class, this.Database.SchemaName + "." + this.NormalizeName(@class.SingularName));

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
                        this.paramInvocationNameByRoleType[roleType] = string.Format(ParameterInvocationFormat, roleType.SingularFullName);
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
                    this.tableNameForRelationByRelationType.Add(relationType, this.Database.SchemaName + "." + this.NormalizeName(relationType.RoleType.SingularFullName));
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
                        this.GetCompositesRoleObjectTable(@class, associationType);
                        this.PrefetchCompositesRoleObjectTable(@class, associationType);

                        if (associationType.IsOne)
                        {
                            this.GetCompositeAssociationObjectTable(@class, associationType);
                            this.PrefetchCompositeAssociationObjectTable(@class, associationType);
                        }

                        this.AddCompositeRoleObjectTable(@class, associationType);
                        this.RemoveCompositeRoleObjectTable(@class, associationType);
                        this.ClearCompositeRoleObjectTable(@class, associationType);
                    }
                }

                foreach (var roleType in @class.DatabaseRoleTypes)
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.SetUnitRoleType(@class, roleType);
                    }
                    else if (!(roleType.AssociationType.IsMany && roleType.IsMany) && roleType.RelationType.ExistExclusiveDatabaseClasses && roleType.IsOne)
                    {
                        this.GetCompositeRoleObjectTable(@class, roleType);
                        this.PrefetchCompositeRoleObjectTable(@class, roleType);

                        if (roleType.AssociationType.IsOne)
                        {
                            this.GetCompositeAssociationOne2OneObjectTable(@class, roleType);
                            this.PrefetchCompositeAssociationObjectTable(@class, roleType);
                        }
                        else
                        {
                            this.GetCompositesAssociationMany2OneObjectTable(@class, roleType);
                            this.PrefetchCompositesAssociationMany2OneObjectTable(@class, roleType);
                        }

                        this.SetCompositeRole(@class, roleType);
                        this.ClearCompositeRole(@class, roleType);
                    }
                }
            }

            foreach (var relationType in this.Database.MetaPopulation.DatabaseRelationTypes)
            {
                if (!relationType.RoleType.ObjectType.IsUnit && ((relationType.AssociationType.IsMany && relationType.RoleType.IsMany) || !relationType.ExistExclusiveDatabaseClasses))
                {
                    this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant());
                    this.procedureNameForClearRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + relationType.RoleType.SingularFullName.ToLowerInvariant());

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

        internal Dictionary<string, string> ProcedureDefinitionByName { get; }

        protected internal Database Database { get; }

        internal string NormalizeName(string name)
        {
            name = name.ToLowerInvariant();
            if (ReservedWords.Names.Contains(name))
            {
                return "\"" + name + "\"";
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
                        return "text";
                    }

                    return "varchar(" + roleType.Size + ")";

                case UnitTags.Integer:
                    return "integer";

                case UnitTags.Decimal:
                    return "numeric(" + roleType.Precision + "," + roleType.Scale + ")";

                case UnitTags.Float:
                    return "double precision";

                case UnitTags.Boolean:
                    return "boolean";

                case UnitTags.DateTime:
                    return "timestamp";

                case UnitTags.Unique:
                    return "uuid";

                case UnitTags.Binary:
                    return "bytea";

                default:
                    return "!UNKNOWN VALUE TYPE!";
            }
        }

        internal NpgsqlDbType GetNpgsqlDbType(IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.Tag)
            {
                case UnitTags.String:
                    return NpgsqlDbType.Varchar;

                case UnitTags.Integer:
                    return NpgsqlDbType.Integer;

                case UnitTags.Decimal:
                    return NpgsqlDbType.Numeric;

                case UnitTags.Float:
                    return NpgsqlDbType.Double;

                case UnitTags.Boolean:
                    return NpgsqlDbType.Boolean;

                case UnitTags.DateTime:
                    return NpgsqlDbType.Timestamp;

                case UnitTags.Unique:
                    return NpgsqlDbType.Uuid;

                case UnitTags.Binary:
                    return NpgsqlDbType.Bytea;

                default:
                    throw new Exception("Unknown Unit Type");
            }
        }

        private void LoadObjects(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForLoad + @class.Name.ToLowerInvariant();

            // Import Objects
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass},{this.ObjectArrayParam.TypeName});
CREATE FUNCTION {name}(
	{this.ParamNameForClass} {SqlTypeForClass},
	{this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    INSERT INTO  {table} ({ColumnNameForClass}, {ColumnNameForObject})
    SELECT p_c, o
    FROM unnest(p_arr_o) AS t(o)
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObject(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObject + @class.Name.ToLowerInvariant();
            this.procedureNameForCreateObjectByClass.Add(@class, name);

            // CreateObject
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass});
CREATE FUNCTION {name}({this.ParamNameForClass} {SqlTypeForClass})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForObject} {SqlTypeForObject};
BEGIN

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial})
    RETURNING {ColumnNameForObject} INTO {this.ParamNameForObject};

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({this.ParamNameForObject},{this.ParamNameForClass});

    RETURN {this.ParamNameForObject};
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void CreateObjects(IClass @class)
        {
            var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObjects + @class.Name.ToLowerInvariant();
            this.procedureNameForCreateObjectsByClass.Add(@class, name);

            // CreateObjects
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass}, {SqlTypeForCount});
CREATE FUNCTION {name}({this.ParamNameForClass} {SqlTypeForClass}, {this.ParamNameForCount} {SqlTypeForCount})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE ID integer;
DECLARE COUNTER integer := 0;
BEGIN
    WHILE COUNTER < {this.ParamNameForCount} LOOP

        INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
        VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial} )
        RETURNING {ColumnNameForObject} INTO ID;

        INSERT INTO {this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass]} ({ColumnNameForObject},{ColumnNameForClass})
        VALUES (ID,{this.ParamNameForClass});

        COUNTER := COUNTER+1;

        RETURN NEXT ID;
    END LOOP;
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void DeleteObject(IClass @class)
        {
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForDeleteObject + @class.Name.ToLowerInvariant();
            this.procedureNameForDeleteObjectByClass.Add(@class, name);

            var definition = $@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForObject} {SqlTypeForObject})
    RETURNS void
    LANGUAGE sql
AS $$

    DELETE FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};

    DELETE FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};
$$;
";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetUnits + @class.Name.ToLowerInvariant();
            this.procedureNameForGetUnitRolesByClass.Add(@class, name);

            // Get Unit Roles
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForObject} {SqlTypeForObject})
    RETURNS TABLE
    ({string.Join(", ", sortedUnitRoleTypes.Select(v => $"{this.columnNameByRelationType[v.RelationType]} {this.GetSqlType(v)}"))})
    LANGUAGE sql
AS $$
    SELECT {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRelationType[v.RelationType]))}
    FROM {this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass]}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};
$$;";
            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchUnitRoles(IClass @class)
        {
            var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
            var table = this.tableNameForObjectByClass[@class.ExclusiveDatabaseClass];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchUnits + @class.Name.ToLowerInvariant();
            this.procedureNameForPrefetchUnitRolesByClass.Add(@class, name);

            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
        {ColumnNameForObject} {SqlTypeForObject},
        {string.Join(", ", sortedUnitRoleTypes.Select(v => $"{this.columnNameByRelationType[v.RelationType]} {this.GetSqlType(v)}"))}
    )
LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRelationType[v.RelationType]))}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-*) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForAssociation};
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-*) [object table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {this.columnNameByRelationType[relationType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.columnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";
            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            var table = this.tableNameForObjectByClass[@class];
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-*) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (1-*) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {this.columnNameByRelationType[relationType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.columnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-*) [object table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = relations.{ColumnNameForAssociation}
    FROM relations
    WHERE {table}.{ColumnNameForObject} = relations.{ColumnNameForRole}
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-*) [object table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM relations
    WHERE {table}.{this.columnNameByRelationType[relationType]} = relations.{ColumnNameForAssociation} AND
          {table}.{ColumnNameForObject} = relations.{ColumnNameForRole}
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForClearRoleByRelationType.Add(relationType, name);

            // Clear Composites Role (1-*) [object table]
            var definition =
                $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM objects
    WHERE {table}.{this.columnNameByRelationType[relationType]} = objects.{ColumnNameForObject}
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetUnitRoleType(IClass @class, IRoleType roleType)
        {
            if (!this.procedureNameForSetUnitRoleByRelationTypeByClass.TryGetValue(@class, out var procedureNameForSetUnitRoleByRelationType))
            {
                procedureNameForSetUnitRoleByRelationType = new Dictionary<IRelationType, string>();
                this.procedureNameForSetUnitRoleByRelationTypeByClass.Add(@class, procedureNameForSetUnitRoleByRelationType);
            }

            var relationType = roleType.RelationType;
            var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).Tag;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            procedureNameForSetUnitRoleByRelationType.Add(relationType, name);

            MappingArrayParameter roles;
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    roles = this.StringMaxRoleArrayParam;
                    break;

                case UnitTags.Integer:
                    roles = this.IntegerRoleArrayParam;
                    break;

                case UnitTags.Float:
                    roles = this.DoubleRoleArrayParam;
                    break;

                case UnitTags.Decimal:
                    roles = this.DecimalRoleArrayParam;
                    break;

                case UnitTags.Boolean:
                    roles = this.BooleanRoleArrayParam;
                    break;

                case UnitTags.DateTime:
                    roles = this.DateTimeRoleArrayParam;
                    break;

                case UnitTags.Unique:
                    roles = this.UniqueRoleArrayParam;
                    break;

                case UnitTags.Binary:
                    roles = this.BinaryRoleArrayParam;
                    break;

                default:
                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName);
            }

            var rolesType = roles.TypeName;

            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = relations.{ColumnNameForRole}
    FROM relations
    WHERE {ColumnNameForObject} = relations.{ColumnNameForAssociation}
$$;";
            this.ProcedureDefinitionByName.Add(procedureNameForSetUnitRoleByRelationType[relationType], definition);
        }

        private void GetCompositeRoleObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    SELECT {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForAssociation}
    INTO {this.ParamNameForCompositeRole};

    RETURN {this.ParamNameForCompositeRole};
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composite Role (1-1 and *-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {this.columnNameByRelationType[relationType]} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositeAssociationOne2OneObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (1-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeAssociationObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (1-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationMany2OneObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]}={this.ParamNameForCompositeRole};
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationMany2OneObjectTable(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchAssociationByRelationType.Add(relationType, name);

            // Prefetch Composite Association (*-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRelationType[relationType]}
    FROM {table}
    WHERE {this.columnNameByRelationType[relationType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void SetCompositeRole(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();

            // Set Composite Role (1-1 and *-1) [object table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = relations.{ColumnNameForRole}
    FROM relations
    WHERE {ColumnNameForObject} = relations.{ColumnNameForAssociation}
$$;";

            this.procedureNameForSetRoleByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(this.procedureNameForSetRoleByRelationType[relationType], definition);
        }

        private void ClearCompositeRole(IClass @class, IRoleType roleType)
        {
            var relationType = roleType.RelationType;
            var table = this.tableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.Name.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();

            // Clear Composite Role (1-1 and *-1) [object table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    UPDATE {table}
    SET {this.columnNameByRelationType[relationType]} = null
    FROM objects
    WHERE {table}.{ColumnNameForObject} = objects.{ColumnNameForObject}
$$;";

            this.procedureNameForClearRoleByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(this.procedureNameForClearRoleByRelationType[relationType], definition);
        }

        private void GetCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composites Role (1-* and *-*) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation};
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objectsArray = this.ObjectArrayParam;
            var objectsArrayType = objectsArray.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);

            // Prefetch Composites Role (1-* and *-*) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsArrayType});
CREATE FUNCTION {name}({objectsArray} {objectsArrayType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForRole} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objectsArray}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForAddRoleByRelationType.Add(relationType, name);

            // Add Composite Role (1-* and *-*) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    INSERT INTO {table} ({ColumnNameForAssociation},{ColumnNameForRole})
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM relations
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void RemoveCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForRemoveRoleByRelationType.Add(relationType, name);

            // Remove Composite Role (1-* and *-*) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    DELETE FROM {table}
    USING relations
    WHERE {table}.{ColumnNameForAssociation}=relations.{ColumnNameForAssociation} AND {table}.{ColumnNameForRole}=relations.{ColumnNameForRole}
$$;";

            this.ProcedureDefinitionByName.Add(this.procedureNameForRemoveRoleByRelationType[relationType], definition);
        }

        private void GetCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetRoleByRelationType.Add(relationType, name);

            // Get Composite Role (1-1 and *-1) [relation table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation}
    INTO {this.ParamNameForCompositeRole};

    RETURN {this.ParamNameForCompositeRole};
END
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant();

            // Prefetch Composite Role (1-1 and *-1) [relation table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForRole} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.procedureNameForPrefetchRoleByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(this.procedureNameForPrefetchRoleByRelationType[relationType], definition);
        }

        private void SetCompositeRoleRelationType(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + relationType.RoleType.SingularFullName.ToLowerInvariant();

            // Set Composite Role (1-1 and *-1) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    INSERT INTO {table}
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole} from relations

    ON CONFLICT ({ColumnNameForAssociation})
    DO UPDATE
        SET {ColumnNameForRole} = excluded.{ColumnNameForRole};
$$;";

            this.procedureNameForSetRoleByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(this.procedureNameForSetRoleByRelationType[relationType], definition);
        }

        private void GetCompositeAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant();

            // Get Composite Association (1-1) [relation table]
            var definition =
$@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(this.procedureNameForGetAssociationByRelationType[relationType], definition);
        }

        private void PrefetchCompositeAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.procedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (1-1) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForAssociation} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void GetCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant();
            this.procedureNameForGetAssociationByRelationType.Add(relationType, name);

            // Get Composite Association (*-1) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void PrefetchCompositesAssociationRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.procedureNameForPrefetchAssociationByRelationType[relationType];

            // Prefetch Composite Association (*-1) [relation table]
            var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE SQL
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void ClearCompositeRoleRelationTable(IRelationType relationType)
        {
            var table = this.tableNameForRelationByRelationType[relationType];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var name = this.procedureNameForClearRoleByRelationType[relationType];

            // Clear Composite Role (1-1 and *-1) [relation table]
            var definition =
                $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    DELETE FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects)
$$;";

            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void UpdateVersionIds()
        {
            // Update Version Ids
            var definition = $@"
DROP FUNCTION IF EXISTS {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    UPDATE {this.TableNameForObjects}
    SET {ColumnNameForVersion} = {ColumnNameForVersion} + 1
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM unnest({this.ObjectArrayParam}) as t({ColumnNameForObject}));
$$;";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForUpdateVersion, definition);
        }

        private void GetVersionIds()
        {
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;

            // Get Version Ids
            var definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetVersion}({objectsType});
CREATE FUNCTION {this.ProcedureNameForGetVersion}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForVersion} {SqlTypeForVersion}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.TableNameForObjects}.{ColumnNameForObject}, {this.TableNameForObjects}.{ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {this.TableNameForObjects}.{ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);
        }

        private void Instantiate()
        {
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;

            // Instantiate
            var definition = $@"
DROP FUNCTION IF EXISTS {this.ProcedureNameForInstantiate}({objectsType});
CREATE FUNCTION {this.ProcedureNameForInstantiate}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForClass} {SqlTypeForClass},
         {ColumnNameForVersion} {SqlTypeForVersion}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {ColumnNameForClass}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {this.TableNameForObjects}.{ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);
        }
    }
}

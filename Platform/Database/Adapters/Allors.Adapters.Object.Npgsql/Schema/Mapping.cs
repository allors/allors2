//------------------------------------------------------------------------------------------------- 
// <copyright file="Mapping.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    using NpgsqlTypes;

    public class Mapping
    {
        public const string ParamFormat = "p_{0}";
        public const string ParamInvocationFormat = ":p_{0}";

        public const string ColumnNameForObject = "o";
        public const string ColumnNameForClass = "c";
        public const string ColumnNameForVersion = "v";
        public const string ColumnNameForAssociation = "a";
        public const string ColumnNameForRole = "r";

        public const string SqlTypeForClass = "uuid";
        public const string SqlTypeForObject = "bigint";
        public const string SqlTypeForVersion = "bigint";
        public const string SqlTypeForCount = "integer";

        public const NpgsqlDbType NpgsqlDbTypeForClass = NpgsqlDbType.Uuid;
        public const NpgsqlDbType NpgsqlDbTypeForObject = NpgsqlDbType.Bigint;
        public const NpgsqlDbType NpgsqlDbTypeForVersion = NpgsqlDbType.Bigint;
        public const NpgsqlDbType NpgsqlDbTypeForCount = NpgsqlDbType.Integer;

        public readonly MappingArrayParameter ObjectArrayParam;
        public readonly MappingArrayParameter CompositeRelationArrayParam;
        public readonly MappingArrayParameter StringRelationArrayParam;
        public readonly MappingArrayParameter StringMaxRelationArrayParam;
        public readonly MappingArrayParameter IntegerRelationArrayParam;
        public readonly MappingArrayParameter LongRelationArrayParam;
        public readonly MappingArrayParameter DecimalRelationArrayParam;
        public readonly MappingArrayParameter DoubleRelationArrayParam;
        public readonly MappingArrayParameter BooleanRelationArrayParam;
        public readonly MappingArrayParameter DateRelationArrayParam;
        public readonly MappingArrayParameter DateTimeRelationArrayParam;
        public readonly MappingArrayParameter UniqueRelationArrayParam;
        public readonly MappingArrayParameter BinaryRelationArrayParam;

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

        internal readonly string ProcedureNameForInstantiate;

        internal readonly string ProcedureNameForGetVersion;
        internal readonly string ProcedureNameForUpdateVersion;

        internal readonly Dictionary<IClass, string> ProcedureNameForLoadObjectByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForCreateObjectByClass;
        internal readonly Dictionary<IClass, string> ProcedureNameForCreateObjectsByClass;

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

        private const string ProcedurePrefixForInstantiate = "i";

        private const string ProcedurePrefixForGetVersion = "gv";
        private const string ProcedurePrefixForSetVersion = "sv";
        private const string ProcedurePrefixForUpdateVersion = "uv";

        private const string ProcedurePrefixForCreateObject = "co_";
        private const string ProcedurePrefixForCreateObjects = "cos_";
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

        private readonly Database database;

        private readonly Dictionary<string, string> procedureDefinitionByName;

        public Mapping(Database database)
        {
            this.database = database;

            this.ObjectArrayParam = new MappingArrayParameter(database, "arr_o", NpgsqlDbType.Bigint);
            this.CompositeRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bigint);
            this.StringRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Varchar);
            this.StringMaxRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Text);
            this.IntegerRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Integer);
            this.LongRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bigint);
            this.DecimalRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Numeric);
            this.DoubleRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Double);
            this.BooleanRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Boolean);
            this.DateRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Date);
            this.DateTimeRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Timestamp);
            this.UniqueRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Uuid);
            this.BinaryRelationArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bytea);

            // Tables
            // ------
            this.TableNameForObjects = database.SchemaName + "." + "_o";
            this.TableNameForObjectByClass = new Dictionary<IClass, string>();
            this.ColumnNameByRelationType = new Dictionary<IRelationType, string>();
            this.UnescapedColumnNameByRelationType = new Dictionary<IRelationType, string>();
            this.ParamNameByRoleType = new Dictionary<IRoleType, string>();

            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                this.TableNameForObjectByClass.Add(@class, this.database.SchemaName + "." + this.NormalizeName(@class.SingularName));

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
                    this.TableNameForRelationByRelationType.Add(relationType, this.database.SchemaName + "." + this.NormalizeName(relationType.RoleType.SingularFullName));
                }
            }

            // Procedures
            // ----------
            this.procedureDefinitionByName = new Dictionary<string, string>();

            this.ProcedureNameForLoadObjectByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForCreateObjectByClass = new Dictionary<IClass, string>();
            this.ProcedureNameForCreateObjectsByClass = new Dictionary<IClass, string>();

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

            // Instantiate
            this.ProcedureNameForInstantiate = this.Database.SchemaName + "." + ProcedurePrefixForInstantiate;
            var definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForInstantiate}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForInstantiate}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS TABLE 
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForClass} {SqlTypeForClass},
         {ColumnNameForVersion} {SqlTypeForVersion}
    )
    LANGUAGE plpgsql
AS $$
    BEGIN

    RETURN QUERY
    SELECT {ColumnNameForObject}, {ColumnNameForClass}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN ( SELECT * FROM unnest({this.ObjectArrayParam}));

    END
$$;";
            this.procedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);

            // Get Version Ids
            this.ProcedureNameForGetVersion = this.Database.SchemaName + "." + ProcedurePrefixForGetVersion;
            definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetVersion}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForGetVersion}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS TABLE 
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForVersion} {SqlTypeForVersion}
    ) 
    LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT {ColumnNameForObject}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject} IN ( SELECT * FROM unnest({this.ObjectArrayParam}));
END
$$;";
            this.procedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);

            // Update Version Ids
            this.ProcedureNameForUpdateVersion = this.Database.SchemaName + "." + ProcedurePrefixForUpdateVersion;

            definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    UPDATE {this.TableNameForObjects}
    SET {ColumnNameForVersion} = {ColumnNameForVersion} + 1
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM unnest({this.ObjectArrayParam}) as t({ColumnNameForObject}));
$$;";

            this.procedureDefinitionByName.Add(this.ProcedureNameForUpdateVersion, definition);



            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                var className = @class.Name.ToLowerInvariant();
                var table = this.TableNameForObjectByClass[@class];

                // Load Objects
                this.ProcedureNameForLoadObjectByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForLoad + className);
                definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForLoadObjectByClass[@class]}({SqlTypeForClass},{this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForLoadObjectByClass[@class]}(
	{ParamNameForClass} {SqlTypeForClass},
	{this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    INSERT INTO allors.c1 (c, o)
    SELECT p_c, o
    FROM unnest(p_arr_o) AS t(o)
$$;";

                this.procedureDefinitionByName.Add(this.ProcedureNameForLoadObjectByClass[@class], definition);

                // CreateObject
                this.ProcedureNameForCreateObjectByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForCreateObject + className);
                definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForCreateObjectByClass[@class]}({SqlTypeForClass});
CREATE FUNCTION {this.ProcedureNameForCreateObjectByClass[@class]}({ParamNameForClass} {SqlTypeForClass})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
    DECLARE  {ParamNameForObject} {SqlTypeForObject};
    BEGIN

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({ParamNameForClass}, {Reference.InitialVersion})
    RETURNING {ColumnNameForObject} INTO {ParamNameForObject};

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({ParamNameForObject},{ParamNameForClass});

    RETURN {ParamNameForObject};
    END
$$;
";

                this.procedureDefinitionByName.Add(this.ProcedureNameForCreateObjectByClass[@class], definition);

                // CreateObjects
                this.ProcedureNameForCreateObjectsByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForCreateObjects + className);
                definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForCreateObjectsByClass[@class]}({SqlTypeForClass}, {SqlTypeForCount});
CREATE FUNCTION {this.ProcedureNameForCreateObjectsByClass[@class]}({ParamNameForClass} {SqlTypeForClass}, {ParamNameForCount} {SqlTypeForCount})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
    DECLARE ID integer; 
    DECLARE COUNTER integer := 0;

    BEGIN

    WHILE COUNTER < {ParamNameForCount} LOOP

        INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
        VALUES ({ParamNameForClass}, {Reference.InitialVersion} )
        RETURNING {ColumnNameForObject} INTO ID;

        INSERT INTO {this.TableNameForObjectByClass[@class.ExclusiveClass]} ({ColumnNameForObject},{ColumnNameForClass})
        VALUES (ID,{ParamNameForClass});
       
        COUNTER := COUNTER+1;

        RETURN NEXT ID;

    END LOOP;

    END
$$;";

                this.procedureDefinitionByName.Add(this.ProcedureNameForCreateObjectsByClass[@class], definition);

                var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
                if (sortedUnitRoleTypes.Length > 0)
                {
                    {
                        // Get Unit Roles
                        this.ProcedureNameForGetUnitRolesByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForGetUnits + className);
                        definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetUnitRolesByClass[@class]}({SqlTypeForObject});
CREATE FUNCTION {this.ProcedureNameForGetUnitRolesByClass[@class]}({ParamNameForObject} {SqlTypeForObject})
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
                                definition += ", ";
                            }

                            definition += this.ColumnNameByRelationType[role.RelationType] + " " + this.GetSqlType(role);
                        }

                        definition += @")
     LANGUAGE plpgsql
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
                                definition += ", ";
                            }

                            definition += this.ColumnNameByRelationType[role.RelationType];
                        }

                        definition += $@"
    FROM {this.TableNameForObjectByClass[@class.ExclusiveClass]}
    WHERE {ColumnNameForObject}={ParamNameForObject};
END
$$;";
                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetUnitRolesByClass[@class], definition);
                    }

                    {
                        // Prefetch Unit Roles
                        this.ProcedureNameForPrefetchUnitRolesByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchUnits + className);
                        definition =
$@"DROP FUNCTION IF EXISTS {this.ProcedureNameForPrefetchUnitRolesByClass[@class]}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForPrefetchUnitRolesByClass[@class]}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
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
                                definition += ", ";
                            }

                            definition += this.ColumnNameByRelationType[role.RelationType] + " " + this.GetSqlType(role);
                        }

                        definition += @")
     LANGUAGE plpgsql
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
                                definition += ", ";
                            }

                            definition += this.ColumnNameByRelationType[role.RelationType];
                        }

                        definition += $@"
    FROM {this.TableNameForObjectByClass[@class.ExclusiveClass]}
    WHERE {ColumnNameForObject} IN ( SELECT * FROM unnest({this.ObjectArrayParam}));
END
$$;";


                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchUnitRolesByClass[@class], definition);
                    }
                }

                //                foreach (var associationType in @class.AssociationTypes)
                //                {
                //                    var relationType = associationType.RelationType;
                //                    var roleType = relationType.RoleType;
                //                    var relationTypeName = roleType.SingularFullName.ToLowerInvariant();

                //                    if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && roleType.IsMany)
                //                    {
                //                        // Get Composites Role (1-*) [object table]
                //                        this.ProcedureNameForGetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                //    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + ColumnNameForObject + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + "=" + ParamNameForAssociation;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);

                //                        // Prefetch Composites Role (1-*) [object table]
                //                        this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS

                //    SELECT " + this.ColumnNameByRelationType[relationType] + ", " + ColumnNameForObject + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);

                //                        if (associationType.IsOne)
                //                        {
                //                            // Get Composite Association (1-*) [object table]
                //                            this.ProcedureNameForGetAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + className + "_" + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                //    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + this.ColumnNameByRelationType[relationType] + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForObject + "=" + ParamNameForCompositeRole;
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

                //                            // Prefetch Composite Association (1-*) [object table]
                //                            this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + className + "_" + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS
                //    SELECT " + this.ColumnNameByRelationType[relationType] + ", " + ColumnNameForObject + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForObject + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchAssociationByRelationType[relationType], definition);
                //                        }

                //                        // Add Composite Role (1-*) [object table]
                //                        this.ProcedureNameForAddRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForAddRole + className + "_" + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForAddRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForAssociation + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForRole;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForAddRoleByRelationType[relationType], definition);

                //                        // Remove Composite Role (1-*) [object table]
                //                        this.ProcedureNameForRemoveRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + className + "_" + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForRemoveRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + @" = null
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForAssociation + @" AND
                //    " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForRole;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForRemoveRoleByRelationType[relationType], definition);

                //                        // Clear Composites Role (1-*) [object table]
                //                        this.ProcedureNameForClearRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + className + "_" + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForClearRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 

                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + @" = null
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS a
                //    ON " + this.ColumnNameByRelationType[relationType] + " = a." + this.TableTypeColumnNameForObject;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
                //                    }
                //                }

                //                var procedureNameForSetUnitRoleByRelationType = new Dictionary<IRelationType, string>();
                //                this.ProcedureNameForSetUnitRoleByRelationTypeByClass.Add(@class, procedureNameForSetUnitRoleByRelationType);

                //                foreach (var roleType in @class.RoleTypes)
                //                {
                //                    var relationType = roleType.RelationType;
                //                    var associationType = relationType.AssociationType;
                //                    var relationTypeName = roleType.SingularFullName.ToLowerInvariant();

                //                    if (roleType.ObjectType.IsUnit)
                //                    {
                //                        procedureNameForSetUnitRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForSetRole + className + "_" + relationTypeName);

                //                        var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).UnitTag;
                //                        switch (unitTypeTag)
                //                        {
                //                            case UnitTags.String:
                //                                // Set String Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForStringRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Integer:
                //                                // Set Integer Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForIntegerRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Float:
                //                                // Set Double Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForFloatRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Decimal:
                //                                // Set Decimal Role
                //                                var decimalRelationTable = this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value];

                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //" + ParamNameForTableType + @" " + decimalRelationTable + @" READONLY
                //AS 
                //UPDATE " + table + @"
                //SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //FROM " + table + @"
                //INNER JOIN " + ParamNameForTableType + @" AS r
                //ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Boolean:
                //                                // Set Boolean Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForBooleanRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.DateTime:
                //                                // Set DateTime Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForDateTimeRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Unique:
                //                                // Set Unique Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForUniqueRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            case UnitTags.Binary:
                //                                // Set Binary Role
                //                                definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForBinaryRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                                break;

                //                            default:
                //                                throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName);
                //                        }

                //                        this.procedureDefinitionByName.Add(procedureNameForSetUnitRoleByRelationType[relationType], definition);
                //                    }
                //                    else
                //                    {
                //                        if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && roleType.IsOne)
                //                        {
                //                            // Get Composite Role (1-1 and *-1) [object table]
                //                            this.ProcedureNameForGetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                //    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                //AS 
                //    SELECT " + this.ColumnNameByRelationType[relationType] + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForObject + "=" + ParamNameForAssociation;
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);

                //                            // Prefetch Composite Role (1-1 and *-1) [object table]
                //                            this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 
                //    SELECT  " + ColumnNameForObject + ", " + this.ColumnNameByRelationType[relationType] + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForObject + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);

                //                            if (associationType.IsOne)
                //                            {
                //                                // Get Composite Association (1-1) [object table]
                //                                this.ProcedureNameForGetAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + className + "_" + relationTypeName);
                //                                definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                //    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                //AS 
                //    SELECT " + ColumnNameForObject + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + "=" + ParamNameForCompositeRole;
                //                                this.procedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

                //                                // Prefetch Composite Association (1-1) [object table]
                //                                this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + className + "_" + relationTypeName);
                //                                definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 
                //    SELECT " + ColumnNameForObject + ", " + this.ColumnNameByRelationType[relationType] + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                                this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchAssociationByRelationType[relationType], definition);
                //                            }
                //                            else
                //                            {
                //                                // Get Composite Association (*-1) [object table]
                //                                this.ProcedureNameForGetAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + className + "_" + relationTypeName);
                //                                definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                //    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                //AS 
                //    SELECT " + ColumnNameForObject + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + "=" + ParamNameForCompositeRole;
                //                                this.procedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

                //                                // Prefetch Composite Association (*-1) [object table]
                //                                this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + className + "_" + relationTypeName);
                //                                definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 
                //    SELECT " + ColumnNameForObject + ", " + this.ColumnNameByRelationType[relationType] + @"
                //    FROM " + table + @"
                //    WHERE " + this.ColumnNameByRelationType[relationType] + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                                this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchAssociationByRelationType[relationType], definition);
                //                            }

                //                            // Set Composite Role (1-1 and *-1) [object table]
                //                            this.ProcedureNameForSetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForSetRole + className + "_" + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForSetRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS r
                //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
                //";
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForSetRoleByRelationType[relationType], definition);

                //                            // Clear Composite Role (1-1 and *-1) [object table]
                //                            this.ProcedureNameForClearRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + className + "_" + relationTypeName);
                //                            definition = @"CREATE PROCEDURE " + this.ProcedureNameForClearRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 
                //    UPDATE " + table + @"
                //    SET " + this.ColumnNameByRelationType[relationType] + @" = null
                //    FROM " + table + @"
                //    INNER JOIN " + ParamNameForTableType + @" AS a
                //    ON " + ColumnNameForObject + " = a." + this.TableTypeColumnNameForObject;
                //                            this.procedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
                //                        }
                //                    }
                //                }
                //            }

                //            foreach (var relationType in this.Database.MetaPopulation.RelationTypes)
                //            {
                //                var associationType = relationType.AssociationType;
                //                var roleType = relationType.RoleType;
                //                var relationTypeName = roleType.SingularFullName.ToLowerInvariant();

                //                if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                //                {
                //                    var table = this.TableNameForRelationByRelationType[relationType];

                //                    this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + relationTypeName);
                //                    this.ProcedureNameForClearRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + relationTypeName);

                //                    if (roleType.IsMany)
                //                    {
                //                        // Get Composites Role (1-* and *-*) [relation table]
                //                        this.ProcedureNameForGetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                //    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForAssociation + "=" + ParamNameForAssociation;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);

                //                        // Prefetch Composites Role (1-* and *-*) [relation table]
                //                        this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS
                //    SELECT " + ColumnNameForAssociation + ", " + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForAssociation + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);

                //                        // Add Composite Role (1-* and *-*) [relation table]
                //                        this.ProcedureNameForAddRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForAddRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForAddRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS
                //    INSERT INTO " + table + " (" + ColumnNameForAssociation + "," + ColumnNameForRole + @")
                //    SELECT " + this.TableTypeColumnNameForAssociation + @", " + this.TableTypeColumnNameForRole + @"
                //    FROM " + ParamNameForTableType + "\n";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForAddRoleByRelationType[relationType], definition);

                //                        // Remove Composite Role (1-* and *-*) [relation table]
                //                        this.ProcedureNameForRemoveRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForRemoveRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS
                //    DELETE T 
                //    FROM " + table + @" T
                //    INNER JOIN " + ParamNameForTableType + @" R
                //    ON T." + ColumnNameForAssociation + " = R." + this.TableTypeColumnNameForAssociation + @"
                //    AND T." + ColumnNameForRole + " = R." + this.TableTypeColumnNameForRole + @";";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForRemoveRoleByRelationType[relationType], definition);
                //                    }
                //                    else
                //                    {
                //                        // Get Composite Role (1-1 and *-1) [relation table]
                //                        this.ProcedureNameForGetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                //    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForAssociation + "=" + ParamNameForAssociation;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);

                //                        // Prefetch Composite Role (1-1 and *-1) [relation table]
                //                        this.ProcedureNameForPrefetchRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS
                //    SELECT " + ColumnNameForAssociation + ", " + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForAssociation + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);

                //                        // Set Composite Role (1-1 and *-1) [relation table]
                //                        this.ProcedureNameForSetRoleByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForSetRole + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForSetRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
                //AS
                //    MERGE " + table + @" T
                //    USING " + ParamNameForTableType + @" AS r
                //    ON T." + ColumnNameForAssociation + @" = r." + this.TableTypeColumnNameForAssociation + @"

                //    WHEN MATCHED THEN
                //    UPDATE SET " + ColumnNameForRole + @"= r." + this.TableTypeColumnNameForRole + @"

                //    WHEN NOT MATCHED THEN
                //    INSERT (" + ColumnNameForAssociation + "," + ColumnNameForRole + @")
                //    VALUES (r." + this.TableTypeColumnNameForAssociation + ", r." + this.TableTypeColumnNameForRole + @");";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForSetRoleByRelationType[relationType], definition);
                //                    }

                //                    if (associationType.IsOne)
                //                    {
                //                        // Get Composite Association (1-1) [relation table]
                //                        this.ProcedureNameForGetAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                //    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + ColumnNameForAssociation + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForRole + "=" + ParamNameForCompositeRole;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

                //                        // Prefetch Composite Association (1-1) [relation table]
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS
                //    SELECT " + ColumnNameForAssociation + "," + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForRole + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchAssociationByRelationType[relationType], definition);
                //                    }
                //                    else
                //                    {
                //                        // Get Composite Association (*-1) [relation table]
                //                        this.ProcedureNameForGetAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationTypeName);
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                //    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                //AS
                //    SELECT " + ColumnNameForAssociation + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForRole + "=" + ParamNameForCompositeRole;
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

                //                        // Prefetch Composite Association (*-1) [relation table]
                //                        definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
                //   " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS
                //    SELECT " + ColumnNameForAssociation + "," + ColumnNameForRole + @"
                //    FROM " + table + @"
                //    WHERE " + ColumnNameForRole + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
                //                        this.procedureDefinitionByName.Add(this.ProcedureNameForPrefetchAssociationByRelationType[relationType], definition);
                //                    }

                //                    // Clear Composite Role (1-1 and *-1) [relation table]
                //                    definition = @"CREATE PROCEDURE " + this.ProcedureNameForClearRoleByRelationType[relationType] + @"
                //    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
                //AS 
                //    DELETE T 
                //    FROM " + table + @" T
                //    INNER JOIN " + ParamNameForTableType + @" A
                //    ON T." + ColumnNameForAssociation + " = A." + this.TableTypeColumnNameForObject;
                //                    this.procedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
                //                }
            }
        }

        public Dictionary<string, string> ProcedureDefinitionByName => this.procedureDefinitionByName;

        protected internal Database Database => this.database;

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
            switch (unit.UnitTag)
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
            switch (unit.UnitTag)
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
    }
}
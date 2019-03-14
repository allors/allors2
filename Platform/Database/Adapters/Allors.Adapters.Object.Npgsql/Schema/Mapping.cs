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
        public readonly MappingArrayParameter CompositeRoleArrayParam;
        public readonly MappingArrayParameter StringRoleArrayParam;
        public readonly MappingArrayParameter StringMaxRoleArrayParam;
        public readonly MappingArrayParameter IntegerRoleArrayParam;
        public readonly MappingArrayParameter LongRoleArrayParam;
        public readonly MappingArrayParameter DecimalRoleArrayParam;
        public readonly MappingArrayParameter DoubleRoleArrayParam;
        public readonly MappingArrayParameter BooleanRoleArrayParam;
        public readonly MappingArrayParameter DateRoleArrayParam;
        public readonly MappingArrayParameter DateTimeRoleArrayParam;
        public readonly MappingArrayParameter UniqueRoleArrayParam;
        public readonly MappingArrayParameter BinaryRoleArrayParam;

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

        internal string ProcedureNameForInstantiate;

        internal string ProcedureNameForGetVersion;
        internal string ProcedureNameForUpdateVersion;

        internal Dictionary<string, string> ProcedureDefinitionByName { get; }

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

        public Mapping(Database database)
        {
            this.database = database;

            this.ObjectArrayParam = new MappingArrayParameter(database, "arr_o", NpgsqlDbType.Bigint);
            this.CompositeRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bigint);
            this.StringRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Varchar);
            this.StringMaxRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Text);
            this.IntegerRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Integer);
            this.LongRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bigint);
            this.DecimalRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Numeric);
            this.DoubleRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Double);
            this.BooleanRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Boolean);
            this.DateRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Date);
            this.DateTimeRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Timestamp);
            this.UniqueRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Uuid);
            this.BinaryRoleArrayParam = new MappingArrayParameter(database, "arr_r", NpgsqlDbType.Bytea);

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
            this.ProcedureDefinitionByName = new Dictionary<string, string>();

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

            this.Instantiate();
            this.GetVersionIds();
            this.UpdateVersionIds();

            foreach (var @class in this.Database.MetaPopulation.Classes)
            {
                this.LoadObjects(@class);
                this.CreateObject(@class);
                this.CreateObjects(@class);

                if (this.Database.GetSortedUnitRolesByObjectType(@class).Length > 0)
                {
                    this.GetUnitRoles(@class);
                    this.PrefetchUnitRoles(@class);
                }

                foreach (var associationType in @class.AssociationTypes)
                {
                    if (!(associationType.IsMany && associationType.RelationType.RoleType.IsMany) && associationType.RelationType.ExistExclusiveClasses && associationType.RelationType.RoleType.IsMany)
                    {
                        this.GetCompositesRoleObjectTable(@class, associationType);
                        this.PrefetchCompositesRoleObjectTable(@class, associationType);

                        if (associationType.IsOne)
                        {
                            this.GetCompositesAssociationObjectTable(@class, associationType);
                            this.PrefetchCompositeAssociationObjectTable(@class, associationType);
                        }

                        this.AddCompositeRoleObjectTable(@class, associationType);
                        this.RemoveCompositeRoleObjectTable(@class, associationType);
                        this.ClearCompositeRoleObjectTable(@class, associationType);
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
                        if (!(roleType.RelationType.AssociationType.IsMany && roleType.IsMany) && roleType.RelationType.ExistExclusiveClasses && roleType.IsOne)
                        {
                            this.GetCompositeRoleObjectTable(@class, roleType);
                            this.PrefetchCompositeRoleObjectTable(@class, roleType);

                            if (roleType.RelationType.AssociationType.IsOne)
                            {
                                this.GetCompositeAssociationOne2OneObjectTable(@class, roleType);
                                this.PrefetchCompositeAssociatoinObjectTable(@class, roleType);
                            }
                            else
                            {
                                this.GetCompositesAssociationMany2OneObjectTable(@class, roleType);
                            }

                            this.SetCompositeRole(@class, roleType);
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

        private void PrefetchCompositeAssociatoinObjectTable(IClass class1, IRoleType type)
        {
            //var relationType = type.RelationType;

            //// Prefetch Composite Association (1-1) [object table]
            //this.ProcedureNameForPrefetchAssociationByRelationType.Add(relationType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + class1.Name.ToLowerInvariant() + "_" + type.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
            //                    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //                AS 
            //                    SELECT " + ColumnNameForObject + ", " + this.ColumnNameByRelationType[relationType]
            //                 + @"
            //                    FROM " + this.TableNameForObjectByClass[class1] + @"
            //                    WHERE " + this.ColumnNameByRelationType[relationType] + " IN (SELECT "
            //                 + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(
            //    this.ProcedureNameForPrefetchAssociationByRelationType[relationType],
            //    definition);
        }

        private void ClearCompositeRoleRelationTable(IRelationType relationType)
        {
            //// Clear Composite Role (1-1 and *-1) [relation table]
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForClearRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //    AS 
            //        DELETE T 
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @" T
            //        INNER JOIN " + ParamNameForTableType + @" A
            //        ON T." + ColumnNameForAssociation + " = A." + this.TableTypeColumnNameForObject;
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
        }

        private void PrefetchCompositesAssociationRelationTable(IRelationType relationType)
        {
            //// Prefetch Composite Association (*-1) [relation table]
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
            //       " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //    AS
            //        SELECT " + ColumnNameForAssociation + "," + ColumnNameForRole + @"
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @"
            //        WHERE " + ColumnNameForRole + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM "
            //                 + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(
            //    this.ProcedureNameForPrefetchAssociationByRelationType[relationType],
            //    definition);
        }

        private void GetCompositesAssociationRelationTable(IRelationType relationType)
        {
            string definition;
            // Get Composite Association (*-1) [relation table]
            this.ProcedureNameForGetAssociationByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant());
            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                AS
                    SELECT " + ColumnNameForAssociation + @"
                    FROM " + this.TableNameForRelationByRelationType[relationType] + @"
                    WHERE " + ColumnNameForRole + "=" + ParamNameForCompositeRole;
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);
        }

        private void PrefetchCompositeAssociationRelationTable(IRelationType relationType)
        {
            //// Prefetch Composite Association (1-1) [relation table]
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //    AS
            //        SELECT " + ColumnNameForAssociation + "," + ColumnNameForRole + @"
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @"
            //        WHERE " + ColumnNameForRole + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM "
            //                 + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(
            //    this.ProcedureNameForPrefetchAssociationByRelationType[relationType],
            //    definition);
        }

        private void GetCompositeAssociationRelationTable(IRelationType relationType)
        {
            string definition;
            // Get Composite Association (1-1) [relation table]
            this.ProcedureNameForGetAssociationByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + relationType.RoleType.SingularFullName.ToLowerInvariant());
            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                AS
                    SELECT " + ColumnNameForAssociation + @"
                    FROM " + this.TableNameForRelationByRelationType[relationType] + @"
                    WHERE " + ColumnNameForRole + "=" + ParamNameForCompositeRole;
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);
        }

        private void SetCompositeRoleRelationType(IRelationType relationType)
        {
            //// Set Composite Role (1-1 and *-1) [relation table]
            //this.ProcedureNameForSetRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForSetRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForSetRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
            //    AS
            //        MERGE " + this.TableNameForRelationByRelationType[relationType] + @" T
            //        USING " + ParamNameForTableType + @" AS r
            //        ON T." + ColumnNameForAssociation + @" = r." + this.TableTypeColumnNameForAssociation + @"

            //        WHEN MATCHED THEN
            //        UPDATE SET " + ColumnNameForRole + @"= r." + this.TableTypeColumnNameForRole + @"

            //        WHEN NOT MATCHED THEN
            //        INSERT (" + ColumnNameForAssociation + "," + ColumnNameForRole + @")
            //        VALUES (r." + this.TableTypeColumnNameForAssociation + ", r." + this.TableTypeColumnNameForRole
            //                 + @");";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForSetRoleByRelationType[relationType], definition);
        }

        private void PrefetchCompositeRoleRelationType(IRelationType relationType)
        {
            //// Prefetch Composite Role (1-1 and *-1) [relation table]
            //this.ProcedureNameForPrefetchRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //    AS
            //        SELECT " + ColumnNameForAssociation + ", " + ColumnNameForRole + @"
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @"
            //        WHERE " + ColumnNameForAssociation + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM "
            //                 + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);
        }

        private void GetCompositeRoleRelationTable(IRelationType relationType)
        {
            string definition;
            // Get Composite Role (1-1 and *-1) [relation table]
            this.ProcedureNameForGetRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                AS
                    SELECT " + ColumnNameForRole + @"
                    FROM " + this.TableNameForRelationByRelationType[relationType] + @"
                    WHERE " + ColumnNameForAssociation + "=" + ParamNameForAssociation;
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);
        }

        private void RemoveCompositeRoleRelationTable(IRelationType relationType)
        {
            //// Remove Composite Role (1-* and *-*) [relation table]
            //this.ProcedureNameForRemoveRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForRemoveRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
            //    AS
            //        DELETE T 
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @" T
            //        INNER JOIN " + ParamNameForTableType + @" R
            //        ON T." + ColumnNameForAssociation + " = R." + this.TableTypeColumnNameForAssociation + @"
            //        AND T." + ColumnNameForRole + " = R." + this.TableTypeColumnNameForRole + @";";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForRemoveRoleByRelationType[relationType], definition);
        }

        private void AddCompositeRoleRelationTable(IRelationType relationType)
        {
            //// Add Composite Role (1-* and *-*) [relation table]
            //this.ProcedureNameForAddRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForAddRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForAddRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
            //    AS
            //        INSERT INTO " + this.TableNameForRelationByRelationType[relationType] + " (" + ColumnNameForAssociation + "," + ColumnNameForRole + @")
            //        SELECT " + this.TableTypeColumnNameForAssociation + @", " + this.TableTypeColumnNameForRole + @"
            //        FROM " + ParamNameForTableType + "\n";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForAddRoleByRelationType[relationType], definition);
        }

        private void PrefetchCompositesRoleRelationTable(IRelationType relationType)
        {
            //// Prefetch Composites Role (1-* and *-*) [relation table]
            //this.ProcedureNameForPrefetchRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //    AS
            //        SELECT " + ColumnNameForAssociation + ", " + ColumnNameForRole + @"
            //        FROM " + this.TableNameForRelationByRelationType[relationType] + @"
            //        WHERE " + ColumnNameForAssociation + " IN (SELECT " + this.TableTypeColumnNameForObject + @" FROM "
            //                 + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);
        }

        private void GetCompositesRoleRelationTable(IRelationType relationType)
        {
            string definition;
            // Get Composites Role (1-* and *-*) [relation table]
            this.ProcedureNameForGetRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetRoleByRelationType[relationType] + @"
                    " + ParamNameForAssociation + @" " + SqlTypeForObject + @"
                AS
                    SELECT " + ColumnNameForRole + @"
                    FROM " + this.TableNameForRelationByRelationType[relationType] + @"
                    WHERE " + ColumnNameForAssociation + "=" + ParamNameForAssociation;
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);
        }

        private void SetCompositeRole(IClass class1, IRoleType type)
        {
            //IRelationType relationType = type.RelationType;
            //string table = this.TableNameForObjectByClass[class1];
            //string relationTypeName = type.SingularFullName.ToLowerInvariant();
            //string className = class1.Name.ToLowerInvariant();
            //// Set Composite Role (1-1 and *-1) [object table]
            //this.ProcedureNameForSetRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForSetRole + className + "_" + relationTypeName);
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForSetRoleByRelationType[relationType] + @"
            //                    " + ParamNameForTableType + @" " + this.TableTypeNameForCompositeRelation + @" READONLY
            //                AS 
            //                    UPDATE " + table + @"
            //                    SET " + this.ColumnNameByRelationType[relationType] + " = r."
            //                 + this.TableTypeColumnNameForRole + @"
            //                    FROM " + table + @"
            //                    INNER JOIN " + ParamNameForTableType + @" AS r
            //                    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //                ";
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForSetRoleByRelationType[relationType], definition);

            //// Clear Composite Role (1-1 and *-1) [object table]
            //this.ProcedureNameForClearRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForClearRole + className + "_" + relationTypeName);
            //definition = @"CREATE PROCEDURE " + this.ProcedureNameForClearRoleByRelationType[relationType] + @"
            //                    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //                AS 
            //                    UPDATE " + table + @"
            //                    SET " + this.ColumnNameByRelationType[relationType] + @" = null
            //                    FROM " + table + @"
            //                    INNER JOIN " + ParamNameForTableType + @" AS a
            //                    ON " + ColumnNameForObject + " = a." + this.TableTypeColumnNameForObject;
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
        }

        private void GetCompositesAssociationMany2OneObjectTable(IClass class1, IRoleType type)
        {
            //IRelationType relationType = type.RelationType;
            //string table = this.TableNameForObjectByClass[class1];
            //string relationTypeName = type.SingularFullName.ToLowerInvariant();
            //string className = class1.Name.ToLowerInvariant();
            //// Get Composite Association (*-1) [object table]
            //this.ProcedureNameForGetAssociationByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + className + "_" + relationTypeName);
            //var definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
            //                    " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
            //                AS 
            //                    SELECT " + ColumnNameForObject + @"
            //                    FROM " + table + @"
            //                    WHERE " + this.ColumnNameByRelationType[relationType] + "="
            //                 + ParamNameForCompositeRole;
            //this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);

            //// Prefetch Composite Association (*-1) [object table]
            //this.ProcedureNameForPrefetchAssociationByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + className + "_" + relationTypeName);
            //definition = @"CREATE PROCEDURE " + this.ProcedureNameForPrefetchAssociationByRelationType[relationType] + @"
            //                    " + ParamNameForTableType + @" " + this.TableTypeNameForObject + @" READONLY
            //                AS 
            //                    SELECT " + ColumnNameForObject + ", " + this.ColumnNameByRelationType[relationType]
            //             + @"
            //                    FROM " + table + @"
            //                    WHERE " + this.ColumnNameByRelationType[relationType] + " IN (SELECT "
            //             + this.TableTypeColumnNameForObject + @" FROM " + ParamNameForTableType + ")";
            //this.ProcedureDefinitionByName.Add(
            //    this.ProcedureNameForPrefetchAssociationByRelationType[relationType],
            //    definition);
        }

        private void GetCompositeAssociationOne2OneObjectTable(IClass class1, IRoleType type)
        {
            IRelationType relationType = type.RelationType;
            // Get Composite Association (1-1) [object table]
            this.ProcedureNameForGetAssociationByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + class1.Name.ToLowerInvariant() + "_" + type.SingularFullName.ToLowerInvariant());
            var definition = @"CREATE PROCEDURE " + this.ProcedureNameForGetAssociationByRelationType[relationType] + @"
                                " + ParamNameForCompositeRole + @" " + SqlTypeForObject + @"
                            AS 
                                SELECT " + ColumnNameForObject + @"
                                FROM " + this.TableNameForObjectByClass[class1] + @"
                                WHERE " + this.ColumnNameByRelationType[relationType] + "="
                             + ParamNameForCompositeRole;
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);
        }

        private void PrefetchCompositeRoleObjectTable(IClass key, IRoleType type)
        {
            IRelationType relationType = type.RelationType;
            // Prefetch Composite Role (1-1 and *-1) [object table]
            this.ProcedureNameForPrefetchRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + type.SingularFullName.ToLowerInvariant());
            var definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForPrefetchRoleByRelationType[relationType]}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForPrefetchRoleByRelationType[relationType]}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS TABLE 
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {this.ColumnNameByRelationType[relationType]} {SqlTypeForObject}
    )
    LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT {ColumnNameForObject}, {this.ColumnNameByRelationType[relationType]}
    FROM {this.TableNameForObjectByClass[key]}
    WHERE {ColumnNameForObject} IN ( SELECT * FROM unnest({this.ObjectArrayParam}))
END
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);
        }

        private void GetCompositeRoleObjectTable(IClass key, IRoleType type)
        {
            IRelationType relationType = type.RelationType;
            string definition;
            // Get Composite Role (1-1 and *-1) [object table]
            this.ProcedureNameForGetRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetRole + type.SingularFullName.ToLowerInvariant());
            definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetRoleByRelationType[relationType]}({SqlTypeForObject});
CREATE FUNCTION {this.ProcedureNameForGetRoleByRelationType[relationType]}({ParamNameForAssociation} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}
    FROM {this.TableNameForObjectByClass[key]}
    WHERE {ColumnNameForObject}={ParamNameForAssociation}
    INTO {ParamNameForCompositeRole};

    RETURN {ParamNameForCompositeRole};
END
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);
        }

        private void SetUnitRoleType(IRoleType roleType, IClass class1)
        {
            //IRelationType relationType = roleType.RelationType;
            //string table = this.TableNameForObjectByClass[class1];
            //var procedureNameForSetUnitRoleByRelationType = new Dictionary<IRelationType, string>();
            //this.ProcedureNameForSetUnitRoleByRelationTypeByClass.Add(class1, procedureNameForSetUnitRoleByRelationType);

            //string definition;
            //procedureNameForSetUnitRoleByRelationType.Add(
            //    relationType,
            //    this.Database.SchemaName + "." + ProcedurePrefixForSetRole + className + "_" + roleType.SingularFullName.ToLowerInvariant());

            //var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).UnitTag;
            //switch (unitTypeTag)
            //{
            //    case UnitTags.String:
            //        // Set String Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForStringRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Integer:
            //        // Set Integer Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForIntegerRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Float:
            //        // Set Double Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForFloatRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Decimal:
            //        // Set Decimal Role
            //        var decimalRelationTable =
            //            this.TableTypeNameForDecimalRelationByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value];

            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //    " + ParamNameForTableType + @" " + decimalRelationTable + @" READONLY
            //    AS 
            //    UPDATE " + table + @"
            //    SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole + @"
            //    FROM " + table + @"
            //    INNER JOIN " + ParamNameForTableType + @" AS r
            //    ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Boolean:
            //        // Set Boolean Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForBooleanRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.DateTime:
            //        // Set DateTime Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForDateTimeRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Unique:
            //        // Set Unique Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForUniqueRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    case UnitTags.Binary:
            //        // Set Binary Role
            //        definition = "CREATE PROCEDURE " + procedureNameForSetUnitRoleByRelationType[relationType] + @"
            //        " + ParamNameForTableType + @" " + this.TableTypeNameForBinaryRelation + @" READONLY
            //    AS 
            //        UPDATE " + table + @"
            //        SET " + this.ColumnNameByRelationType[relationType] + " = r." + this.TableTypeColumnNameForRole
            //                     + @"
            //        FROM " + table + @"
            //        INNER JOIN " + ParamNameForTableType + @" AS r
            //        ON " + ColumnNameForObject + " = r." + this.TableTypeColumnNameForAssociation + @"
            //    ";
            //        break;

            //    default:
            //        throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName);
            //}

            //this.ProcedureDefinitionByName.Add(procedureNameForSetUnitRoleByRelationType[relationType], definition);
        }

        private void ClearCompositeRoleObjectTable(IClass class1, IAssociationType type)
        {
            IRelationType relationType = type.RelationType;
            string table = this.TableNameForObjectByClass[class1];
            string definition;
            // Clear Composites Role (1-*) [object table]
            this.ProcedureNameForClearRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForClearRole + class1.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant());
            definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForClearRoleByRelationType[relationType]}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForClearRoleByRelationType[relationType]}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$

    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = null
    FROM {table}
    INNER JOIN {ParamNameForTableType} AS a
    ON {this.ColumnNameByRelationType[relationType]} = a.{ColumnNameForObject}

$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForClearRoleByRelationType[relationType], definition);
        }

        private void RemoveCompositeRoleObjectTable(IClass @class, IAssociationType associationType)
        {
            var relationType = associationType.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();

            // Remove Composite Role (1-*) [object table]

            var definition =
$@"DROP FUNCTION IF EXISTS {name} ({objectsType}, {rolesType});
CREATE FUNCTION {name} ({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql

AS $$

    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = null
    FROM relations
    WHERE {this.ColumnNameByRelationType[relationType]} = relations.{ColumnNameForAssociation} AND {ColumnNameForObject} = relations.{ColumnNameForRole}

$$;";
            this.ProcedureNameForRemoveRoleByRelationType.Add(relationType, name);
            this.ProcedureDefinitionByName.Add(name, definition);
        }

        private void AddCompositeRoleObjectTable(IClass @class, IAssociationType type)
        {
            var relationType = type.RelationType;
            var table = this.TableNameForObjectByClass[@class];
            var objects = this.ObjectArrayParam;
            var objectsType = objects.TypeName;
            var roles = this.CompositeRoleArrayParam;
            var rolesType = roles.TypeName;
            var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + @class.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant();

            // Add Composite Role (1-*) [object table]
            var definition = 
$@"DROP FUNCTION IF EXISTS {name} ({objectsType}, {rolesType});
CREATE FUNCTION {name} ({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$

    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.ColumnNameByRelationType[relationType]} = relations.{ColumnNameForAssociation}
    FROM relations
    WHERE {ColumnNameForObject} = relations.{ColumnNameForRole}

$$;";
            this.ProcedureDefinitionByName.Add(name, definition);
            this.ProcedureNameForAddRoleByRelationType.Add(relationType, name);
        }

        private void PrefetchCompositeAssociationObjectTable(IClass class1, IAssociationType type)
        {
            IRelationType relationType = type.RelationType;
            // Prefetch Composite Association (1-*) [object table]
            this.ProcedureNameForPrefetchAssociationByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + class1.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant());
            var definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForPrefetchAssociationByRelationType[relationType]}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForPrefetchAssociationByRelationType[relationType]}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS TABLE 
    (
         {this.ColumnNameByRelationType[relationType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE plpgsql
AS $$
DECLARE {ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    RETURN QUERY
    SELECT {this.ColumnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {this.TableNameForObjectByClass[class1]}
    WHERE {ColumnNameForObject} IN ( SELECT * FROM unnest({this.ObjectArrayParam}));
END
$$;";
            this.ProcedureDefinitionByName.Add(
                this.ProcedureNameForPrefetchAssociationByRelationType[relationType],
                definition);
        }

        private void GetCompositesAssociationObjectTable(IClass class1, IAssociationType type)
        {
            IRelationType relationType = type.RelationType;
            // Get Composite Association (1-*) [object table]
            this.ProcedureNameForGetAssociationByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + class1.Name.ToLowerInvariant() + "_" + relationType.RoleType.SingularFullName.ToLowerInvariant());
            var definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetAssociationByRelationType[relationType]}({SqlTypeForObject});
CREATE FUNCTION {this.ProcedureNameForGetAssociationByRelationType[relationType]}({ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {this.ColumnNameByRelationType[relationType]}
    FROM {this.TableNameForObjectByClass[class1]}
    WHERE {ColumnNameForObject}={ParamNameForCompositeRole}
    INTO {ParamNameForAssociation};

    RETURN {ParamNameForAssociation};
END
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetAssociationByRelationType[relationType], definition);
        }

        private void PrefetchCompositesRoleObjectTable(IClass key, IAssociationType type)
        {
            IRelationType relationType = type.RelationType;

            // Prefetch Composites Role (1-*) [object table]
            this.ProcedureNameForPrefetchRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            var definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForPrefetchRoleByRelationType[relationType]}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForPrefetchRoleByRelationType[relationType]}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS TABLE 
    (
         {this.ColumnNameByRelationType[relationType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT {this.ColumnNameByRelationType[relationType]}, {ColumnNameForObject}
    FROM {this.TableNameForObjectByClass[key]}
    WHERE {this.ColumnNameByRelationType[relationType]} IN ( SELECT * FROM unnest({this.ObjectArrayParam}));
END
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForPrefetchRoleByRelationType[relationType], definition);
        }

        private void GetCompositesRoleObjectTable(IClass key, IAssociationType type)
        {
            IRelationType relationType = type.RelationType;

            // Get Composites Role (1-*) [object table]
            this.ProcedureNameForGetRoleByRelationType.Add(
                relationType,
                this.Database.SchemaName + "." + ProcedurePrefixForGetRole + relationType.RoleType.SingularFullName.ToLowerInvariant());
            var definition =
                $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetRoleByRelationType[relationType]}({SqlTypeForObject});
CREATE FUNCTION {this.ProcedureNameForGetRoleByRelationType[relationType]}({ParamNameForAssociation} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    RETURN QUERY
    SELECT {ColumnNameForObject}
    FROM {this.TableNameForObjectByClass[key]}
    WHERE {this.ColumnNameByRelationType[relationType]}={ParamNameForAssociation};
END
$$;";
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetRoleByRelationType[relationType], definition);
        }

        private void PrefetchUnitRoles(IClass @class)
        {
            IRoleType[] sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);

            this.ProcedureNameForPrefetchUnitRolesByClass.Add(
                @class,
                this.Database.SchemaName + "." + ProcedurePrefixForPrefetchUnits + @class.Name.ToLowerInvariant());
            var definition =
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

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForPrefetchUnitRolesByClass[@class], definition);
        }

        private void GetUnitRoles(IClass @class)
        {
            IRoleType[] sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);

            this.ProcedureNameForGetUnitRolesByClass.Add(
                @class,
                this.Database.SchemaName + "." + ProcedurePrefixForGetUnits + @class.Name.ToLowerInvariant());
            var definition = $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetUnitRolesByClass[@class]}({SqlTypeForObject});
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
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetUnitRolesByClass[@class], definition);
        }

        private void CreateObjects(IClass @class)
        {
            this.ProcedureNameForCreateObjectsByClass.Add(@class, this.Database.SchemaName + "." + ProcedurePrefixForCreateObjects + @class.Name.ToLowerInvariant());
            var definition = $@"
DROP FUNCTION IF EXISTS {this.ProcedureNameForCreateObjectsByClass[@class]}({SqlTypeForClass}, {SqlTypeForCount});
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

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForCreateObjectsByClass[@class], definition);
        }

        private void CreateObject(IClass @class)
        {
            this.ProcedureNameForCreateObjectByClass.Add(
                @class,
                this.Database.SchemaName + "." + ProcedurePrefixForCreateObject + @class.Name.ToLowerInvariant());
            var definition = $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForCreateObjectByClass[@class]}({SqlTypeForClass});
CREATE FUNCTION {this.ProcedureNameForCreateObjectByClass[@class]}({ParamNameForClass} {SqlTypeForClass})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
    DECLARE  {ParamNameForObject} {SqlTypeForObject};
    BEGIN

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({ParamNameForClass}, {Reference.InitialVersion})
    RETURNING {ColumnNameForObject} INTO {ParamNameForObject};

    INSERT INTO {this.TableNameForObjectByClass[@class]} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({ParamNameForObject},{ParamNameForClass});

    RETURN {ParamNameForObject};
    END
$$;
";

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForCreateObjectByClass[@class], definition);
        }

        private void LoadObjects(IClass @class)
        {
            this.ProcedureNameForLoadObjectByClass.Add(
                @class,
                this.Database.SchemaName + "." + ProcedurePrefixForLoad + @class.Name.ToLowerInvariant());
            var definition =
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

            this.ProcedureDefinitionByName.Add(this.ProcedureNameForLoadObjectByClass[@class], definition);
        }

        private void UpdateVersionIds()
        {
            this.ProcedureNameForUpdateVersion = this.Database.SchemaName + "." + ProcedurePrefixForUpdateVersion;

            var definition = $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam.TypeName});
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
            this.ProcedureNameForGetVersion = this.Database.SchemaName + "." + ProcedurePrefixForGetVersion;
            var definition = $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetVersion}({this.ObjectArrayParam.TypeName});
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
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);
        }

        private void Instantiate()
        {
            this.ProcedureNameForInstantiate = this.Database.SchemaName + "." + ProcedurePrefixForInstantiate;
            var definition = $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForInstantiate}({this.ObjectArrayParam.TypeName});
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
            this.ProcedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);
        }
        
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
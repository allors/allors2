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

    using Allors.Meta;

    public abstract class Schema : IEnumerable<SchemaTable>
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

        public readonly string ParamInvocationFormat;
        public readonly string ParamFormat;

        private readonly Database database;

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

        protected Schema(Database database, string paramInvocationFormat, string paramFormat, string prefix, string postfix)
        {
            this.database = database;
            this.ParamInvocationFormat = paramInvocationFormat;
            this.ParamFormat = paramFormat;
            this.prefix = prefix;
            this.postfix = postfix;
        }

        public abstract SchemaValidationErrors SchemaValidationErrors { get; }

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

        public abstract IEnumerable<SchemaProcedure> Procedures { get; }

        /// <summary>
        /// Gets the type used to store object (ids) .
        /// </summary>
        protected abstract DbType ObjectDbType { get; }
        
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

        public abstract SchemaParameter CreateParameter(string name, DbType dbType);

        protected virtual void OnConstructed()
        {
            this.countParam = this.CreateParameter("COUNT", DbType.Int32);
            this.matchRoleParam = this.CreateParameter("MR", DbType.Guid);

            this.typeDbType = DbType.Guid;
            this.cacheDbType = DbType.Int32;
            this.singletonDbType = DbType.Int32;
            this.versionDbType = DbType.Guid;

            this.tablesByName = new Dictionary<string, SchemaTable>();

            this.tableByClass = new Dictionary<IClass, SchemaTable>();
            this.tablesByRelationType = new Dictionary<IRelationType, SchemaTable>();
            this.columnsByRelationType = new Dictionary<IRelationType, SchemaColumn>();

            this.objectId = new SchemaColumn(this, "O", this.ObjectDbType, false, true, SchemaIndexType.None);
            this.cacheId = new SchemaColumn(this, "C", this.CacheDbType, false, false, SchemaIndexType.None);
            this.associationId = new SchemaColumn(this, "A", this.ObjectDbType, false, true, SchemaIndexType.None);
            this.roleId = new SchemaColumn(this, "R", this.ObjectDbType, false, true, SchemaIndexType.None);
            this.typeId = new SchemaColumn(this, "T", this.TypeDbType, false, false, SchemaIndexType.None);

            // Objects
            this.objects = new SchemaTable(this, SchemaTableKind.System, AllorsPrefix + "O");
            this.objectsObjectId = new SchemaColumn(this, this.ObjectId.Name, this.ObjectDbType, true, true, SchemaIndexType.None);
            this.objectsCacheId = new SchemaColumn(this, "C", this.CacheDbType, false, false, SchemaIndexType.None);
            this.objectsTypeId = new SchemaColumn(this, this.TypeId.Name, this.TypeDbType, false, false, SchemaIndexType.None);

            this.Objects.AddColumn(this.ObjectsObjectId);
            this.Objects.AddColumn(this.ObjectsTypeId);
            this.Objects.AddColumn(this.ObjectsCacheId);
            this.tablesByName.Add(this.Objects.Name, this.Objects);

            this.CreateTablesFromMeta();
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
                            var column = new SchemaColumn(this, associationType.SingularName, this.ObjectDbType, false, false, relationType.IsIndexed ? SchemaIndexType.Combined : SchemaIndexType.None, relationType);
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
    }
}
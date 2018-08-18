//------------------------------------------------------------------------------------------------- 
// <copyright file="Commands.cs" company="Allors bvba">
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
// <summary>Defines the Session type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    using Allors.Meta;

    public sealed class Commands
    {
        private static readonly long[] EmptyObjectIds = { };

        private readonly Session Session;

        private Database Database => this.Session.Database;

        private readonly Connection connection;

        #region Fields
        private Dictionary<IClass, Command> getUnitRolesByClass;
        private Dictionary<IClass, Dictionary<IRoleType, Command>> setUnitRoleByRoleTypeByClass;
        private Dictionary<IClass, Dictionary<IList<IRoleType>, Command>> setUnitRolesByRoleTypeByClass;
        private Dictionary<IRoleType, Command> getCompositeRoleByRoleType;
        private Dictionary<IRoleType, Command> setCompositeRoleByRoleType;
        private Dictionary<IRoleType, Command> getCompositesRoleByRoleType;
        private Dictionary<IRoleType, Command> addCompositeRoleByRoleType;
        private Dictionary<IRoleType, Command> removeCompositeRoleByRoleType;
        private Dictionary<IRoleType, Command> clearCompositeAndCompositesRoleByRoleType;
        private Dictionary<IAssociationType, Command> getCompositeAssociationByAssociationType;
        private Dictionary<IAssociationType, Command> getCompositesAssociationByAssociationType;

        private Command instantiateObject;
        private Command instantiateObjects;
        private Dictionary<IClass, Command> createObjectByClass;
        private Dictionary<IClass, Command> createObjectsByClass;
        private Dictionary<IClass, Command> deleteObjectByClass;
        private Dictionary<IClass, Command> insertObjectByClass;

        private Command getVersion;
        private Command updateVersions;
        #endregion

        #region Properties
        private Dictionary<IClass, Command> GetUnitRolesByClass
        {
            get
            {
                return this.getUnitRolesByClass ?? (this.getUnitRolesByClass = new Dictionary<IClass, Command>());
            }
        }

        private Dictionary<IClass, Dictionary<IRoleType, Command>> SetUnitRoleByRoleTypeByClass
        {
            get
            {
                return this.setUnitRoleByRoleTypeByClass ?? (this.setUnitRoleByRoleTypeByClass = new Dictionary<IClass, Dictionary<IRoleType, Command>>());
            }
        }

        private Dictionary<IClass, Dictionary<IList<IRoleType>, Command>> SetUnitRolesByRoleTypeByClass
        {
            get
            {
                return this.setUnitRolesByRoleTypeByClass ?? (this.setUnitRolesByRoleTypeByClass = new Dictionary<IClass, Dictionary<IList<IRoleType>, Command>>());
            }
        }

        private Dictionary<IRoleType, Command> GetCompositeRoleByRoleType
        {
            get
            {
                return this.getCompositeRoleByRoleType ?? (this.getCompositeRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }

        private Dictionary<IRoleType, Command> SetCompositeRoleByRoleType
        {
            get
            {
                return this.setCompositeRoleByRoleType ?? (this.setCompositeRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }

        private Dictionary<IRoleType, Command> GetCompositesRoleByRoleType
        {
            get
            {
                return this.getCompositesRoleByRoleType ?? (this.getCompositesRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }


        private Dictionary<IRoleType, Command> AddCompositeRoleByRoleType
        {
            get
            {
                return this.addCompositeRoleByRoleType ?? (this.addCompositeRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }

        private Dictionary<IRoleType, Command> RemoveCompositeRoleByRoleType
        {
            get
            {
                return this.removeCompositeRoleByRoleType ?? (this.removeCompositeRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }

        private Dictionary<IRoleType, Command> ClearCompositeAndCompositesRoleByRoleType
        {
            get
            {
                return this.clearCompositeAndCompositesRoleByRoleType ?? (this.clearCompositeAndCompositesRoleByRoleType = new Dictionary<IRoleType, Command>());
            }
        }

        private Dictionary<IAssociationType, Command> GetCompositeAssociationByAssociationType
        {
            get
            {
                return this.getCompositeAssociationByAssociationType ?? (this.getCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Command>());
            }
        }

        private Dictionary<IAssociationType, Command> GetCompositesAssociationByAssociationType
        {
            get
            {
                return this.getCompositesAssociationByAssociationType ?? (this.getCompositesAssociationByAssociationType = new Dictionary<IAssociationType, Command>());
            }
        }

        private Dictionary<IClass, Command> CreateObjectByClass
        {
            get
            {
                return this.createObjectByClass ?? (this.createObjectByClass = new Dictionary<IClass, Command>());
            }
        }

        private Dictionary<IClass, Command> CreateObjectsByClass
        {
            get
            {
                return this.createObjectsByClass ?? (this.createObjectsByClass = new Dictionary<IClass, Command>());
            }
        }

        private Dictionary<IClass, Command> DeleteObjectByClass
        {
            get
            {
                return this.deleteObjectByClass ?? (this.deleteObjectByClass = new Dictionary<IClass, Command>());
            }
        }

        private Dictionary<IClass, Command> InsertObjectByClass
        {
            get
            {
                return this.insertObjectByClass ?? (this.insertObjectByClass = new Dictionary<IClass, Command>());
            }
        }
        #endregion

        internal Commands(Session session, Connection connection)
        {
            this.Session = session;
            this.connection = connection;
        }

        internal void ResetCommands()
        {
            this.getUnitRolesByClass = null;
            this.setUnitRoleByRoleTypeByClass = null;

            this.getCompositeRoleByRoleType = null;
            this.setCompositeRoleByRoleType = null;
            this.getCompositesRoleByRoleType = null;
            this.addCompositeRoleByRoleType = null;
            this.removeCompositeRoleByRoleType = null;
            this.clearCompositeAndCompositesRoleByRoleType = null;
            this.getCompositeAssociationByAssociationType = null;
            this.getCompositesAssociationByAssociationType = null;

            this.instantiateObject = null;
            this.instantiateObjects = null;
            this.setUnitRolesByRoleTypeByClass = null;
            this.createObjectByClass = null;
            this.createObjectsByClass = null;
            this.insertObjectByClass = null;
            this.deleteObjectByClass = null;

            this.getVersion = null;
            this.updateVersions = null;
        }

        internal void DeleteObject(Strategy strategy)
        {
            this.deleteObjectByClass = this.deleteObjectByClass ?? new Dictionary<IClass, Command>();

            var @class = strategy.Class;

            Command command;
            if (!this.DeleteObjectByClass.TryGetValue(@class, out command))
            {
                var sql = "BEGIN\n";

                sql += "DELETE FROM " + this.Database.Mapping.TableNameForObjects + "\n";
                sql += "WHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamNameForObject + ";\n";

                sql += "DELETE FROM " + this.Database.Mapping.TableNameForObjectByClass[@class.ExclusiveClass] + "\n";
                sql += "WHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamNameForObject + ";\n";

                sql += "END;";

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.AddObjectParameter(strategy.ObjectId);

                this.deleteObjectByClass[@class] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForObject].Value = strategy.ObjectId;
            }

            command.ExecuteNonQuery();
        }

        internal void GetUnitRoles(Roles roles)
        {
            var reference = roles.Reference;
            var @class = reference.Class;

            Command command;
            if (!this.GetUnitRolesByClass.TryGetValue(@class, out command))
            {
                var sql = this.Database.Mapping.ProcedureNameForGetUnitRolesByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectParameter(reference.ObjectId);
                this.getUnitRolesByClass[@class] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForObject].Value = reference.ObjectId;
            }

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var sortedUnitRoles = this.Database.GetSortedUnitRolesByObjectType(reference.Class);

                    for (var i = 0; i < sortedUnitRoles.Length; i++)
                    {
                        var roleType = sortedUnitRoles[i];

                        object unit = null;
                        if (!reader.IsDBNull(i))
                        {
                            var unitTypeTag = ((IUnit)roleType.ObjectType).UnitTag;
                            switch (unitTypeTag)
                            {
                                case UnitTags.String:
                                    unit = reader.GetString(i);
                                    break;
                                case UnitTags.Integer:
                                    unit = reader.GetInt32(i);
                                    break;
                                case UnitTags.Float:
                                    unit = reader.GetDouble(i);
                                    break;
                                case UnitTags.Decimal:
                                    unit = reader.GetDecimal(i);
                                    break;
                                case UnitTags.DateTime:
                                    var dateTime = reader.GetDateTime(i);
                                    if (dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
                                    {
                                        unit = dateTime;
                                    }
                                    else
                                    {
                                        unit = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
                                    }

                                    break;
                                case UnitTags.Boolean:
                                    unit = reader.GetBoolean(i);
                                    break;
                                case UnitTags.Unique:
                                    unit = reader.GetGuid(i);
                                    break;
                                case UnitTags.Binary:
                                    var byteArray = (byte[])reader.GetValue(i);
                                    unit = byteArray;
                                    break;
                                default:
                                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.Name);
                            }
                        }

                        roles.CachedObject.SetValue(roleType, unit);
                    }
                }
            }
        }

        internal void SetUnitRole(List<UnitRelation> relations, IClass exclusiveRootClass, IRoleType roleType)
        {
            var schema = this.Database.Mapping;

            Dictionary<IRoleType, Command> commandByRoleType;
            if (!this.SetUnitRoleByRoleTypeByClass.TryGetValue(exclusiveRootClass, out commandByRoleType))
            {
                commandByRoleType = new Dictionary<IRoleType, Command>();
                this.setUnitRoleByRoleTypeByClass.Add(exclusiveRootClass, commandByRoleType);
            }

            var tableTypeName = schema.GetTableTypeName(roleType);
            
            Command command;
            if (!commandByRoleType.TryGetValue(roleType, out command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetUnitRoleByRelationTypeByClass[exclusiveRootClass][roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;

                var sqlParameter = command.CreateParameter();
                sqlParameter.SqlDbType = SqlDbType.Structured;
                sqlParameter.TypeName = tableTypeName;
                sqlParameter.ParameterName = Mapping.ParamNameForTableType;
                sqlParameter.Value = this.Database.CreateUnitRelationTable(roleType, relations);

                command.Parameters.Add(sqlParameter);
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateUnitRelationTable(roleType, relations);
            }

            command.ExecuteNonQuery();
        }

        internal void SetUnitRoles(Roles roles, List<IRoleType> sortedRoleTypes)
        {
            var exclusiveRootClass = roles.Reference.Class.ExclusiveClass;

            Dictionary<IList<IRoleType>, Command> setUnitRoleByRoleType;
            if (!this.SetUnitRolesByRoleTypeByClass.TryGetValue(exclusiveRootClass, out setUnitRoleByRoleType))
            {
                setUnitRoleByRoleType = new Dictionary<IList<IRoleType>, Command>(new SortedRoleTypeComparer());
                this.setUnitRolesByRoleTypeByClass.Add(exclusiveRootClass, setUnitRoleByRoleType);
            }

            Command command;
            if (!setUnitRoleByRoleType.TryGetValue(sortedRoleTypes, out command))
            {
                command = this.connection.CreateCommand();
                command.AddObjectParameter(roles.Reference.ObjectId);

                var sql = new StringBuilder();
                sql.Append("UPDATE " + this.Database.Mapping.TableNameForObjectByClass[exclusiveRootClass] + " SET\n");

                var count = 0;
                foreach (var roleType in sortedRoleTypes)
                {
                    if (count > 0)
                    {
                        sql.Append(" , ");
                    }

                    ++count;

                    var column = this.Database.Mapping.ColumnNameByRelationType[roleType.RelationType];
                    sql.Append(column + "=" + this.Database.Mapping.ParamNameByRoleType[roleType]);

                    var unit = roles.ModifiedRoleByRoleType[roleType];
                    var sqlParameter1 = command.CreateParameter();
                    sqlParameter1.ParameterName = this.Database.Mapping.ParamNameByRoleType[roleType];
                    sqlParameter1.SqlDbType = this.Database.Mapping.GetSqlDbType(roleType);
                    sqlParameter1.Value = unit ?? DBNull.Value;

                    command.Parameters.Add(sqlParameter1);
                }

                sql.Append("\nWHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamNameForObject + "\n");

                command.CommandText = sql.ToString();
                command.ExecuteNonQuery();

                setUnitRoleByRoleType.Add(sortedRoleTypes, command);
            }
            else
            {
                command.Parameters[Mapping.ParamNameForObject].Value = roles.Reference.ObjectId;

                foreach (var roleType in sortedRoleTypes)
                {
                    var unit = roles.ModifiedRoleByRoleType[roleType];
                    command.Parameters[this.Database.Mapping.ParamNameByRoleType[roleType]].Value = unit ?? DBNull.Value;
                }

                command.ExecuteNonQuery();
            }
        }

        internal void GetCompositeRole(Roles roles, IRoleType roleType)
        {
            var reference = roles.Reference;

            Command command;
            if (!this.GetCompositeRoleByRoleType.TryGetValue(roleType, out command))
            {
                var sql = this.Database.Mapping.ProcedureNameForGetRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddAssociationTableParameter(reference.ObjectId);
                this.getCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForAssociation].Value = reference.ObjectId;
            }

            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                roles.CachedObject.SetValue(roleType, null);
            }
            else
            {
                var objectId = this.Session.State.GetObjectIdForExistingObject(result.ToString());
                // TODO: Should add to objectsToLoad
                roles.CachedObject.SetValue(roleType, objectId);
            }
        }

        internal void SetCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            if (!this.SetCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetRoleByRelationType[roleType.RelationType];

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleTableParameter(relations);
                this.setCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateCompositeRelationTable(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void GetCompositesRole(Roles roles, IRoleType roleType)
        {
            var reference = roles.Reference;

            if (!this.GetCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var associationType = roleType.AssociationType;

                string sql;
                if (associationType.IsMany || !roleType.RelationType.ExistExclusiveClasses)
                {
                    sql = this.Database.Mapping.ProcedureNameForGetRoleByRelationType[roleType.RelationType];
                }
                else
                {
                    sql = this.Database.Mapping.ProcedureNameForGetRoleByRelationType[roleType.RelationType];
                }

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddAssociationParameter(reference.ObjectId);
                this.getCompositesRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForAssociation].Value = reference.ObjectId;
            }

            var objectIds = new List<long>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = this.Session.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            roles.CachedObject.SetValue(roleType, objectIds.ToArray());
        }

        internal void AddCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            if (!this.AddCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForAddRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleTableParameter(relations);
                this.addCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateCompositeRelationTable(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void RemoveCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            if (!this.RemoveCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForRemoveRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleTableParameter(relations);
                this.removeCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateCompositeRelationTable(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void ClearCompositeAndCompositesRole(IList<long> associations, IRoleType roleType)
        {
            var sql = this.Database.Mapping.ProcedureNameForClearRoleByRelationType[roleType.RelationType];

            if (!this.ClearCompositeAndCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectTableParameter(associations);
                this.clearCompositeAndCompositesRoleByRoleType[roleType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateObjectTable(associations);
            }

            command.ExecuteNonQuery();
        }

        internal Reference GetCompositeAssociation(Reference role, IAssociationType associationType)
        {
            Reference associationObject = null;

            if (!this.GetCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleParameter(role.ObjectId);
                this.getCompositeAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForCompositeRole].Value = role.ObjectId;
            }

            object result = command.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                var id = this.Session.State.GetObjectIdForExistingObject(result.ToString());

                associationObject = associationType.ObjectType.ExistExclusiveClass ? 
                                        this.Session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, id, this.Session) : 
                                        this.Session.State.GetOrCreateReferenceForExistingObject(id, this.Session);
            }

            return associationObject;
        }

        internal long[] GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            if (!this.GetCompositesAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleParameter(role.ObjectId);
                this.getCompositesAssociationByAssociationType[associationType] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForCompositeRole].Value = role.ObjectId;
            }

            var objectIds = new List<long>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = this.Session.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            return objectIds.ToArray();
        }

        internal Reference CreateObject(IClass @class)
        {
            if (!this.CreateObjectByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForCreateObjectByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddTypeParameter(@class);
                this.createObjectByClass[@class] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForClass].Value = (object)@class.Id;
            }

            var result = command.ExecuteScalar();
            var objectId = long.Parse(result.ToString());
            return this.Session.State.CreateReferenceForNewObject(@class, objectId, this.Session);
        }

        internal IList<Reference> CreateObjects(IClass @class, int count)
        {
            if (!this.CreateObjectsByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForCreateObjectsByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandType = CommandType.StoredProcedure;
                command.AddTypeParameter(@class);
                command.AddCountParameter(count);
                this.createObjectsByClass[@class] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForClass].Value = @class.Id;
                command.Parameters[Mapping.ParamNameForCount].Value = count;
            }

            var objectIds = new List<object>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    object id = long.Parse(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            var strategies = new List<Reference>();

            foreach (var id in objectIds)
            {
                var objectId = long.Parse(id.ToString());
                var strategySql = this.Session.State.CreateReferenceForNewObject(@class, objectId, this.Session);
                strategies.Add(strategySql);
            }

            return strategies;
        }

        internal Reference InsertObject(IClass @class, long objectId)
        {
            if (!this.InsertObjectByClass.TryGetValue(@class, out var command))
            {
                var schema = this.Database.Mapping;

                // TODO: Make this a single pass Query.
                var sql = "IF EXISTS (\n";
                sql += "    SELECT " + Mapping.ColumnNameForObject + "\n";
                sql += "    FROM " + schema.TableNameForObjectByClass[@class.ExclusiveClass] + "\n";
                sql += "    WHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamNameForObject + "\n";
                sql += ")\n";
                sql += "    SELECT 1\n";
                sql += "ELSE\n";
                sql += "    BEGIN\n";

                sql += "    SET IDENTITY_INSERT " + schema.TableNameForObjects + " ON\n";

                sql += "    INSERT INTO " + schema.TableNameForObjects + " (" + Mapping.ColumnNameForObject + "," + Mapping.ColumnNameForClass + "," + Mapping.ColumnNameForVersion + ")\n";
                sql += "    VALUES (" + Mapping.ParamNameForObject + "," + Mapping.ParamNameForClass + ", " + Reference.InitialVersion + ");\n";

                sql += "    SET IDENTITY_INSERT " + schema.TableNameForObjects + " OFF;\n";

                sql += "    INSERT INTO " + schema.TableNameForObjectByClass[@class.ExclusiveClass] + " (" + Mapping.ColumnNameForObject + "," + Mapping.ColumnNameForClass + ")\n";
                sql += "    VALUES (" + Mapping.ParamNameForObject + "," + Mapping.ParamNameForClass + ");\n";

                sql += "    SELECT 0;\n";
                sql += "    END";

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.AddObjectParameter(objectId);
                command.AddTypeParameter(@class);

                this.insertObjectByClass[@class] = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForObject].Value = objectId;
                command.Parameters[Mapping.ParamNameForClass].Value = (object)@class.Id ?? DBNull.Value;
            }

            var result = command.ExecuteScalar();
            if (result == null)
            {
                throw new Exception("Reader returned no rows");
            }

            if (long.Parse(result.ToString()) > 0)
            {
                throw new Exception("Duplicate id error");
            }

            return this.Session.State.CreateReferenceForNewObject(@class, objectId, this.Session);
        }

        internal Reference InstantiateObject(long objectId)
        {
            var command = this.instantiateObject;
            if (command == null)
            {
                var sql = "SELECT " + Mapping.ColumnNameForClass + ", " + Mapping.ColumnNameForVersion + "\n";
                sql += "FROM " + this.Database.Mapping.TableNameForObjects + "\n";
                sql += "WHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamNameForObject + "\n";

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.AddObjectParameter(objectId);
                this.instantiateObject = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForObject].Value = objectId;
            }

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var classId = reader.GetGuid(0);
                    var version = reader.GetInt64(1);

                    var type = (IClass)this.Database.MetaPopulation.Find(classId);
                    return this.Session.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.Session);
                }

                return null;
            }
        }

        internal IEnumerable<Reference> InstantiateReferences(IEnumerable<long> objectIds)
        {
            var command = this.instantiateObjects;
            if (command == null)
            {
                var sql = this.Database.Mapping.ProcedureNameForInstantiate;
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectTableParameter(objectIds);
                this.instantiateObjects = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateObjectTable(objectIds);
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var objectIdString = reader.GetValue(0).ToString();
                    var classId = reader.GetGuid(1);
                    var version = reader.GetInt64(2);

                    var objectId = long.Parse(objectIdString);
                    var type = (IClass)this.Database.ObjectFactory.GetObjectTypeForType(classId);
                    var reference = this.Session.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.Session);

                    yield return reference;
                }
            }
        }

        internal Dictionary<long, long> GetVersions(ISet<Reference> references)
        {
            var command = this.getVersion;

            if (command == null)
            {
                var sql = this.Database.Mapping.ProcedureNameForGetVersion;
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectTableParameter(references);
                this.getVersion = command;
            }
            else
            {
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateObjectTable(references);
            }

            var versionByObjectId = new Dictionary<long, long>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var objectId = long.Parse(reader[0].ToString());
                    var version = reader.GetInt64(1);

                    versionByObjectId.Add(objectId, version);
                }
            }

            return versionByObjectId;
        }

        internal void UpdateVersion()
        {
            var command = this.updateVersions;
            if (command == null)
            {
                var sql = this.Database.Mapping.ProcedureNameForUpdateVersion;
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                // TODO: Remove dependency on State
                command.AddObjectTableParameter(this.Session.State.ModifiedRolesByReference.Keys);
                this.updateVersions = command;
            }
            else
            {
                // TODO: Remove dependency on State
                command.Parameters[Mapping.ParamNameForTableType].Value = this.Database.CreateObjectTable(this.Session.State.ModifiedRolesByReference.Keys);
            }

            command.ExecuteNonQuery();
        }

        private class SortedRoleTypeComparer : IEqualityComparer<IList<IRoleType>>
        {
            public bool Equals(IList<IRoleType> x, IList<IRoleType> y)
            {
                if (x.Count == y.Count)
                {
                    for (var i = 0; i < x.Count; i++)
                    {
                        if (!x[i].Equals(y[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            public int GetHashCode(IList<IRoleType> roleTypes)
            {
                var hashCode = 0;
                foreach (var roleType in roleTypes)
                {
                    hashCode = hashCode ^ roleType.GetHashCode();
                }

                return hashCode;
            }
        }
    }
}
//-------------------------------------------------------------------------------------------------
// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Session type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    using Allors.Meta;

    public sealed class Commands
    {
        private readonly Session session;

        private readonly Connection connection;

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

        private Command getVersion;
        private Command updateVersions;

        internal Commands(Session session, Connection connection)
        {
            this.session = session;
            this.connection = connection;
        }

        private Database Database => this.session.Database;

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
            this.deleteObjectByClass = null;

            this.getVersion = null;
            this.updateVersions = null;
        }

        internal void DeleteObject(Strategy strategy)
        {
            this.deleteObjectByClass = this.deleteObjectByClass ?? new Dictionary<IClass, Command>();
            var @class = strategy.Class;

            if (!this.deleteObjectByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForDeleteObjectByClass[@class];

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
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
            this.getUnitRolesByClass = this.getUnitRolesByClass ?? new Dictionary<IClass, Command>();

            var reference = roles.Reference;
            var @class = reference.Class;

            if (!this.getUnitRolesByClass.TryGetValue(@class, out var command))
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
            this.setUnitRoleByRoleTypeByClass = this.setUnitRoleByRoleTypeByClass ?? new Dictionary<IClass, Dictionary<IRoleType, Command>>();

            var schema = this.Database.Mapping;

            if (!this.setUnitRoleByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var commandByRoleType))
            {
                commandByRoleType = new Dictionary<IRoleType, Command>();
                this.setUnitRoleByRoleTypeByClass.Add(exclusiveRootClass, commandByRoleType);
            }

            if (!commandByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetUnitRoleByRelationTypeByClass[exclusiveRootClass][roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;

                command.AddUnitRoleArrayParameter(roleType, relations);
            }
            else
            {
                command.SetUnitRoleArrayParameter(roleType, relations);
            }

            command.ExecuteNonQuery();
        }

        internal void SetUnitRoles(Roles roles, List<IRoleType> sortedRoleTypes)
        {
            this.setUnitRolesByRoleTypeByClass = this.setUnitRolesByRoleTypeByClass ?? new Dictionary<IClass, Dictionary<IList<IRoleType>, Command>>();

            var exclusiveRootClass = roles.Reference.Class.ExclusiveClass;

            if (!this.setUnitRolesByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var setUnitRoleByRoleType))
            {
                setUnitRoleByRoleType = new Dictionary<IList<IRoleType>, Command>(new SortedRoleTypeComparer());
                this.setUnitRolesByRoleTypeByClass.Add(exclusiveRootClass, setUnitRoleByRoleType);
            }

            if (!setUnitRoleByRoleType.TryGetValue(sortedRoleTypes, out var command))
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
                    sql.Append(column + "=" + this.Database.Mapping.ParamInvocationNameByRoleType[roleType]);

                    var unit = roles.ModifiedRoleByRoleType[roleType];
                    var sqlParameter = command.CreateParameter();
                    sqlParameter.ParameterName = this.Database.Mapping.ParamNameByRoleType[roleType];
                    sqlParameter.NpgsqlDbType = this.Database.Mapping.GetNpgsqlDbType(roleType);
                    sqlParameter.Value = unit ?? DBNull.Value;

                    command.Parameters.Add(sqlParameter);
                }

                sql.Append("\nWHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamInvocationNameForObject + "\n");

                command.CommandText = sql.ToString();
                command.CommandType = CommandType.Text;
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
            this.getCompositeRoleByRoleType = this.getCompositeRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            var reference = roles.Reference;

            if (!this.getCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForGetRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddAssociationParameter(reference.ObjectId);
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
                var objectId = this.session.State.GetObjectIdForExistingObject(result.ToString());
                // TODO: Should add to objectsToLoad
                roles.CachedObject.SetValue(roleType, objectId);
            }
        }

        internal void SetCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.setCompositeRoleByRoleType = this.setCompositeRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            if (!this.setCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetRoleByRelationType[roleType.RelationType];

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleArrayParameter(relations);
                this.setCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetCompositeRoleArrayParameter(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void GetCompositesRole(Roles roles, IRoleType roleType)
        {
            this.getCompositesRoleByRoleType = this.getCompositesRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            var reference = roles.Reference;

            if (!this.getCompositesRoleByRoleType.TryGetValue(roleType, out var command))
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
                    var id = this.session.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            roles.CachedObject.SetValue(roleType, objectIds.ToArray());
        }

        internal void AddCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.addCompositeRoleByRoleType = this.addCompositeRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            if (!this.addCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForAddRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleArrayParameter(relations);
                this.addCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetCompositeRoleArrayParameter(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void RemoveCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.removeCompositeRoleByRoleType = this.removeCompositeRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            if (!this.removeCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForRemoveRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleArrayParameter(relations);
                this.removeCompositeRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetCompositeRoleArrayParameter(relations);
            }

            command.ExecuteNonQuery();
        }

        internal void ClearCompositeAndCompositesRole(IList<long> associations, IRoleType roleType)
        {
            this.clearCompositeAndCompositesRoleByRoleType = this.clearCompositeAndCompositesRoleByRoleType ?? new Dictionary<IRoleType, Command>();

            var sql = this.Database.Mapping.ProcedureNameForClearRoleByRelationType[roleType.RelationType];

            if (!this.clearCompositeAndCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddObjectArrayParameter(associations);
                this.clearCompositeAndCompositesRoleByRoleType[roleType] = command;
            }
            else
            {
                command.SetObjectArrayParameter(associations);
            }

            command.ExecuteNonQuery();
        }

        internal Reference GetCompositeAssociation(Reference role, IAssociationType associationType)
        {
            this.getCompositeAssociationByAssociationType = this.getCompositeAssociationByAssociationType ?? new Dictionary<IAssociationType, Command>();

            Reference associationObject = null;

            if (!this.getCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
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

            var result = command.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                var id = this.session.State.GetObjectIdForExistingObject(result.ToString());

                associationObject = associationType.ObjectType.ExistExclusiveClass ?
                                        this.session.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveClass, id, this.session) :
                                        this.session.State.GetOrCreateReferenceForExistingObject(id, this.session);
            }

            return associationObject;
        }

        internal long[] GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            this.getCompositesAssociationByAssociationType = this.getCompositesAssociationByAssociationType ?? new Dictionary<IAssociationType, Command>();

            if (!this.getCompositesAssociationByAssociationType.TryGetValue(associationType, out var command))
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
                    var id = this.session.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            return objectIds.ToArray();
        }

        internal Reference CreateObject(IClass @class)
        {
            this.createObjectByClass = this.createObjectByClass ?? new Dictionary<IClass, Command>();

            if (!this.createObjectByClass.TryGetValue(@class, out var command))
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
            return this.session.State.CreateReferenceForNewObject(@class, objectId, this.session);
        }

        internal IList<Reference> CreateObjects(IClass @class, int count)
        {
            this.createObjectsByClass = this.createObjectsByClass ?? new Dictionary<IClass, Command>();

            if (!this.createObjectsByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForCreateObjectsByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
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
                var strategySql = this.session.State.CreateReferenceForNewObject(@class, objectId, this.session);
                strategies.Add(strategySql);
            }

            return strategies;
        }

        internal Reference InstantiateObject(long objectId)
        {
            var command = this.instantiateObject;
            if (command == null)
            {
                var sql = "SELECT " + Mapping.ColumnNameForClass + ", " + Mapping.ColumnNameForVersion + "\n";
                sql += "FROM " + this.Database.Mapping.TableNameForObjects + "\n";
                sql += "WHERE " + Mapping.ColumnNameForObject + "=" + Mapping.ParamInvocationNameForObject + "\n";

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
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
                    return this.session.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.session);
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
                command.AddObjectArrayParameter(objectIds);
                this.instantiateObjects = command;
            }
            else
            {
                command.SetObjectArrayParameter(objectIds);
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
                    var reference = this.session.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.session);

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
                command.AddObjectArrayParameter(references);
                this.getVersion = command;
            }
            else
            {
                command.SetObjectArrayParameter(references);
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
                command.AddObjectArrayParameter(this.session.State.ModifiedRolesByReference.Keys);
                this.updateVersions = command;
            }
            else
            {
                // TODO: Remove dependency on State
                command.SetObjectArrayParameter(this.session.State.ModifiedRolesByReference.Keys);
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
                    hashCode ^= roleType.GetHashCode();
                }

                return hashCode;
            }
        }
    }
}

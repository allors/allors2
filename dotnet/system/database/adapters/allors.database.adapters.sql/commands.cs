// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using Meta;

    public sealed class Commands
    {
        private readonly Transaction transaction;

        private readonly IConnection connection;

        private Dictionary<IClass, ICommand> getUnitRolesByClass;
        private Dictionary<IClass, Dictionary<IRoleType, ICommand>> setUnitRoleByRoleTypeByClass;
        private Dictionary<IClass, Dictionary<IList<IRoleType>, ICommand>> setUnitRolesByRoleTypeByClass;
        private Dictionary<IRoleType, ICommand> getCompositeRoleByRoleType;
        private Dictionary<IRoleType, ICommand> setCompositeRoleByRoleType;
        private Dictionary<IRoleType, ICommand> getCompositesRoleByRoleType;
        private Dictionary<IRoleType, ICommand> addCompositeRoleByRoleType;
        private Dictionary<IRoleType, ICommand> removeCompositeRoleByRoleType;
        private Dictionary<IRoleType, ICommand> clearCompositeAndCompositesRoleByRoleType;
        private Dictionary<IAssociationType, ICommand> getCompositeAssociationByAssociationType;
        private Dictionary<IAssociationType, ICommand> getCompositesAssociationByAssociationType;

        private ICommand instantiateObject;
        private ICommand instantiateObjects;

        private Dictionary<IClass, ICommand> createObjectByClass;
        private Dictionary<IClass, ICommand> createObjectsByClass;
        private Dictionary<IClass, ICommand> deleteObjectByClass;

        private ICommand getVersion;
        private ICommand updateVersions;

        internal Commands(Transaction transaction, IConnection connection)
        {
            this.transaction = transaction;
            this.connection = connection;
        }

        private Database Database => this.transaction.Database;

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
            this.deleteObjectByClass ??= new Dictionary<IClass, ICommand>();

            var @class = strategy.Class;

            if (!this.deleteObjectByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForDeleteObjectByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.deleteObjectByClass[@class] = command;
            }

            command.ObjectParameter(strategy.ObjectId);
            command.ExecuteNonQuery();
        }

        internal void GetUnitRoles(Strategy strategy)
        {
            this.getUnitRolesByClass ??= new Dictionary<IClass, ICommand>();

            var reference = strategy.Reference;
            var @class = reference.Class;

            if (!this.getUnitRolesByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForGetUnitRolesByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.getUnitRolesByClass[@class] = command;
            }

            command.ObjectParameter(reference.ObjectId);

            using (var reader = command.ExecuteReader())
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
                            switch (((IUnit)roleType.ObjectType).Tag)
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
                                    unit = (byte[])reader.GetValue(i);
                                    break;

                                default:
                                    throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.Name);
                            }
                        }

                        strategy.CachedObject.SetValue(roleType, unit);
                    }
                }
            }
        }

        internal void SetUnitRole(List<UnitRelation> relations, IClass exclusiveRootClass, IRoleType roleType)
        {
            this.setUnitRoleByRoleTypeByClass ??= new Dictionary<IClass, Dictionary<IRoleType, ICommand>>();

            if (!this.setUnitRoleByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var commandByRoleType))
            {
                commandByRoleType = new Dictionary<IRoleType, ICommand>();
                this.setUnitRoleByRoleTypeByClass.Add(exclusiveRootClass, commandByRoleType);
            }

            if (!commandByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetUnitRoleByRelationTypeByClass[exclusiveRootClass][roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
            }

            command.UnitTableParameter(roleType, relations);

            command.ExecuteNonQuery();
        }

        internal void SetUnitRoles(Strategy strategy, List<IRoleType> sortedRoleTypes)
        {
            this.setUnitRolesByRoleTypeByClass ??= new Dictionary<IClass, Dictionary<IList<IRoleType>, ICommand>>();

            var exclusiveRootClass = strategy.Reference.Class.ExclusiveDatabaseClass;

            if (!this.setUnitRolesByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var setUnitRoleByRoleType))
            {
                setUnitRoleByRoleType = new Dictionary<IList<IRoleType>, ICommand>(new SortedRoleTypeComparer());
                this.setUnitRolesByRoleTypeByClass.Add(exclusiveRootClass, setUnitRoleByRoleType);
            }

            if (!setUnitRoleByRoleType.TryGetValue(sortedRoleTypes, out var command))
            {
                command = this.connection.CreateCommand();
                command.ObjectParameter(strategy.Reference.ObjectId);

                var sql = new StringBuilder();
                sql.Append("UPDATE ").Append(this.Database.Mapping.TableNameForObjectByClass[exclusiveRootClass]).Append(" SET\n");

                var count = 0;
                foreach (var roleType in sortedRoleTypes)
                {
                    if (count > 0)
                    {
                        sql.Append(" , ");
                    }

                    ++count;

                    var column = this.Database.Mapping.ColumnNameByRelationType[roleType.RelationType];
                    sql.Append(column).Append('=').Append(this.Database.Mapping.ParamInvocationNameByRoleType[roleType]);

                    var unit = strategy.EnsureModifiedRoleByRoleType[roleType];
                    command.AddUnitRoleParameter(roleType, unit);
                }

                sql.Append("\nWHERE ").Append(Mapping.ColumnNameForObject).Append('=').Append(this.Database.Mapping.ParamInvocationNameForObject).Append('\n');

                command.CommandText = sql.ToString();
                command.ExecuteNonQuery();

                setUnitRoleByRoleType.Add(sortedRoleTypes, command);
            }
            else
            {
                command.ObjectParameter(strategy.Reference.ObjectId);

                foreach (var roleType in sortedRoleTypes)
                {
                    var unit = strategy.EnsureModifiedRoleByRoleType[roleType];
                    command.AddUnitRoleParameter(roleType, unit);
                }

                command.ExecuteNonQuery();
            }
        }

        internal void GetCompositeRole(Strategy strategy, IRoleType roleType)
        {
            this.getCompositeRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            var reference = strategy.Reference;

            if (!this.getCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForGetRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.getCompositeRoleByRoleType[roleType] = command;
            }

            command.AddAssociationParameter(reference.ObjectId);

            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                strategy.CachedObject.SetValue(roleType, null);
            }
            else
            {
                var objectId = this.transaction.State.GetObjectIdForExistingObject(result.ToString());
                // TODO: Should add to objectsToLoad
                strategy.CachedObject.SetValue(roleType, objectId);
            }
        }

        internal void SetCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.setCompositeRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            if (!this.setCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForSetRoleByRelationType[roleType.RelationType];

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.setCompositeRoleByRoleType[roleType] = command;
            }

            command.AddCompositeRoleTableParameter(relations);

            command.ExecuteNonQuery();
        }

        internal void GetCompositesRole(Strategy strategy, IRoleType roleType)
        {
            this.getCompositesRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            var reference = strategy.Reference;

            if (!this.getCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var associationType = roleType.AssociationType;

                string sql;
                if (associationType.IsMany || !roleType.RelationType.ExistExclusiveDatabaseClasses)
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
                this.getCompositesRoleByRoleType[roleType] = command;
            }

            command.AddAssociationParameter(reference.ObjectId);

            var objectIds = new List<long>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = this.transaction.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            strategy.CachedObject.SetValue(roleType, objectIds.ToArray());
        }

        internal void AddCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.addCompositeRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            if (!this.addCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForAddRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.addCompositeRoleByRoleType[roleType] = command;
            }

            command.AddCompositeRoleTableParameter(relations);

            command.ExecuteNonQuery();
        }

        internal void RemoveCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.removeCompositeRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            if (!this.removeCompositeRoleByRoleType.TryGetValue(roleType, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForRemoveRoleByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.removeCompositeRoleByRoleType[roleType] = command;
            }

            command.AddCompositeRoleTableParameter(relations);

            command.ExecuteNonQuery();
        }

        internal void ClearCompositeAndCompositesRole(IList<long> associations, IRoleType roleType)
        {
            this.clearCompositeAndCompositesRoleByRoleType ??= new Dictionary<IRoleType, ICommand>();

            var sql = this.Database.Mapping.ProcedureNameForClearRoleByRelationType[roleType.RelationType];

            if (!this.clearCompositeAndCompositesRoleByRoleType.TryGetValue(roleType, out var command))
            {
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.clearCompositeAndCompositesRoleByRoleType[roleType] = command;
            }

            command.ObjectTableParameter(associations);

            command.ExecuteNonQuery();
        }

        internal Reference GetCompositeAssociation(Reference role, IAssociationType associationType)
        {
            this.getCompositeAssociationByAssociationType ??= new Dictionary<IAssociationType, ICommand>();

            Reference associationObject = null;

            if (!this.getCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.getCompositeAssociationByAssociationType[associationType] = command;
            }

            command.AddCompositeRoleParameter(role.ObjectId);

            var result = command.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                var id = this.transaction.State.GetObjectIdForExistingObject(result.ToString());

                associationObject = associationType.ObjectType.ExistExclusiveDatabaseClass ?
                                        this.transaction.State.GetOrCreateReferenceForExistingObject(associationType.ObjectType.ExclusiveDatabaseClass, id, this.transaction) :
                                        this.transaction.State.GetOrCreateReferenceForExistingObject(id, this.transaction);
            }

            return associationObject;
        }

        internal long[] GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            this.getCompositesAssociationByAssociationType ??= new Dictionary<IAssociationType, ICommand>();

            if (!this.getCompositesAssociationByAssociationType.TryGetValue(associationType, out var command))
            {
                var roleType = associationType.RoleType;
                var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRelationType[roleType.RelationType];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.getCompositesAssociationByAssociationType[associationType] = command;
            }

            command.AddCompositeRoleParameter(role.ObjectId);

            var objectIds = new List<long>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = this.transaction.State.GetObjectIdForExistingObject(reader[0].ToString());
                    objectIds.Add(id);
                }
            }

            return objectIds.ToArray();
        }

        internal Reference CreateObject(IClass @class)
        {
            this.createObjectByClass ??= new Dictionary<IClass, ICommand>();

            if (!this.createObjectByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForCreateObjectByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.createObjectByClass[@class] = command;
            }

            command.AddTypeParameter(@class);

            var result = command.ExecuteScalar();
            var objectId = long.Parse(result.ToString());
            return this.transaction.State.CreateReferenceForNewObject(@class, objectId, this.transaction);
        }

        internal IList<Reference> CreateObjects(IClass @class, int count)
        {
            this.createObjectsByClass ??= new Dictionary<IClass, ICommand>();

            if (!this.createObjectsByClass.TryGetValue(@class, out var command))
            {
                var sql = this.Database.Mapping.ProcedureNameForCreateObjectsByClass[@class];
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandType = CommandType.StoredProcedure;
                this.createObjectsByClass[@class] = command;
            }

            command.AddTypeParameter(@class);
            command.AddCountParameter(count);

            var objectIds = new List<object>();
            using (var reader = command.ExecuteReader())
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
                var strategySql = this.transaction.State.CreateReferenceForNewObject(@class, objectId, this.transaction);
                strategies.Add(strategySql);
            }

            return strategies;
        }

        internal Reference InstantiateObject(long objectId)
        {
            var command = this.instantiateObject;
            if (command == null)
            {
                var sql =
@$"SELECT {Mapping.ColumnNameForClass}, {Mapping.ColumnNameForVersion}
FROM {this.Database.Mapping.TableNameForObjects}
WHERE {Mapping.ColumnNameForObject}={this.Database.Mapping.ParamInvocationNameForObject}
";

                command = this.connection.CreateCommand();
                command.CommandText = sql;
                this.instantiateObject = command;
            }

            command.ObjectParameter(objectId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var classId = reader.GetGuid(0);
                    var version = reader.GetInt64(1);

                    var type = (IClass)this.Database.MetaPopulation.FindById(classId);
                    return this.transaction.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.transaction);
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
                this.instantiateObjects = command;
            }

            command.ObjectTableParameter(objectIds);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var objectIdString = reader.GetValue(0).ToString();
                    var classId = reader.GetGuid(1);
                    var version = reader.GetInt64(2);

                    var objectId = long.Parse(objectIdString);
                    var type = (IClass)this.Database.ObjectFactory.GetObjectType(classId);

                    yield return this.transaction.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.transaction);
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
                this.getVersion = command;
            }

            command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

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

        internal void UpdateVersion(IEnumerable<long> changed)
        {
            var command = this.updateVersions;
            if (command == null)
            {
                var sql = this.Database.Mapping.ProcedureNameForUpdateVersion;
                command = this.connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                this.updateVersions = command;
            }

            // TODO: Remove dependency on State
            command.ObjectTableParameter(changed);
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

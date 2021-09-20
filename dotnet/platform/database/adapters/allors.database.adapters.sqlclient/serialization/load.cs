// <copyright file="Load.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Adapters.Schema;
    using Adapters;
    using Allors.Meta;

    internal class Load
    {
        private static readonly byte[] EmptyByteArray = new byte[0];

        private readonly Database database;
        private readonly ObjectNotLoadedEventHandler objectNotLoaded;
        private readonly RelationNotLoadedEventHandler relationNotLoaded;
        private readonly Xml xml;

        private readonly Dictionary<long, IObjectType> objectTypeByObjectId;
        private readonly Dictionary<long, long> objectVersionByObjectId;

        private readonly Dictionary<IRelationType, Dictionary<long, long>> associationIdByRoleIdByRelationTypeId;
        private readonly Dictionary<IRelationType, Dictionary<long, object>> roleByAssociationIdByRelationTypeId;

        internal Load(Database database, ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded, Xml xml)
        {
            this.database = database;
            this.objectNotLoaded = objectNotLoaded;
            this.relationNotLoaded = relationNotLoaded;
            this.xml = xml;

            this.objectTypeByObjectId = new Dictionary<long, IObjectType>();
            this.objectVersionByObjectId = new Dictionary<long, long>();

            this.associationIdByRoleIdByRelationTypeId = new Dictionary<IRelationType, Dictionary<long, long>>();
            this.roleByAssociationIdByRelationTypeId = new Dictionary<IRelationType, Dictionary<long, object>>();
        }

        internal void Execute()
        {
            this.Read();

            this.database.Init();

            using (var connection = new SqlConnection(this.database.ConnectionString))
            {
                try
                {
                    connection.Open();

                    this.WriteObjectsTable(connection);

                    this.WriteObjectTables(connection);

                    this.WriteRelationTables(connection);

                    connection.Close();
                }
                catch (Exception e)
                {
                    try
                    {
                        connection.Close();
                    }
                    finally
                    {
                        this.database.Init();
                        throw e;
                    }
                }
            }
        }

        private static string[] GetColumnNames(SqlConnection connection, string tableName)
        {
            var command = new SqlCommand("SELECT * FROM " + tableName, connection);

            var columns = new List<string>();
            using (var reader = command.ExecuteReader())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }
            }

            return columns.ToArray();
        }

        private void Read()
        {
            Serialization.CheckVersion(this.xml.Population.Version);

            foreach (var xmlObjectType in this.xml.Population.Objects.ObjectTypes)
            {
                if (!string.IsNullOrWhiteSpace(xmlObjectType.Objects))
                {
                    var objectTypeId = xmlObjectType.Id;
                    var objectType = this.database.ObjectFactory.GetObjectTypeForType(objectTypeId);

                    var canLoad = objectType != null && objectType.IsClass;

                    var objectIdsString = xmlObjectType.Objects;
                    var objectIdStringArray = objectIdsString.Split(Serialization.ObjectsSplitterCharArray);

                    foreach (var objectIdString in objectIdStringArray)
                    {
                        var objectArray = objectIdString.Split(Serialization.ObjectSplitterCharArray);
                        var objectId = long.Parse(objectArray[0]);

                        if (canLoad)
                        {
                            var version = objectArray.Length > 1 ? long.Parse(objectArray[1]) : Reference.InitialVersion;

                            this.objectTypeByObjectId.Add(objectId, objectType);
                            this.objectVersionByObjectId.Add(objectId, version);
                        }
                        else
                        {
                            this.OnObjectNotLoaded(objectTypeId, objectId);
                        }
                    }
                }
            }

            foreach (var xmlRelationType in this.xml.Population.Relations.RelationTypes)
            {
                if (xmlRelationType is RelationTypeUnit xmlRelationTypeUnit)
                {
                    var relationTypeId = xmlRelationTypeUnit.Id;
                    var relationType = (IRelationType)this.database.MetaPopulation.Find(relationTypeId);

                    if (relationType == null || relationType.RoleType.ObjectType.IsComposite)
                    {
                        foreach (var xmlRelation in xmlRelationTypeUnit.Relations)
                        {
                            this.OnRelationNotLoaded(relationTypeId, xmlRelation.Association, xmlRelation.Role);
                        }
                    }
                    else
                    {
                        var roleByObjectId = this.GetRoleByAssociationId(relationType);
                        this.ReadUnitRelations(xmlRelationTypeUnit, relationType, roleByObjectId);
                    }
                }
                else
                {
                    var xmlRelationTypeComposite = (RelationTypeComposite)xmlRelationType;
                    var relationTypeId = xmlRelationTypeComposite.Id;
                    var relationType = (IRelationType)this.database.MetaPopulation.Find(relationTypeId);

                    if (relationType == null || relationType.RoleType.ObjectType.IsUnit)
                    {
                        foreach (var xmlRelation in xmlRelationTypeComposite.Relations)
                        {
                            this.OnRelationNotLoaded(relationTypeId, xmlRelation.Association, xmlRelation.Role);
                        }
                    }
                    else
                    {
                        if (!(relationType.AssociationType.IsMany && relationType.RoleType.IsMany) && relationType.ExistExclusiveClasses && relationType.RoleType.IsMany)
                        {
                            var associationIdByRoleId = this.GetAssociationIdByRoleId(relationType);
                            this.ReadCompositeRelations(xmlRelationTypeComposite, relationType, associationIdByRoleId);
                        }
                        else
                        {
                            var roleByObjectId = this.GetRoleByAssociationId(relationType);
                            this.ReadCompositeRelations(xmlRelationTypeComposite, relationType, roleByObjectId);
                        }
                    }
                }
            }
        }

        private void ReadUnitRelations(RelationTypeUnit xmlRelationTypeUnit, IRelationType relationType, Dictionary<long, object> roleByObjectId)
        {
            foreach (var xmlRelation in xmlRelationTypeUnit.Relations)
            {
                var associationId = xmlRelation.Association;
                this.objectTypeByObjectId.TryGetValue(associationId, out var associationConcreteClass);

                if (xmlRelation.Role == null)
                {
                    if (associationConcreteClass == null || !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                    {
                        this.OnRelationNotLoaded(relationType.Id, associationId, string.Empty);
                    }
                    else
                    {
                        switch (((IUnit)relationType.RoleType.ObjectType).UnitTag)
                        {
                            case UnitTags.String:
                            {
                                roleByObjectId.Add(associationId, string.Empty);
                            }

                            break;

                            case UnitTags.Binary:
                            {
                                roleByObjectId.Add(associationId, EmptyByteArray);
                            }

                            break;

                            default:
                                this.OnRelationNotLoaded(relationType.Id, associationId, string.Empty);
                                break;
                        }
                    }
                }
                else
                {
                    var value = xmlRelation.Role;
                    if (associationConcreteClass == null || !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                    {
                        this.OnRelationNotLoaded(relationType.Id, associationId, value);
                    }
                    else
                    {
                        try
                        {
                            var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).UnitTag;
                            var unit = Serialization.ReadString(value, unitTypeTag);

                            roleByObjectId.Add(associationId, unit);
                        }
                        catch
                        {
                            this.OnRelationNotLoaded(relationType.Id, associationId, value);
                        }
                    }
                }
            }
        }

        private void ReadCompositeRelations(RelationTypeComposite xmlRelationTypeComposite, IRelationType relationType, Dictionary<long, long> associationIdByRoleId)
        {
            foreach (var xmlRelation in xmlRelationTypeComposite.Relations)
            {
                if (!string.IsNullOrWhiteSpace(xmlRelation.Role))
                {
                    var associationId = xmlRelation.Association;
                    this.objectTypeByObjectId.TryGetValue(associationId, out var associationConcreteClass);

                    var value = xmlRelation.Role;
                    var rs = value.Split(Serialization.ObjectsSplitterCharArray);

                    if (associationConcreteClass == null ||
                        !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass) ||
                        (relationType.RoleType.IsOne && rs.Length > 1))
                    {
                        foreach (var r in rs)
                        {
                            this.OnRelationNotLoaded(relationType.Id, associationId, r);
                        }
                    }
                    else
                    {
                        foreach (var r in rs)
                        {
                            var roleId = long.Parse(r);
                            this.objectTypeByObjectId.TryGetValue(roleId, out var roleConcreteClass);

                            if (roleConcreteClass == null ||
                                !this.database.ContainsConcreteClass(relationType.RoleType.ObjectType, roleConcreteClass))
                            {
                                this.OnRelationNotLoaded(relationType.Id, associationId, r);
                            }
                            else
                            {
                                associationIdByRoleId.Add(roleId, associationId);
                            }
                        }
                    }
                }
            }
        }

        private void ReadCompositeRelations(RelationTypeComposite xmlRelationTypeComposite, IRelationType relationType, Dictionary<long, object> roleByAssociationId)
        {
            foreach (var xmlRelation in xmlRelationTypeComposite.Relations)
            {
                if (!string.IsNullOrWhiteSpace(xmlRelation.Role))
                {
                    var associationId = xmlRelation.Association;
                    this.objectTypeByObjectId.TryGetValue(associationId, out var associationConcreteClass);

                    var value = xmlRelation.Role;
                    var rs = value.Split(Serialization.ObjectsSplitterCharArray);

                    if (associationConcreteClass == null
                        || !this.database.ContainsConcreteClass(
                            relationType.AssociationType.ObjectType,
                            associationConcreteClass) || (relationType.RoleType.IsOne && rs.Length > 1))
                    {
                        foreach (var r in rs)
                        {
                            this.OnRelationNotLoaded(relationType.Id, associationId, r);
                        }
                    }
                    else
                    {
                        if (relationType.RoleType.IsOne)
                        {
                            var roleId = long.Parse(rs[0]);
                            roleByAssociationId.Add(associationId, roleId);
                        }
                        else
                        {
                            var roleList = new List<long>();
                            foreach (var r in rs)
                            {
                                var role = long.Parse(r);
                                this.objectTypeByObjectId.TryGetValue(role, out var roleConcreteClass);

                                if (roleConcreteClass == null || !this.database.ContainsConcreteClass(
                                        relationType.RoleType.ObjectType,
                                        roleConcreteClass))
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationId, r);
                                }
                                else
                                {
                                    roleList.Add(role);
                                }
                            }

                            roleByAssociationId.Add(associationId, roleList.ToArray());
                        }
                    }
                }
            }
        }

        private void WriteObjectsTable(SqlConnection connection)
        {
            var tableName = this.database.Mapping.TableNameForObjects;

            var columnNames = GetColumnNames(connection, tableName);
            var objectsTableReader = new ObjectsTableReader(this.objectTypeByObjectId, this.objectVersionByObjectId, columnNames);

            using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, null))
            {
                sqlBulkCopy.BulkCopyTimeout = 0;
                sqlBulkCopy.BatchSize = 5000;
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.WriteToServer(objectsTableReader);
            }
        }

        private void WriteObjectTables(SqlConnection connection)
        {
            var mapping = this.database.Mapping;

            var associationIdsByClass = this.objectTypeByObjectId.GroupBy(v => (IClass)v.Value, v => v.Key).ToDictionary(g => g.Key, g => g.ToArray());

            foreach (var pair in associationIdsByClass)
            {
                var @class = pair.Key;
                var objectIds = pair.Value;

                var tableName = mapping.TableNameForObjectByClass[@class];

                var columnNames = GetColumnNames(connection, tableName);
                var objectTableReader = new ObjectTableReader(@class, mapping, objectIds, this.associationIdByRoleIdByRelationTypeId, this.roleByAssociationIdByRelationTypeId, columnNames);

                using (var sqlBulkCopy = new SqlBulkCopy(connection))
                {
                    sqlBulkCopy.BulkCopyTimeout = 0;
                    sqlBulkCopy.BatchSize = 5000;
                    sqlBulkCopy.DestinationTableName = tableName;
                    sqlBulkCopy.WriteToServer(objectTableReader);
                }
            }
        }

        private void WriteRelationTables(SqlConnection connection)
        {
            var mapping = this.database.Mapping;

            foreach (var relationType in mapping.Database.MetaPopulation.RelationTypes)
            {
                var associationType = relationType.AssociationType;
                var roleType = relationType.RoleType;

                if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !relationType.ExistExclusiveClasses))
                {
                    if (this.roleByAssociationIdByRelationTypeId.TryGetValue(relationType, out var roleByAssociationId))
                    {
                        var tableName = mapping.TableNameForRelationByRelationType[relationType];
                        var columnNames = GetColumnNames(connection, tableName);

                        var relationTableReader = new RelationTableReader(roleByAssociationId, columnNames);

                        using (var sqlBulkCopy = new SqlBulkCopy(connection))
                        {
                            sqlBulkCopy.BulkCopyTimeout = 0;
                            sqlBulkCopy.BatchSize = 5000;
                            sqlBulkCopy.DestinationTableName = tableName;
                            sqlBulkCopy.WriteToServer(relationTableReader);
                        }
                    }
                }
            }
        }

        private Dictionary<long, long> GetAssociationIdByRoleId(IRelationType relationType)
        {
            if (!this.associationIdByRoleIdByRelationTypeId.TryGetValue(relationType, out var associationIdByRoleId))
            {
                associationIdByRoleId = new Dictionary<long, long>();
                this.associationIdByRoleIdByRelationTypeId[relationType] = associationIdByRoleId;
            }

            return associationIdByRoleId;
        }

        private Dictionary<long, object> GetRoleByAssociationId(IRelationType relationType)
        {
            if (!this.roleByAssociationIdByRelationTypeId.TryGetValue(relationType, out var roleByAssociationId))
            {
                roleByAssociationId = new Dictionary<long, object>();
                this.roleByAssociationIdByRelationTypeId[relationType] = roleByAssociationId;
            }

            return roleByAssociationId;
        }

        #region Load Errors

        private void OnObjectNotLoaded(Guid objectTypeId, long allorsObjectId)
        {
            if (this.objectNotLoaded != null)
            {
                this.objectNotLoaded(this, new ObjectNotLoadedEventArgs(objectTypeId, allorsObjectId));
            }
            else
            {
                throw new Exception("Object not loaded: " + objectTypeId + ":" + allorsObjectId);
            }
        }

        private void OnRelationNotLoaded(Guid relationTypeId, long associationObjectId, string roleContents)
        {
            var args = new RelationNotLoadedEventArgs(relationTypeId, associationObjectId, roleContents);
            if (this.relationNotLoaded != null)
            {
                this.relationNotLoaded(this, args);
            }
            else
            {
                throw new Exception("Role not loaded: " + args);
            }
        }

        #endregion Load Errors
    }
}

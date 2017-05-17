// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Load.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Xml;

    using Adapters;

    using Allors.Meta;

    internal class Load
    {
        private static readonly byte[] EmptyByteArray = new byte[0];

        private readonly Database database;
        private readonly ObjectNotLoadedEventHandler objectNotLoaded;
        private readonly RelationNotLoadedEventHandler relationNotLoaded;
        private readonly XmlReader reader;

        private readonly Dictionary<long, IObjectType> objectTypeByObjectId;
        private readonly Dictionary<long, long> objectVersionByObjectId;

        private readonly Dictionary<IRelationType, Dictionary<long, long>> associationIdByRoleIdByRelationTypeId;
        private readonly Dictionary<IRelationType, Dictionary<long, object>> roleByAssociationIdByRelationTypeId;

        internal Load(Database database, ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded, XmlReader reader)
        {
            this.database = database;
            this.objectNotLoaded = objectNotLoaded;
            this.relationNotLoaded = relationNotLoaded;
            this.reader = reader;

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

        private bool Read()
        {
            while (this.reader.Read())
            {
                // only process elements, ignore others
                if (this.reader.NodeType.Equals(XmlNodeType.Element))
                {
                    if (this.reader.Name.Equals(Serialization.Population))
                    {
                        Serialization.CheckVersion(this.reader);

                        if (!this.reader.IsEmptyElement)
                        {
                            this.ReadPopulation();
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        private void ReadPopulation()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    // only process elements, ignore others
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Objects))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.ReadObjects();
                            }
                        }
                        else if (this.reader.Name.Equals(Serialization.Relations))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.ReadRelations();
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.Population + ">");
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.Population))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Population + ">");
                        }

                        return;
                }
            }
        }

        private void ReadObjects()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Database))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.ReadObjectTypes();
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace objects in a Database.");
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.Objects + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.Objects))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Objects + ">");
                        }

                        return;
                }
            }
        }

        private void ReadObjectTypes()
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    // only process elements, ignore others
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.ObjectType))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                var objectTypeIdString = this.reader.GetAttribute(Serialization.Id);
                                if (string.IsNullOrEmpty(objectTypeIdString))
                                {
                                    throw new Exception("Object type id is missing");
                                }

                                var objectTypeId = new Guid(objectTypeIdString);
                                var objectType = this.database.ObjectFactory.GetObjectTypeForType(objectTypeId);

                                var canLoad = objectType != null && objectType.IsClass;

                                var objectIdsString = this.reader.ReadElementContentAsString();
                                var objectIdStringArray = objectIdsString.Split(Serialization.ObjectsSplitterCharArray);

                                foreach (var objectIdString in objectIdStringArray)
                                {
                                    if (canLoad)
                                    {
                                        var objectArray = objectIdString.Split(Serialization.ObjectSplitterCharArray);

                                        var objectId = long.Parse(objectArray[0]);
                                        var version = objectArray.Length > 1 ? long.Parse(objectArray[1]) : Reference.InitialVersion;
                                        
                                        this.objectTypeByObjectId.Add(objectId, objectType);
                                        this.objectVersionByObjectId.Add(objectId, version);
                                    }
                                    else
                                    {
                                        this.OnObjectNotLoaded(objectTypeId, objectIdString);
                                    }
                                }

                                skip = reader.IsStartElement() ||
                                       (reader.NodeType == XmlNodeType.EndElement && this.reader.Name.Equals(Serialization.Database));
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.Database + ">");
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.Database))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Database + ">");
                        }

                        return;
                }
            }
        }

        private void ReadRelations()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Database))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.ReadRelationTypes();
                            }
                        }
                        else if (this.reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace relations in a Database.");
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.Relations + ">");
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.Relations))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Relations + ">");
                        }

                        return;
                }
            }
        }

        private void ReadRelationTypes()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    // only process elements, ignore others
                    case XmlNodeType.Element:
                        if (!this.reader.IsEmptyElement)
                        {
                            if (this.reader.Name.Equals(Serialization.RelationTypeUnit)
                                || this.reader.Name.Equals(Serialization.RelationTypeComposite))
                            {
                                var relationTypeIdString = this.reader.GetAttribute(Serialization.Id);
                                if (string.IsNullOrEmpty(relationTypeIdString))
                                {
                                    throw new Exception("Relation type has no id");
                                }

                                var relationTypeId = new Guid(relationTypeIdString);
                                var relationType = (IRelationType)this.database.MetaPopulation.Find(relationTypeId);

                                if (this.reader.Name.Equals(Serialization.RelationTypeUnit))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType.IsComposite)
                                    {
                                        this.CantLoadUnitRole(relationTypeId);
                                    }
                                    else
                                    {
                                        var roleByObjectId = GetRoleByAssociationId(relationType);
                                        this.ReadUnitRelations(relationType, roleByObjectId);
                                    }
                                }
                                else if (this.reader.Name.Equals(Serialization.RelationTypeComposite))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType.IsUnit)
                                    {
                                        this.CantLoadCompositeRole(relationTypeId);
                                    }
                                    else
                                    {
                                        if (!(relationType.AssociationType.IsMany && relationType.RoleType.IsMany) && relationType.ExistExclusiveClasses && relationType.RoleType.IsMany)
                                        {
                                            var associationIdByRoleId = GetAssociationIdByRoleId(relationType);
                                            this.ReadCompositeRelations(relationType, associationIdByRoleId);
                                        }
                                        else
                                        {
                                            var roleByObjectId = GetRoleByAssociationId(relationType);
                                            this.ReadCompositeRelations(relationType, roleByObjectId);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.Database + ">");
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.Database))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Database + ">");
                        }

                        return;
                }
            }
        }

        private void ReadUnitRelations(IRelationType relationType, Dictionary<long, object> roleByObjectId)
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = this.reader.GetAttribute(Serialization.Association);
                            if (string.IsNullOrEmpty(associationIdString))
                            {
                                throw new Exception("Association id is missing");
                            }

                            var associationId = long.Parse(associationIdString);
                            IObjectType associationConcreteClass;
                            this.objectTypeByObjectId.TryGetValue(associationId, out associationConcreteClass);

                            if (this.reader.IsEmptyElement)
                            {
                                if (associationConcreteClass == null || !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationIdString, string.Empty);
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
                                            this.OnRelationNotLoaded(relationType.Id, associationIdString, string.Empty);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                var value = this.reader.ReadElementContentAsString();
                                if (associationConcreteClass == null || !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationIdString, value);
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
                                        this.OnRelationNotLoaded(relationType.Id, associationIdString, value);
                                    }
                                }

                                skip = reader.IsStartElement() ||
                                       (reader.NodeType == XmlNodeType.EndElement && this.reader.Name.Equals(Serialization.RelationTypeUnit));
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.RelationTypeUnit + ">");
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.RelationTypeUnit))
                        {
                            throw new Exception("Expected closing element </" + Serialization.RelationTypeUnit + ">");
                        }

                        return;
                }
            }
        }

        private void ReadCompositeRelations(IRelationType relationType, Dictionary<long,long> associationIdByRoleId)
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = this.reader.GetAttribute(Serialization.Association);
                            if (string.IsNullOrEmpty(associationIdString))
                            {
                                throw new Exception("Association id is missing");
                            }

                            if (this.reader.IsEmptyElement)
                            {
                                throw new Exception("Role is missing");
                            }

                            if (!this.reader.IsEmptyElement)
                            {

                                var associationId = long.Parse(associationIdString);
                                IObjectType associationConcreteClass;
                                this.objectTypeByObjectId.TryGetValue(associationId, out associationConcreteClass);

                                var value = this.reader.ReadElementContentAsString();
                                var rs = value.Split(Serialization.ObjectsSplitterCharArray);

                                if (associationConcreteClass == null ||
                                    !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType,associationConcreteClass) ||
                                    (relationType.RoleType.IsOne && rs.Length > 1))
                                {
                                    foreach (var r in rs)
                                    {
                                        this.OnRelationNotLoaded(relationType.Id, associationIdString, r);
                                    }
                                }
                                else
                                {
                                    foreach (var r in rs)
                                    {
                                        var roleId = long.Parse(r);
                                        IObjectType roleConcreteClass;
                                        this.objectTypeByObjectId.TryGetValue(roleId, out roleConcreteClass);

                                        if (roleConcreteClass == null ||
                                            !this.database.ContainsConcreteClass(relationType.RoleType.ObjectType, roleConcreteClass))
                                        {
                                            this.OnRelationNotLoaded(relationType.Id, associationIdString, r);
                                        }
                                        else
                                        {
                                            associationIdByRoleId.Add(roleId, associationId);
                                        }
                                    }
                                }

                                skip = reader.IsStartElement() ||
                                       (reader.NodeType == XmlNodeType.EndElement &&
                                        this.reader.Name.Equals(Serialization.RelationTypeComposite));
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.RelationTypeComposite + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.RelationTypeComposite))
                        {
                            throw new Exception("Expected closing element </" + Serialization.RelationTypeComposite + ">");
                        }

                        return;
                }
            }
        }

        private void ReadCompositeRelations(IRelationType relationType, Dictionary<long, object> roleByAssociationId)
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = this.reader.GetAttribute(Serialization.Association);
                            if (string.IsNullOrEmpty(associationIdString))
                            {
                                throw new Exception("Association id is missing");
                            }

                            if (this.reader.IsEmptyElement)
                            {
                                throw new Exception("Role is missing");
                            }

                            var associationId = long.Parse(associationIdString);
                            IObjectType associationConcreteClass;
                            this.objectTypeByObjectId.TryGetValue(associationId, out associationConcreteClass);

                            if (!this.reader.IsEmptyElement)
                            {

                                var value = this.reader.ReadElementContentAsString();
                                var rs = value.Split(Serialization.ObjectsSplitterCharArray);

                                if (associationConcreteClass == null ||
                                    !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass) ||
                                    (relationType.RoleType.IsOne && rs.Length > 1))
                                {
                                    foreach (var r in rs)
                                    {
                                        this.OnRelationNotLoaded(relationType.Id, associationIdString, r);
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
                                            IObjectType roleConcreteClass;
                                            this.objectTypeByObjectId.TryGetValue(role, out roleConcreteClass);

                                            if (roleConcreteClass == null ||
                                                !this.database.ContainsConcreteClass(relationType.RoleType.ObjectType, roleConcreteClass))
                                            {
                                                this.OnRelationNotLoaded(relationType.Id, associationIdString, r);
                                            }
                                            else
                                            {
                                                roleList.Add(role);
                                            }
                                        }

                                        roleByAssociationId.Add(associationId, roleList.ToArray());
                                    }
                                }

                                skip = reader.IsStartElement() ||
                                       (reader.NodeType == XmlNodeType.EndElement &&
                                        this.reader.Name.Equals(Serialization.RelationTypeComposite));
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + Serialization.RelationTypeComposite + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.RelationTypeComposite))
                        {
                            throw new Exception("Expected closing element </" + Serialization.RelationTypeComposite + ">");
                        }

                        return;
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
                    Dictionary<long, object> roleByAssociationId;
                    if (this.roleByAssociationIdByRelationTypeId.TryGetValue(relationType, out roleByAssociationId))
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
            Dictionary<long, long> associationIdByRoleId;
            if (!this.associationIdByRoleIdByRelationTypeId.TryGetValue(relationType, out associationIdByRoleId))
            {
                associationIdByRoleId = new Dictionary<long, long>();
                this.associationIdByRoleIdByRelationTypeId[relationType] = associationIdByRoleId;
            }
            return associationIdByRoleId;
        }

        private Dictionary<long, object> GetRoleByAssociationId(IRelationType relationType)
        {
            Dictionary<long, object> roleByAssociationId;
            if (!this.roleByAssociationIdByRelationTypeId.TryGetValue(relationType, out roleByAssociationId))
            {
                roleByAssociationId = new Dictionary<long, object>();
                this.roleByAssociationIdByRelationTypeId[relationType] = roleByAssociationId;
            }
            return roleByAssociationId;
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

        #region Load Errors
        private void OnObjectNotLoaded(Guid objectTypeId, string allorsObjectId)
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

        private void OnRelationNotLoaded(Guid relationTypeId, string associationObjectId, string roleContents)
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

        private void CantLoadUnitRole(Guid relationTypeId)
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var a = this.reader.GetAttribute(Serialization.Association);
                            var value = string.Empty;

                            if (!this.reader.IsEmptyElement)
                            {
                                value = this.reader.ReadElementContentAsString();
                            }

                            this.OnRelationNotLoaded(relationTypeId, a, value);
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void CantLoadCompositeRole(Guid relationTypeId)
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = this.reader.GetAttribute(Serialization.Association);
                            if (string.IsNullOrEmpty(associationIdString))
                            {
                                throw new Exception("Association id is missing");
                            }

                            if (this.reader.IsEmptyElement)
                            {
                                this.OnRelationNotLoaded(relationTypeId, associationIdString, null);
                            }
                            else
                            {
                                var value = this.reader.ReadElementContentAsString();
                                var rs = value.Split(Serialization.ObjectsSplitterCharArray);
                                foreach (var r in rs)
                                {
                                    this.OnRelationNotLoaded(relationTypeId, associationIdString, r);
                                }
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }
        #endregion
    }
}
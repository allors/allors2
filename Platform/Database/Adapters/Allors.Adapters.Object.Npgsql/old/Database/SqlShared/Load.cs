// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Load.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using Allors.Meta;

    public abstract class Load
    {
        private static readonly byte[] EmptyByteArray = new byte[0];

        private readonly Database database;
        private readonly ObjectNotLoadedEventHandler objectNotLoaded;
        private readonly RelationNotLoadedEventHandler relationNotLoaded;
        private readonly XmlReader reader;

        private readonly Dictionary<long, IClass> objectTypeByObjectId;

        protected Load(Database database, ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded, XmlReader reader)
        {
            this.database = database;
            this.objectNotLoaded = objectNotLoaded;
            this.relationNotLoaded = relationNotLoaded;
            this.reader = reader;

            this.objectTypeByObjectId = new Dictionary<long, IClass>();
        }

        public void Execute(ManagementSession session)
        {
            while (this.reader.Read())
            {
                // only process elements, ignore others
                if (this.reader.NodeType.Equals(XmlNodeType.Element))
                {
                    if (this.reader.Name.Equals(Serialization.Population))
                    {
                        // TODO: check version
                        // Serialization.CheckVersion(this.xml.Population.Version);

                        if (!this.reader.IsEmptyElement)
                        {
                            this.LoadPopulation(session);
                        }

                        return;
                    }
                }
            }
        }

        protected virtual void LoadPopulation(ManagementSession session)
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
                                this.LoadObjects(session);
                            }

                            this.LoadObjectsPostProcess(session);
                        }
                        else if (reader.Name.Equals(Serialization.Relations))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadRelations(session);
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

        protected virtual void LoadObjects(ManagementSession session)
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
                                this.LoadDatabaseObjectTypes(session);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace objects in a database.");
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

        protected virtual void LoadDatabaseObjectTypes(ManagementSession session)
        {
            while (this.reader.Read())
            {
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
                                var canLoad = objectType is IClass;

                                var objectIdsString = this.reader.ReadString();
                                var objectIdStringArray = objectIdsString.Split(Serialization.ObjectsSplitterCharArray);

                                var objectIds = new long[objectIdStringArray.Length];
                                for (var i = 0; i < objectIds.Length; i++)
                                {
                                    var objectIdString = objectIdStringArray[i];
                                    var objectId = long.Parse(objectIdString);
 
                                    if (canLoad)
                                    {
                                        objectIds[i] = objectId;
                                        this.objectTypeByObjectId[objectId] = (IClass)objectType;
                                    }
                                    else
                                    {
                                        this.OnObjectNotLoaded(objectTypeId, objectId);
                                    }
                                }

                                if (canLoad)
                                {
                                    var loadObjects = session.LoadObjectsFactory.Create(objectType);
                                    loadObjects.Execute((IClass)objectType, objectIds);
                                }
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

        protected virtual void LoadObjectsPostProcess(ManagementSession session)
        {
            foreach (var type in this.database.MetaPopulation.Composites)
            {
                if (type is IClass)
                {
                    var sql = new StringBuilder();
                    sql.Append("INSERT INTO " + this.database.Schema.Objects + " (" + this.database.Schema.ObjectId + "," + this.database.Schema.TypeId + "," + this.database.Schema.CacheId + ")\n");
                    sql.Append("SELECT " + this.database.Schema.ObjectId + "," + this.database.Schema.TypeId + ", " + Reference.InitialVersion + "\n");
                    sql.Append("FROM " + this.database.Schema.Table(type.ExclusiveClass));

                    lock (this)
                    {
                        using (var command = session.CreateCommand(sql.ToString()))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        protected virtual void LoadRelations(ManagementSession session)
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
                                this.LoadDatabaseRelationTypes(session);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace relations in a database.");
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

        protected virtual void LoadDatabaseRelationTypes(ManagementSession session)
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
                                    if (relationType == null || relationType.RoleType.ObjectType is IComposite)
                                    {
                                        this.CantLoadUnitRole(relationTypeId);
                                    }
                                    else
                                    {
                                        var relationsByExclusiveLeafClass = new Dictionary<IObjectType, List<UnitRelation>>();
                                        this.LoadUnitRelations(relationType, relationsByExclusiveLeafClass);

                                        foreach (var dictionaryEntry in relationsByExclusiveLeafClass)
                                        {
                                            var exclusiveLeafClass = dictionaryEntry.Key;
                                            var relations = dictionaryEntry.Value;
                                            var loadUnitRelations = session.LoadUnitRelationsFactory.Create();
                                            loadUnitRelations.Execute(relations, exclusiveLeafClass, relationType.RoleType);
                                        }
                                    }
                                }
                                else if (this.reader.Name.Equals(Serialization.RelationTypeComposite))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType is IUnit)
                                    {
                                        this.CantLoadCompositeRole(relationTypeId);
                                    }
                                    else
                                    {
                                        var relations = new List<CompositeRelation>();
                                        this.LoadCompositeRelations(relationType, relations);
                                        if (relations.Count > 0)
                                        {
                                            var loadCompositeRelations = session.LoadCompositeRelationsFactory.Create(relationType.RoleType);
                                            loadCompositeRelations.Execute(relations);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(
                                    "Unknown child element <" + this.reader.Name + "> in parent element <"
                                    + Serialization.Database + ">");
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

        protected void LoadUnitRelations(IRelationType relationType, Dictionary<IObjectType, List<UnitRelation>> relationsByExclusiveLeafClass)
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

                            var associationId = long.Parse(associationIdString);
                            IClass associationConcreteClass;
                            this.objectTypeByObjectId.TryGetValue(associationId, out associationConcreteClass);

                            if (this.reader.IsEmptyElement)
                            {
                                if (associationConcreteClass == null ||
                                    !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationId, string.Empty);
                                }
                                else
                                {
                                    var exclusiveLeafClass = associationConcreteClass.ExclusiveClass;
                                    var unitType = (IUnit)relationType.RoleType.ObjectType;
                                    switch (unitType.UnitTag)
                                    {
                                        case UnitTags.String:
                                            {
                                                List<UnitRelation> relations;
                                                if (!relationsByExclusiveLeafClass.TryGetValue(associationConcreteClass.ExclusiveClass, out relations))
                                                {
                                                    relations = new List<UnitRelation>();
                                                    relationsByExclusiveLeafClass[exclusiveLeafClass] = relations;
                                                }

                                                var unitRelation = new UnitRelation(associationId, string.Empty);
                                                relations.Add(unitRelation);
                                            }
                                            
                                            break;

                                        case UnitTags.Binary:
                                            {
                                                List<UnitRelation> relations;
                                                if (!relationsByExclusiveLeafClass.TryGetValue(associationConcreteClass.ExclusiveClass, out relations))
                                                {
                                                    relations = new List<UnitRelation>();
                                                    relationsByExclusiveLeafClass[exclusiveLeafClass] = relations;
                                                }

                                                var unitRelation = new UnitRelation(associationId, EmptyByteArray);
                                                relations.Add(unitRelation);
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
                                var value = this.reader.ReadString();
                                if (associationConcreteClass == null ||
                                   !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass))
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationId, value);
                                }
                                else
                                {
                                    try
                                    {
                                        var exclusiveLeafClass = associationConcreteClass.ExclusiveClass;
                                        var unitType = (IUnit)relationType.RoleType.ObjectType;
                                        var unitTypeTag = unitType.UnitTag;
                                        var unit = Serialization.ReadString(value, unitTypeTag);

                                        List<UnitRelation> relations;
                                        if (!relationsByExclusiveLeafClass.TryGetValue(associationConcreteClass.ExclusiveClass, out relations))
                                        {
                                            relations = new List<UnitRelation>();
                                            relationsByExclusiveLeafClass[exclusiveLeafClass] = relations;
                                        }

                                        var unitRelation = new UnitRelation(associationId, unit);
                                        relations.Add(unitRelation);
                                    }
                                    catch
                                    {
                                        this.OnRelationNotLoaded(relationType.Id, associationId, value);
                                    }
                                }
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

        protected void LoadCompositeRelations(IRelationType relationType, List<CompositeRelation> relations)
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
                                throw new Exception("Role is missing");
                            }

                            var association = long.Parse(associationIdString);
                            IClass associationConcreteClass;
                            this.objectTypeByObjectId.TryGetValue(association, out associationConcreteClass);

                            var value = this.reader.ReadString();
                            var rs = value.Split(Serialization.ObjectsSplitterCharArray);

                            if (associationConcreteClass == null ||
                                !this.database.ContainsConcreteClass(relationType.AssociationType.ObjectType, associationConcreteClass) || 
                                (relationType.RoleType.IsOne && rs.Length > 1))
                            {
                                foreach (var r in rs)
                                {
                                    this.OnRelationNotLoaded(relationType.Id, association, r);
                                }
                            }
                            else
                            {
                                foreach (var r in rs)
                                {
                                    var role = long.Parse(r);
                                    IClass roleConcreteClass;
                                    this.objectTypeByObjectId.TryGetValue(role, out roleConcreteClass);

                                    if (roleConcreteClass == null ||
                                        !this.database.ContainsConcreteClass((IComposite)relationType.RoleType.ObjectType, roleConcreteClass))
                                    {
                                        this.OnRelationNotLoaded(relationType.Id, association, r);
                                    }
                                    else
                                    {
                                        relations.Add(new CompositeRelation(association, role));
                                    }
                                }
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
                            var associationId = long.Parse(a);
                            var value = string.Empty;

                            if (!this.reader.IsEmptyElement)
                            {
                                value = this.reader.ReadString();
                            }

                            this.OnRelationNotLoaded(relationTypeId, associationId, value);
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

                            var associationId = long.Parse(associationIdString);

                            if (this.reader.IsEmptyElement)
                            {
                                this.OnRelationNotLoaded(relationTypeId, associationId, null);
                            }
                            else
                            {
                                var value = this.reader.ReadString();
                                var rs = value.Split(Serialization.ObjectsSplitterCharArray);
                                foreach (var r in rs)
                                {
                                    this.OnRelationNotLoaded(relationTypeId, associationId, r);
                                }
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

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
    }
}
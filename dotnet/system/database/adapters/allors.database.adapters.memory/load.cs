// <copyright file="Import.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Adapters;

    using Meta;
    using Version = Allors.Version;

    public class Load
    {
        private static readonly byte[] emptyByteArray = Array.Empty<byte>();

        private readonly Transaction transaction;
        private readonly XmlReader reader;

        public Load(Transaction transaction, XmlReader reader)
        {
            this.transaction = transaction;
            this.reader = reader;
        }

        public void Execute()
        {
            while (this.reader.Read())
            {
                // only process elements, ignore others
                if (this.reader.NodeType.Equals(XmlNodeType.Element) && this.reader.Name.Equals(Serialization.Population))
                {
                    var version = this.reader.GetAttribute(Serialization.Version);
                    if (string.IsNullOrEmpty(version))
                    {
                        throw new ArgumentException("Save population has no version.");
                    }

                    Serialization.CheckVersion(int.Parse(version));

                    if (!this.reader.IsEmptyElement)
                    {
                        this.LoadPopulation();
                    }

                    break;
                }
            }
        }

        private void LoadPopulation()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Objects))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.LoadObjects();
                            }
                        }
                        else if (this.reader.Name.Equals(Serialization.Relations))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                this.LoadRelations();
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

        private void LoadObjects()
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
                                this.LoadObjectTypes();
                            }
                        }
                        else if (this.reader.Name.Equals(Serialization.Workspace))
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

        private void LoadObjectTypes()
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.ObjectType))
                        {
                            if (!this.reader.IsEmptyElement)
                            {
                                var objectTypeIdString = this.reader.GetAttribute(Serialization.Id);
                                if (string.IsNullOrEmpty(objectTypeIdString))
                                {
                                    throw new Exception("object type has no id");
                                }

                                var objectTypeId = new Guid(objectTypeIdString);
                                var objectType = this.transaction.Database.ObjectFactory.GetObjectType(objectTypeId);

                                var objectIdsString = this.reader.ReadElementContentAsString();
                                foreach (var objectIdString in objectIdsString.Split(Serialization.ObjectsSplitterCharArray))
                                {
                                    var objectArray = objectIdString.Split(Serialization.ObjectSplitterCharArray);

                                    var objectId = long.Parse(objectArray[0]);
                                    var objectVersion = objectArray.Length > 1
                                        ? long.Parse(objectArray[1])
                                        : (long) Version.DatabaseInitial;

                                    if (objectType is IClass)
                                    {
                                        this.transaction.InsertStrategy((IClass)objectType, objectId, objectVersion);
                                    }
                                    else
                                    {
                                        this.transaction.Database.OnObjectNotLoaded(objectTypeId, objectId);
                                    }
                                }

                                skip = this.reader.IsStartElement() ||
                                       (this.reader.NodeType == XmlNodeType.EndElement && this.reader.Name.Equals(Serialization.Database));
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                                Serialization.Database + ">");
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

        private void LoadRelations()
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
                                this.LoadDatabaseRelationTypes();
                            }
                        }
                        else if (this.reader.Name.Equals(Serialization.Workspace))
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

        private void LoadDatabaseRelationTypes()
        {
            while (this.reader.Read())
            {
                switch (this.reader.NodeType)
                {
                    // eat everything but elements
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
                                var relationType = (IRelationType)this.transaction.Database.MetaPopulation.FindById(relationTypeId);

                                if (this.reader.Name.Equals(Serialization.RelationTypeUnit))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType is IComposite)
                                    {
                                        this.CantLoadUnitRole(relationTypeId);
                                    }
                                    else
                                    {
                                        this.LoadUnitRelations(relationType);
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
                                        this.LoadCompositeRelations(relationType);
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

        private void LoadUnitRelations(IRelationType relationType)
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = this.reader.GetAttribute(Serialization.Association);
                            var associationId = long.Parse(associationIdString);
                            var strategy = this.LoadInstantiateStrategy(associationId);

                            var value = string.Empty;
                            if (!this.reader.IsEmptyElement)
                            {
                                value = this.reader.ReadElementContentAsString();

                                skip = this.reader.IsStartElement() ||
                                       (this.reader.NodeType == XmlNodeType.EndElement &&
                                       this.reader.Name.Equals(Serialization.RelationTypeUnit));
                            }

                            if (strategy == null)
                            {
                                this.transaction.Database.OnRelationNotLoaded(relationType.Id, associationId, value);
                            }
                            else
                            {
                                try
                                {
                                    this.transaction.Database.UnitRoleChecks(strategy, relationType.RoleType);
                                    if (this.reader.IsEmptyElement)
                                    {
                                        var unitType = (IUnit)relationType.RoleType.ObjectType;
                                        switch (unitType.Tag)
                                        {
                                            case UnitTags.String:
                                                strategy.SetUnitRole(relationType.RoleType, string.Empty);
                                                break;

                                            case UnitTags.Binary:
                                                strategy.SetUnitRole(relationType.RoleType, emptyByteArray);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var unitType = (IUnit)relationType.RoleType.ObjectType;
                                        var unitTypeTag = unitType.Tag;

                                        var unit = Serialization.ReadString(value, unitTypeTag);
                                        strategy.SetUnitRole(relationType.RoleType, unit);
                                    }
                                }
                                catch
                                {
                                    this.transaction.Database.OnRelationNotLoaded(relationType.Id, associationId, value);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                                Serialization.RelationTypeUnit + ">");
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

        private void LoadCompositeRelations(IRelationType relationType)
        {
            var skip = false;
            while (skip || this.reader.Read())
            {
                skip = false;

                switch (this.reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (this.reader.Name.Equals(Serialization.Relation))
                        {
                            var associationId = long.Parse(this.reader.GetAttribute(Serialization.Association));
                            var association = this.LoadInstantiateStrategy(associationId);

                            var value = string.Empty;
                            if (!this.reader.IsEmptyElement)
                            {
                                value = this.reader.ReadElementContentAsString();

                                skip = this.reader.IsStartElement() ||
                                       (this.reader.NodeType == XmlNodeType.EndElement &&
                                       this.reader.Name.Equals(Serialization.RelationTypeComposite));

                                var roleIdsString = value;
                                var roleIdStringArray = roleIdsString.Split(Serialization.ObjectsSplitterCharArray);

                                if (association == null ||
                                    !this.transaction.Database.ContainsClass(
                                        relationType.AssociationType.ObjectType, association.UncheckedObjectType) ||
                                    (relationType.RoleType.IsOne && roleIdStringArray.Length != 1))
                                {
                                    foreach (var roleId in roleIdStringArray)
                                    {
                                        this.transaction.Database.OnRelationNotLoaded(relationType.Id, associationId, roleId);
                                    }
                                }
                                else if (relationType.RoleType.IsOne)
                                {
                                    var roleIdString = long.Parse(roleIdStringArray[0]);
                                    var roleStrategy = this.LoadInstantiateStrategy(roleIdString);
                                    if (roleStrategy == null || !this.transaction.Database.ContainsClass((IComposite)relationType.RoleType.ObjectType, roleStrategy.UncheckedObjectType))
                                    {
                                        this.transaction.Database.OnRelationNotLoaded(relationType.Id, associationId, roleIdStringArray[0]);
                                    }
                                    else if (relationType.RoleType.AssociationType.IsMany)
                                    {
                                        association.SetCompositeRoleMany2One(relationType.RoleType, roleStrategy);
                                    }
                                    else
                                    {
                                        association.SetCompositeRoleOne2One(relationType.RoleType, roleStrategy);
                                    }
                                }
                                else
                                {
                                    var roleStrategies = new HashSet<Strategy>();
                                    foreach (var roleIdString in roleIdStringArray)
                                    {
                                        var roleId = long.Parse(roleIdString);
                                        var role = this.LoadInstantiateStrategy(roleId);
                                        if (role == null ||
                                            !this.transaction.Database.ContainsClass(
                                                (IComposite)relationType.RoleType.ObjectType,
                                                role.UncheckedObjectType))
                                        {
                                            this.transaction.Database.OnRelationNotLoaded(relationType.Id, associationId, roleId.ToString());
                                        }
                                        else
                                        {
                                            roleStrategies.Add(role);
                                        }
                                    }

                                    if (relationType.RoleType.AssociationType.IsMany)
                                    {
                                        association.SetCompositesRolesMany2Many(relationType.RoleType, roleStrategies);
                                    }
                                    else
                                    {
                                        association.SetCompositesRolesOne2Many(relationType.RoleType, roleStrategies);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                                Serialization.RelationTypeComposite + ">");
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (!this.reader.Name.Equals(Serialization.RelationTypeComposite))
                        {
                            throw new Exception("Expected closing element </" + Serialization.RelationTypeComposite +
                                                ">");
                        }

                        return;
                }
            }
        }

        private Strategy LoadInstantiateStrategy(long id) => this.transaction.GetStrategy(id);

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

                            this.transaction.Database.OnRelationNotLoaded(relationTypeId, long.Parse(a), value);
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
                            var associationId = long.Parse(associationIdString);
                            if (string.IsNullOrEmpty(associationIdString))
                            {
                                throw new Exception("Association id is missing");
                            }

                            if (this.reader.IsEmptyElement)
                            {
                                this.transaction.Database.OnRelationNotLoaded(relationTypeId, associationId, null);
                            }
                            else
                            {
                                var value = this.reader.ReadElementContentAsString();
                                foreach (var r in value.Split(Serialization.ObjectsSplitterCharArray))
                                {
                                    this.transaction.Database.OnRelationNotLoaded(relationTypeId, associationId, r);
                                }
                            }
                        }

                        break;

                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }
    }
}

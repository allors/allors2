// <copyright file="Import.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using Adapters;
    using Meta;
    using global::Npgsql;
    using NpgsqlTypes;

    public class Load
    {
        private readonly Database database;
        private readonly NpgsqlConnection connection;
        private readonly ObjectNotLoadedEventHandler objectNotLoaded;
        private readonly RelationNotLoadedEventHandler relationNotLoaded;

        private readonly Dictionary<long, IClass> classByObjectId;

        public Load(Database database, NpgsqlConnection connection, ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded)
        {
            this.database = database;
            this.connection = connection;
            this.objectNotLoaded = objectNotLoaded;
            this.relationNotLoaded = relationNotLoaded;

            this.classByObjectId = new Dictionary<long, IClass>();
        }

        public void Execute(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType.Equals(XmlNodeType.Element) && reader.Name.Equals(Serialization.Population))
                {
                    var version = reader.GetAttribute(Serialization.Version);
                    if (string.IsNullOrEmpty(version))
                    {
                        throw new ArgumentException("Save population has no version.");
                    }

                    Serialization.CheckVersion(int.Parse(version));

                    if (!reader.IsEmptyElement)
                    {
                        this.LoadPopulation(reader);
                    }

                    break;
                }
            }
        }

        private void LoadPopulation(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Objects))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadObjects(reader.ReadSubtree());
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Relations))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadRelations(reader.ReadSubtree());
                            }
                        }

                        break;
                }
            }
        }

        private void LoadObjects(XmlReader reader)
        {
            reader.MoveToContent();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Database))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadObjectsDatabase(reader.ReadSubtree());
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace objects in a database.");
                        }

                        break;
                }
            }
        }

        private void LoadObjectsDatabase(XmlReader reader)
        {
            var xmlObjects = new Objects(this.database, this.OnObjectNotLoaded, this.classByObjectId, reader);
            var mapping = this.database.Mapping;
            using (var writer = this.connection.BeginBinaryImport($"COPY {mapping.TableNameForObjects} ({Mapping.ColumnNameForObject}, {Mapping.ColumnNameForClass}, {Mapping.ColumnNameForVersion}) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (var values in xmlObjects)
                {
                    writer.StartRow();
                    writer.Write(values[0], NpgsqlDbType.Bigint);
                    writer.Write(values[1], NpgsqlDbType.Uuid);
                    writer.Write(values[2], NpgsqlDbType.Bigint);
                }

                writer.Complete();
            }

            // TODO: move this to a stored procedure
            // insert from _o table into class tables
            using (var transaction = this.connection.BeginTransaction())
            {
                foreach (var @class in this.database.MetaPopulation.DatabaseClasses)
                {
                    var tableName = this.database.Mapping.TableNameForObjectByClass[@class];

                    using (var command = this.connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.Text;
                        command.CommandText = $@"
insert into {tableName} (o, c)
select o, c from allors._o
where c = '{@class.Id}'";

                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        private void LoadRelations(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Database))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadRelationsDatabase(reader);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace relations in a database.");
                        }

                        break;
                }
            }
        }

        private void LoadRelationsDatabase(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (!reader.IsEmptyElement)
                        {
                            if (reader.Name.Equals(Serialization.RelationTypeUnit)
                                || reader.Name.Equals(Serialization.RelationTypeComposite))
                            {
                                var relationTypeIdString = reader.GetAttribute(Serialization.Id);
                                if (string.IsNullOrEmpty(relationTypeIdString))
                                {
                                    throw new Exception("Relation type has no id");
                                }

                                var relationTypeId = new Guid(relationTypeIdString);
                                var relationType = (IRelationType)this.database.MetaPopulation.FindById(relationTypeId);

                                if (reader.Name.Equals(Serialization.RelationTypeUnit))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType is IComposite)
                                    {
                                        this.CantLoadUnitRole(reader.ReadSubtree(), relationTypeId);
                                    }
                                    else
                                    {
                                        this.LoadUnitRelations(reader.ReadSubtree(), relationType);
                                    }
                                }
                                else if (reader.Name.Equals(Serialization.RelationTypeComposite))
                                {
                                    if (relationType == null || relationType.RoleType.ObjectType is IUnit)
                                    {
                                        this.CantLoadCompositeRole(reader.ReadSubtree(), relationTypeId);
                                    }
                                    else
                                    {
                                        this.LoadCompositeRelations(reader.ReadSubtree(), relationType);
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }

        private void LoadUnitRelations(XmlReader reader, IRelationType relationType)
        {
            var allowedClasses = new HashSet<IClass>(relationType.AssociationType.ObjectType.DatabaseClasses);
            var unitRelationsByClass = new Dictionary<IClass, List<UnitRelation>>();

            var skip = false;
            while (skip || reader.Read())
            {
                skip = false;

                switch (reader.NodeType)
                {
                    // eat everything but elements
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Relation))
                        {
                            var associationIdString = reader.GetAttribute(Serialization.Association);
                            var associationId = long.Parse(associationIdString);

                            this.classByObjectId.TryGetValue(associationId, out var @class);

                            if (@class == null || !allowedClasses.Contains(@class))
                            {
                                this.CantLoadUnitRole(reader.ReadSubtree(), relationType.Id);
                            }
                            else
                            {
                                if (!unitRelationsByClass.TryGetValue(@class, out var unitRelations))
                                {
                                    unitRelations = new List<UnitRelation>();
                                    unitRelationsByClass[@class] = unitRelations;
                                }

                                var value = string.Empty;
                                if (!reader.IsEmptyElement)
                                {
                                    value = reader.ReadElementContentAsString();
                                }

                                try
                                {
                                    object unit = null;
                                    if (reader.IsEmptyElement)
                                    {
                                        var unitType = (IUnit)relationType.RoleType.ObjectType;
                                        switch (unitType.Tag)
                                        {
                                            case UnitTags.String:
                                                unit = string.Empty;
                                                break;

                                            case UnitTags.Binary:
                                                unit = Array.Empty<byte>();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var unitType = (IUnit)relationType.RoleType.ObjectType;
                                        var unitTypeTag = unitType.Tag;
                                        unit = Serialization.ReadString(value, unitTypeTag);
                                    }

                                    unitRelations.Add(new UnitRelation(associationId, unit));
                                }
                                catch
                                {
                                    this.OnRelationNotLoaded(relationType.Id, associationId, value);
                                }

                                skip = reader.IsStartElement();
                            }
                        }

                        break;
                }
            }

            var con = this.database.ConnectionFactory.Create();
            try
            {
                foreach (var kvp in unitRelationsByClass)
                {
                    var @class = kvp.Key;
                    var unitRelations = kvp.Value;

                    var sql = this.database.Mapping.ProcedureNameForSetUnitRoleByRelationTypeByClass[@class][relationType];
                    var command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.StoredProcedure;
                    command.UnitTableParameter(relationType.RoleType, unitRelations);
                    command.ExecuteNonQuery();
                }

                con.Commit();
            }
            catch
            {
                con.Rollback();
            }
        }

        private void LoadCompositeRelations(XmlReader reader, IRelationType relationType)
        {
            var con = this.database.ConnectionFactory.Create();
            try
            {
                var relations = new CompositeRelations(
                    this.database,
                    relationType,
                    this.CantLoadCompositeRole,
                    this.OnRelationNotLoaded,
                    this.classByObjectId,
                    reader);

                var sql = relationType.RoleType.IsOne ?
                              this.database.Mapping.ProcedureNameForSetRoleByRelationType[relationType] :
                              this.database.Mapping.ProcedureNameForAddRoleByRelationType[relationType];

                var command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.AddCompositeRoleTableParameter(relations.ToArray());
                command.ExecuteNonQuery();

                con.Commit();
            }
            catch
            {
                con.Rollback();
            }
        }

        private void CantLoadUnitRole(XmlReader reader, Guid relationTypeId)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals(Serialization.Relation))
                {
                    var a = reader.GetAttribute(Serialization.Association);
                    var value = string.Empty;

                    if (!reader.IsEmptyElement)
                    {
                        value = reader.ReadElementContentAsString();
                    }

                    this.OnRelationNotLoaded(relationTypeId, long.Parse(a), value);
                }
            }
        }

        private void CantLoadCompositeRole(XmlReader reader, Guid relationTypeId)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals(Serialization.Relation))
                {
                    var associationIdString = reader.GetAttribute(Serialization.Association);
                    var associationId = long.Parse(associationIdString);
                    if (string.IsNullOrEmpty(associationIdString))
                    {
                        throw new Exception("Association id is missing");
                    }

                    if (reader.IsEmptyElement)
                    {
                        this.OnRelationNotLoaded(relationTypeId, associationId, null);
                    }
                    else
                    {
                        var value = reader.ReadElementContentAsString();
                        foreach (var r in value.Split(Serialization.ObjectsSplitterCharArray))
                        {
                            this.OnRelationNotLoaded(relationTypeId, associationId, r);
                        }
                    }
                }
            }
        }

        #region Import Errors

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

        #endregion Import Errors
    }
}

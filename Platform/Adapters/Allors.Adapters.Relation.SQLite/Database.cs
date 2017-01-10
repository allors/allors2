// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.IO;
    using System.Xml;
    using Allors.Meta;
    using Adapters;

    public class Database : IDatabase
    {
        internal const long CacheDefaultValue = 0;

        private const string FullUriKey = "fulluri";
        private const string SharedCacheKey = "?cache=shared";
        private const string InMemoryKey = ":memory:";
        private const string UriQueySeparator = "?";
        
        private static readonly byte[] EmptyByteArray = new byte[0];

        private readonly object lockObject = new object();
        private readonly Mapping mapping;
        private readonly bool isInMemory;

        // Configuration
        private readonly string id;
        private readonly IObjectFactory objectFactory;
        private readonly IRoleCache roleCache;
        private readonly IClassCache classCache;
        private readonly string connectionString;
        private readonly int commandTimeout;
        private readonly IsolationLevel isolationLevel;
        private readonly bool autoIncrement;

        private readonly SQLiteConnection inMemoryConnection;

        private readonly IdSequence idSequence;

        private Dictionary<string, object> properties;

        private SQLiteConnection connection;
        private SQLiteTransaction transaction;

        private bool? isValid;

        public Database(Configuration configuration)
        {
            this.connectionString = configuration.ConnectionString;
            if (this.connectionString == null)
            {
                throw new Exception("Configuration.ConnectionString is missing");
            }

            if (!this.connectionString.ToLowerInvariant().Contains(FullUriKey))
            {
                throw new Exception("Configuration.ConnectionString has no FullUri");
            }

            this.isInMemory = this.connectionString.ToLowerInvariant().Contains(InMemoryKey);

            if (this.isInMemory && !this.connectionString.ToLowerInvariant().Contains(SharedCacheKey))
            {
                throw new Exception("Configuration.ConnectionString is in memory and FullUri has no \"?cache=shared\" parameter");
            }

            if (this.isInMemory)
            {
                this.id = Guid.NewGuid().ToString("N").ToLowerInvariant();
            }
            else
            {
                var connectionStringBuilder = new SQLiteConnectionStringBuilder(this.connectionString);
                var dataSource = connectionStringBuilder.FullUri;
                if (dataSource.Contains(UriQueySeparator))
                {
                    dataSource = dataSource.Substring(0, dataSource.IndexOf(UriQueySeparator, StringComparison.Ordinal));
                }

                this.id = Path.GetFileName(dataSource);
            }

            this.objectFactory = configuration.ObjectFactory;
            if (this.objectFactory == null)
            {
                throw new Exception("Configuration.ObjectFactory is missing");
            }

            this.roleCache = configuration.RoleCache ?? new RoleCache();
            this.classCache = configuration.ClassCache ?? new ClassCache();
            this.commandTimeout = configuration.CommandTimeout ?? 30;
            this.isolationLevel = configuration.IsolationLevel ?? IsolationLevel.Serializable;
            this.autoIncrement = configuration.AutoIncrement ?? false;

            this.mapping = new Mapping(this);

            if (this.isInMemory)
            {
                this.inMemoryConnection = new SQLiteConnection(this.ConnectionString);
                this.inMemoryConnection.Open();
            }

            this.idSequence = IdSequence.GetOrCreate(this);
        }

        ~Database()
        {
            try
            {
                if (this.isInMemory)
                {
                    this.inMemoryConnection.Close();
                }
            }
            catch
            {
            }
        }
        
        public event ObjectNotLoadedEventHandler ObjectNotLoaded;

        public event RelationNotLoadedEventHandler RelationNotLoaded;
        
        public string Id 
        {
            get
            {
                return this.id;
            }
        }

        public IObjectFactory ObjectFactory 
        {
            get
            {
                return this.objectFactory;
            }
        }

        public IClassCache ClassCache
        {
            get
            {
                return this.classCache;
            }
        }

        public IRoleCache RoleCache
        {
            get
            {
                return this.roleCache;
            }
        }

        public bool IsDatabase
        {
            get
            {
                return true;
            }
        }

        public bool IsWorkspace 
        {
            get
            {
                return false;
            }
        }

        public bool IsShared 
        {
            get
            {
                return true;
            }
        }

        public IMetaPopulation MetaPopulation 
        {
            get
            {
                return this.ObjectFactory.MetaPopulation;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this.commandTimeout;
            }
        }

        public IsolationLevel IsolationLevel
        {
            get
            {
                return this.isolationLevel;
            }
        }

        public Mapping Mapping
        {
            get
            {
                return this.mapping;
            }
        }

        public bool IsValid
        {
            get
            {
                if (!this.isValid.HasValue)
                {
                    lock (this.lockObject)
                    {
                        if (!this.isValid.HasValue)
                        {
                            var validate = this.Validate();
                            return validate.Success;
                        }
                    }
                }

                return this.isValid.Value;
            }
        }

        public bool AutoIncrement
        {
            get
            {
                return this.autoIncrement;
            }
        }

        internal IdSequence IdSequence
        {
            get
            {
                return this.idSequence;
            }
        }

        public object this[string name]
        {
            get
            {
                if (this.properties == null)
                {
                    return null;
                }

                object value;
                this.properties.TryGetValue(name, out value);
                return value;
            }

            set
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.properties.Remove(name);
                }
                else
                {
                    this.properties[name] = value;
                }
            }
        }

        public void Init()
        {
            new Initialization(this.Mapping, new Schema(this), this.AutoIncrement).Execute();

            this.idSequence.Reset();

            this.roleCache.Invalidate();
            this.classCache.Invalidate();

            this.properties = null;
        }

        ISession IDatabase.CreateSession()
        {
            return this.CreateSession();
        }

        public Session CreateSession()
        {
            if (!this.IsValid)
            {
                throw new Exception("Schema is invalid.");
            }

            return new Session(this);
        }

        public void Load(XmlReader reader)
        {
            this.Init();

            this.LoadAllors(reader);

            this.roleCache.Invalidate();
            this.classCache.Invalidate();
        }

        public void Save(XmlWriter writer)
        {
            this.SaveAllors(writer);
        }

        public Validation Validate()
        {
            var validateResult = new Validation(this);
            this.isValid = validateResult.Success;
            return validateResult;
        }

        public SQLiteCommand CreateCommand(string cmdText)
        {
            if (this.connection == null)
            {
                this.connection = new SQLiteConnection(this.ConnectionString);
                this.connection.Open();
                this.transaction = this.connection.BeginTransaction(this.IsolationLevel);
            }

            var command = new SQLiteCommand(cmdText, this.connection, this.transaction)
            {
                CommandTimeout = this.CommandTimeout
            };
            return command;
        }

        #region Serialization
        private void LoadAllors(XmlReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    // only process elements, ignore others
                    if (reader.NodeType.Equals(XmlNodeType.Element))
                    {
                        if (reader.Name.Equals(Serialization.Population))
                        {
                            Serialization.CheckVersion(reader);

                            if (!reader.IsEmptyElement)
                            {
                                this.LoadPopulation(reader);
                            }

                            return;
                        }
                    }
                }
            }
            finally
            {
                this.Commit();
            }
        }

        private void LoadPopulation(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Objects))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadObjectsX(reader);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Relations))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadRelationTypes(reader);
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void LoadObjectsX(XmlReader reader)
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
                                this.LoadObjectTypes(reader);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.Workspace))
                        {
                            throw new Exception("Can not load workspace objects in a database.");
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + reader.Name + "> in parent element <" + Serialization.Objects + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!reader.Name.Equals(Serialization.Objects))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Objects + ">");
                        }

                        return;
                }
            }
        }

        private void LoadObjectTypes(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    // only process elements, ignore others
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.ObjectType))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadObjects(reader);
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + reader.Name + "> in parent element <" + Serialization.Database + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!reader.Name.Equals(Serialization.Database))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Database + ">");
                        }

                        return;
                }
            }
        }

        private void LoadObjects(XmlReader reader)
        {
            var metaObjectIdString = reader.GetAttribute(Serialization.Id);
            var metaObjectId = new Guid(metaObjectIdString);

            var objectType = (IObjectType)this.ObjectFactory.MetaPopulation.Find(metaObjectId);

            if (!reader.IsEmptyElement)
            {
                var oidsString = reader.ReadString();
                var oids = oidsString.Split(Serialization.ObjectsSplitterCharArray);

                for (var i = 0; i < oids.Length; i++)
                {
                    var objectArray = oids[i].Split(Serialization.ObjectSplitterCharArray);

                    var objectId = new ObjectIdLong(long.Parse(objectArray[0]));
                    var cacheId = new ObjectVersionLong(objectArray[1]);

                    if (!(objectType is IClass))
                    {
                        this.OnObjectNotLoaded(metaObjectId, objectId.ToString());
                    }
                    else
                    {
                        // Objects
                        var cmdText = @"
INSERT INTO " + Mapping.TableNameForObjects + " (" + Mapping.ColumnNameForObject + "," + Mapping.ColumnNameForType + "," + Mapping.ColumnNameForCache + @")
VALUES (" + Mapping.ParameterNameForObject + "," + Mapping.ParameterNameForType + "," + Mapping.ParameterNameForCache + ")";

                        using (var command = this.CreateCommand(cmdText))
                        {
                            command.Parameters.Add(Mapping.ParameterNameForObject, Mapping.DbTypeForId).Value = objectId.Value;
                            command.Parameters.Add(Mapping.ParameterNameForType, Mapping.DbTypeForType).Value = objectType.Id;
                            command.Parameters.Add(Mapping.ParameterNameForCache, Mapping.DbTypeForCache).Value = cacheId.Value;

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void LoadCompositeRelations(XmlReader reader, IRelationType relationType)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Relation))
                        {
                            var associationString = reader.GetAttribute(Serialization.Association);
                            var association = new ObjectIdLong(long.Parse(associationString));

                            if (reader.IsEmptyElement)
                            {
                                this.OnRelationNotLoaded(relationType.Id, association.ToString(), null);
                            }
                            else
                            {
                                var value = reader.ReadString();
                                var roleStrings = value.Split(Serialization.ObjectsSplitterCharArray);

                                if (relationType.RoleType.IsOne && roleStrings.Length > 1)
                                {
                                    foreach (var roleString in roleStrings)
                                    {
                                        this.OnRelationNotLoaded(relationType.Id, association.ToString(), roleString);
                                    }
                                }

                                foreach (var roleString in roleStrings)
                                {
                                    var role = new ObjectIdLong(long.Parse(roleString));
                                    //this.OnRelationNotLoaded(relationType.Id, association.ToString(), r);

                                    var cmdText = @"
INSERT INTO " + this.Mapping.GetTableName(relationType) + " (" + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole + @")
VALUES (" + Mapping.ParameterNameForAssociation + "," + Mapping.ParameterNameForRole + ")";

                                    using (var command = this.CreateCommand(cmdText))
                                    {
                                        command.Parameters.Add(Mapping.ParameterNameForAssociation, Mapping.DbTypeForId).Value = association.Value;
                                        command.Parameters.Add(Mapping.ParameterNameForRole, Mapping.DbTypeForId).Value = role.Value;

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void LoadRelationTypes(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.RelationTypeUnit))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.LoadRelations(reader, true);
                            }
                        }
                        else if (reader.Name.Equals(Serialization.RelationTypeComposite))
                        {
                            if (!reader.IsEmptyElement)
                            {
                                LoadRelations(reader, false);
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void LoadRelations(XmlReader reader, bool isUnit)
        {
            var metaRelationIdString = reader.GetAttribute(Serialization.Id);
            var metaRelationId = new Guid(metaRelationIdString);

            var relationType = (IRelationType)this.ObjectFactory.MetaPopulation.Find(metaRelationId);

            if (!reader.IsEmptyElement)
            {
                if (relationType == null || (relationType.RoleType.ObjectType is IUnit) != isUnit)
                {
                    this.CantLoadRelation(reader, metaRelationId);
                }
                else
                {
                    if (relationType.RoleType.ObjectType is IUnit)
                    {
                        this.LoadUnitRelations(reader, relationType);
                    }
                    else
                    {
                        this.LoadCompositeRelations(reader, relationType);
                    }
                }
            }
        }

        private void LoadUnitRelations(XmlReader reader, IRelationType relationType)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Relation))
                        {
                            var associationString = reader.GetAttribute(Serialization.Association);
                            var association = new ObjectIdLong(long.Parse(associationString));
                            if (reader.IsEmptyElement)
                            {
                                object role;

                                // OnRelationNotLoaded(relationType.Id, association.ToString(), String.Empty);
                                if (relationType.RoleType.ObjectType.Id == UnitIds.StringId)
                                {
                                    role = string.Empty;
                                }
                                else if (relationType.RoleType.ObjectType.Id == UnitIds.BinaryId)
                                {
                                    role = EmptyByteArray;
                                }
                                else
                                {
                                    this.OnRelationNotLoaded(relationType.Id, association.ToString(), string.Empty);
                                    continue;
                                }

                                var cmdText = @"
INSERT INTO " + this.Mapping.GetTableName(relationType) + " (" + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole + @")
VALUES (" + Mapping.ParameterNameForAssociation + "," + Mapping.ParameterNameForRole + ")";

                                using (var command = this.CreateCommand(cmdText))
                                {
                                    command.Parameters.Add(Mapping.ParameterNameForAssociation, Mapping.DbTypeForId).Value = association.Value;
                                    command.Parameters.Add(Mapping.ParameterNameForRole, Mapping.GetSqlDbType(relationType.RoleType)).Value = role;

                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                var value = reader.ReadString();
                                try
                                {
                                    // OnRelationNotLoaded(relationType.Id, association.ToString(), value);
                                    var unitTypeTag = ((IUnit)relationType.RoleType.ObjectType).UnitTag;
                                    var role = Serialization.ReadString(value, unitTypeTag);

                                    var cmdText = @"
INSERT INTO " + this.Mapping.GetTableName(relationType) + " (" + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole + @")
VALUES (" + Mapping.ParameterNameForAssociation + "," + Mapping.ParameterNameForRole + ")";

                                    using (var command = this.CreateCommand(cmdText))
                                    {
                                        command.Parameters.Add(Mapping.ParameterNameForAssociation, Mapping.DbTypeForId).Value = association.Value;
                                        command.Parameters.Add(Mapping.ParameterNameForRole, Mapping.GetSqlDbType(relationType.RoleType)).Value = role;

                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch
                                {
                                    this.OnRelationNotLoaded(relationType.Id, association.ToString(), value);
                                }
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void OnObjectNotLoaded(Guid metaTypeId, string allorsObjectId)
        {
            if (this.ObjectNotLoaded != null)
            {
                this.ObjectNotLoaded(this, new ObjectNotLoadedEventArgs(metaTypeId, allorsObjectId));
            }
            else
            {
                throw new Exception("Object not loaded: " + metaTypeId + ":" + allorsObjectId);
            }
        }

        private void OnRelationNotLoaded(Guid metaRelationId, string associationObjectId, string roleContents)
        {
            var args = new RelationNotLoadedEventArgs(metaRelationId, associationObjectId, roleContents);
            if (this.RelationNotLoaded != null)
            {
                this.RelationNotLoaded(this, args);
            }
            else
            {
                throw new Exception("RelationType not loaded: " + args);
            }
        }

        private void CantLoadRelation(XmlReader reader, Guid metaRelationId)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals(Serialization.Relation))
                        {
                            var a = reader.GetAttribute(Serialization.Association);
                            var value = string.Empty;

                            if (!reader.IsEmptyElement)
                            {
                                value = reader.ReadString();
                            }

                            this.OnRelationNotLoaded(metaRelationId, a, value);
                        }

                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private void SaveAllors(XmlWriter writer)
        {
            try
            {
                var writeDocument = false;
                if (writer.WriteState == WriteState.Start)
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement(Serialization.Allors);
                    writeDocument = true;
                }

                writer.WriteStartElement(Serialization.Population);
                writer.WriteAttributeString(Serialization.Version, Serialization.VersionCurrent);

                writer.WriteStartElement(Serialization.Objects);
                writer.WriteStartElement(Serialization.Database);
                SaveObjects(writer);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement(Serialization.Relations);
                writer.WriteStartElement(Serialization.Database);
                SaveRelations(writer);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();

                if (writeDocument)
                {
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            finally
            {
                this.Rollback();
            }
        }

        private void SaveObjects(XmlWriter writer)
        {
            var orderedClasses = new List<IClass>(this.ObjectFactory.MetaPopulation.Classes);
            orderedClasses.Sort();
            foreach (var type in orderedClasses)
            {
                var atLeastOne = false;

                var cmdText = @"
SELECT " + Mapping.ColumnNameForObject + @", " + Mapping.ColumnNameForCache + @"
FROM " + Mapping.TableNameForObjects + @"
WHERE " + Mapping.ColumnNameForType + "=" + Mapping.ParameterNameForType;

                using (var command = this.CreateCommand(cmdText))
                {
                    command.Parameters.Add(Mapping.ParameterNameForType, Mapping.DbTypeForType).Value = type.Id;

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (atLeastOne == false)
                            {
                                atLeastOne = true;

                                writer.WriteStartElement(Serialization.ObjectType);
                                writer.WriteAttributeString(Serialization.Id, type.Id.ToString("N").ToLowerInvariant());
                            }
                            else
                            {
                                writer.WriteString(Serialization.ObjectsSplitter);
                            }

                            var objectId = reader[0].ToString();
                            var objectVersion = reader[1].ToString();

                            writer.WriteString(objectId + Serialization.ObjectSplitter + objectVersion);
                        }

                        reader.Close();
                    }
                }

                if (atLeastOne)
                {
                    writer.WriteEndElement();
                }
            }
        }

        private void SaveRelations(XmlWriter writer)
        {
            var orderedRelationType = new List<IRelationType>(this.ObjectFactory.MetaPopulation.RelationTypes);
            orderedRelationType.Sort();
            foreach (var relation in orderedRelationType)
            {
                if (relation.AssociationType.ObjectType.ExistClass)
                {
                    var role = relation.RoleType;
                    
                    var cmdText = @"
SELECT " + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole + @"
FROM " + this.Mapping.GetTableName(relation) + @"
ORDER BY " + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole;

                    using (var command = this.CreateCommand(cmdText))
                    {
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            if (role.IsMany)
                            {
                                using (var relationTypeManyXmlWriter = new RelationTypeManyXmlWriter(relation, writer))
                                {
                                    while (reader.Read())
                                    {
                                        var a = long.Parse(reader.GetValue(0).ToString());
                                        var r = long.Parse(reader.GetValue(1).ToString());
                                        relationTypeManyXmlWriter.Write(a, r);
                                    }

                                    relationTypeManyXmlWriter.Close();
                                }
                            }
                            else
                            {
                                using (var relationTypeOneXmlWriter = new RelationTypeOneXmlWriter(relation, writer))
                                {
                                    while (reader.Read())
                                    {
                                        var a = long.Parse(reader.GetValue(0).ToString());

                                        if (role.ObjectType is IUnit)
                                        {
                                            var unitTypeTag = ((IUnit)role.ObjectType).UnitTag;
                                            object r;
                                            switch (unitTypeTag)
                                            {
                                                case UnitTags.AllorsBinary:
                                                    r = reader.GetValue(1);
                                                    break;
                                                case UnitTags.AllorsBoolean:
                                                    r = reader.GetBoolean(1);
                                                    break;
                                                case UnitTags.AllorsDateTime:
                                                    r = reader.GetDateTime(1);
                                                    break;
                                                case UnitTags.AllorsDecimal:
                                                    r = reader.GetDecimal(1);
                                                    break;
                                                case UnitTags.AllorsFloat:
                                                    r = reader.GetDouble(1);
                                                    break;
                                                case UnitTags.AllorsInteger:
                                                    r = reader.GetInt32(1);
                                                    break;
                                                case UnitTags.AllorsString:
                                                    r = reader.GetString(1);
                                                    break;
                                                case UnitTags.AllorsUnique:
                                                    r = reader.GetGuid(1);
                                                    break;
                                                default:
                                                    throw new Exception("Unknown unit tag " + unitTypeTag);
                                            }

                                            var content = Serialization.WriteString(unitTypeTag, r);
                                            relationTypeOneXmlWriter.Write(a, content);
                                        }
                                        else
                                        {
                                            var r = reader.GetValue(1);
                                            relationTypeOneXmlWriter.Write(a, XmlConvert.ToString(long.Parse(r.ToString())));
                                        }
                                    }

                                    relationTypeOneXmlWriter.Close();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        private void Commit()
        {
            try
            {
                if (this.transaction != null)
                {
                    this.transaction.Commit();
                }
            }
            finally
            {
                this.transaction = null;
                if (this.connection != null)
                {
                    try
                    {
                        this.connection.Close();
                    }
                    finally
                    {
                        this.connection = null;
                    }
                }
            }
        }

        private void Rollback()
        {
            try
            {
                if (this.transaction != null)
                {
                    this.transaction.Rollback();
                }
            }
            finally
            {
                this.transaction = null;
                if (this.connection != null)
                {
                    try
                    {
                        this.connection.Close();
                    }
                    finally
                    {
                        this.connection = null;
                    }
                }
            }
        }
    }
}
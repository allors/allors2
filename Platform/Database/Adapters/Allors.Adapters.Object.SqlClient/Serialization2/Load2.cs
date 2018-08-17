// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Load2.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Data.SqlClient;
    using System.Xml;

    using Adapters;

    using Allors.Meta;

    public class Load2
    {
        public const long InitialVersion = 0;

        private readonly Database database;
        private readonly SqlConnection connection;
        private readonly ObjectNotLoadedEventHandler objectNotLoaded;
        private readonly RelationNotLoadedEventHandler relationNotLoaded;

        public Load2(Database database, SqlConnection connection, ObjectNotLoadedEventHandler objectNotLoaded, RelationNotLoadedEventHandler relationNotLoaded)
        {
            this.database = database;
            this.connection = connection;
            this.objectNotLoaded = objectNotLoaded;
            this.relationNotLoaded = relationNotLoaded;
        }

        public void Execute(XmlReader reader)
        {
            reader.MoveToContent();

            while (reader.Read())
            {
                if (reader.NodeType.Equals(XmlNodeType.Element))
                {
                    if (reader.Name.Equals(Serialization.Population))
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
                                reader.Skip();
                                //this.LoadRelations();
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown child element <" + reader.Name + "> in parent element <" + Serialization.Population + ">");
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if (!reader.Name.Equals(Serialization.Population))
                        {
                            throw new Exception("Expected closing element </" + Serialization.Population + ">");
                        }

                        return;
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
                        else
                        {
                            throw new Exception("Unknown child element <" + reader.Name + "> in parent element <" + Serialization.Objects + ">");
                        }

                        break;
                }
            }
        }
        
        private void LoadObjectsDatabase(XmlReader reader)
        {
            reader.MoveToContent();

            var xmlObjects = new Objects(this.database, this.OnObjectNotLoaded, reader);
            using (var objectsReader = new ObjectsReader(xmlObjects))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(this.connection, SqlBulkCopyOptions.KeepIdentity, null))
                {
                    sqlBulkCopy.BulkCopyTimeout = 0;
                    sqlBulkCopy.BatchSize = 5000;
                    sqlBulkCopy.DestinationTableName = this.database.Mapping.TableNameForObjects;
                    sqlBulkCopy.WriteToServer(objectsReader);
                }
            }
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
        #endregion
    }
}
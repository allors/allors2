// <copyright file="Objects.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using Adapters;
    using Meta;
    using Version = Version;

    public class Objects : IEnumerable<object[]>
    {
        private readonly Database database;
        private readonly Action<Guid, long> onObjectNotLoaded;
        private readonly Dictionary<long, IClass> classByObjectId;
        private readonly XmlReader reader;

        public Objects(
            Database database,
            Action<Guid, long> onObjectNotLoaded,
            Dictionary<long, IClass> classByObjectId,
            XmlReader reader)
        {
            this.database = database;
            this.onObjectNotLoaded = onObjectNotLoaded;
            this.classByObjectId = classByObjectId;
            this.reader = reader;
        }

        public IEnumerator<object[]> GetEnumerator()
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
                                var objectType = this.database.ObjectFactory.GetObjectType(objectTypeId);

                                var objectIdsString = this.reader.ReadElementContentAsString();
                                foreach (var objectIdString in objectIdsString.Split(Serialization.ObjectsSplitterCharArray))
                                {
                                    var objectArray = objectIdString.Split(Serialization.ObjectSplitterCharArray);

                                    var objectId = long.Parse(objectArray[0]);
                                    var objectVersion = objectArray.Length > 1
                                        ? long.Parse(objectArray[1])
                                        : (long) Version.DatabaseInitial;

                                    if (objectType is IClass @class)
                                    {
                                        this.classByObjectId[objectId] = @class;
                                        yield return new object[] { objectId, @class.Id, objectVersion };
                                    }
                                    else
                                    {
                                        this.onObjectNotLoaded(objectTypeId, objectId);
                                    }
                                }

                                skip = this.reader.IsStartElement();
                            }
                            else
                            {
                                this.reader.Skip();
                            }
                        }

                        break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}

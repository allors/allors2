// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Objects.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;

    using Allors.Meta;

    public class Objects : IEnumerable<object[]>
    {
        private readonly Database database;
        private readonly Action<Guid, long> onObjectNotLoaded;
        private readonly XmlReader reader;

        public Objects(Database database, Action<Guid, long> onObjectNotLoaded, XmlReader reader)
        {
            this.database = database;
            this.onObjectNotLoaded = onObjectNotLoaded;
            this.reader = reader;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            while (this.reader.Read())
            {
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
                                var objectType = this.database.ObjectFactory.GetObjectTypeForType(objectTypeId);

                                var objectIdsString = this.reader.ReadElementContentAsString();
                                var objectIdStringArray = objectIdsString.Split(Serialization.ObjectsSplitterCharArray);

                                foreach (var objectIdString in objectIdStringArray)
                                {
                                    var objectArray = objectIdString.Split(Serialization.ObjectSplitterCharArray);

                                    var objectId = long.Parse(objectArray[0]);
                                    var objectVersion = objectArray.Length > 1
                                        ? long.Parse(objectArray[1])
                                        : Load2.InitialVersion;

                                    if (objectType is IClass)
                                    {
                                        yield return new object[] { objectId, objectType.Id, objectVersion };
                                    }
                                    else
                                    {
                                        this.onObjectNotLoaded(objectTypeId, objectId);
                                    }
                                }
                            }
                            else
                            {
                                this.reader.Skip();
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

                        break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
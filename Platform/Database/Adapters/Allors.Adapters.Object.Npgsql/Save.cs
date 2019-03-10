// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Save.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Xml;

    using Allors.Meta;

    public abstract class Save
    {
        private readonly Database database;
        private readonly XmlWriter writer;

        protected Save(Database database, XmlWriter writer)
        {
            this.database = database;
            this.writer = writer;
        }

        public virtual void Execute(ManagementSession session)
        {
            var writeDocument = false;
            if (this.writer.WriteState == WriteState.Start)
            {
                this.writer.WriteStartDocument();
                this.writer.WriteStartElement(Serialization.Allors);
                writeDocument = true;
            }

            this.writer.WriteStartElement(Serialization.Population);
            this.writer.WriteAttributeString(Serialization.Version, Serialization.VersionCurrent.ToString());

            this.writer.WriteStartElement(Serialization.Objects);
            this.writer.WriteStartElement(Serialization.Database);
            this.SaveObjects(session);
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();

            this.writer.WriteStartElement(Serialization.Relations);
            this.writer.WriteStartElement(Serialization.Database);
            this.SaveRelations(session);
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();

            if (writeDocument)
            {
                this.writer.WriteEndElement();
                this.writer.WriteEndDocument();
            }
        }

        protected void SaveObjects(ManagementSession session)
        {
            var concreteCompositeType = new List<IClass>(this.database.MetaPopulation.Classes);
            concreteCompositeType.Sort();
            foreach (var type in concreteCompositeType)
            {
                var atLeastOne = false;

                var sql = "SELECT " + this.database.Schema.ObjectId + "\n";
                sql += "FROM " + this.database.Schema.Table(type.ExclusiveClass) + "\n";
                sql += "WHERE " + this.database.Schema.TypeId + "=" + this.database.Schema.TypeId.Param.InvocationName + "\n";
                sql += "ORDER BY " + this.database.Schema.ObjectId;

                using (var command = session.CreateCommand(sql))
                {
                    command.AddInParameter(this.database.Schema.TypeId.Param.Name, type.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (atLeastOne == false)
                            {
                                atLeastOne = true;

                                this.writer.WriteStartElement(Serialization.ObjectType);
                                this.writer.WriteAttributeString(Serialization.Id, type.Id.ToString());
                            }
                            else
                            {
                                this.writer.WriteString(Serialization.ObjectsSplitter);
                            }

                            var objectId = long.Parse(reader[0].ToString());
                            this.writer.WriteString(objectId.ToString());
                        }

                        reader.Close();
                    }
                }

                if (atLeastOne)
                {
                    this.writer.WriteEndElement();
                }
            }
        }

        protected void SaveRelations(ManagementSession session)
        {
            var exclusiveLeafClassesByObjectType = new Dictionary<IObjectType, HashSet<IClass>>();

            var relations = new List<IRelationType>(this.database.MetaPopulation.RelationTypes);
            relations.Sort();

            foreach (var relation in relations)
            {
                var associationType = relation.AssociationType;

                if (associationType.ObjectType.ExistClass)
                {
                    var roleType = relation.RoleType;
                
                    var sql = string.Empty;
                    if (roleType.ObjectType is IUnit)
                    {
                        HashSet<IClass> exclusiveLeafClasses;
                        if (!exclusiveLeafClassesByObjectType.TryGetValue(associationType.ObjectType, out exclusiveLeafClasses))
                        {
                            exclusiveLeafClasses = new HashSet<IClass>();
                            foreach (var concreteClass in associationType.ObjectType.Classes)
                            {
                                exclusiveLeafClasses.Add(concreteClass.ExclusiveClass);
                            }

                            exclusiveLeafClassesByObjectType[associationType.ObjectType] = exclusiveLeafClasses;
                        }
                       
                        var first = true;
                        foreach (var exclusiveLeafClass in exclusiveLeafClasses)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                sql += "UNION\n";
                            }

                            sql += "SELECT " + this.database.Schema.ObjectId + " As " + this.database.Schema.AssociationId + ", " + this.database.Schema.Column(roleType) + " As " + this.database.Schema.RoleId + "\n";
                            sql += "FROM " + this.database.Schema.Table(exclusiveLeafClass) + "\n";
                            sql += "WHERE " + this.database.Schema.Column(roleType) + " IS NOT NULL\n";
                        }

                        sql += "ORDER BY " + this.database.Schema.AssociationId;
                    }
                    else
                    {
                        if ((roleType.IsMany && associationType.IsMany) || !relation.ExistExclusiveClasses)
                        {
                            sql += "SELECT " + this.database.Schema.AssociationId + "," + this.database.Schema.RoleId + "\n";
                            sql += "FROM " + this.database.Schema.Table(relation) + "\n";
                            sql += "ORDER BY " + this.database.Schema.AssociationId + "," + this.database.Schema.RoleId;
                        }
                        else
                        {
                            // use foreign keys
                            if (roleType.IsOne)
                            {
                                sql += "SELECT " + this.database.Schema.ObjectId + " As " + this.database.Schema.AssociationId + ", " + this.database.Schema.Column(roleType) + " As " + this.database.Schema.RoleId + "\n";
                                sql += "FROM " + this.database.Schema.Table(associationType.ObjectType.ExclusiveClass) + "\n";
                                sql += "WHERE " + this.database.Schema.Column(roleType) + " IS NOT NULL\n";
                                sql += "ORDER BY " + this.database.Schema.AssociationId;
                            }
                            else
                            {
                                // role.Many
                                var compositeType = (IComposite)roleType.ObjectType;
                                sql += "SELECT " + this.database.Schema.Column(associationType) + " As " + this.database.Schema.AssociationId + ", " + this.database.Schema.ObjectId + " As " + this.database.Schema.RoleId + "\n";
                                sql += "FROM " + this.database.Schema.Table(compositeType.ExclusiveClass) + "\n";
                                sql += "WHERE " + this.database.Schema.Column(associationType) + " IS NOT NULL\n";
                                sql += "ORDER BY " + this.database.Schema.AssociationId + "," + this.database.Schema.RoleId;
                            }
                        }
                    }

                    using (var command = session.CreateCommand(sql))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (roleType.IsMany)
                            {
                                using (var relationTypeManyXmlWriter = new RelationTypeManyXmlWriter(relation, this.writer))
                                {
                                    while (reader.Read())
                                    {
                                        var a = long.Parse(reader[0].ToString());
                                        var r = long.Parse(reader[1].ToString());
                                        relationTypeManyXmlWriter.Write(a, r);
                                    }

                                    relationTypeManyXmlWriter.Close();
                                }
                            }
                            else
                            {
                                using (var relationTypeOneXmlWriter = new RelationTypeOneXmlWriter(relation, this.writer))
                                {
                                    while (reader.Read())
                                    {
                                        var a = long.Parse(reader[0].ToString());

                                        if (roleType.ObjectType is IUnit)
                                        {
                                            var unitType = (IUnit)roleType.ObjectType;
                                            var unitTypeTag = unitType.UnitTag;
                                            var r = command.GetValue(reader, unitTypeTag, 1);
                                            var content = Serialization.WriteString(unitTypeTag, r);
                                            relationTypeOneXmlWriter.Write(a, content);
                                        }
                                        else
                                        {
                                            var r = reader[1];
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
    }
}
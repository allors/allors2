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

namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;

    using Allors.Meta;
    using Adapters;

    internal class Save
    {
        private readonly Database database;
        private readonly XmlWriter writer;

        internal Save(Database database, XmlWriter writer)
        {
            this.database = database;
            this.writer = writer;
        }

        internal virtual void Execute(ManagementSession session)
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

                var sql = "SELECT " + Mapping.ColumnNameForObject + ", " + Mapping.ColumnNameForVersion + "\n";
                sql += "FROM " + this.database.Mapping.TableNameForObjects + "\n";
                sql += "WHERE " + Mapping.ColumnNameForClass + "=" + Mapping.ParamInvocationNameForClass + "\n";
                sql += "ORDER BY " + Mapping.ColumnNameForObject;

                using (var command = session.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.AddInParameter(Mapping.ParamNameForClass, type.Id);

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
                            var version = reader[1].ToString();

                            this.writer.WriteString(objectId + Serialization.ObjectSplitter + version);
                        }
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
            var exclusiverRootClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();

            var relations = new List<IRelationType>(this.database.MetaPopulation.RelationTypes);
            relations.Sort();

            foreach (var relation in relations)
            {
                var associationType = relation.AssociationType;

                if (associationType.ObjectType.ExistClass)
                {
                    var roleType = relation.RoleType;

                    var sql = string.Empty;
                    if (roleType.ObjectType.IsUnit)
                    {
                        HashSet<IObjectType> exclusiveRootClasses;
                        if (!exclusiverRootClassesByObjectType.TryGetValue(associationType.ObjectType, out exclusiveRootClasses))
                        {
                            exclusiveRootClasses = new HashSet<IObjectType>();
                            foreach (var concreteClass in associationType.ObjectType.Classes)
                            {
                                exclusiveRootClasses.Add(concreteClass.ExclusiveClass);
                            }

                            exclusiverRootClassesByObjectType[associationType.ObjectType] = exclusiveRootClasses;
                        }

                        var first = true;
                        foreach (var exclusiveRootClass in exclusiveRootClasses)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                sql += "UNION\n";
                            }

                            sql += "SELECT " + Mapping.ColumnNameForObject + " As " + Mapping.ColumnNameForAssociation + ", " + this.database.Mapping.ColumnNameByRelationType[roleType.RelationType] + " As " + Mapping.ColumnNameForRole + "\n";
                            sql += "FROM " + this.database.Mapping.TableNameForObjectByClass[(IClass)exclusiveRootClass] + "\n";
                            sql += "WHERE " + this.database.Mapping.ColumnNameByRelationType[roleType.RelationType] + " IS NOT NULL\n";
                        }

                        sql += "ORDER BY " + Mapping.ColumnNameForAssociation;
                    }
                    else
                    {
                        if ((roleType.IsMany && associationType.IsMany) || !relation.ExistExclusiveClasses)
                        {
                            sql += "SELECT " + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole + "\n";
                            sql += "FROM " + this.database.Mapping.TableNameForRelationByRelationType[relation] + "\n";
                            sql += "ORDER BY " + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole;
                        }
                        else
                        {
                            // use foreign keys
                            if (roleType.IsOne)
                            {
                                sql += "SELECT " + Mapping.ColumnNameForObject + " As " + Mapping.ColumnNameForAssociation + ", " + this.database.Mapping.ColumnNameByRelationType[roleType.RelationType] + " As " + Mapping.ColumnNameForRole + "\n";
                                sql += "FROM " + this.database.Mapping.TableNameForObjectByClass[associationType.ObjectType.ExclusiveClass] + "\n";
                                sql += "WHERE " + this.database.Mapping.ColumnNameByRelationType[roleType.RelationType] + " IS NOT NULL\n";
                                sql += "ORDER BY " + Mapping.ColumnNameForAssociation;
                            }
                            else
                            {
                                // role.Many
                                sql += "SELECT " + this.database.Mapping.ColumnNameByRelationType[associationType.RelationType] + " As " + Mapping.ColumnNameForAssociation + ", " + Mapping.ColumnNameForObject + " As " + Mapping.ColumnNameForRole + "\n";
                                sql += "FROM " + this.database.Mapping.TableNameForObjectByClass[((IComposite)roleType.ObjectType).ExclusiveClass] + "\n";
                                sql += "WHERE " + this.database.Mapping.ColumnNameByRelationType[associationType.RelationType] + " IS NOT NULL\n";
                                sql += "ORDER BY " + Mapping.ColumnNameForAssociation + "," + Mapping.ColumnNameForRole;
                            }
                        }
                    }

                    using (var command = session.Connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;
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

                                        if (roleType.ObjectType.IsUnit)
                                        {
                                            var unitTypeTag = ((IUnit)roleType.ObjectType).UnitTag;
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

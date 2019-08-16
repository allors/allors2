// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Save.cs" company="Allors bvba">
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

namespace Allors.Adapters.Memory
{
    using System.Collections.Generic;
    using System.Xml;

    using Allors.Meta;
    using Adapters;

    public class Save
    {
        private readonly Session session;
        private readonly XmlWriter writer;
        private readonly Dictionary<IObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType;

        public Save(Session session, XmlWriter writer, Dictionary<IObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType)
        {
            this.session = session;
            this.writer = writer;
            this.sortedNonDeletedStrategiesByObjectType = sortedNonDeletedStrategiesByObjectType;
        }

        public void Execute()
        {
            var writeDocument = false;
            if (this.writer.WriteState == WriteState.Start)
            {
                this.writer.WriteStartDocument();
                this.writer.WriteStartElement(Serialization.Allors);
                writeDocument = true;
            }

            this.SavePopulation();

            if (writeDocument)
            {
                this.writer.WriteEndElement();
                this.writer.WriteEndDocument();
            }
        }

        internal void SavePopulation()
        {
            this.writer.WriteStartElement(Serialization.Population);
            this.writer.WriteAttributeString(Serialization.Version, Serialization.VersionCurrent.ToString());

            this.SaveObjectType();

            this.SaveRelationType();

            this.writer.WriteEndElement();
        }

        internal virtual void SaveObjectType()
        {
            this.writer.WriteStartElement(Serialization.Objects);
            this.writer.WriteStartElement(Serialization.Database);

            var sortedObjectTypes = new List<IObjectType>(this.sortedNonDeletedStrategiesByObjectType.Keys);
            sortedObjectTypes.Sort();

            foreach (var objectType in sortedObjectTypes)
            {
                var sortedNonDeletedStrategies = this.sortedNonDeletedStrategiesByObjectType[objectType];

                if (sortedNonDeletedStrategies.Count > 0)
                {
                    this.writer.WriteStartElement(Serialization.ObjectType);
                    this.writer.WriteAttributeString(Serialization.Id, objectType.Id.ToString("N").ToLowerInvariant());

                    for (var i = 0; i < sortedNonDeletedStrategies.Count; i++)
                    {
                        var strategy = sortedNonDeletedStrategies[i];
                        if (i > 0)
                        {
                            this.writer.WriteString(Serialization.ObjectsSplitter);
                        }

                        this.writer.WriteString(strategy.ObjectId + Serialization.ObjectSplitter + strategy.ObjectVersion);
                    }

                    this.writer.WriteEndElement();
                }
            }

            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }

        internal virtual void SaveRelationType()
        {
            this.writer.WriteStartElement(Serialization.Relations);
            this.writer.WriteStartElement(Serialization.Database);

            var sortedStrategiesByRoleType = new Dictionary<IRoleType, List<Strategy>>();
            foreach (var dictionaryEntry in this.sortedNonDeletedStrategiesByObjectType)
            {
                var strategies = dictionaryEntry.Value;
                foreach (var strategy in strategies)
                {
                    strategy.FillRoleForSave(sortedStrategiesByRoleType);
                }
            }

            var strategySorter = new Strategy.ObjectIdComparer();
            foreach (var dictionaryEntry in sortedStrategiesByRoleType)
            {
                var strategies = dictionaryEntry.Value;
                strategies.Sort(strategySorter);
            }

            var sortedRelationTypes = new List<IRelationType>(this.session.Population.MetaPopulation.RelationTypes);
            sortedRelationTypes.Sort();
            foreach (var relationType in sortedRelationTypes)
            {
                var roleType = relationType.RoleType;

                List<Strategy> strategies;
                sortedStrategiesByRoleType.TryGetValue(roleType, out strategies);

                if (strategies != null)
                {
                    this.writer.WriteStartElement(roleType.ObjectType is IUnit
                                                 ? Serialization.RelationTypeUnit
                                                 : Serialization.RelationTypeComposite);

                    this.writer.WriteAttributeString(Serialization.Id, relationType.Id.ToString("N").ToLowerInvariant());

                    if (roleType.ObjectType is IUnit)
                    {
                        foreach (var strategy in strategies)
                        {
                            strategy.SaveUnit(this.writer, roleType);
                        }
                    }
                    else
                    {
                        if (roleType.IsMany)
                        {
                            foreach (var strategy in strategies)
                            {
                                strategy.SaveComposites(this.writer, roleType);
                            }
                        }
                        else
                        {
                            foreach (var strategy in strategies)
                            {
                                strategy.SaveComposite(this.writer, roleType);
                            }
                        }
                    }

                    this.writer.WriteEndElement();
                }
            }

            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }
    }
}

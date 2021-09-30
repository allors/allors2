// <copyright file="CompositeRelations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using Adapters;
    using Meta;

    internal class CompositeRelations : IEnumerable<CompositeRelation>
    {
        private readonly Database database;
        private readonly IRelationType relationType;
        private readonly Action<Guid, long, string> onRelationNotLoaded;
        private readonly Dictionary<long, IClass> classByObjectId;
        private readonly XmlReader reader;
        private readonly Action<XmlReader, Guid> cantLoadCompositeRole;

        public CompositeRelations(
            Database database,
            IRelationType relationType,
            Action<XmlReader, Guid> cantLoadCompositeRole,
            Action<Guid, long, string> onRelationNotLoaded,
            Dictionary<long, IClass> classByObjectId,
            XmlReader reader)
        {
            this.database = database;
            this.relationType = relationType;
            this.cantLoadCompositeRole = cantLoadCompositeRole;
            this.onRelationNotLoaded = onRelationNotLoaded;
            this.classByObjectId = classByObjectId;
            this.reader = reader;
        }

        public IEnumerator<CompositeRelation> GetEnumerator()
        {
            var allowedAssociationClasses = new HashSet<IClass>(this.relationType.AssociationType.ObjectType.DatabaseClasses);
            var allowedRoleClasses = new HashSet<IClass>(((IComposite)this.relationType.RoleType.ObjectType).DatabaseClasses);

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

                            this.classByObjectId.TryGetValue(associationId, out var associationClass);

                            if (associationClass == null || !allowedAssociationClasses.Contains(associationClass))
                            {
                                this.cantLoadCompositeRole(this.reader.ReadSubtree(), this.relationType.Id);
                            }
                            else
                            {
                                var value = string.Empty;
                                if (!this.reader.IsEmptyElement)
                                {
                                    value = this.reader.ReadElementContentAsString();

                                    var roleIdsString = value;
                                    var roleIdStringArray = roleIdsString.Split(Serialization.ObjectsSplitterCharArray);

                                    if (this.relationType.RoleType.IsOne && roleIdStringArray.Length != 1)
                                    {
                                        foreach (var roleId in roleIdStringArray)
                                        {
                                            this.onRelationNotLoaded(this.relationType.Id, associationId, roleId);
                                        }
                                    }
                                    else if (this.relationType.RoleType.IsOne)
                                    {
                                        var roleId = long.Parse(roleIdStringArray[0]);

                                        this.classByObjectId.TryGetValue(roleId, out var roleClass);

                                        if (roleClass == null || !allowedRoleClasses.Contains(roleClass))
                                        {
                                            this.onRelationNotLoaded(this.relationType.Id, associationId, roleIdStringArray[0]);
                                        }
                                        else
                                        {
                                            yield return new CompositeRelation(associationId, roleId);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var roleIdString in roleIdStringArray)
                                        {
                                            var roleId = long.Parse(roleIdString);

                                            this.classByObjectId.TryGetValue(roleId, out var roleClass);

                                            if (roleClass == null || !allowedRoleClasses.Contains(roleClass))
                                            {
                                                this.onRelationNotLoaded(this.relationType.Id, associationId, roleId.ToString());
                                            }
                                            else
                                            {
                                                yield return new CompositeRelation(associationId, roleId);
                                            }
                                        }
                                    }

                                    skip = this.reader.IsStartElement();
                                }
                            }
                        }

                        break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}

// <copyright file="RelationTypeOneXmlWriter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IRelationTypeOneXmlWriter type.</summary>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Xml;

    using Adapters;

    using Meta;

    /// <summary>
    /// Writes all relations from a <see cref="IRelationType"/> with a Role
    /// with multiplicity of one  to the <see cref="XmlWriter"/> during a <see cref="IDatabase#Save"/>.
    /// </summary>
    internal class RelationTypeOneXmlWriter : IDisposable
    {
        /// <summary>
        /// The <see cref="relationType"/>.
        /// </summary>
        private readonly IRelationType relationType;

        /// <summary>
        /// The <see cref="xmlWriter"/>.
        /// </summary>
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// At least one role was written.
        /// </summary>
        private bool isInUse;

        /// <summary>
        /// Indicates that this <see cref="RelationTypeOneXmlWriter"/> has been closed.
        /// </summary>
        private bool isClosed;

        /// <summary>
        /// Initializes a new state of the <see cref="RelationTypeOneXmlWriter"/> class.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="xmlWriter">The XML writer.</param>
        internal RelationTypeOneXmlWriter(IRelationType relationType, XmlWriter xmlWriter)
        {
            this.relationType = relationType;
            this.xmlWriter = xmlWriter;
            this.isClosed = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => this.Close();

        /// <summary>
        /// Closes this "<see cref="RelationTypeOneXmlWriter"/>.
        /// </summary>
        internal void Close()
        {
            if (!this.isClosed)
            {
                this.isClosed = true;

                if (this.isInUse)
                {
                    this.xmlWriter.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Writes the the association and role to the <see cref="xmlWriter"/>.
        /// </summary>
        /// <param name="associationId">The association id.</param>
        /// <param name="roleContents">The role contents.</param>
        internal void Write(long associationId, string roleContents)
        {
            if (!this.isInUse)
            {
                this.isInUse = true;
                if (this.relationType.RoleType.ObjectType.IsUnit)
                {
                    this.xmlWriter.WriteStartElement(Serialization.RelationTypeUnit);
                }
                else
                {
                    this.xmlWriter.WriteStartElement(Serialization.RelationTypeComposite);
                }

                this.xmlWriter.WriteAttributeString(Serialization.Id, this.relationType.Id.ToString());
            }

            this.xmlWriter.WriteStartElement(Serialization.Relation);
            this.xmlWriter.WriteAttributeString(Serialization.Association, XmlConvert.ToString(associationId));
            this.xmlWriter.WriteString(roleContents);
            this.xmlWriter.WriteEndElement();
        }
    }
}

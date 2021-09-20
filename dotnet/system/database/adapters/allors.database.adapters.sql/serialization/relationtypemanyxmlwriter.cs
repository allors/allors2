// <copyright file="RelationTypeManyXmlWriter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Text;
    using System.Xml;

    using Adapters;

    using Meta;

    /// <summary>
    /// Writes all relations from a <see cref="IRelationType"/> with a <see cref="IRoleType"/> with multiplicity of many
    /// to the <see cref="XmlWriter"/> during a <see cref="IDatabase#Save"/>.
    /// </summary>
    internal class RelationTypeManyXmlWriter : IDisposable
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
        /// The previously written <see cref="Association"/> Id.
        /// </summary>
        private long previousAssociationId;

        /// <summary>
        /// The <see cref="StringBuilder"/> that accumulates all the roles for one relation.
        /// </summary>
        private StringBuilder rolesStringBuilder;

        /// <summary>
        /// Initializes a new state of the <see cref="RelationTypeManyXmlWriter"/> class.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <param name="xmlWriter">The XML writer.</param>
        internal RelationTypeManyXmlWriter(IRelationType relationType, XmlWriter xmlWriter)
        {
            this.relationType = relationType;
            this.xmlWriter = xmlWriter;
            this.rolesStringBuilder = new StringBuilder();
            this.previousAssociationId = -1;
            this.isClosed = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => this.Close();

        /// <summary>
        /// Closes this "<see cref="RelationTypeManyXmlWriter"/>.
        /// </summary>
        internal void Close()
        {
            if (!this.isClosed)
            {
                this.isClosed = true;

                this.WriteRolesIfPresent();

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
        /// <param name="roleId">The role id.</param>
        internal void Write(long associationId, long roleId)
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

            if (this.previousAssociationId != associationId)
            {
                this.WriteRolesIfPresent();
                this.rolesStringBuilder = new StringBuilder();
                this.previousAssociationId = associationId;
            }

            if (this.rolesStringBuilder.Length != 0)
            {
                this.rolesStringBuilder.Append(Serialization.ObjectsSplitter);
            }

            this.rolesStringBuilder.Append(XmlConvert.ToString(roleId));
        }

        /// <summary>
        /// Writes the roles if the <see cref="RelationTypeManyXmlWriter#rolesStringBuilder"/> contains accumulated roles.
        /// </summary>
        private void WriteRolesIfPresent()
        {
            if (this.rolesStringBuilder.Length > 0)
            {
                this.xmlWriter.WriteStartElement(Serialization.Relation);
                this.xmlWriter.WriteAttributeString(Serialization.Association, XmlConvert.ToString(this.previousAssociationId));
                this.xmlWriter.WriteString(this.rolesStringBuilder.ToString());
                this.xmlWriter.WriteEndElement();
            }
        }
    }
}

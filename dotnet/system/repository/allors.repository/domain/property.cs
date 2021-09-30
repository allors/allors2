// <copyright file="Property.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using Inflector;
    using Text;

    public class Property
    {
        private readonly Inflector inflector;

        public Property(Inflector inflector, Composite definingType, string name)
        {
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.DefiningType = definingType;
            this.inflector = inflector;
            this.RoleName = name;
        }

        public string Id => ((dynamic)this.AttributeByName.Get(AttributeNames.Id))?.Value;

        public string[] WorkspaceNames
        {
            get
            {
                dynamic attribute = this.AttributeByName.Get("Workspace");
                return attribute?.Names ?? Array.Empty<string>();
            }
        }

        public Origin Origin => (Origin)(int)(((dynamic)this.AttributeByName.Get(AttributeNames.Origin))?.Value ?? Origin.Database);

        public bool Required => (bool)(((dynamic)this.AttributeByName.Get(AttributeNames.Required))?.Value ?? false);

        public bool Unique => (bool)(((dynamic)this.AttributeByName.Get(AttributeNames.Unique))?.Value ?? false);

        public bool HasDatabaseOrigin => this.Origin == Origin.Database;

        public XmlDoc XmlDoc { get; set; }

        public Composite DefiningType { get; }

        public Type Type { get; set; }

        public Property DefiningProperty { get; set; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public Multiplicity Multiplicity
        {
            get
            {
                if (this.Type is Unit)
                {
                    return Multiplicity.OneToOne;
                }

                dynamic attribute = this.AttributeByName.Get("Multiplicity");
                if (attribute == null)
                {
                    return Multiplicity.ManyToOne;
                }

                return (Multiplicity)(int)attribute.Value;
            }
        }

        public bool IsRoleOne => !this.IsRoleMany;

        public bool IsRoleMany
        {
            get
            {
                switch (this.Multiplicity)
                {
                    case Multiplicity.OneToMany:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsAssociationOne => !this.IsAssociationMany;

        public bool IsAssociationMany
        {
            get
            {
                switch (this.Multiplicity)
                {
                    case Multiplicity.ManyToOne:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public string RoleName { get; }

        public string RoleSingularName
        {
            get
            {
                if (this.IsRoleOne)
                {
                    return this.RoleName;
                }

                dynamic attribute = this.AttributeByName.Get("Singular");
                return attribute != null ? attribute.Value : this.inflector.Singularize(this.RoleName);
            }
        }

        public string AssignedRoleSingularName => !this.RoleSingularName.Equals(this.Type.SingularName) ? this.RoleSingularName : null;

        public string RolePluralName
        {
            get
            {
                if (this.IsRoleMany)
                {
                    return this.RoleName;
                }

                dynamic attribute = this.AttributeByName.Get("Plural");
                return attribute != null ? attribute.Value : this.inflector.Pluralize(this.RoleName);
            }
        }

        public string AssignedRolePluralName => !Pluralizer.Pluralize(this.RoleSingularName).Equals(this.RolePluralName) ? this.RolePluralName : null;

        public string AssociationName
        {
            get
            {
                if (this.IsAssociationMany)
                {
                    return this.DefiningType.PluralName + "Where" + this.RoleSingularName;
                }

                return this.DefiningType.SingularName + "Where" + this.RoleSingularName;
            }
        }

        public override string ToString() => $"{this.DefiningType.SingularName}.{this.RoleSingularName}";
    }
}

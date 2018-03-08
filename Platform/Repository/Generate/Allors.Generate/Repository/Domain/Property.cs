//------------------------------------------------------------------------------------------------- 
// <copyright file="Property.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors.Repository.Attributes;

    public class Property
    {
        private readonly Inflector.Inflector inflector;

        private string name;
        
        public Property(Inflector.Inflector inflector, Composite definingType, string name)
        {
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.DefiningType = definingType;
            this.inflector = inflector;
            this.name = name;
        }

        public string Id => ((IdAttribute)this.AttributeByName.Get(AttributeNames.Id))?.Value;

        public string AssociationId => ((AssociationIdAttribute)this.AttributeByName.Get(AttributeNames.AssociationId))?.Value;

        public string RoleId => ((RoleIdAttribute)this.AttributeByName.Get(AttributeNames.RoleId))?.Value;

        public XmlDoc XmlDoc { get; set; }

        public Composite DefiningType { get; }

        public Type Type { get; internal set; }

        public Property DefiningProperty { get; internal set; }

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

                Attribute attribute;
                return this.AttributeByName.TryGetValue("Multiplicity", out attribute) ? ((MultiplicityAttribute)attribute).Value : Multiplicity.ManyToOne;
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

        public string RoleName => this.name;
       
        public string RoleSingularName 
        {
            get
            {
                if (this.IsRoleOne)
                {
                    return this.name;
                }
                else
                {
                    Attribute attribute;
                    return this.AttributeByName.TryGetValue("Singular", out attribute) ? ((SingularAttribute)attribute).Value : this.inflector.Singularize(this.name);
                }
            }
        }

        public string RolePluralName
        {
            get
            {
                if (this.IsRoleMany)
                {
                    return this.name;
                }
                else
                {
                    Attribute attribute;
                    return this.AttributeByName.TryGetValue("Plural", out attribute) ? ((PluralAttribute)attribute).Value : this.inflector.Pluralize(this.name);
                }
            }
        }

        public string AssociationName
        {
            get
            {
                if (this.IsAssociationMany)
                {
                    return this.DefiningType.PluralName + "Where" + this.RoleSingularName;
                }
                else
                {
                    return this.DefiningType.SingularName + "Where" + this.RoleSingularName;
                }
            }
        }

        public override string ToString()
        {
            return $"{this.DefiningType.SingularName}.{this.RoleSingularName}";
        }
    }
}
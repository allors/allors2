//------------------------------------------------------------------------------------------------- 
// <copyright file="Composite.cs" company="Allors bvba">
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
    using System.Linq;

    public abstract class Composite : Type
    {
        private readonly Inflector.Inflector inflector;

        protected Composite(Inflector.Inflector inflector, Guid id, string name)
            : base(id, name)
        {
            this.inflector = inflector;
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();
            this.ImplementedInterfaces = new List<Interface>();
            this.PropertyByRoleName = new Dictionary<string, Property>();
            this.DefinedReversePropertyByAssociationName = new Dictionary<string, Property>();
            this.InheritedReversePropertyByAssociationName = new Dictionary<string, Property>();
            this.MethodByName = new Dictionary<string, Method>();
        }

        public XmlDoc XmlDoc { get; set; }

        public string PluralName
        {
            get
            {
                dynamic attribute = this.AttributeByName.Get("Plural");
                return attribute != null ? attribute.Value : this.inflector.Pluralize(this.SingularName);
            }
        }

        public abstract Interface[] Interfaces { get; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public IList<Interface> ImplementedInterfaces { get; }

        public Dictionary<string, Property> PropertyByRoleName { get; }

        public Dictionary<string, Property> DefinedReversePropertyByAssociationName { get; }

        public Dictionary<string, Property> InheritedReversePropertyByAssociationName { get; }

        public Dictionary<string, Method> MethodByName { get; }

        public Property[] Properties => this.PropertyByRoleName.Values.ToArray();

        public Property[] DefinedProperties => this.PropertyByRoleName.Values.Where(v => v.DefiningProperty == null).ToArray();

        public Property[] ImplementedProperties => this.PropertyByRoleName.Values.Where(v => v.DefiningProperty != null).ToArray();

        public Property[] DefinedReverseProperties => this.DefinedReversePropertyByAssociationName.Values.ToArray();

        public Property[] InheritedReverseProperties => this.InheritedReversePropertyByAssociationName.Values.ToArray();

        public Method[] Methods => this.MethodByName.Values.ToArray();

        public Method[] DefinedMethods => this.MethodByName.Values.Where(v => v.DefiningMethod == null).ToArray();

        public Method[] InheritedMethods => this.MethodByName.Values.Where(v => v.DefiningMethod != null).ToArray();

        public override string ToString() => this.SingularName;
    }
}

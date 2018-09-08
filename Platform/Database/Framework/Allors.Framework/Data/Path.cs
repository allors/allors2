// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Path.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public class Path
    {
        public Path()
        {
        }

        public Path(params IPropertyType[] propertyTypes)
        {
            if (propertyTypes.Length > 0)
            {
                var previous = this;
                previous.PropertyType = propertyTypes[0];

                for (var i = 1; i < propertyTypes.Length; i++)
                {
                    previous.Next = new Path { PropertyType = propertyTypes[i] };
                    previous = previous.Next;
                }
            }
        }

        public Path(IMetaPopulation metaPopulation, params Guid[] propertyTypeIds)
            : this(propertyTypeIds.Select(v => (IPropertyType)metaPopulation.Find(v)).ToArray())
        {
        }

        public bool ExistPropertyType => this.PropertyType != null;

        public IPropertyType PropertyType { get; set; }

        public bool ExistNext => this.Next != null;

        public Path Next { get; set; }

        public Path End => this.ExistNext ? this.Next.End : this;

        public string Name
        {
            get
            {
                var name = new StringBuilder();
                name.Append(this.PropertyType.Name);
                if (this.ExistNext)
                {
                    this.Next.AppendToName(name);
                }

                return name.ToString();
            }
        }

        public static bool TryParse(IComposite composite, string pathString, out Path path)
        {
            var propertyType = Resolve(composite, pathString);
            path = propertyType == null ? null : new Path(propertyType);
            return path != null;
        }

        public Protocol.Path Save()
        {
            return new Protocol.Path
                       {
                           PropertyType = this.PropertyType.Id,
                           Next = this.Next.Save()
                       };
        }

        public IObjectType GetObjectType()
        {
            if (this.ExistNext)
            {
                return this.Next.GetObjectType();
            }

            return this.PropertyType.ObjectType;
        }

        public override string ToString()
        {
            return this.Name;
        }

        private static IPropertyType Resolve(IComposite composite, string propertyName)
        {
            var lowerCasePropertyName = propertyName.ToLowerInvariant();

            foreach (var roleType in composite.RoleTypes)
            {
                if (roleType.SingularName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    roleType.SingularFullName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    roleType.PluralName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    roleType.PluralFullName.ToLowerInvariant().Equals(lowerCasePropertyName))
                {
                    return roleType;
                }
            }

            foreach (var associationType in composite.AssociationTypes)
            {
                if (associationType.SingularName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    associationType.SingularFullName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    associationType.SingularPropertyName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    associationType.PluralName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    associationType.PluralFullName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                    associationType.PluralPropertyName.ToLowerInvariant().Equals(lowerCasePropertyName))
                {
                    return associationType;
                }
            }

            return null;
        }

        private void AppendToName(StringBuilder name)
        {
            name.Append("." + this.PropertyType.Name);

            if (this.ExistNext)
            {
                this.Next.AppendToName(name);
            }
        }

    }
}
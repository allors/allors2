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
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public class Step
    {
        public Step()
        {
        }

        internal Step(IPropertyType[] propertyTypes, int index)
        {
            this.PropertyType = propertyTypes[index];

            var nextIndex = index + 1;
            if (nextIndex < propertyTypes.Length)
            {
                this.Next = new Step(propertyTypes, nextIndex);
            }
        }

        public Tree Tree { get; set; }

        public IPropertyType PropertyType { get; set; }

        public bool ExistNext => this.Next != null;

        public Step Next { get; set; }

        public Step End => this.ExistNext ? this.Next.End : this;

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
                Tree = this.Tree?.Save(),
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
            var name = new StringBuilder();
            name.Append(this.PropertyType.Name);
            if (this.ExistNext)
            {
                this.Next.AppendToName(name);
            }

            return name.ToString();
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
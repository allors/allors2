
// <copyright file="Path.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Linq;
    using System.Text;

    public class Path
    {
        public Path()
        {
        }

        public Path(params PropertyType[] propertyTypes)
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

        public Path(MetaPopulation metaPopulation, params string[] propertyTypeIds)
            : this(propertyTypeIds.Select(x => (PropertyType)metaPopulation.Find(new Guid(x))).ToArray())
        {
        }

        public bool ExistPropertyType => this.PropertyType != null;

        public PropertyType PropertyType { get; set; }

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

        public static bool TryParse(Composite composite, string pathString, out Path path)
        {
            var propertyType = Resolve(composite, pathString);
            path = propertyType == null ? null : new Path(propertyType);
            return path != null;
        }

        public override string ToString() => this.Name;

        public ObjectType GetObjectType()
        {
            if (this.ExistNext)
            {
                return this.Next.GetObjectType();
            }

            return this.PropertyType.GetObjectType();
        }

        private static PropertyType Resolve(Composite composite, string propertyName)
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

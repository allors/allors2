// <copyright file="Select.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Text;
    using Meta;

    public class Select : IVisitable
    {
        public Select()
        {
        }

        public Select(params IPropertyType[] propertyTypes) : this(propertyTypes, 0)
        {
        }

        internal Select(IPropertyType[] propertyTypes, int index)
        {
            this.PropertyType = propertyTypes[index];

            var nextIndex = index + 1;
            if (nextIndex < propertyTypes.Length)
            {
                this.Next = new Select(propertyTypes, nextIndex);
            }
        }

        public bool IsOne
        {
            get
            {
                if (this.PropertyType.IsMany)
                {
                    return false;
                }

                return this.ExistNext ? this.Next.IsOne : this.PropertyType.IsOne;
            }
        }

        public IEnumerable<Node> Include { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Select Next { get; set; }

        public bool ExistNext => this.Next != null;

        public Select End => this.ExistNext ? this.Next.End : this;

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
                this.Next.ToStringAppendToName(name);
            }

            return name.ToString();
        }

        private void ToStringAppendToName(StringBuilder name)
        {
            name.Append('.').Append(this.PropertyType.Name);

            if (this.ExistNext)
            {
                this.Next.ToStringAppendToName(name);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitSelect(this);
    }
}

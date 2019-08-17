// <copyright file="Step.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Allors.Workspace.Meta;
    using System.Text;

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

        public Tree Include { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Step Next { get; set; }

        public bool ExistNext => this.Next != null;

        public Step End => this.ExistNext ? this.Next.End : this;

        public Protocol.Data.Step ToJson() =>
            new Protocol.Data.Step
            {
                Include = this.Include?.ToJson(),
                PropertyType = this.PropertyType.Id,
                Next = this.Next.ToJson(),
            };

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

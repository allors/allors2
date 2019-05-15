// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Step.cs" company="Allors bvba">
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

        public Protocol.Data.Step Save()
        {
            return new Protocol.Data.Step
            {
                Include = this.Include?.Save(),
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
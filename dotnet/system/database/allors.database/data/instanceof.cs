// <copyright file="Instanceof.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using Meta;


    public class Instanceof : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Instanceof(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Parameter { get; set; }

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && (arguments == null || !arguments.HasArgument(this.Parameter));

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var composite = this.Parameter != null ? (IComposite)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter)) : this.ObjectType;

            if (this.PropertyType != null)
            {
                if (this.PropertyType is IRoleType roleType)
                {
                    compositePredicate.AddInstanceof(roleType, composite);
                }
                else
                {
                    var associationType = (IAssociationType)this.PropertyType;
                    compositePredicate.AddInstanceof(associationType, composite);
                }
            }
            else
            {
                compositePredicate.AddInstanceof(composite);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitInstanceOf(this);
    }
}

// <copyright file="Contains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using Meta;


    public class Contains : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Contains(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public string Parameter { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && (arguments == null || !arguments.HasArgument(this.Parameter));

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var containedObject = this.Parameter != null ? transaction.GetObject(arguments.ResolveObject(this.Parameter)) : this.Object;

            if (this.PropertyType is IRoleType roleType)
            {
                compositePredicate.AddContains(roleType, containedObject);
            }
            else
            {
                var associationType = (IAssociationType)this.PropertyType;
                compositePredicate.AddContains(associationType, containedObject);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitContains(this);
    }
}

// <copyright file="Exists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using Meta;

    public class Exists : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Parameter { get; set; }

        public IPropertyType PropertyType { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && (arguments == null || !arguments.HasArgument(this.Parameter));

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var propertyType = this.Parameter != null ? (IPropertyType)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter)) : this.PropertyType;

            if (propertyType != null)
            {
                if (propertyType is IRoleType roleType)
                {
                    compositePredicate.AddExists(roleType);
                }
                else
                {
                    var associationType = (IAssociationType)propertyType;
                    compositePredicate.AddExists(associationType);
                }
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitExists(this);
    }
}

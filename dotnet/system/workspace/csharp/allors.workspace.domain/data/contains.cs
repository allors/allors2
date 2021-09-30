// <copyright file="Contains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    
    using Meta;

    public class Contains : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Contains(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitContains(this);
    }
}

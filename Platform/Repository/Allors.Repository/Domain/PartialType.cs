// <copyright file="PartialType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System.Collections.Generic;

    public abstract class PartialType
    {
        protected PartialType(string name)
        {
            this.Name = name;
            this.PropertyByName = new Dictionary<string, Property>();
            this.MethodByName = new Dictionary<string, Method>();
        }

        public string Name { get; }

        public Dictionary<string, Property> PropertyByName { get; }

        public Dictionary<string, Method> MethodByName { get; }

        public override string ToString() => this.Name;
    }
}

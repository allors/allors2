// <copyright file="MultiplicityAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class OriginAttribute : RepositoryAttribute
    {
        public OriginAttribute(Origin value) => this.Value = value;

        public Origin Value { get; set; }
    }
}

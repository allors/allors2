// <copyright file="IdAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Method)]
    public class IdAttribute : RepositoryAttribute
    {
        public IdAttribute(string value) => this.Value = value;

        public string Value { get; set; }
    }
}

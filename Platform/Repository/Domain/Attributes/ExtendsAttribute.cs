// <copyright file="ExtendsAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Struct)]
    public class ExtendsAttribute : RepositoryAttribute
    {
        public ExtendsAttribute(string value) => this.Value = value;

        public string Value { get; set; }
    }
}

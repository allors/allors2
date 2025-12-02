// <copyright file="Multiplicity.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class TagsAttribute : RepositoryAttribute
    {
        public TagsAttribute(params string[] values) => this.Values = values;

        public string[] Values { get; set; }
    }
}

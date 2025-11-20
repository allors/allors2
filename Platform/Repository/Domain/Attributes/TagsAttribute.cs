// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class TagsAttribute : RepositoryAttribute
    {
        public TagsAttribute(params string[] values) => this.Values = values;

        public string[] Values { get; set; }
    }
}

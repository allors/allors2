// <copyright file="PluralAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property)]
    public class PluralAttribute : RepositoryAttribute
    {
        public PluralAttribute(string value) => this.Value = value;

        public string Value { get; set; }
    }
}

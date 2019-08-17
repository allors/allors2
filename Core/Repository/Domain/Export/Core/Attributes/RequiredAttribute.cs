// <copyright file="RequiredAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : RepositoryAttribute
    {
        public RequiredAttribute(bool value = true) => this.Value = value;

        public bool Value { get; set; }
    }
}

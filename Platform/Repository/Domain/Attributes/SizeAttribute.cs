// <copyright file="SizeAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class SizeAttribute : RepositoryAttribute
    {
        public SizeAttribute(int value) => this.Value = value;

        public int Value { get; set; }
    }
}

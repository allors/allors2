// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class MediaTypeAttribute : RepositoryAttribute
    {
        public MediaTypeAttribute(string value) => this.Value = value;

        public string Value { get; set; }
    }
}

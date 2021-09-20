// <copyright file="WorkspaceAttribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class WorkspaceAttribute : RepositoryAttribute
    {
        private static readonly string[] DefaultNames = { "Default" };

        public WorkspaceAttribute(params string[] names) => this.Names = names.Length > 0 ? names : DefaultNames;

        public string[] Names { get; }
    }
}

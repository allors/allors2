// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Security
{
    using System.Collections.Generic;
    using Meta;

    public interface IAccessControlList
    {
        ISet<IMethodType> Execute { get; }

        ISet<IRoleType> Read { get; }

        ISet<IRoleType> Write { get; }
    }
}

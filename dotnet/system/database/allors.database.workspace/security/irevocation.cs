// <copyright file="AccessControlList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Security
{
    /// <summary>
    /// List of permissions for an object/user combination.
    /// </summary>
    public interface IRevocation
    {
        IStrategy Strategy { get; }

        IPermission[] Permissions { get; }
    }
}

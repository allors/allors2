// <copyright file="AccessControlList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Security
{
    using System.Collections.Generic;
    using Meta;

    /// <summary>
    /// List of permissions for an object/user combination.
    /// </summary>
    public interface IAccessControlList
    {
        IEnumerable<IGrant> Grants { get; }

        IEnumerable<IRevocation> Revocations { get; }

        bool CanRead(IRoleType roleType);

        bool CanWrite(IRoleType roleType);

        bool CanExecute(IMethodType methodType);
    }
}

// <copyright file="Permission.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Security
{
    using Meta;

    public interface IWritePermission : IPermission
    {
        IRelationType RelationType { get; }
    }
}

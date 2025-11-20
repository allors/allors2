// <copyright file="IExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IExtent
    {
        IComposite ObjectType { get; }

        Sort[] Sorting { get; set; }

        Protocol.Data.Extent ToJson();
    }
}

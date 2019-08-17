// <copyright file="IExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

        bool HasMissingArguments(IReadOnlyDictionary<string, object> arguments);
    }
}

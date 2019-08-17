// <copyright file="IExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Allors.Workspace.Meta;
    using System.Collections.Generic;

    public interface IExtent
    {
        IComposite ObjectType { get; }

        Sort[] Sorting { get; set; }

        Protocol.Data.Extent ToJson();

        bool HasMissingArguments(IReadOnlyDictionary<string, object> arguments);
    }
}

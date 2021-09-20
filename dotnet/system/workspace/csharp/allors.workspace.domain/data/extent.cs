// <copyright file="IExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public interface Extent : IVisitable
    {
        IComposite ObjectType { get; }

        // TODO: move to Result
        Sort[] Sorting { get; set; }
    }
}

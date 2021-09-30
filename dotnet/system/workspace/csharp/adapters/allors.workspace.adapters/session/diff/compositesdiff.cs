// <copyright file="UnitDiff.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;
    using Meta;

    public class CompositesDiff : Diff, ICompositesDiff
    {
        public CompositesDiff(IRelationType relationType, Strategy association) : base(relationType, association)
        {
        }

        public IReadOnlyList<IStrategy> OriginalRoles { get; set; }

        public IReadOnlyList<IStrategy> ChangedRoles { get; set; }
    }
}

// <copyright file="UnitDiff.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public class UnitDiff : Diff, IUnitDiff
    {
        public UnitDiff(IRelationType relationType, Strategy association) : base(relationType, association)
        {
        }

        public object OriginalRole { get; set; }

        public object ChangedRole { get; set; }
    }
}

// <copyright file="UnitDiff.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public class Diff : IDiff
    {
        protected Diff(IRelationType relationType, Strategy association)
        {
            this.RelationType = relationType;
            this.Association = association;
        }

        public IRelationType RelationType { get; set; }

        public IStrategy Association { get; set; }
    }
}

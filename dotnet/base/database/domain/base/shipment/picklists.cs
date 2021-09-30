// <copyright file="PickLists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PickLists
    {
        public Extent<PickList> PendingPickLists
        {
            get
            {
                var pickLists = new PickLists(this.Session).Extent();
                pickLists.Filter.AddNot().AddExists(M.PickList.Picker);
                pickLists.Filter.AddEquals(M.PickList.PickListState, new PickListStates(this.Session).Created);

                return pickLists;
            }
        }

        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.PickListState);

        protected override void BaseSecure(Security config)
        {
            var created = new PickListStates(this.Session).Created;
            var onHold = new PickListStates(this.Session).OnHold;
            var picked = new PickListStates(this.Session).Picked;
            var cancelled = new PickListStates(this.Session).Cancelled;

            config.Deny(this.ObjectType, created, this.Meta.Continue);
            config.Deny(this.ObjectType, onHold, this.Meta.Hold);

            config.Deny(this.ObjectType, picked, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
        }
    }
}

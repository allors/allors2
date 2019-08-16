// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickLists.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;
    using Meta;

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

        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.PickListState);
        }

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

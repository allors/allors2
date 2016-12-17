// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickLists.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
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
                pickLists.Filter.AddEquals(M.PickList.CurrentObjectState, new PickListObjectStates(this.Session).Created);

                return pickLists;
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.PickListObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
            
            config.GrantOperations(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.CurrentPickListStatus, Operations.Read);
            config.GrantCustomer(this.ObjectType, Meta.PickListStatuses, Operations.Read);

            var created = new PickListObjectStates(Session).Created;
            var onHold = new PickListObjectStates(Session).OnHold;
            var picked = new PickListObjectStates(Session).Picked;
            var cancelled = new PickListObjectStates(Session).Cancelled;

            config.Deny(this.ObjectType, created, Meta.Continue);
            config.Deny(this.ObjectType, onHold, Meta.Hold);

            config.Deny(this.ObjectType, picked, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
        }
    }
}
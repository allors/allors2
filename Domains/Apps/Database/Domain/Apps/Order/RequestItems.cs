// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestItems.cs" company="Allors bvba">
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

    public partial class RequestItems
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var draft = new RequestItemStates(this.Session).Draft;
            var cancelled = new RequestItemStates(this.Session).Cancelled;
            var submitted = new RequestItemStates(this.Session).Submitted;

            var cancel = this.Meta.Cancel;
            var hold = this.Meta.Hold;
            var submit = this.Meta.Submit;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, cancelled, cancel, submit, hold);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
        }
    }
}
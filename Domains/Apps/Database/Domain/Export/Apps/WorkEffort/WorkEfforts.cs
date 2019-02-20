// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEfforts.cs" company="Allors bvba">
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
    using Meta;

    public partial class WorkEfforts
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new WorkEffortStates(this.Session).Created;
            var cancelled = new WorkEffortStates(this.Session).Cancelled;
            var completed = new WorkEffortStates(this.Session).Completed;
            var finished = new WorkEffortStates(this.Session).Finished;

            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var invoice = this.Meta.Invoice;

            config.Deny(this.ObjectType, created, M.WorkEffort.Reopen);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Execute, Operations.Write);

            config.Deny(this.ObjectType, cancelled, cancel, invoice);
            config.Deny(this.ObjectType, completed, cancel);
            config.Deny(this.ObjectType, finished, cancel, invoice, reopen);
        }
    }
}
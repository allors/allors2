// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItems.cs" company="Allors bvba">
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
    using Allors;

    public partial class SalesInvoiceItems
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, SalesInvoiceItemObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            ObjectState paid = new SalesInvoiceItemObjectStates(Session).Paid;
            ObjectState writtenOff = new SalesInvoiceItemObjectStates(Session).WrittenOff;
            ObjectState cancelled = new SalesInvoiceItemObjectStates(Session).Cancelled;

            config.Deny(this.ObjectType, paid, Operation.Write, Operation.Execute);
            config.Deny(this.ObjectType, writtenOff, Operation.Write, Operation.Execute);
            config.Deny(this.ObjectType, cancelled, Operation.Write, Operation.Execute);
        }
    }
}
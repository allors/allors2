// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Roles.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

    public partial class Roles
    {
        public static readonly Guid ProductQuoteApproverId = new Guid("07D39583-C82C-4EA0-92F1-288FB8E17FA3");
        public static readonly Guid BlueCollarWorkerId = new Guid("3C2D223E-6056-447A-A3F9-AED2413D717D");

        public Role ProductQuoteApprover => this.Sticky[ProductQuoteApproverId];

        public Role BlueCollarWorker => this.Sticky[BlueCollarWorkerId];

        protected override void AppsSetup(Setup config)
        {
            base.AppsSetup(config);

            new RoleBuilder(this.Session).WithName("ProductQuote approver").WithUniqueId(ProductQuoteApproverId).Build();
            new RoleBuilder(this.Session).WithName("Blue-collar worker").WithUniqueId(BlueCollarWorkerId).Build();
        }
    }
}
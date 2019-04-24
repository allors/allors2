// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Security.cs" company="Allors bvba">
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

using System.Collections.Generic;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Security
    {
        public void GrantBlueCollarWorker(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.BlueCollarWorkerId, objectType, operations);
        }

        public void GrantExceptBlueCollarWorker(ObjectType objectType, ICollection<OperandType> excepts, params Operations[] operations)
        {
            this.GrantExcept(Roles.BlueCollarWorkerId, objectType, excepts, operations);
        }

        public void GrantProductQuoteApprover(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.ProductQuoteApproverId, objectType, operations);
        }

        public void GrantPurchaseOrderApprover(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.PurchaseOrderApproverId, objectType, operations);
        }

        private void AppsOnPreSetup()
        {
        }

        private void AppsOnPostSetup()
        {
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Security.v.cs" company="Allors bvba">
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
        private void CustomOnPreSetup()
        {
            // Default access policy
            var security = new Security(this.session);
            foreach (ObjectType @class in session.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantCreator(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantProductQuoteApprover(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantPurchaseOrderApproverLevel1(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantPurchaseOrderApproverLevel2(@class, Operations.Read, Operations.Write, Operations.Execute);

                if (@class.Equals(M.WorkEffortInventoryAssignment.ObjectType))
                {
                    var excepts = new HashSet<OperandType>
                    {
                        M.WorkEffortInventoryAssignment.BillableQuantity,
                        M.WorkEffortInventoryAssignment.UnitSellingPrice,
                        M.WorkEffortInventoryAssignment.AssignedUnitSellingPrice,
                        M.WorkEffortInventoryAssignment.UnitPurchasePrice,
                    };

                    security.GrantExceptBlueCollarWorker(@class, excepts, Operations.Read, Operations.Write, Operations.Execute);
                }
                else
                {
                    security.GrantBlueCollarWorker(@class, Operations.Read, Operations.Write, Operations.Execute);
                }
            }
        }

        private void CustomOnPostSetup()
        {
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs" company="Allors bvba">
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
    using Allors.Meta;

    public partial class Security
    {
        public void GrantCustomer(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.CustomerRoleId, objectType, operations);
        }

        public void GrantCustomer(ObjectType objectType, OperandType operandType, params Operations[] operations)
        {
            this.Grant(Roles.CustomerRoleId, objectType, operandType, operations);
        }

        public void GrantSupplier(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.SupplierRoleId, objectType, operations);
        }

        public void GrantSupplier(ObjectType objectType, OperandType operandType, params Operations[] operations)
        {
            this.Grant(Roles.SupplierRoleId, objectType, operandType, operations);
        }

        public void GrantPartner(ObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.PartnerRoleId, objectType, operations);
        }

        public void GrantPartner(ObjectType objectType, OperandType operandType, params Operations[] operations)
        {
            this.Grant(Roles.PartnerRoleId, objectType, operandType, operations);
        }
    }
}
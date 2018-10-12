// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemTransaction.cs" company="Allors bvba">
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
    using System.Linq;

    public partial class InventoryItemTransaction
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            derivation.AddDependency(this.InventoryItemWhereInventoryItemTransaction, this);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.InventoryItemWhereInventoryItemTransaction.Part.InventoryItemKind.IsSerialized)
            {
                if (this.Quantity != 1 && this.Quantity != -1 && this.Quantity != 0)
                {
                    var message = "Serialized Inventory Items only accept Quantities of -1, 0, and 1.";
                    derivation.Validation.AddError(this, this.Meta.Quantity, message);
                }
            }
        }
    }
}
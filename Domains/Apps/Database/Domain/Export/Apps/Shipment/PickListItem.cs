// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickListItem.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class PickListItem
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
            {
                derivation.AddDependency(itemIssuance, this);
                derivation.MarkAsModified(itemIssuance, M.ItemIssuance.PickListItem);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.QuantityPicked > this.Quantity)
            {
                derivation.Validation.AddError(this, M.PickListItem.QuantityPicked, ErrorMessages.PickListItemQuantityMoreThanAllowed);
            }
        }
    }
}
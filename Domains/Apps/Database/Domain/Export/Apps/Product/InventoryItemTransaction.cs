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
    using System.Collections.Generic;
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

            //this.AttachToInventoryItem(derivation);
        }

        private void AttachToInventoryItem(IDerivation derivation)
        {
            // Match on required properties
            bool matched = false;
            bool possibleMatches = true;

            var inventoryItems = this.Part.InventoryItemsWherePart.ToArray();
            var matchingItems = inventoryItems.Where(i => i.Facility.Equals(this.Facility));
            possibleMatches = matchingItems.Count() > 0;

            if (possibleMatches)
            {
                matchingItems = matchingItems.Where(m => m.UnitOfMeasure.Equals(this.UnitOfMeasure));
                possibleMatches = matchingItems.Count() > 0;
            }

            // Match on optional properties
            if (possibleMatches && this.ExistLot && matchingItems.Count() > 0)
            {
                matchingItems = matchingItems.Where(m => m.Lot.Equals(this.Lot));
                possibleMatches = matchingItems.Count() > 0;
            }

            if (possibleMatches)
            {
                // Match on Non/SerialisedInventoryItemState
                foreach (InventoryItem item in matchingItems)
                {
                    if (item is NonSerialisedInventoryItem nonSerialItem)
                    {
                        if (nonSerialItem.NonSerialisedInventoryItemState.Equals(this.Reason.DefaultNonSerialisedInventoryItemState))
                        {
                            matched = true;
                            item.AddInventoryItemTransaction(this);
                            break;
                        }
                    }
                    else if (item is SerialisedInventoryItem serialItem)
                    {
                        if (serialItem.SerialisedInventoryItemState.Equals(this.Reason.DefaultSerialisedInventoryItemState))
                        {
                            matched = true;
                            item.AddInventoryItemTransaction(this);
                            break;
                        }
                    }
                }
            }
            
            if (!matched)
            {
                var facility = this.Facility ?? this.Part.DefaultFacility;
                var unitOfMeasure = this.UnitOfMeasure ?? this.Part.UnitOfMeasure;

                if (this.Part.InventoryItemKind.IsSerialized)
                {
                    var builder = new SerialisedInventoryItemBuilder(this.strategy.Session)
                        .WithFacility(facility)
                        .WithUnitOfMeasure(unitOfMeasure)
                        .WithPart(this.Part)
                        .WithSerialisedInventoryItemState(this.Reason.DefaultSerialisedInventoryItemState);

                    if (this.ExistLot)
                    {
                        builder.WithLot(this.Lot);
                    }

                    InventoryItem item = builder.Build();
                    item.AddInventoryItemTransaction(this);
                }
                else if (this.Part.InventoryItemKind.IsNonSerialized)
                {
                    var builder = new NonSerialisedInventoryItemBuilder(this.strategy.Session)
                        .WithFacility(facility)
                        .WithUnitOfMeasure(unitOfMeasure)
                        .WithPart(this.Part)
                        .WithNonSerialisedInventoryItemState(this.Reason.DefaultNonSerialisedInventoryItemState);

                    if (this.ExistLot)
                    {
                        builder.WithLot(this.Lot);
                    }

                    InventoryItem item = builder.Build();
                    item.AddInventoryItemTransaction(this);
                }
            }
        }
    }
}
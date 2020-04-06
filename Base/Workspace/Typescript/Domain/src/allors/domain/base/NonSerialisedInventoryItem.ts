import { domain } from '../domain';
import { NonSerialisedInventoryItem } from '../generated/NonSerialisedInventoryItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { InventoryItemTransaction } from '..';
import { assert } from '../../framework';
import { NonUnifiedPart } from '../generated';

declare module '../generated/NonSerialisedInventoryItem.g' {
  interface NonSerialisedInventoryItem {
    facilityName: string;
    quantityOnHand: number;
    availableToPromise: number;
    quantityCommittedOut: number;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.NonSerialisedInventoryItem);
  assert(cls);

  Object.defineProperty(cls.prototype, 'facilityName', {
    configurable: true,
    get(this: NonSerialisedInventoryItem): string  {
      return this.Facility?.Name ?? '';
    },
  });

  Object.defineProperty(cls.prototype, 'quantityOnHand', {
    configurable: true,
    get(this: NonSerialisedInventoryItem) {
      let quantity = 0;

      this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
        const reason = inventoryTransaction.Reason;

        if (reason?.IncreasesQuantityOnHand && inventoryTransaction.Quantity) {
          if (reason.IncreasesQuantityOnHand) {
            quantity += parseFloat(inventoryTransaction.Quantity);
          } else {
            quantity -= parseFloat(inventoryTransaction.Quantity);
          }
        }

      });

      return quantity;
    },
  });

  Object.defineProperty(cls.prototype, 'quantityCommittedOut', {
    configurable: true,
    get(this: NonSerialisedInventoryItem) {
      let quantity = 0;

      this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
        const reason = inventoryTransaction.Reason;

        if (reason?.IncreasesQuantityCommittedOut && inventoryTransaction.Quantity) {
          if (reason.IncreasesQuantityCommittedOut) {
            quantity += parseFloat(inventoryTransaction.Quantity);
          } else {
            quantity -= parseFloat(inventoryTransaction.Quantity);
          }
        }
      });

      return quantity;
    },
  });

  Object.defineProperty(cls.prototype, 'availableToPromise', {
    configurable: true,
    get(this: NonSerialisedInventoryItem) {

      let quantity = this.quantityOnHand - this.quantityCommittedOut;

      if (quantity < 0) {
        quantity = 0;
      }

      return quantity;
    },
  });

});

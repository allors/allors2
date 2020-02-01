import { domain } from '../domain';
import { NonSerialisedInventoryItem } from '../generated/NonSerialisedInventoryItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { InventoryItemTransaction, PickListItem } from '..';

declare module '../generated/NonSerialisedInventoryItem.g' {
  interface NonSerialisedInventoryItem {
    facilityName;
    quantityOnHand;
    availableToPromise;
    quantityCommittedOut;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.NonSerialisedInventoryItem).prototype as any;

  Object.defineProperty(obj, 'facilityName', {
    configurable: true,
    get(this: NonSerialisedInventoryItem): string {

      return this.Facility.Name;
    },
  });

  Object.defineProperty(obj, 'quantityOnHand', {
    configurable: true,
    get(this: NonSerialisedInventoryItem) {
      let quantity = 0;

      this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
        const reason = inventoryTransaction.Reason;

        if (reason.IncreasesQuantityOnHand === true) {
          quantity += parseFloat(inventoryTransaction.Quantity);
        } else if (reason.IncreasesQuantityOnHand === false) {
          quantity -= parseFloat(inventoryTransaction.Quantity);
        }
      });

      return quantity;
    },
  });

  Object.defineProperty(obj, 'quantityCommittedOut', {
    configurable: true,
    get(this: NonSerialisedInventoryItem) {
      let quantity = 0;

      this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
        const reason = inventoryTransaction.Reason;

        if (reason.IncreasesQuantityCommittedOut === true) {
          quantity += parseFloat(inventoryTransaction.Quantity);
        } else if (reason.IncreasesQuantityCommittedOut === false) {
          quantity -= parseFloat(inventoryTransaction.Quantity);
        }
      });

      return quantity;
    },
  });

  Object.defineProperty(obj, 'availableToPromise', {
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

import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { NonSerialisedInventoryItem } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendNonSerialisedInventoryItem(workspace: Workspace) {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.NonSerialisedInventoryItem);
  assert(cls);

  Object.defineProperty(cls.prototype, 'facilityName', {
    configurable: true,
    get(this: NonSerialisedInventoryItem): string  {
      return this.Facility?.Name ?? '';
    },
  });

  // Object.defineProperty(cls.prototype, 'quantityOnHand', {
  //   configurable: true,
  //   get(this: NonSerialisedInventoryItem) {
  //     let quantity = 0;

  //     this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
  //       const reason = inventoryTransaction.Reason;

  //       if (reason?.IncreasesQuantityOnHand && inventoryTransaction.Quantity) {
  //         if (reason.IncreasesQuantityOnHand) {
  //           quantity += parseFloat(inventoryTransaction.Quantity);
  //         } else {
  //           quantity -= parseFloat(inventoryTransaction.Quantity);
  //         }
  //       }

  //     });

  //     return quantity;
  //   },
  // });

  // Object.defineProperty(cls.prototype, 'quantityCommittedOut', {
  //   configurable: true,
  //   get(this: NonSerialisedInventoryItem) {
  //     let quantity = 0;

  //     this.InventoryItemTransactionsWhereInventoryItem.forEach((inventoryTransaction: InventoryItemTransaction) => {
  //       const reason = inventoryTransaction.Reason;

  //       if (reason?.IncreasesQuantityCommittedOut && inventoryTransaction.Quantity) {
  //         if (reason.IncreasesQuantityCommittedOut) {
  //           quantity += parseFloat(inventoryTransaction.Quantity);
  //         } else {
  //           quantity -= parseFloat(inventoryTransaction.Quantity);
  //         }
  //       }
  //     });

  //     return quantity;
  //   },
  // });

  // Object.defineProperty(cls.prototype, 'availableToPromise', {
  //   configurable: true,
  //   get(this: NonSerialisedInventoryItem) {

  //     let quantity = this.quantityOnHand - this.quantityCommittedOut;

  //     if (quantity < 0) {
  //       quantity = 0;
  //     }

  //     return quantity;
  //   },
  // });

};

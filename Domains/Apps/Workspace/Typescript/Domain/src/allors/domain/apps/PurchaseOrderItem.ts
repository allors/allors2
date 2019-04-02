import { domain } from '../domain';
import { PurchaseOrderItem } from '../generated/PurchaseOrderItem.g';

declare module '../generated/PurchaseOrderItem.g' {
  interface PurchaseOrderItem {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: PurchaseOrderItem = workspace.prototypeByName['PurchaseOrderItem'];

  Object.defineProperty(obj, 'displayName', {
    get(this: PurchaseOrderItem) {

      return this.Description || this.Part.Name;
    },
  });

});

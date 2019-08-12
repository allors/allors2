import { domain } from '../domain';
import { PurchaseOrder } from '../generated/PurchaseOrder.g';

declare module '../generated/PurchaseOrder.g' {
  interface PurchaseOrder {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: PurchaseOrder = workspace.prototypeByName['PurchaseOrder'];

  Object.defineProperty(obj, 'displayName', {
    get(this: PurchaseOrder): string {

      return this.OrderNumber + ' ' + this.TakenViaSupplier.PartyName;
    },
  });

});

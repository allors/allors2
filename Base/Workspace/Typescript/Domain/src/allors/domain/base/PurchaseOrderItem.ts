import { domain } from '../domain';
import { PurchaseOrderItem } from '../generated/PurchaseOrderItem.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/PurchaseOrderItem.g' {
  interface PurchaseOrderItem {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.PurchaseOrderItem).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: PurchaseOrderItem) {

      return this.PurchaseOrderWherePurchaseOrderItem.OrderNumber + ' ' + this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier.PartyName + ' ' + this.Part.Name;
    },
  });

});

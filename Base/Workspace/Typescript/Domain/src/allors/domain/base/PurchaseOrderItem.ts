import { domain } from '../domain';
import { PurchaseOrderItem } from '../generated/PurchaseOrderItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/PurchaseOrderItem.g' {
  interface PurchaseOrderItem {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PurchaseOrderItem);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PurchaseOrderItem): string {

      return this.PurchaseOrderWherePurchaseOrderItem.OrderNumber + ' ' + this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier.PartyName + ' ' + this.Part.Name;
    },
  });

});

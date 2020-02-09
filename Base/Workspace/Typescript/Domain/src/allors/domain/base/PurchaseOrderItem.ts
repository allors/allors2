import { domain } from '../domain';
import { PurchaseOrderItem } from '../generated/PurchaseOrderItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';
import { inlineLists } from 'common-tags';

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
      const purchaseOrder = this.PurchaseOrderWherePurchaseOrderItem;
      return inlineLists`${[purchaseOrder?.OrderNumber, purchaseOrder?.TakenViaSupplier?.PartyName, this.Part?.Name].filter(v => v)}`;
    },
  });

});

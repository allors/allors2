import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { PurchaseOrder } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface PurchaseOrder {
    displayName: string;
  }
}

export function extendPurchaseOrder(workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PurchaseOrder);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PurchaseOrder): string {
      return (
        (this.OrderNumber ?? '') +
        (this.TakenViaSupplier?.PartyName
          ? ` ${this.TakenViaSupplier?.PartyName}`
          : '')
      );
    },
  });
}

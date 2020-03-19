import { domain } from '../domain';
import { PurchaseOrder } from '../generated/PurchaseOrder.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/PurchaseOrder.g' {
  interface PurchaseOrder {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PurchaseOrder);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PurchaseOrder): string {

      return (this.OrderNumber ?? '') + (this.TakenViaSupplier?.PartyName ? ` ${this.TakenViaSupplier?.PartyName}` : '');
    },
  });

});

import { domain } from '../domain';
import { PurchaseOrder } from '../generated/PurchaseOrder.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/PurchaseOrder.g' {
  interface PurchaseOrder {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.PurchaseOrder).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: PurchaseOrder): string {

      return this.OrderNumber + ' ' + this.TakenViaSupplier.PartyName;
    },
  });

});

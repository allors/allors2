import { domain } from '../domain';
import { WorkEffortInventoryAssignment } from '../generated/WorkEffortInventoryAssignment.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/WorkEffortInventoryAssignment.g' {
  interface WorkEffortInventoryAssignment {
    totalSellingPrice: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WorkEffortInventoryAssignment);
  assert(cls);

  Object.defineProperty(cls.prototype, 'totalSellingPrice', {
    configurable: true,
    get(this: WorkEffortInventoryAssignment): string {
      if (this.CanReadUnitSellingPrice) {
        const quantity = this.BillableQuantity ? this.BillableQuantity : this.Quantity;
        return (parseFloat(quantity) * parseFloat(this.UnitSellingPrice)).toFixed(2);
      } else {
        return '0';
      }
    },
  });
});

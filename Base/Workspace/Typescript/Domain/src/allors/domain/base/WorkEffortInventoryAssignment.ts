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
        const quantity = this.BillableQuantity ? this.BillableQuantity : this.Quantity ? this.Quantity : '0';
        const unitSellingPrice = this.UnitSellingPrice ? this.UnitSellingPrice : '0';
        return (parseFloat(quantity) * parseFloat(unitSellingPrice)).toFixed(2);
      } else {
        return '0';
      }
    },
  });
});

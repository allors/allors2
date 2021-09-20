import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { WorkEffortInventoryAssignment } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendWorkEffortInventoryAssignment(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WorkEffortInventoryAssignment);
  assert(cls);

  Object.defineProperty(cls.prototype, 'totalSellingPrice', {
    configurable: true,
    get(this: WorkEffortInventoryAssignment): string {
      if (this.CanReadUnitSellingPrice) {
        const quantity = this.DerivedBillableQuantity ? this.DerivedBillableQuantity : this.Quantity ? this.Quantity : '0';
        const unitSellingPrice = this.UnitSellingPrice ? this.UnitSellingPrice : '0';
        return (parseFloat(quantity) * parseFloat(unitSellingPrice)).toFixed(2);
      } else {
        return '0';
      }
    },
  });
}

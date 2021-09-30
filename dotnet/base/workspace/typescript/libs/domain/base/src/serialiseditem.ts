import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { SerialisedItem, UnifiedGood } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';


export function extendSerialisedItem(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.SerialisedItem);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: SerialisedItem): string {
      return this.ItemNumber + ' ' + this.Name + ' SN: ' + this.SerialNumber;
    },
  });

  Object.defineProperty(cls.prototype, 'age', {
    configurable: true,
    get(this: SerialisedItem): number {
      if (this.CanReadPurchasePrice && this.ManufacturingYear != null) {
        return new Date().getFullYear() - this.ManufacturingYear;
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'yearsToGo', {
    configurable: true,
    get(this: SerialisedItem): number {
      const good = this.PartWhereSerialisedItem as UnifiedGood | null;

      if (this.CanReadPurchasePrice && this.ManufacturingYear != null && good?.LifeTime != null) {
        return good.LifeTime - this.age < 0 ? 0 : good.LifeTime - this.age;
      } else {
        return 0;
      }
    },
  });
}

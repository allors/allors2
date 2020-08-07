import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { SerialisedInventoryItem } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface SerialisedInventoryItem {
    facilityName: string;
  }
}

export function extendSerialisedInventoryItem(workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.SerialisedInventoryItem);
  assert(cls);

  Object.defineProperty(cls, 'facilityName', {
    configurable: true,
    get(this: SerialisedInventoryItem): string {
      return this.Facility?.Name ?? '';
    },
  });
}

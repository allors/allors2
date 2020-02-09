import { domain } from '../domain';
import { SerialisedInventoryItem } from '../generated/SerialisedInventoryItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/SerialisedInventoryItem.g' {
  interface SerialisedInventoryItem {
    facilityName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.SerialisedInventoryItem);
  assert(cls);

  Object.defineProperty(cls, 'facilityName', {
    configurable: true,
    get(this: SerialisedInventoryItem): string {

      return this.Facility?.Name ?? '';
    },
  });

});

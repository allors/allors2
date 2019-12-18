import { domain } from '../domain';
import { SerialisedInventoryItem } from '../generated/SerialisedInventoryItem.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/SerialisedInventoryItem.g' {
  interface SerialisedInventoryItem {
    facilityName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.SerialisedInventoryItem).prototype as any;

  Object.defineProperty(obj, 'facilityName', {
    configurable: true,
    get(this: SerialisedInventoryItem): string {

      return this.Facility.Name;
    },
  });

});

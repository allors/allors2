import { domain } from '../domain';
import { NonSerialisedInventoryItem } from '../generated/NonSerialisedInventoryItem.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/NonSerialisedInventoryItem.g' {
  interface NonSerialisedInventoryItem {
    facilityName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.NonSerialisedInventoryItem).prototype as any;

  Object.defineProperty(obj, 'facilityName', {
    configurable: true,
    get(this: NonSerialisedInventoryItem): string {

      return this.Facility.Name;
    },
  });

});

import { domain } from '../domain';
import { WebAddress } from '../generated/WebAddress.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/WebAddress.g' {
  interface WebAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.WebAddress).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: WebAddress) {
      if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
      }

      return 'N/A';
    },
  });

});

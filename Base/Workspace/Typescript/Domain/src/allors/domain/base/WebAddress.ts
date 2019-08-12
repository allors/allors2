import { domain } from '../domain';
import { WebAddress } from '../generated/WebAddress.g';

declare module '../generated/WebAddress.g' {
  interface WebAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: WebAddress = workspace.prototypeByName['WebAddress'];

  Object.defineProperty(obj, 'displayName', {
    get(this: WebAddress) {
      if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
      }

      return 'N/A';
    },
  });

});

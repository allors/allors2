import { domain } from '../domain';
import { EmailAddress } from '../generated/EmailAddress.g';

declare module '../generated/EmailAddress.g' {
  interface EmailAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: EmailAddress = workspace.prototypeByName['EmailAddress'];

  Object.defineProperty(obj, 'displayName', {
    get(this: EmailAddress) {
      if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
      }

      return 'N/A';
    },
  });

});

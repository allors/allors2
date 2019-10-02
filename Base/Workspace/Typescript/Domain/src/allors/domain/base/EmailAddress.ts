import { domain } from '../domain';
import { EmailAddress } from '../generated/EmailAddress.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/EmailAddress.g' {
  interface EmailAddress {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.EmailAddress).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: EmailAddress) {
      if (this.ElectronicAddressString) {
        return this.ElectronicAddressString;
      }

      return 'N/A';
    },
  });

});

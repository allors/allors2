import { domain } from '../domain';
import { EmailAddress } from '../generated/EmailAddress.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/EmailAddress.g' {
  interface EmailAddress {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.EmailAddress);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: EmailAddress) {
      return this.ElectronicAddressString ?? 'N/A';
    },
  });

});

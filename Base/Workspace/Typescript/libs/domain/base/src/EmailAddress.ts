import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { EmailAddress } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface EmailAddress {
    displayName: string;
  }
}

export function extendEmailAddress(workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.EmailAddress);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: EmailAddress) {
      return this.ElectronicAddressString ?? 'N/A';
    },
  });
}

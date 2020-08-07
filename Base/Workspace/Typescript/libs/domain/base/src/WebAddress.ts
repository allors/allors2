import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { WebAddress } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface WebAddress {
    displayName: string;
  }
}

export function extendWebAddress(workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WebAddress);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: WebAddress) {
      return this.ElectronicAddressString ?? 'N/A';
    },
  });
}

import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { TelecommunicationsNumber } from '@allors/domain/generated';
import { inlineLists } from 'common-tags';

declare module '@allors/domain/generated' {
  interface TelecommunicationsNumber {
    displayName: string;
  }
}

export function extendTelecommunicationsNumber(workspace) {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.TelecommunicationsNumber);
  assert(cls);

  Object.defineProperty(cls?.prototype, 'displayName', {
    configurable: true,
    get(this: TelecommunicationsNumber) {
      return inlineLists`${[this.CountryCode, this.AreaCode, this.ContactNumber].filter(v => v)}`;
    },
  });

};

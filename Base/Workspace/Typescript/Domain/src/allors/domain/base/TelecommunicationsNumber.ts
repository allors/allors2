import { domain } from '../domain';
import { TelecommunicationsNumber } from '../generated/TelecommunicationsNumber.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';
import { inlineLists } from 'common-tags';

declare module '../generated/TelecommunicationsNumber.g' {
  interface TelecommunicationsNumber {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.TelecommunicationsNumber);
  assert(cls);

  Object.defineProperty(cls?.prototype, 'displayName', {
    configurable: true,
    get(this: TelecommunicationsNumber) {
      return inlineLists`${[this.CountryCode, this.AreaCode, this.ContactNumber].filter(v => v)}`;
    },
  });

});

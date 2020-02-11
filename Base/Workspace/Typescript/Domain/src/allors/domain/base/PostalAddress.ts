import { domain } from '../domain';
import { PostalAddress } from '../generated/PostalAddress.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';
import { oneLine, stripIndents, inlineLists } from 'common-tags';

declare module '../generated/PostalAddress.g' {
  interface PostalAddress {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PostalAddress);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PostalAddress) {

      return stripIndents`
      ${[this.Address1, this.Address2, this.Address3].filter(v => v).map(v => oneLine`${v}`)}
      ${inlineLists`${[this.PostalCode, this.Locality].filter(v => v)}`}
      ${this.Country.Name}
      `;
    },
  });

});

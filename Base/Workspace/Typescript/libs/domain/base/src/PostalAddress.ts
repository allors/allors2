import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { PostalAddress } from '@allors/domain/generated';
import { oneLine, stripIndents, inlineLists } from 'common-tags';
import { Workspace } from '@allors/domain/system';

export function extendPostalAddress(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PostalAddress);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PostalAddress) {
      return stripIndents`
      ${[this.Address1, this.Address2, this.Address3]
        .filter((v) => v)
        .map((v) => oneLine`${v}`)}
      ${inlineLists`${[this.PostalCode, this.Locality].filter((v) => v)}`}
      ${this.Country?.Name ?? ''}
      `;
    },
  });
}

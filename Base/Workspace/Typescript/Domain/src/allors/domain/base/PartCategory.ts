import { domain } from '../domain';
import { PartCategory } from '../generated/PartCategory.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/PartCategory.g' {
  interface PartCategory {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.PartCategory);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: PartCategory): string {

      const selfAndPrimaryAncestors = [this];
      let ancestor: PartCategory | null = this;
      while (ancestor != null && selfAndPrimaryAncestors.indexOf(ancestor) < 0) {
        selfAndPrimaryAncestors.push(ancestor);
        ancestor = ancestor.PrimaryParent;
      }

      selfAndPrimaryAncestors.reverse();
      const displayName = selfAndPrimaryAncestors.map(v => v.Name).join('/');
      return displayName;
    },
  });

});

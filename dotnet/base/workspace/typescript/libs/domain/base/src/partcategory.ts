import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { PartCategory } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendPartCategory(workspace: Workspace) {

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

};

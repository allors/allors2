import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { ProductCategory } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendProductCategory(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.ProductCategory);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: ProductCategory): string {
      const selfAndPrimaryAncestors = [this];
      let ancestor: ProductCategory | null = this;
      while (
        ancestor != null &&
        selfAndPrimaryAncestors.indexOf(ancestor) < 0
      ) {
        selfAndPrimaryAncestors.push(ancestor);
        ancestor = ancestor.PrimaryParent;
      }

      selfAndPrimaryAncestors.reverse();
      const displayName = selfAndPrimaryAncestors.map((v) => v.Name).join('/');
      return displayName;
    },
  });
}

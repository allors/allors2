import { domain } from '../domain';
import { PartCategory } from '../generated/PartCategory.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/PartCategory.g' {
  interface PartCategory {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.PartCategory).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: PartCategory): string {

      const selfAndPrimaryAncestors = [this];
      let ancestor = this;
      while (ancestor && selfAndPrimaryAncestors.indexOf(ancestor) < 0) {
        selfAndPrimaryAncestors.push(ancestor);
        ancestor = ancestor.PrimaryParent;
      }

      selfAndPrimaryAncestors.reverse();
      const displayName = selfAndPrimaryAncestors.map(v => v.Name).join('/');
      return displayName;
    },
  });

});

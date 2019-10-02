import { domain } from '../domain';
import { ProductCategory } from '../generated/ProductCategory.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/ProductCategory.g' {
  interface ProductCategory {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.ProductCategory).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: ProductCategory): string {

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

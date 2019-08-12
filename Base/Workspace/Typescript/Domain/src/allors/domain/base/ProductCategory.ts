import { domain } from '../domain';
import { ProductCategory } from '../generated/ProductCategory.g';

declare module '../generated/ProductCategory.g' {
  interface ProductCategory {
    displayName;
  }
}

domain.extend((workspace) => {

  const obj: ProductCategory = workspace.prototypeByName['ProductCategory'];

  Object.defineProperty(obj, 'displayName', {
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

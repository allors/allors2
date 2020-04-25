import { domain } from '../domain';
import { UnifiedGood } from '../generated/UnifiedGood.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/UnifiedGood.g' {
  interface UnifiedGood {
    categoryNames: string;
    motorized: boolean;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.UnifiedGood);
  assert(cls);

  Object.defineProperty(cls.prototype, 'categoryNames', {
    configurable: true,
    get(this: UnifiedGood) {
      return this.ProductCategoriesWhereProduct.map(v => v.displayName).join(', ');
    },
  });
});

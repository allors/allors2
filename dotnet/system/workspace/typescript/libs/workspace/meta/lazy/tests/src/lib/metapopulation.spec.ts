import { MetaPopulation } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

describe('MetaPopulation', () => {
  describe('default constructor', () => {
    const metaPopulation = new LazyMetaPopulation({}) as MetaPopulation;

    it('should be newable', () => {
      expect(metaPopulation).toBeDefined();
    });
  });
});

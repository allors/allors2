import { Unit } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

interface M extends LazyMetaPopulation {
  Binary: Unit;

  Boolean: Unit;

  DateTime: Unit;

  Decimal: Unit;

  Float: Unit;

  Integer: Unit;

  String: Unit;

  Unique: Unit;
}

describe('Unit in MetaPopulation', () => {
  describe('default constructor', () => {
    const metaPopulation = new LazyMetaPopulation({}) as M;

    it('should have Binary unit', () => {
      expect(metaPopulation.Binary).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.Boolean).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.DateTime).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.Decimal).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.Float).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.Integer).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.String).not.toBeNull();
    });

    it('should have Binary unit', () => {
      expect(metaPopulation.Unique).not.toBeNull();
    });
  });
});

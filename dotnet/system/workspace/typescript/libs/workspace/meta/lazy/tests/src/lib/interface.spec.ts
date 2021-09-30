import { MetaData, Origin, Interface } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

type Named = Interface;

interface M extends LazyMetaPopulation {
  Named: Named;
}

describe('Interface in MetaPopulation', () => {
  describe('with minimal interface metadata', () => {
    const data: MetaData = {
      i: [[9, 'Named']],
    };

    const metaPopulation = new LazyMetaPopulation(data) as M;
    const { Named } = metaPopulation;

    it('should have the interface with its defaults', () => {
      expect(Named).toBeDefined();
      expect(Named.metaPopulation).toBe(metaPopulation);
      expect(Named.tag).toBe(9);
      expect(Named.origin).toBe(Origin.Database);
      expect(Named.singularName).toBe('Named');
      expect(Named.pluralName).toBe('Nameds');
      expect(Named.isUnit).toBeFalsy();
      expect(Named.isComposite).toBeTruthy();
      expect(Named.isInterface).toBeTruthy();
      expect(Named.isClass).toBeFalsy();
    });
  });

  describe('with maximal interface metadata', () => {
    type I1 = Interface;
    type I2 = Interface;

    interface M extends LazyMetaPopulation {
      I1: I1;
      I2: I2;
    }

    const metaPopulation = new LazyMetaPopulation({
      i: [
        [10, 'I1', [], [], [], 'PluralI1'],
        [11, 'I2', [], [], [], 'PluralI2'],
      ],
      o: [[10], [11]],
    }) as M;

    const { I1, I2 } = metaPopulation;

    it('should have the class with its defaults', () => {
      expect(I1).toBeDefined();
      expect(I1.metaPopulation).toBe(metaPopulation);
      expect(I1.tag).toBe(10);
      expect(I1.origin).toBe(Origin.Workspace);
      expect(I1.singularName).toBe('I1');
      expect(I1.pluralName).toBe('PluralI1');
      expect(I1.isUnit).toBeFalsy();
      expect(I1.isComposite).toBeTruthy();
      expect(I1.isInterface).toBeTruthy();
      expect(I1.isClass).toBeFalsy();

      expect(I2).toBeDefined();
      expect(I2.metaPopulation).toBe(metaPopulation);
      expect(I2.tag).toBe(11);
      expect(I2.origin).toBe(Origin.Session);
      expect(I2.singularName).toBe('I2');
      expect(I2.pluralName).toBe('PluralI2');
      expect(I2.isUnit).toBeFalsy();
      expect(I2.isComposite).toBeTruthy();
      expect(I2.isInterface).toBeTruthy();
      expect(I2.isClass).toBeFalsy();
    });
  });
});

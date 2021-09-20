import { Origin } from '@allors/workspace/meta/system';
import { Class } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

describe('Class in MetaPopulation', () => {
  describe('with minimal class metadata', () => {
    type Organisation = Class;

    interface M extends LazyMetaPopulation {
      Organisation: Organisation;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [[10, 'Organisation']],
    }) as M;

    const { Organisation } = metaPopulation;

    it('should have the class with its defaults', () => {
      expect(Organisation).toBeDefined();
      expect(Organisation.metaPopulation).toBe(metaPopulation);
      expect(Organisation.tag).toBe(10);
      expect(Organisation.origin).toBe(Origin.Database);
      expect(Organisation.singularName).toBe('Organisation');
      expect(Organisation.pluralName).toBe('Organisations');
      expect(Organisation.isUnit).toBeFalsy();
      expect(Organisation.isComposite).toBeTruthy();
      expect(Organisation.isInterface).toBeFalsy();
      expect(Organisation.isClass).toBeTruthy();
    });
  });

  describe('with maximal class metadata', () => {
    type C1 = Class;
    type C2 = Class;

    interface M extends LazyMetaPopulation {
      C1: C1;
      C2: C2;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [10, 'C1', [], [], [], 'PluralC1'],
        [11, 'C2', [], [], [], 'PluralC2'],
      ],
      o: [[10], [11]],
    }) as M;

    const { C1, C2 } = metaPopulation;

    it('should have the class with its defaults', () => {
      expect(C1).toBeDefined();
      expect(C1.metaPopulation).toBe(metaPopulation);
      expect(C1.tag).toBe(10);
      expect(C1.origin).toBe(Origin.Workspace);
      expect(C1.singularName).toBe('C1');
      expect(C1.pluralName).toBe('PluralC1');
      expect(C1.isUnit).toBeFalsy();
      expect(C1.isComposite).toBeTruthy();
      expect(C1.isInterface).toBeFalsy();
      expect(C1.isClass).toBeTruthy();

      expect(C2).toBeDefined();
      expect(C2.metaPopulation).toBe(metaPopulation);
      expect(C2.tag).toBe(11);
      expect(C2.origin).toBe(Origin.Session);
      expect(C2.singularName).toBe('C2');
      expect(C2.pluralName).toBe('PluralC2');
      expect(C2.isUnit).toBeFalsy();
      expect(C2.isComposite).toBeTruthy();
      expect(C2.isInterface).toBeFalsy();
      expect(C2.isClass).toBeTruthy();
    });
  });
});

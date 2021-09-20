import { Interface, Class } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

type S = Interface;

type I = Interface;

type C = Class;

interface M extends LazyMetaPopulation {
  S: S;
  I: I;
  C: C;
}

describe('Inheritance in MetaPopulation', () => {
  describe('with class, interface and superinterface metadata', () => {
    const metaPopulation = new LazyMetaPopulation({
      i: [
        [11, 'I', [10]],
        [10, 'S'],
      ],
      c: [[12, 'C', [11]]],
    }) as M;

    const { C, I, S } = metaPopulation;

    it('should have the hierarchy set', () => {
      expect(C.directSupertypes.size).toBe(1);
      expect(C.directSupertypes).toContain(I);
      expect(C.supertypes.size).toBe(2);
      expect(C.supertypes).toContain(I);
      expect(C.supertypes).toContain(S);
      expect(C.classes.size).toBe(1);
      expect(C.classes).toContain(C);

      expect(I.directSupertypes.size).toBe(1);
      expect(I.directSupertypes).toContain(S);
      expect(I.supertypes.size).toBe(1);
      expect(I.supertypes).toContain(S);
      expect(I.subtypes.size).toBe(1);
      expect(I.subtypes).toContain(C);
      expect(I.classes.size).toBe(1);
      expect(I.classes).toContain(C);

      expect(S.directSupertypes.size).toBe(0);
      expect(S.supertypes.size).toBe(0);
      expect(S.subtypes.size).toBe(2);
      expect(S.subtypes).toContain(I);
      expect(S.subtypes).toContain(C);
      expect(S.classes.size).toBe(1);
      expect(S.classes).toContain(C);
    });
  });
});

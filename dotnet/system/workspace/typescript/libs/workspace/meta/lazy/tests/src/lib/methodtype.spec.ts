import { Origin, Composite, Interface, MethodType } from '@allors/workspace/meta/system';
import { LazyMetaPopulation } from '@allors/workspace/meta/lazy/system';

interface Action extends Interface {
  Do: MethodType;
}

interface Organisation extends Composite {
  Do: MethodType;
}

interface M extends LazyMetaPopulation {
  Action: Action;

  Organisation: Organisation;
}

describe('MethodType in MetaPopulation', () => {
  describe('with minimal method metadata', () => {
    const metaPopulation = new LazyMetaPopulation({
      c: [[10, 'Organisation', [], [], [[11, 'Do']]]],
    }) as M;

    const { Organisation } = metaPopulation;
    const { Do: methodType } = Organisation;

    it('should have the relation with its defaults', () => {
      expect(methodType).toBeDefined();
      expect(methodType.objectType).toBe(Organisation);
      expect(methodType.name).toBe('Do');
      expect(methodType.origin).toBe(Origin.Database);
    });
  });

  describe('with inherited method metadata', () => {
    const metaPopulation = new LazyMetaPopulation({
      i: [[9, 'Action', [], [], [[11, 'Do']]]],
      c: [[10, 'Organisation', [9]]],
    }) as M;

    const { Action, Organisation } = metaPopulation;
    const { Do: actionDo } = Action;
    const { Do: organisationDo } = Organisation;

    it('should have the same RoleType', () => {
      expect(actionDo).toBeDefined();
      expect(organisationDo).toBeDefined();
      expect(organisationDo).toEqual(actionDo);
    });
  });
});

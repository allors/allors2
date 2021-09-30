import { MetaPopulation, ObjectType } from '@allors/meta/system';

import { data, Meta } from '@allors/meta/generated';

describe('Meta',
  () => {

    const metaPopulation = new MetaPopulation(data);
    const metaDomain = metaPopulation as Meta & { [key: string]: any };

    it('is defined',
      () => {
        expect(metaDomain).toBeDefined();
      });

    it('metaInterfaces should be defined',
      () => {
        data.interfaces.forEach((v) => {
          expect(metaDomain[v.name]).toBeDefined();
        });
      });

    it('metaClasses should be defined',
      () => {
        data.classes.forEach((v) => {
          expect(metaDomain[v.name]).toBeDefined();
        });
      });

    it('metaObject.objectType should be defined',
      () => {
        data.interfaces.concat(data.classes).forEach((v) => {
          const metaObjectType: ObjectType = metaDomain[v.name];
          expect(metaObjectType).toBeDefined();
        });
      });

    it('metaObject roleTypes should be defined',
      () => {
        data.interfaces.concat(data.classes).forEach((v) => {
          const metaObjectType: ObjectType & { [key: string]: any } = metaDomain[v.name];
          const objectType = metaObjectType;

          const roleTypes = Object.keys(objectType.roleTypeByName).map((w) => (objectType.roleTypeByName as {[key: string]: any})[w]);

          roleTypes.forEach((w) => {
            const metaRoleType = metaObjectType[w.name];
            expect(metaRoleType).toBeDefined();
          });
        });
      });

    it('metaObject associationTypes should be defined',
      () => {
        data.interfaces.concat(data.classes).forEach((v) => {
          const metaObjectType: ObjectType & { [key: string]: any } = metaDomain[v.name];
          const objectType = metaObjectType;

          const associationTypes = Object.keys(objectType.associationTypeByName).map((w) => (objectType.associationTypeByName as & { [key: string]: any })[w]);

          associationTypes.forEach((w) => {
            const metaAssociationType = metaObjectType[w.name];
            expect(metaAssociationType).toBeDefined();
          });
        });
      });

    it('hierarchy should be defined for roles',
      () => {
        expect(metaDomain.C1.Name).toBeDefined();
        expect(metaDomain.I1.Name).toBeDefined();
        expect(metaDomain.I12.Name).toBeDefined();
      });

    it('hierarchy should be defined for associations',
      () => {
        expect(metaDomain.C1.C2WhereS1One2One).toBeDefined();
        expect(metaDomain.I1.C2WhereS1One2One).toBeDefined();
        expect(metaDomain.S1.C2WhereS1One2One).toBeDefined();
      });

  });

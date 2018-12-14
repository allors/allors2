import { assert } from 'chai';
import 'mocha';

import { MetaPopulation, ObjectType } from '../../src/allors/framework';
import { data, MetaDomain } from '../../src/allors/meta';

describe('MetaDomain',
    () => {

        const metaPopulation = new MetaPopulation(data);
        const metaDomain: MetaDomain = metaPopulation as MetaDomain;

        it('is defined',
            () => {
                assert.isDefined(metaDomain);
            });

        it('metaInterfaces should be defined',
            () => {
                data.interfaces.forEach((v) => {
                    assert.isDefined(metaDomain[v.name]);
                });
            });

        it('metaClasses should be defined',
            () => {
                data.classes.forEach((v) => {
                    assert.isDefined(metaDomain[v.name]);
                });
            });

        it('metaObject.objectType should be defined',
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: ObjectType = metaDomain[v.name];
                    assert.isDefined(metaObjectType);
                });
            });

        it('metaObject roleTypes should be defined',
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: ObjectType = metaDomain[v.name];
                    const objectType = metaObjectType;

                    const roleTypes = Object.keys(objectType.roleTypeByName).map((w) => objectType.roleTypeByName[w]);

                    roleTypes.forEach((w) => {
                        const metaRoleType = metaObjectType[w.name];
                        assert.isDefined(metaRoleType);
                    });
                });
            });

        it('metaObject associationTypes should be defined',
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: ObjectType = metaDomain[v.name];
                    const objectType = metaObjectType;

                    const associationTypes = Object.keys(objectType.associationTypeByName).map((w) => objectType.associationTypeByName[w]);

                    associationTypes.forEach((w) => {
                        const metaAssociationType = metaObjectType[w.name];
                        assert.isDefined(metaAssociationType);
                    });
                });
            });

        it('hierarchy should be defined for roles',
            () => {
                assert.isDefined(metaDomain.C1.Name);
                assert.isDefined(metaDomain.I1.Name);
                assert.isDefined(metaDomain.I12.Name);
            });

        it('hierarchy should be defined for associations',
            () => {
                assert.isDefined(metaDomain.C1.C2WhereS1One2One);
                assert.isDefined(metaDomain.I1.C2WhereS1One2One);
                assert.isDefined(metaDomain.S1.C2WhereS1One2One);
            });

    });

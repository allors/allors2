import { domain } from '../../src/allors/domain';
import { MetaPopulation, Workspace } from '../../src/allors/framework';
import { data, PathFactory } from '../../src/allors/meta';

import { assert } from 'chai';
import 'mocha';

describe('Path',
    () => {
        let metaPopulation: MetaPopulation;
        let factory: PathFactory;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            factory = new PathFactory(metaPopulation);
        });

        describe('with empty step path',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({});

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.isDefined(path);
                });
            });

        describe('with one role path',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({
                        Employees: {},
                    });

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.deepEqual(path, { propertytype: 'b95c7b34a295460082c8826cc2186a00' });
                });
            });

        describe('with two roles path',
            () => {
                it('should serialize to correct json', () => {

                    const original =
                        factory.Organisation({
                            Employees: {
                                Photo: {},
                            },
                        });

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.deepEqual(path, {
                        next: {
                            propertytype: 'f6624facdb8e4fb29e8618021b59d31d',
                        },
                        propertytype: 'b95c7b34a295460082c8826cc2186a00',
                    });

                });
            });

        describe('with a subclass role path',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.User({
                        Person_CycleOne: {},
                    });

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.deepEqual(path, { propertytype: '79ffeed6e06a42f4b12fd7f7c98b6499' });
                });
            });

        describe('with a non exsiting role path',
            () => {
                it('should throw exception', () => {

                    assert.throw(() => {
                        factory.Organisation({
                            Oops: {},
                        } as any);
                        }, Error);
                });
            });

        describe('with one association path',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({
                        PeopleWhereCycleOne: {},
                    });

                    const json = JSON.stringify(original);
                    const path = JSON.parse(json);

                    assert.deepEqual(path, { propertytype: 'dec66a7b56f54010a2e737e25124bc77' });
                });
            });

        describe('with one subclass association path',
            () => {
                it('should serialize to correct json', () => {

                    const orginal = factory.Deletable({
                        Organisation_PeopleWhereCycleOne: {},
                    });

                    const json = JSON.stringify(orginal);
                    const path = JSON.parse(json);

                    assert.deepEqual(path, { propertytype: 'dec66a7b56f54010a2e737e25124bc77' });
                });
            });
    });

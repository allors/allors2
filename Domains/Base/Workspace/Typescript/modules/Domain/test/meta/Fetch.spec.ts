import { domain } from '../../src/allors/domain';
import { MetaPopulation, Workspace } from '../../src/allors/framework';
import { data, FetchFactory } from '../../src/allors/meta';

import { assert } from 'chai';
import 'mocha';

describe('Fetch',
    () => {
        let metaPopulation: MetaPopulation;
        let factory: FetchFactory;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            factory = new FetchFactory(metaPopulation);
        });

        describe('with empty fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({});

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.isDefined(fetch);
                });
            });

        describe('with one role fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({
                        Employees: {},
                    });

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: 'b95c7b34a295460082c8826cc2186a00' } });
                });
            });

        describe('with two roles fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original =
                        factory.Organisation({
                            Employees: {
                                Photo: {},
                            },
                        });

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, {
                        step: {
                            next: {
                                propertytype: 'f6624facdb8e4fb29e8618021b59d31d',
                            },
                            propertytype: 'b95c7b34a295460082c8826cc2186a00',
                        }
                    });

                });
            });

        describe('with a subclass role fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.User({
                        Person_CycleOne: {},
                    });

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: '79ffeed6e06a42f4b12fd7f7c98b6499' } });
                });
            });

        describe('with a non exsiting role fetch',
            () => {
                it('should throw exception', () => {

                    assert.throw(() => {
                        factory.Organisation({
                            Oops: {},
                        } as any);
                    }, Error);
                });
            });

        describe('with one association fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({
                        PeopleWhereCycleOne: {},
                    });

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: 'dec66a7b56f54010a2e737e25124bc77' } });
                });
            });

        describe('with one subclass association fetch',
            () => {
                it('should serialize to correct json', () => {

                    const orginal = factory.Deletable({
                        Organisation_PeopleWhereCycleOne: {},
                    });

                    const json = JSON.stringify(orginal);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: 'dec66a7b56f54010a2e737e25124bc77' } });
                });
            });
    });

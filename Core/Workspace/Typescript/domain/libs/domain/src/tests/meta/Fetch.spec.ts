import { MetaPopulation } from '@allors/workspace/meta';
import { Workspace } from '@allors/workspace/domain';

import { data, FetchFactory, Meta } from '@allors/meta';
import { domain } from '@allors/domain';

describe('Fetch',
    () => {
        let m: Meta;
        let factory: FetchFactory;

        beforeEach(async () => {
            m = new MetaPopulation(data) as Meta;
            const workspace = new Workspace(m);
            domain.apply(workspace);

            factory = new FetchFactory(m);
        });

        // describe('with empty fetch',
        //     () => {
        //         it('should serialize to correct json', () => {

        //             const original = factory.Organisation({});

        //             const json = JSON.stringify(original);
        //             const fetch = JSON.parse(json);

        //             assert.isDefined(fetch);
        //         });
        //     });

        // describe('with one role fetch',
        //     () => {
        //         it('should serialize to correct json', () => {

        //             const original = factory.Organisation({
        //                 Employees: {},
        //             });

        //             const json = JSON.stringify(original);
        //             const fetch = JSON.parse(json);

        //             assert.deepEqual(fetch, { step: { propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00' } });
        //         });
        //     });

        // describe('with two roles fetch',
        //     () => {
        //         it('should serialize to correct json', () => {

        //             const original =
        //                 factory.Organisation({
        //                     Employees: {
        //                         Photo: {},
        //                     },
        //                 });

        //             const json = JSON.stringify(original);
        //             const fetch = JSON.parse(json);

        //             assert.deepEqual(fetch, {
        //                 step: {
        //                     next: {
        //                         propertytype: 'f6624fac-db8e-4fb2-9e86-18021b59d31d',
        //                     },
        //                     propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
        //                 }
        //             });

        //         });
        //     });

        describe('with a subclass role fetch',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.User({
                        Person_CycleOne: {},
                    });

                    const json = JSON.stringify(original);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: '79ffeed6-e06a-42f4-b12f-d7f7c98b6499' } });
                });
            });

        // describe('with a non exsiting role fetch',
        //     () => {
        //         it('should throw exception', () => {

        //             assert.throw(() => {
        //                 factory.Organisation({
        //                     Oops: {},
        //                 } as any);
        //             }, Error);
        //         });
        //     });

        // describe('with one association fetch',
        //     () => {
        //         it('should serialize to correct json', () => {

        //             const original = factory.Organisation({
        //                 PeopleWhereCycleOne: {},
        //             });

        //             const json = JSON.stringify(original);
        //             const fetch = JSON.parse(json);

        //             assert.deepEqual(fetch, { step: { propertytype: 'dec66a7b-56f5-4010-a2e7-37e25124bc77' } });
        //         });
        //     });

        describe('with one subclass association fetch',
            () => {
                it('should serialize to correct json', () => {

                    const orginal = factory.Deletable({
                        Organisation_PeopleWhereCycleOne: {},
                    });

                    const json = JSON.stringify(orginal);
                    const fetch = JSON.parse(json);

                    assert.deepEqual(fetch, { step: { propertytype: 'dec66a7b-56f5-4010-a2e7-37e25124bc77' } });
                });
            });
    });

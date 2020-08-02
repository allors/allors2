import { domain } from '../../src/allors/domain';
import { MetaPopulation, Workspace } from '../../src/allors/framework';
import { data, PullFactory, Meta } from '../../src/allors/meta';

import { assert } from 'chai';
import 'mocha';

describe('Filter',
    () => {
        let m: Meta;
        let factory: PullFactory;

        beforeEach(async () => {
            m = new MetaPopulation(data) as Meta;
            const workspace = new Workspace(m);
            domain.apply(workspace);

            factory = new PullFactory(m);
        });

        describe('with empty flatPull',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({});

                    const json = JSON.stringify(original);
                    const pull = JSON.parse(json);

                    assert.isDefined(pull);
                });
            });
    });

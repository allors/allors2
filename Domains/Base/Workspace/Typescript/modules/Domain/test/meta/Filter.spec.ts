import { domain } from '../../src/allors/domain';
import { MetaPopulation, Workspace } from '../../src/allors/framework';
import { data, PullFactory } from '../../src/allors/meta';

import { assert } from 'chai';
import 'mocha';

describe('Filter',
    () => {
        let metaPopulation: MetaPopulation;
        let factory: FilterFactory;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);

            factory = new PullFactory(metaPopulation);
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

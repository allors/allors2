import { MetaPopulation } from '@allors/workspace/meta';
import { Workspace } from '@allors/workspace/domain';

import { domain } from '@allors/domain';
import { data, PullFactory, Meta } from '@allors/meta';

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

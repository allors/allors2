import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';

import { data, PullFactory, Meta } from '@allors/meta/generated';

describe('Pull',
    () => {
        let m: Meta;
        let factory: PullFactory;
        let workspace: Workspace;

        beforeEach(async () => {
            m = new MetaPopulation(data) as Meta;
            workspace = new Workspace(m);

            factory = new PullFactory(m);
        });

        describe('with empty flatPull',
            () => {
                it('should serialize to correct json', () => {

                    const original = factory.Organisation({});

                    const json = JSON.stringify(original);
                    const pull = JSON.parse(json);

                    expect(pull).toBeDefined();
                });
            });
    });

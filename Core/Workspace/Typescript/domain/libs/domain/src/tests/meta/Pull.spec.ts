import { MetaPopulation } from '@allors/workspace/meta';
import { Workspace } from '@allors/workspace/domain';

import { data, PullFactory, Meta } from '@allors/meta';
import { extend } from '../../index';

describe('Pull',
    () => {
        let m: Meta;
        let factory: PullFactory;
        let workspace: Workspace;

        beforeEach(async () => {
            m = new MetaPopulation(data) as Meta;
            workspace = new Workspace(m);
            extend(workspace);

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

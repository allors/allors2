import { MetaPopulation } from '@allors/workspace/meta';

import { data } from '@allors/meta';

describe('MetaPopulation',
    () => {
        describe('default constructor',
        () => {

            const metaPopulation = new MetaPopulation(data);

            it('should be newable',
                () => {
                    assert.isNotNull(metaPopulation);
                });

            describe('init with empty data population', () => {

                it('should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique (from Core)',
                    () => {
                        ['Binary', 'Boolean', 'DateTime', 'Decimal', 'Float', 'Integer', 'String', 'Unique'].forEach((name) => {
                            const unit = (metaPopulation.objectTypeByName as {[key: string]: any})[name];
                            assert.isNotNull(unit);
                        });
                    });

                it('should contain Media, ObjectState, Counter, Person, Role, UserGroup (from Base)',
                    () => {
                        ['Media', 'ObjectState', 'Counter', 'Person', 'Role', 'UserGroup'].forEach((name) => {
                            const unit = (metaPopulation.objectTypeByName as {[key: string]: any})[name];
                            assert.isNotNull(unit);
                        });
                    });
            });
        });
});

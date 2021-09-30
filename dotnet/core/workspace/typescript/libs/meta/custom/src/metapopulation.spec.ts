import { MetaPopulation } from '@allors/meta/system';

import { data } from '@allors/meta/generated';

describe('MetaPopulation',
    () => {
        describe('default constructor',
        () => {

            const metaPopulation = new MetaPopulation(data);

            it('should be newable',
                () => {
                    expect(metaPopulation).not.toBeNull();
                });

            describe('init with empty data population', () => {

                it('should contain Binary, Boolean, DateTime, Decimal, Float, Integer, String, Unique (from Core)',
                    () => {
                        ['Binary', 'Boolean', 'DateTime', 'Decimal', 'Float', 'Integer', 'String', 'Unique'].forEach((name) => {
                            const unit = (metaPopulation.objectTypeByName as {[key: string]: any})[name];
                            expect(unit).not.toBeNull();
                        });
                    });

                it('should contain Media, ObjectState, Counter, Person, Role, UserGroup (from Base)',
                    () => {
                        ['Media', 'ObjectState', 'Counter', 'Person', 'Role', 'UserGroup'].forEach((name) => {
                            const unit = (metaPopulation.objectTypeByName as {[key: string]: any})[name];
                            expect(unit).not.toBeNull();
                        });
                    });
            });
        });
});

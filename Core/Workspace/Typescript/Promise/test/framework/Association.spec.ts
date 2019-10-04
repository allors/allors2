import { C1, C2 } from '../../src/allors/domain';
import { PullRequest } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';

describe('Association',
    () => {
        let fixture: Fixture;

        beforeEach(async () => {
            fixture = new Fixture();
            await fixture.init(fixture.FULL_POPULATION);
        });

        describe('C1',
            () => {
                it('should access association', async () => {

                    const { x, ctx, pull } = fixture;

                    const pulls = [
                        pull.C1({
                            include: {
                                C1C1One2One: x,
                                C1C1One2Manies: x,
                                C1C1Many2One: x,
                                C1C1Many2Manies: x,
                                C1C2One2One: x,
                                C1C2One2Manies: x,
                                C1C2Many2One: x,
                                C1C2Many2Manies: x,
                            }
                        }),
                        pull.C2()
                    ];

                    ctx.session.reset();

                    const loaded = await ctx
                        .load(new PullRequest({ pulls }));

                    const c1s = loaded.collections['C1s'] as C1[];
                    const c2s = loaded.collections['C2s'] as C2[];

                    const c1A = c1s.find((v) => v.Name === 'c1A');
                    const c1B = c1s.find((v) => v.Name === 'c1B');
                    const c1C = c1s.find((v) => v.Name === 'c1C');
                    const c1D = c1s.find((v) => v.Name === 'c1D');

                    const c2A = c2s.find((v) => v.Name === 'c2A');
                    const c2B = c2s.find((v) => v.Name === 'c2B');
                    const c2C = c2s.find((v) => v.Name === 'c2C');
                    const c2D = c2s.find((v) => v.Name === 'c2D');

                    // One to One
                    assert.isNull(c2A.C1WhereC1C2One2One);
                    assert.equal(c2B.C1WhereC1C2One2One, c1B);
                    assert.equal(c2C.C1WhereC1C2One2One, c1C);
                    assert.equal(c2D.C1WhereC1C2One2One, c1D);

                    // Many to One
                    assert.isEmpty(c2A.C1sWhereC1C2Many2One);
                    assert.equal(c2B.C1sWhereC1C2Many2One.length, 1);
                    assert.include(c2B.C1sWhereC1C2Many2One, c1B);
                    assert.equal(c2C.C1sWhereC1C2Many2One.length, 2);
                    assert.include(c2C.C1sWhereC1C2Many2One, c1C);
                    assert.include(c2C.C1sWhereC1C2Many2One, c1D);
                    assert.isEmpty(c2D.C1sWhereC1C2Many2One);
                });
            });
    });

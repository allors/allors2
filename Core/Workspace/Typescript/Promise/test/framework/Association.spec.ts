import { C1, C2 } from '../../src/allors/domain';
import { PullRequest, Equals } from '../../src/allors/framework';

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

    describe('C2',
      () => {
        it('should access one2many association', async () => {

          const { x, ctx, m, pull } = fixture;

          const pulls = [
            pull.C2({
              predicate: new Equals({
                propertyType: m.C2.Name,
                value: 'c2C'
              }),
              include: {
                C1WhereC1C2One2Many: x
              }
            })
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const c2s = loaded.collections['C2s'] as C2[];

          const c2C = c2s.find((v) => v.Name === 'c2C');

          const c1WhereC1C2One2Many = c2C?.C1WhereC1C2One2Many;

          // One to One
          assert.isNotNull(c1WhereC1C2One2Many);
          assert.equal(c1WhereC1C2One2Many?.Name, 'c1C');
        });

        it('should access one2one association', async () => {

          const { x, ctx, m, pull } = fixture;

          const pulls = [
            pull.C2({
              predicate: new Equals({
                propertyType: m.C2.Name,
                value: 'c2C'
              }),
              include: {
                C1WhereC1C2One2One: x
              }
            })
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const c2s = loaded.collections['C2s'] as C2[];

          const c2C = c2s.find((v) => v.Name === 'c2C');

          const c1WhereC1C2One2One = c2C?.C1WhereC1C2One2One;

          // One to One
          assert.isNotNull(c1WhereC1C2One2One);
          assert.equal(c1WhereC1C2One2One?.Name, 'c1C');
        });
      });
  });

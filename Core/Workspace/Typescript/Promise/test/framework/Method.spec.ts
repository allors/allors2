import { Organisation } from '../../src/allors/domain';
import { PullRequest } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';

describe('Method',
  () => {
    let fixture: Fixture;

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init(fixture.FULL_POPULATION);
    });

    describe('JustDoIt on Organisation ',
      () => {
        it('should update JustDidIt', async () => {

          const { x, ctx, m, pull } = fixture;

          ctx.session.reset();

          const loaded = await ctx.load(pull.Organisation());
          const organisations = loaded.collections['Organisations'] as Organisation[];

          const organisation = organisations[0];

          assert.isFalse(organisation.JustDidIt);

          const result = await ctx.invoke(organisation.JustDoIt);
          assert.isFalse(result.response.hasErrors);

          const loaded2 = await ctx.load(pull.Organisation({object: organisation}));

          assert.isTrue(organisation.JustDidIt);
        });

      });
  });

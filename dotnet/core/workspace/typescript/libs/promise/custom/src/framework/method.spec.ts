import { Organisation } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

import 'jest-extended';

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

          const { ctx, pull } = fixture;

          ctx.session.reset();

          const loaded = await ctx.load(pull.Organisation());
          const organisations = loaded.collections['Organisations'] as Organisation[];

          const organisation = organisations[0];

          expect(organisation.JustDidIt).toBeFalse();

          const result = await ctx.invoke(organisation.JustDoIt);
          expect(result.response.hasErrors).toBeFalse();

          const organisationPull = pull.Organisation({object: organisation});
          await ctx.load(organisationPull);

          ctx.session.reset();

          expect(organisation.JustDidIt).toBeTruthy();
        });

      });
  });

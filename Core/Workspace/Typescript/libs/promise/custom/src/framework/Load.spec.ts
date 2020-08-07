import { Pull, Extent } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { C1 } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

describe('Load',
  () => {
    let fixture: Fixture;

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init(fixture.FULL_POPULATION);
    });

    describe('as someone without access controls',
      () => {
        it('should return no accesscontrol', async () => {

          await fixture.login('noacl');

          const { ctx, m } = fixture;
          const pulls = [
            new Pull({
              extent: new Extent(m.C1),
            })
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const c1s = loaded.collections['C1s'] as C1[];

          expect(() => {
            for (const c1 of c1s) {
              for (const roleType of m.C1.roleTypes) {
                c1.get(roleType);
              }
              for (const associationType of m.C1.associationTypes) {
                c1.getAssociation(associationType);
              }
            }
          }).not.toThrow();
        });

      });

    describe('as someone with an access control but without permissions',
      () => {
        it('should return no accesscontrol', async () => {

          await fixture.login('noperm');

          const { ctx, m } = fixture;
          const pulls = [
            new Pull({
              extent: new Extent(m.C1),
            })
          ];

          ctx.session.reset();

          const loaded = await ctx
            .load(new PullRequest({ pulls }));

          const c1s = loaded.collections['C1s'] as C1[];

          expect(() => {
            for (const c1 of c1s) {
              for (const roleType of m.C1.roleTypes) {
                c1.get(roleType);
              }
              for (const associationType of m.C1.associationTypes) {
                c1.getAssociation(associationType);
              }
            }
          }).not.toThrow();
        });

      });

  });

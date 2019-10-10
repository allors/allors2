import { C1, C2 } from '../../src/allors/domain';
import { PullRequest, Equals, Pull, Filter } from '../../src/allors/framework';

import { assert, expect } from 'chai';
import 'mocha';

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
              extent: new Filter(m.C1),
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
          }).to.not.throw();
        });

      });

    describe('as somenone with an access control but without permissions',
      () => {
        it('should return no accesscontrol', async () => {

          await fixture.login('noperm');

          const { ctx, m } = fixture;
          const pulls = [
            new Pull({
              extent: new Filter(m.C1),
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
          }).to.not.throw();
        });

      });

  });

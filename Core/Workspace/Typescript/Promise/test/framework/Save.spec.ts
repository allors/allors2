import { C1, C2 } from '../../src/allors/domain';
import { PullRequest, Equals } from '../../src/allors/framework';

import { assert, expect } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';

describe('Save',
  () => {
    let fixture: Fixture;

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init(fixture.FULL_POPULATION);
    });

    describe('a new object',
      () => {
        it('should sync the newly created object', async () => {

          const { x, ctx, m, pull } = fixture;

          const newObject = ctx.session.create('C1');

          const saved = await ctx
            .save();

          expect(() => {
            for (const roleType of m.C1.roleTypes) {
              newObject.get(roleType);
            }
            for (const associationType of m.C1.associationTypes) {
              newObject.getAssociation(associationType);
            }
          }).to.not.throw();
        });

      });
  });

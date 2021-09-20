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

          const { ctx, m } = fixture;

          const newObject = ctx.session.create('C1');


          expect(() => {
            for (const roleType of m.C1.roleTypes) {
              newObject.get(roleType);
            }
            for (const associationType of m.C1.associationTypes) {
              newObject.getAssociation(associationType);
            }
          }).not.toThrow();
        });

      });
  });

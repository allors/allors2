import { UnitSample, I1, C1 } from '../../src/allors/domain';

import { assert } from 'chai';
import 'mocha';
import { Fixture } from '../Fixture';

describe('Create',
  () => {
    let fixture: Fixture;

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init();
    });


    describe('a new Object',
      () => {
        it('written unit roles should be readable', async () => {
          const { ctx, m } = fixture;
          const c1 = ctx.session.create(m.C1) as C1;

          c1.C1AllorsBinary = 'AQID';
          c1.C1AllorsBoolean = true;
          c1.C1AllorsDateTime = '1973-03-27T00:00:00.0000000Z';
          c1.C1AllorsDecimal = '12.34';
          c1.C1AllorsDouble = 123;
          c1.C1AllorsInteger = 1000;
          c1.C1AllorsString = 'a string';
          c1.C1AllorsUnique = '2946cf37-71be-4681-8fe6-d0024d59beff';

          assert.isTrue(c1.CanReadC1AllorsBinary);
          assert.isTrue(c1.CanReadC1AllorsBoolean);
          assert.isTrue(c1.CanReadC1AllorsDateTime);
          assert.isTrue(c1.CanReadC1AllorsDecimal);
          assert.isTrue(c1.CanReadC1AllorsDouble);
          assert.isTrue(c1.CanReadC1AllorsInteger);
          assert.isTrue(c1.CanReadC1AllorsString);
          assert.isTrue(c1.CanReadC1AllorsUnique);

          assert.isDefined(c1.C1AllorsBinary);
          assert.isDefined(c1.C1AllorsBoolean);
          assert.isDefined(c1.C1AllorsDateTime);
          assert.isDefined(c1.C1AllorsDecimal);
          assert.isDefined(c1.C1AllorsDouble);
          assert.isDefined(c1.C1AllorsInteger);
          assert.isDefined(c1.C1AllorsString);
          assert.isDefined(c1.C1AllorsUnique);

          assert.equal('AQID', c1.C1AllorsBinary);
          assert.equal(true, c1.C1AllorsBoolean);
          assert.equal('1973-03-27T00:00:00.0000000Z', c1.C1AllorsDateTime);
          assert.equal('12.34', c1.C1AllorsDecimal);
          assert.equal(123, c1.C1AllorsDouble);
          assert.equal(1000, c1.C1AllorsInteger);
          assert.equal('a string', c1.C1AllorsString);
          assert.equal('2946cf37-71be-4681-8fe6-d0024d59beff', c1.C1AllorsUnique);
        });

        it('written concrete unit roles should be readable', async () => {
          const { ctx, m } = fixture;
          const c1 = ctx.session.create(m.C1) as C1;

          c1.I1AllorsBinary = 'AQID';
          c1.I1AllorsBoolean = true;
          c1.I1AllorsDateTime = '1973-03-27T00:00:00.0000000Z';
          c1.I1AllorsDecimal = '12.34';
          c1.I1AllorsDouble = 123;
          c1.I1AllorsInteger = 1000;
          c1.I1AllorsString = 'a string';
          c1.I1AllorsUnique = '2946cf37-71be-4681-8fe6-d0024d59beff';

          assert.isTrue(c1.CanReadI1AllorsBinary);
          assert.isTrue(c1.CanReadI1AllorsBoolean);
          assert.isTrue(c1.CanReadI1AllorsDateTime);
          assert.isTrue(c1.CanReadI1AllorsDecimal);
          assert.isTrue(c1.CanReadI1AllorsDouble);
          assert.isTrue(c1.CanReadI1AllorsInteger);
          assert.isTrue(c1.CanReadI1AllorsString);
          assert.isTrue(c1.CanReadI1AllorsUnique);

          assert.isDefined(c1.I1AllorsBinary);
          assert.isDefined(c1.I1AllorsBoolean);
          assert.isDefined(c1.I1AllorsDateTime);
          assert.isDefined(c1.I1AllorsDecimal);
          assert.isDefined(c1.I1AllorsDouble);
          assert.isDefined(c1.I1AllorsInteger);
          assert.isDefined(c1.I1AllorsString);
          assert.isDefined(c1.I1AllorsUnique);

          assert.equal('AQID', c1.I1AllorsBinary);
          assert.equal(true, c1.I1AllorsBoolean);
          assert.equal('1973-03-27T00:00:00.0000000Z', c1.I1AllorsDateTime);
          assert.equal('12.34', c1.I1AllorsDecimal);
          assert.equal(123, c1.I1AllorsDouble);
          assert.equal(1000, c1.I1AllorsInteger);
          assert.equal('a string', c1.I1AllorsString);
          assert.equal('2946cf37-71be-4681-8fe6-d0024d59beff', c1.I1AllorsUnique);
        });
      });
  });

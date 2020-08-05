import { C1 } from '@allors/domain/generated';

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

          expect(c1.CanReadC1AllorsBinary).toBeTruthy();
          expect(c1.CanReadC1AllorsBoolean).toBeTruthy();
          expect(c1.CanReadC1AllorsDateTime).toBeTruthy();
          expect(c1.CanReadC1AllorsDecimal).toBeTruthy();
          expect(c1.CanReadC1AllorsDouble).toBeTruthy();
          expect(c1.CanReadC1AllorsInteger).toBeTruthy();
          expect(c1.CanReadC1AllorsString).toBeTruthy();
          expect(c1.CanReadC1AllorsUnique).toBeTruthy();

          expect(c1.C1AllorsBinary).toBeDefined();
          expect(c1.C1AllorsBoolean).toBeDefined();
          expect(c1.C1AllorsDateTime).toBeDefined();
          expect(c1.C1AllorsDecimal).toBeDefined();
          expect(c1.C1AllorsDouble).toBeDefined();
          expect(c1.C1AllorsInteger).toBeDefined();
          expect(c1.C1AllorsString).toBeDefined();
          expect(c1.C1AllorsUnique).toBeDefined();

          expect('AQID').toBe( c1.C1AllorsBinary);
          expect(true).toBe( c1.C1AllorsBoolean);
          expect('1973-03-27T00:00:00.0000000Z').toBe( c1.C1AllorsDateTime);
          expect('12.34').toBe( c1.C1AllorsDecimal);
          expect(123).toBe( c1.C1AllorsDouble);
          expect(1000).toBe( c1.C1AllorsInteger);
          expect('a string').toBe( c1.C1AllorsString);
          expect('2946cf37-71be-4681-8fe6-d0024d59beff').toBe( c1.C1AllorsUnique);
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

          expect(c1.CanReadI1AllorsBinary).toBeTruthy();
          expect(c1.CanReadI1AllorsBoolean).toBeTruthy();
          expect(c1.CanReadI1AllorsDateTime).toBeTruthy();
          expect(c1.CanReadI1AllorsDecimal).toBeTruthy();
          expect(c1.CanReadI1AllorsDouble).toBeTruthy();
          expect(c1.CanReadI1AllorsInteger).toBeTruthy();
          expect(c1.CanReadI1AllorsString).toBeTruthy();
          expect(c1.CanReadI1AllorsUnique).toBeTruthy();

          expect(c1.I1AllorsBinary).toBeDefined();
          expect(c1.I1AllorsBoolean).toBeDefined();
          expect(c1.I1AllorsDateTime).toBeDefined();
          expect(c1.I1AllorsDecimal).toBeDefined();
          expect(c1.I1AllorsDouble).toBeDefined();
          expect(c1.I1AllorsInteger).toBeDefined();
          expect(c1.I1AllorsString).toBeDefined();
          expect(c1.I1AllorsUnique).toBeDefined();

          expect('AQID').toBe( c1.I1AllorsBinary);
          expect(true).toBe( c1.I1AllorsBoolean);
          expect('1973-03-27T00:00:00.0000000Z').toBe( c1.I1AllorsDateTime);
          expect('12.34').toBe( c1.I1AllorsDecimal);
          expect(123).toBe( c1.I1AllorsDouble);
          expect(1000).toBe( c1.I1AllorsInteger);
          expect('a string').toBe( c1.I1AllorsString);
          expect('2946cf37-71be-4681-8fe6-d0024d59beff').toBe( c1.I1AllorsUnique);
        });
      });
  });

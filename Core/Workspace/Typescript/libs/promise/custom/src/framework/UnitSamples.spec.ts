import { UnitSample } from '@allors/domain/generated';

import { Fixture } from '../Fixture';

describe('Unit Samples',
  () => {
    let fixture: Fixture;

    beforeEach(async () => {
      fixture = new Fixture();
      await fixture.init();
    });

    describe('Step 1',
      () => {
        it('should return units with values', async () => {

          const { ctx } = fixture;

          ctx.session.reset();

          const loaded = await ctx.load('TestUnitSamples/Pull', { step: 1 });

          const unitSample = loaded.objects['unitSample'] as UnitSample;

          expect(unitSample.CanReadAllorsBinary).toBeTruthy();
          expect(unitSample.CanReadAllorsBoolean).toBeTruthy();
          expect(unitSample.CanReadAllorsDateTime).toBeTruthy();
          expect(unitSample.CanReadAllorsDecimal).toBeTruthy();
          expect(unitSample.CanReadAllorsDouble).toBeTruthy();
          expect(unitSample.CanReadAllorsInteger).toBeTruthy();
          expect(unitSample.CanReadAllorsString).toBeTruthy();
          expect(unitSample.CanReadAllorsUnique).toBeTruthy();

          expect(unitSample.AllorsBinary).toBeDefined();
          expect(unitSample.AllorsBoolean).toBeDefined();
          expect(unitSample.AllorsDateTime).toBeDefined();
          expect(unitSample.AllorsDecimal).toBeDefined();
          expect(unitSample.AllorsDouble).toBeDefined();
          expect(unitSample.AllorsInteger).toBeDefined();
          expect(unitSample.AllorsString).toBeDefined();
          expect(unitSample.AllorsUnique).toBeDefined();

          expect('AQID').toBe( unitSample.AllorsBinary);
          expect(true).toBe( unitSample.AllorsBoolean);
          expect('1973-03-27T00:00:00.0000000Z').toBe( unitSample.AllorsDateTime);
          expect('12.34').toBe( unitSample.AllorsDecimal);
          expect(123).toBe( unitSample.AllorsDouble);
          expect(1000).toBe( unitSample.AllorsInteger);
          expect('a string').toBe( unitSample.AllorsString);
          expect('2946cf37-71be-4681-8fe6-d0024d59beff').toBe( unitSample.AllorsUnique);
        });
      });
  });

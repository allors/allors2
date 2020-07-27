import { Person, Media, UnitSample } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, Node, Tree, Result, Fetch, MetaPopulation } from '../../src/allors/framework';

import { assert } from 'chai';
import 'mocha';

import { Fixture } from '../Fixture';
import { TreeFactory } from '../../src/allors/meta';

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

          assert.isTrue(unitSample.CanReadAllorsBinary);
          assert.isTrue(unitSample.CanReadAllorsBoolean);
          assert.isTrue(unitSample.CanReadAllorsDateTime);
          assert.isTrue(unitSample.CanReadAllorsDecimal);
          assert.isTrue(unitSample.CanReadAllorsDouble);
          assert.isTrue(unitSample.CanReadAllorsInteger);
          assert.isTrue(unitSample.CanReadAllorsString);
          assert.isTrue(unitSample.CanReadAllorsUnique);

          assert.isDefined(unitSample.AllorsBinary);
          assert.isDefined(unitSample.AllorsBoolean);
          assert.isDefined(unitSample.AllorsDateTime);
          assert.isDefined(unitSample.AllorsDecimal);
          assert.isDefined(unitSample.AllorsDouble);
          assert.isDefined(unitSample.AllorsInteger);
          assert.isDefined(unitSample.AllorsString);
          assert.isDefined(unitSample.AllorsUnique);

          assert.equal('AQID', unitSample.AllorsBinary);
          assert.equal(true, unitSample.AllorsBoolean);
          assert.equal('1973-03-27T00:00:00.0000000Z', unitSample.AllorsDateTime);
          assert.equal('12.34', unitSample.AllorsDecimal);
          assert.equal(123, unitSample.AllorsDouble);
          assert.equal(1000, unitSample.AllorsInteger);
          assert.equal('a string', unitSample.AllorsString);
          assert.equal('2946cf37-71be-4681-8fe6-d0024d59beff', unitSample.AllorsUnique);
        });
      });
  });

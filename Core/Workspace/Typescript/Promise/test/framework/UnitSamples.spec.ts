import { Person, Media, UnitSample } from '../../src/allors/domain';
import { PullRequest, Pull, Filter, TreeNode, Tree, Result, Fetch, MetaPopulation } from '../../src/allors/framework';

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

          const { scope } = fixture;

          scope.session.reset();

          const loaded = await scope.load('TestUnitSamples/Pull', { step: 1 });

          const unitSample = loaded.objects['unitSample'] as UnitSample;

          assert.isDefined(unitSample.AllorsBinary);
          assert.isDefined(unitSample.AllorsBoolean);
          assert.isDefined(unitSample.AllorsDateTime);
          assert.isDefined(unitSample.AllorsDecimal);
          assert.isDefined(unitSample.AllorsDouble);
          assert.isDefined(unitSample.AllorsInteger);
          assert.isDefined(unitSample.AllorsString);
          assert.isDefined(unitSample.AllorsUnique);
        });
      });
  });

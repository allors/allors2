import { MetaPopulation } from '@allors/workspace/meta';
import { data } from './index';

describe('Meta', function () {
  it('can construct MetaPopulation', function () {

    const metaPopulation = new MetaPopulation(data);

    expect(metaPopulation).toBeDefined();
  });
});

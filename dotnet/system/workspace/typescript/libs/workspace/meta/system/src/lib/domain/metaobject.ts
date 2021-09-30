import { Origin } from '@allors/workspace/meta/system';
import { MetaPopulation } from './MetaPopulation';

export interface MetaObject {
  metaPopulation: MetaPopulation;

  tag: number;

  origin: Origin;
}

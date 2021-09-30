import { Composite } from './Composite';

export interface Interface extends Composite {
  subtypes: Set<Composite>;
}

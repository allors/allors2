import { Predicate } from './Predicate';

export interface ParameterizablePredicate extends Predicate {
  parameter?: string;
}

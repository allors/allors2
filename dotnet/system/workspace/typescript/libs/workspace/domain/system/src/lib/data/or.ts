import { Predicate } from './Predicate';

export interface Or extends Predicate {
  operands: Predicate[];
}

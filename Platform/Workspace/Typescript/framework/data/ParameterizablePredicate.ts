import { Predicate, PredicateArgs } from './Predicate';
import { ObjectType } from '../meta/ObjectType';

export type ParameterizablePredicateArgs = Pick<
  ParameterizablePredicate,
  'parameter'
> &
  PredicateArgs;

export abstract class ParameterizablePredicate extends Predicate {
  parameter?: string;

  abstract get objectType(): ObjectType;

  constructor() {
    super();
  }
}

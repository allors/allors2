import { ObjectType } from '@allors/meta/system';

import { Predicate, PredicateArgs } from './Predicate';

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

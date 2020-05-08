import { ObjectType } from '../meta';
import { Predicate } from './Predicate';

export abstract class ParametrizedPredicate implements Predicate {
  dependencies: string[];
  parameter: string;

  abstract get objectType(): ObjectType;
}

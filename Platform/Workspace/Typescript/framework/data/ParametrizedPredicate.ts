import { ObjectType } from '../meta';

export abstract class ParametrizedPredicate {
  parameter: string;

  abstract get objectType(): ObjectType;
}

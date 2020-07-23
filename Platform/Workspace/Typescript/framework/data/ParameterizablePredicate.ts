import { ObjectType } from "../meta";
import { Predicate } from "./Predicate";

export type ParameterizablePredicateArgs = Pick<
  ParameterizablePredicate,
  "dependencies" | "parameter"
>;

export abstract class ParameterizablePredicate implements Predicate {
  dependencies?: string[];
  parameter?: string;

  abstract get objectType(): ObjectType;
}

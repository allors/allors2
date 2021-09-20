import { Predicate } from "./Predicate";

export interface And extends Predicate {
  operands: Predicate[];
}

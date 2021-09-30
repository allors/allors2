import { Predicate } from "./Predicate";

export interface Not extends Predicate {
  operand?: Predicate;
}

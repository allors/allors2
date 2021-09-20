import { UnitTypes } from "@allors/workspace/domain/system";
import { Extent } from "./Extent";
import { PredicateKind } from "./PredicateKind";

export interface Predicate {
  /** kind */
  k: PredicateKind;

  /** AssociationType */
  a: number;

  /** RoleType */
  r: number;

  /** ObjectType */
  t: number;

  /** Parameter */
  p: string;

  /** Dependencies */
  d: string[];

  /** Operand */
  op: Predicate;

  /** Operands */
  ops: Predicate[];

  /** Object */
  ob: number;

  /** Objects */
  obs: number[];

  /** Value */
  v: UnitTypes;

  /** Values */
  vs: UnitTypes[];

  /** Extent */
  e: Extent;
}

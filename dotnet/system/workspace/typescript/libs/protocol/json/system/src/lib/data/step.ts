import { Node } from "./Node";

export interface Step {
  /** AssociationType */
  a: number;

  /** RoleType */
  r: number;

  /** Next */
  n: Step;

  /** Include */
  i: Node[];
}

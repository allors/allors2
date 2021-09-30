import { Node } from "./Node";
import { Step } from "./Step";

export interface Select {
  /** Step */
  s: Step;

  /** Include */
  i: Node[];
}

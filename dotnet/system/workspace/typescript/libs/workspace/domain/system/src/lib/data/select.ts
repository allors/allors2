import { Step } from "./Step";
import { Tree } from "./Tree";

export interface Select {
  step?: Step;

  include?: Tree | Node[];
}

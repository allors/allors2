import { Select } from "./Select";

export interface Result {
  /** SelectRef */
  r: string;

  /** Select */
  s: Select;

  /** Name */
  n: string;

  /** Skip */
  k: number;

  /** Take */
  t: number;
}

import { Result } from "./Result";

export interface Pull {
  /** ExtentRef */
  er: string;

  /** Extent */
  e: string;

  /** ObjectType */
  t: number;

  /** Object */
  o: number;

  /** Results */
  r: Result[];

  /** Arguments */
  a: { [name: string]: string };
}

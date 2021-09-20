import { Response } from '@allors/protocol/json/system';
import { PullResponseObject } from "./PullResponseObject";

export interface PullResponse extends Response {
  /** Collections */
  c: { [name: string]: number[] };

  /** Objects */
  o: { [name: string]: number };

  /** Values */
  v: { [name: string]: string };

  /** Pool */
  p: PullResponseObject[];

  /** AccessControls */
  a: number[][];
}

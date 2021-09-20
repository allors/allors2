import { SyncResponseObject } from "./SyncResponseObject";

export interface SyncResponse {
  /** Objects */
  o: SyncResponseObject[];

  /** AccessControls */
  a: number[][];
}

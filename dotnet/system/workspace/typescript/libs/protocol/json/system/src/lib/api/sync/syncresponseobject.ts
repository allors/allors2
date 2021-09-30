import { SyncResponseRole } from "./SyncResponseRole";

export interface SyncResponseObject {
  /** Id */
  i: number;

  /** Version */
  v: number;

  /** ObjectType */
  t: number;

  /** AccessControls */
  a: number[];

  /** DeniedPermissions */
  d: number[];

  /** Roles */
  r: SyncResponseRole[];
}

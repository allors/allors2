import { SecurityResponseAccessControl } from "./SecurityResponseAccessControl";

export interface SecurityResponse {
  /** AccessControls */
  a: SecurityResponseAccessControl[];

  /** Permissions */
  p: number[][];
}

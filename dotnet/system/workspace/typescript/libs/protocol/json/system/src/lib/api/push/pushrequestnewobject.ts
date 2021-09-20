import { PushRequestRole } from "./PushRequestRole";

export interface PushRequestNewObject {
  /** WorkspaceId */
  w: number;

  /** ObjectType */
  t: number;

  /** Roles */
  r?: PushRequestRole[];
}

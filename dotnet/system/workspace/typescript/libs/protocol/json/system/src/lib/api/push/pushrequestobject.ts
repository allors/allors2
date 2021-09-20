import { PushRequestRole } from "./PushRequestRole";

export interface PushRequestObject {
  /** DatabaseId */
  d: number;

  /** Version */
  v: number;

  /** Roles */
  r?: PushRequestRole[];
}

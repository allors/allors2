import { PushRequestNewObject } from "./PushRequestNewObject";
import { PushRequestObject } from "./PushRequestObject";

export interface PushRequest {
  /** NewObjects*/
  n: PushRequestNewObject[];

  /** Objects */
  o: PushRequestObject[];
}

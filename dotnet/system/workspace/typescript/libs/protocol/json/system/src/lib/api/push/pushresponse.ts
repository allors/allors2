import { Response } from '@allors/protocol/json/system';
import { PushResponseNewObject } from "./PushResponseNewObject";

export interface PushResponse extends Response {
  /** NewObjects */
  n: PushResponseNewObject[];
}

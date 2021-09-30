import { Invocation } from "./Invocation";
import { InvokeOptions } from "./InvokeOptions";

export interface InvokeRequest {
  /** List */
  l: Invocation[];

  /** Options */
  o: InvokeOptions[];
}

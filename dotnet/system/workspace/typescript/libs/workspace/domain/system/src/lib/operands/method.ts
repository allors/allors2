import { MethodType } from "@allors/workspace/meta/system";
import { IObject } from "../IObject";

export interface Method {
  object: IObject;

  MethodType: MethodType;
}

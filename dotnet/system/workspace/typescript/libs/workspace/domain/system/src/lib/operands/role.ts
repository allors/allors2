import { RelationType } from "@allors/workspace/meta/system";
import { IObject } from "../IObject";

export interface Role {
  object: IObject;

  relationType: RelationType;
}

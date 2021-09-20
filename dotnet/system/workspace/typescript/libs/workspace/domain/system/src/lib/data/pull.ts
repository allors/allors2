import { ObjectType } from '@allors/workspace/meta/system';
import { CompositeTypes, ParameterTypes } from "../Types";
import { IExtent } from "./IExtent";
import { Result } from "./Result";

export interface Pull {
  objectType?: ObjectType;

  extentRef?: string;

  extent?: IExtent;

  object?: CompositeTypes;

  results?: Result[];

  arguments?: { [name: string]: ParameterTypes };
}

import { IObject } from '../IObject';
import { UnitTypes } from '../Types';
import { IResult } from './IResult';

export interface IPullResult extends IResult {
  collections: Map<string, IObject[]>;

  objects: Map<string, IObject>;

  values: Map<string, UnitTypes>;
}

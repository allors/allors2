import { ISessionObject } from '../workspace/ISessionObject';
import { SessionObject } from '../workspace/SessionObject';

export type UnitTypes = string | Date | boolean | number;
export type CompositeTypes = ISessionObject | string;
export type ParameterTypes = UnitTypes | CompositeTypes | CompositeTypes[];

export function isSessionObject(obj: any): obj is SessionObject {
  return obj instanceof SessionObject;
}

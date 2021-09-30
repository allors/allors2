import { IObject } from './IObject';

export type UnitTypes = string | Date | boolean | number;
export type CompositeTypes = IObject | string;
export type ParameterTypes = UnitTypes | CompositeTypes | CompositeTypes[];

// todo: move to Database
export function isSessionObject(obj: unknown): obj is IObject {
  return (obj as IObject).id != null;
}

import { ISessionObject } from '../workspace/SessionObject';
import { ids } from '../../meta/generated';
import { ObjectType } from '../meta/ObjectType';

export type UnitTypes = string | Date | boolean | number;
export type CompositeTypes = ISessionObject | string;

export function serializeArray(roles: UnitTypes[], objectType: ObjectType): string[] {
  if (roles) {
    return roles.map(v => serialize(v, objectType));
  }

  return [];
}

export function serialize(role: UnitTypes, objectType: ObjectType): string {
  switch (objectType.id) {
    case ids.Boolean:
      return role ? 'true' : 'false';

    case ids.DateTime:
      if (role instanceof Date) {
        return (role as Date).toISOString();
      }
      break;
  }

  role = role.toString();
}

export function deserialize(value: string, objectType: ObjectType): UnitTypes {
  switch (objectType.id) {
    case ids.Boolean:
      return value === 'true' ? true : false;
    case ids.Float:
      return parseFloat(value);
    case ids.Integer:
      return parseInt(value, 10);
  }

  return value;
}

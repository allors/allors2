import { ISessionObject, SessionObject } from '../workspace/SessionObject';
import { ids } from '../../meta/generated/ids.g';
import { ObjectType } from '../meta/ObjectType';

export type UnitTypes = string | Date | boolean | number;
export type CompositeTypes = ISessionObject | string;
export type ParameterTypes = UnitTypes | CompositeTypes | CompositeTypes[];

export function serializeObject(roles: { [name: string]: ParameterTypes; } | undefined): { [name: string]: string; } {
  if (roles) {
    return Object
      .keys(roles)
      .reduce((obj, v) => {
        const role = roles[v];
        if (Array.isArray(role)) {
          obj[v] = role.map((w) => serialize(w)).join(',');
        } else {
          obj[v] = serialize(role);
        }
        return obj;
      }, {} as { [key: string]: any });
  }

  return {};
}

export function serializeArray(roles: UnitTypes[]): (string | null)[] {
  if (roles) {
    return roles.map(v => serialize(v));
  }

  return [];
}

export function serialize(role: UnitTypes | CompositeTypes): string | null {

  if (role === undefined || role === null) {
    return null;
  }

  if (typeof role === 'string') {
    return role;
  }

  if (role instanceof Date) {
    return (role as Date).toISOString();
  }

  if (role instanceof SessionObject) {
    return role.id;
  }

  return role.toString();
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

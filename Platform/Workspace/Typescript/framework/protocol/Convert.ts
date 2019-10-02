import { ids } from '../../meta/generated';
import { ObjectType } from '../meta/ObjectType';
import { UnitTypes } from './Types';

export function unitToString(role: UnitTypes, objectType: ObjectType): string {
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

export function stringToUnit(value: string, objectType: ObjectType): UnitTypes {
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

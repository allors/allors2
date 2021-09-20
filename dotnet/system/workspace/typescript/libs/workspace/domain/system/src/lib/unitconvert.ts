import { UnitTags } from '@allors/workspace/meta/system';
import { UnitTypes } from './Types';

export function toString(tag: number, value: UnitTypes): string | null {
  if (value == null) {
    return null;
  }

  switch (tag) {
    case UnitTags.Binary:
    case UnitTags.Decimal:
    case UnitTags.String:
    case UnitTags.Unique:
      return value as string;
    case UnitTags.DateTime:
      return (value as Date).toISOString();
    case UnitTags.Boolean:
      return value ? 'true' : 'false';
    case UnitTags.Float:
    case UnitTags.Integer:
      return (value as number).toString();
    default:
      throw new Error(`Unknown unit type with tag {tag}`);
  }
}

export function fromString(tag: number, value: string): UnitTypes | null {
  if (value == null) {
    return null;
  }

  switch (tag) {
    case UnitTags.Binary:
    case UnitTags.Decimal:
    case UnitTags.String:
    case UnitTags.Unique:
      return value;
    case UnitTags.DateTime:
      return Date.parse(value);
    case UnitTags.Boolean:
      return value === 'true';
    case UnitTags.Float:
      return Number.parseFloat(value);
    case UnitTags.Integer:
      return Number.parseInt(value);
    default:
      throw new Error(`Unknown unit type with tag {tag}`);
  }
}

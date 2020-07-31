
// TODO: Reverse dependency
import { ids } from '@allors/meta/generated/ids.g';


export const unitIdByTypeName: { [key: string]: string } = {
  Binary: ids.Binary,
  Boolean: ids.Boolean,
  DateTime: ids.DateTime,
  Decimal: ids.Decimal,
  Float: ids.Float,
  Integer: ids.Integer,
  String: ids.String,
  Unique: ids.Unique,
};

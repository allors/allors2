import { unitIds } from './unitIds';

export const unitIdByTypeName: { [key: string]: string } = {
  Binary: unitIds.Binary,
  Boolean: unitIds.Boolean,
  DateTime: unitIds.DateTime,
  Decimal: unitIds.Decimal,
  Float: unitIds.Float,
  Integer: unitIds.Integer,
  String: unitIds.String,
  Unique: unitIds.Unique,
};

import { UnitTypes } from "@allors/workspace/domain/system";

export interface PushRequestRole {
  /** RelationType */
  t: number;

  /** SetUnitRole */
  u?: UnitTypes;

  /** SetCompositeRole */
  c?: number;

  /** AddCompositesRole */
  a?: number[];

  /** RemoveCompositesRole */
  r?: number[];
}

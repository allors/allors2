import { SortDirection } from "@allors/workspace/domain/system";

export interface Sort {
  /** RoleType */
  r: number;

  /** Direction */
  d: SortDirection;
}

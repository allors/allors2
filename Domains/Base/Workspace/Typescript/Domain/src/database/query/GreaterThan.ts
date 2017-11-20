import { RoleType } from "@allors/base-meta";

import { Predicate } from "./Predicate";

export class GreaterThan implements Predicate {
  public roleType: RoleType;
  public value: any;

  constructor(fields?: Partial<GreaterThan>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "GreaterThan",
      rt: this.roleType.id,
      v: this.value,
    };
  }
}

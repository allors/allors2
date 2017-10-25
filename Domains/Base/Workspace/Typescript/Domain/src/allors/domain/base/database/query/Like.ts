import { RoleType } from "@baseMeta";

import { Predicate } from "./Predicate";

export class Like implements Predicate {
  public roleType: RoleType;
  public value: any;

  constructor(fields?: Partial<Like>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "Like",
      rt: this.roleType.id,
      v: this.value,
    };
  }
}

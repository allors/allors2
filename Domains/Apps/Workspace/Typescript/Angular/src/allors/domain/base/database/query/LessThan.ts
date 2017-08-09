import { RoleType } from "../../../../meta";
import { Predicate } from "./Predicate";

export class LessThan implements Predicate {
  public roleType: RoleType;
  public value: any;

  constructor(fields?: Partial<LessThan>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "LessThan",
      rt: this.roleType.id,
      v: this.value,
    };
  }
}

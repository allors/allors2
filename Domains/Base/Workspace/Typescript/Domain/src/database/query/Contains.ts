import { AssociationType, RoleType } from "@allors/base-meta";

import { ISessionObject } from "./../../workspace/SessionObject";
import { Predicate } from "./Predicate";
import { Query } from "./Query";

export class Contains implements Predicate {
  public associationType: AssociationType;
  public roleType: RoleType;
  public object: ISessionObject;

  constructor(fields?: Partial<Contains>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "Contains",
      at: this.associationType ? this.associationType.id : undefined,
      o: this.object ? this.object.id : undefined,
      rt: this.roleType.id ? this.roleType.id : undefined,
    };
  }
}

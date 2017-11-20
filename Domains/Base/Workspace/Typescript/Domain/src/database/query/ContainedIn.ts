import { AssociationType, RoleType } from "@allors/base-meta";

import { ISessionObject } from "./../../workspace/SessionObject";
import { Predicate } from "./Predicate";
import { Query } from "./Query";

export class ContainedIn implements Predicate {
  public associationType: AssociationType;
  public roleType: RoleType;
  public query: Query;
  public objects: ISessionObject[];

  constructor(fields?: Partial<ContainedIn>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "ContainedIn",
      at: this.associationType ? this.associationType.id : undefined,
      rt: this.roleType ? this.roleType.id : undefined,
      q: this.query,
      o: this.objects ? this.objects.map((v: ISessionObject) => v.id) : undefined,
    };
  }
}

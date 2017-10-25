import { AssociationType, ObjectType, RoleType } from "@baseMeta";

import { Predicate } from "./Predicate";

export class Instanceof implements Predicate {
  public associationType: AssociationType;
  public roleType: RoleType;
  public objectType: ObjectType;

  constructor(fields?: Partial<Instanceof>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "Instanceof",
      at: this.associationType ? this.associationType.id : undefined,
      ot: this.objectType.id,
      rt: this.roleType.id ? this.roleType.id : undefined,
    };
  }
}

import { ObjectType } from "../../../../meta";
import { Predicate } from "./Predicate";

export class Or implements Predicate {
  public predicates: Predicate[];

  constructor(fields?: Partial<Or>) {
    Object.assign(this, fields);
    this.predicates = this.predicates ? this.predicates : [];
   }

  public toJSON(): any {
    return {
      _T: "Or",
      ps: this.predicates,
    };
  }
}

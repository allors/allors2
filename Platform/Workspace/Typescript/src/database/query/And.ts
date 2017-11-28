import { ObjectType } from "../../meta";
import { Predicate } from "./Predicate";

export class And implements Predicate {
  public predicates: Predicate[];

  constructor(fields?: Partial<And>) {
    Object.assign(this, fields);
    this.predicates = this.predicates ? this.predicates : [];
  }

  public toJSON(): any {
    return {
      _T: "And",
      ps: this.predicates,
    };
  }
}

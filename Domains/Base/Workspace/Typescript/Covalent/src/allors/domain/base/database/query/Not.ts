import { ObjectType } from "../../../../meta";
import { Predicate } from "./Predicate";

export class Not implements Predicate {
  public predicate: Predicate;

  constructor(fields?: Partial<Not>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      _T: "Not",
      p: this.predicate,
    };
  }
}

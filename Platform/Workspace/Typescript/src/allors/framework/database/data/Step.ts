import { AssociationType, ObjectType, RoleType } from "../../meta";
import { Tree } from "./Tree";

export class Step {
  public selector: AssociationType | RoleType;

  public next: Step | Tree;

  constructor(fields?: Partial<Step>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      selector: this.selector.id,
      next: this.next,
    };
  }
}

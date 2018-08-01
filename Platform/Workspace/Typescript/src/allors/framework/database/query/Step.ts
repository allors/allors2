import { AssociationType, ObjectType, RoleType } from "../../meta";
import { Include } from "./Include";

export class Step {
  public selector: AssociationType | RoleType;

  public next: Step | Include;

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

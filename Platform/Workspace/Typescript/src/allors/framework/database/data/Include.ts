import { AssociationType, ObjectType, RoleType } from "../../meta";
import { TreeNode } from "./TreeNode";

export class Include {

  public tree: TreeNode[];

  constructor(fields?: Partial<Include>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      tree: this.tree,
    };
  }
}

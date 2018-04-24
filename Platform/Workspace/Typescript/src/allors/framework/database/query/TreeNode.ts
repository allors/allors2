import { ObjectType, RoleType } from "../../meta";

export class TreeNode {

  public roleType: RoleType;
  public nodes: TreeNode[];

  constructor(fields?: Partial<TreeNode>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      n: this.nodes,
      rt: this.roleType.id,
    };
  }

  public parse(json: any, objectType: ObjectType, roleTypeName: string) {
    this.roleType = objectType.roleTypeByName[roleTypeName];

    const childJson = json[roleTypeName];
    if (childJson) {
      Object.keys(childJson)
        .map((childRoleName) => {
          const childTreeNode = new TreeNode();
          childTreeNode.parse(childJson, this.roleType.objectType, childRoleName);
          return childTreeNode;
        });
    }
  }
}

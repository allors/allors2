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

    if (!this.roleType) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = roleTypeName.split("_");

      const subType = metaPopulation.objectTypeByName[subTypeName];
      if (subType) {
        this.roleType = subType.roleTypeByName[subStepName];
      }
    }

    if (!this.roleType) {
      throw new Error("Unknown role: " + roleTypeName);
    }

    const childJson = json[roleTypeName];
    if (childJson) {
      const nodes = Object.keys(childJson)
        .map((childRoleName) => {
          const childTreeNode = new TreeNode();
          childTreeNode.parse(childJson, this.roleType.objectType, childRoleName);
          return childTreeNode;
        });

      if (nodes && nodes.length) {
        this.nodes = nodes;
      }
    }
  }
}

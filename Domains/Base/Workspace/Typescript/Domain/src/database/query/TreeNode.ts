import { RoleType } from "@allors/base-meta";

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
}

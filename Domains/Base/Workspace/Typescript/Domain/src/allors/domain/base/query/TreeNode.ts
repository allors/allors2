import { RoleType } from '../../../meta';

export class TreeNode {

    roleType: RoleType;
    nodes: TreeNode[];

    constructor(fields?: Partial<TreeNode>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        rt: this.roleType.id,
        n: this.nodes,
      };
    }
}

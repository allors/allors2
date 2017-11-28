import { RoleType } from "../../meta";
export declare class TreeNode {
    roleType: RoleType;
    nodes: TreeNode[];
    constructor(fields?: Partial<TreeNode>);
    toJSON(): any;
}

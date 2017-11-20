import { Path } from "./Path";
import { TreeNode } from "./TreeNode";
export declare class Fetch {
    name: string;
    id: string;
    path: Path;
    include: TreeNode[];
    constructor(fields?: Partial<Fetch>);
    toJSON(): any;
}

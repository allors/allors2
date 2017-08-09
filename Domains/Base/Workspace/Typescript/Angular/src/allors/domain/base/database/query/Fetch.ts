import { WorkspaceObject } from "../../workspace";

import { Path } from "./Path";
import { TreeNode } from "./TreeNode";

export class Fetch {
    public name: string;

    public id: string;

    public path: Path;

    public include: TreeNode[];

    constructor(fields?: Partial<Fetch>) {
       Object.assign(this, fields);
    }

    public toJSON(): any {
      return {
        id: this.id,
        include: this.include,
        name: this.name,
        path: this.path,
      };
    }
}

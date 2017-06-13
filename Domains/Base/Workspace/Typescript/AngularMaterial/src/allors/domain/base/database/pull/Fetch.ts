import { WorkspaceObject } from '../../WorkspaceObject';
import { Path } from './Path';
import { TreeNode } from './TreeNode';

export class Fetch {
    name: string;

    id: string;

    path: Path;

    include: TreeNode[];

    constructor(fields?: Partial<Fetch>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        name: this.name,
        id: this.id,
        path: this.path,
        include: this.include,
      };
    }
}

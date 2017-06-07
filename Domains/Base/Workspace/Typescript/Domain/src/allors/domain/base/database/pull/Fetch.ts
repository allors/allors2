import { WorkspaceObject } from '../../WorkspaceObject';
import { TreeNode } from './TreeNode';

export class Fetch {
    name: string;

    id: string;

    include: TreeNode[];

    constructor(fields?: Partial<Fetch>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        name: this.name,
        id: this.id,
        include: this.include,
      };
    }
}

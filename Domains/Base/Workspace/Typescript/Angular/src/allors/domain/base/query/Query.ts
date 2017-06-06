import { ObjectType } from '../../../meta';
import { TreeNode } from './TreeNode';
import { Predicate } from './Predicate';

export class Query {
    name: String;

    type: ObjectType;

    predicate: Predicate;

    tree: TreeNode[];

    constructor(fields?: Partial<Query>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        n: this.name,
        t: this.type.id,
        p: this.predicate,
        tn: this.tree
      };
    }
}

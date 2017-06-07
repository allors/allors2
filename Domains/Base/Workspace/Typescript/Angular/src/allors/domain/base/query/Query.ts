import { ObjectType } from '../../../meta';
import { Predicate } from './Predicate';
import { TreeNode } from './TreeNode';
import { Sort } from './Sort';
import { Page } from './Page';

export class Query {
    name: String;

    objectType: ObjectType;

    predicate: Predicate;

    fetch: TreeNode[];

    sort: Sort[];

    page: Page;

    constructor(fields?: Partial<Query>) {
       Object.assign(this, fields);
    }

    toJSON() {
      return {
        n: this.name,
        ot: this.objectType.id,
        p: this.predicate,
        f: this.fetch,
        s: this.sort,
        pa: this.page
      };
    }
}

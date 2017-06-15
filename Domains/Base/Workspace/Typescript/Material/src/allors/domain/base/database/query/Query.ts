import { ObjectType, ObjectTyped } from '../../../../meta';
import { Predicate } from './Predicate';
import { TreeNode } from './TreeNode';
import { Sort } from './Sort';
import { Page } from './Page';

export class Query {
  name: String;

  objectType: ObjectType | ObjectTyped;

  predicate: Predicate;

  include: TreeNode[];

  sort: Sort[];

  page: Page;

  constructor(fields?: Partial<Query>) {
    Object.assign(this, fields);
  }

  toJSON() {
    function isObjectTyped(objectType: ObjectType | ObjectTyped): objectType is ObjectTyped {
      return (<ObjectTyped>objectType).ObjectType !== undefined;
    }

    return {
      n: this.name,
      ot: isObjectTyped(this.objectType) ? this.objectType.ObjectType.id : this.objectType.id,
      p: this.predicate,
      i: this.include,
      s: this.sort,
      pa: this.page
    };
  }
}

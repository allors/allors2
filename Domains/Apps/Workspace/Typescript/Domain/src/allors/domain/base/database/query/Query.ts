import { ObjectType, ObjectTyped } from "../../../../meta";
import { Page } from "./Page";
import { Predicate } from "./Predicate";
import { Sort } from "./Sort";
import { TreeNode } from "./TreeNode";

export class Query {
  public name: String;

  public objectType: ObjectType | ObjectTyped;

  public predicate: Predicate;

  public union: Query[];

  public intersect: Query[];

  public except: Query[];

  public include: TreeNode[];

  public sort: Sort[];

  public page: Page;

  constructor(fields?: Partial<Query>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    function isObjectTyped(objectType: ObjectType | ObjectTyped): objectType is ObjectTyped {
      return (objectType as ObjectTyped).ObjectType !== undefined;
    }

    return {
      ex: this.except,
      i: this.include,
      in: this.intersect,
      n: this.name,
      ot: isObjectTyped(this.objectType) ? this.objectType.ObjectType.id : this.objectType.id,
      p: this.predicate,
      pa: this.page,
      s: this.sort,
      un: this.union,
    };
  }
}

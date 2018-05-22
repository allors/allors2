import { MetaObjectType, ObjectType } from '../../meta';
import { Page } from './Page';
import { Predicate } from './Predicate';
import { Sort } from './Sort';
import { TreeNode } from './TreeNode';

export class Query {
  public name: string;

  public objectType: ObjectType | MetaObjectType;

  public predicate: Predicate;

  public union: Query[];

  public intersect: Query[];

  public except: Query[];

  public sort: Sort[];

  public page: Page;

  public include: TreeNode[] | any;

  constructor(fields?: Partial<Query> | MetaObjectType | ObjectType) {
    if ((fields as MetaObjectType)._objectType) {
      this.objectType = fields as any;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as MetaObjectType;
    const objectType = metaObjectType._objectType ? metaObjectType._objectType : this.objectType as ObjectType;

    let include = this.include;
    if (this.include && !(this.include instanceof Array)) {
      include = Object.keys(this.include)
        .map((roleName) => {
          const treeNode = new TreeNode();
          treeNode.parse(this.include, objectType, roleName);
          return treeNode;
        });
    }

    let name = this.name;
    if (!this.name) {
      name = objectType.plural;
    }

    return {
      ex: this.except,
      i: include,
      in: this.intersect,
      n: name,
      ot: objectType.id,
      p: this.predicate,
      pa: this.page,
      s: this.sort,
      un: this.union,
    };
  }
}

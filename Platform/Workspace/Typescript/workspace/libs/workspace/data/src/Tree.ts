import { ObjectType } from '@allors/workspace/meta';

import { Node } from './Node';

export type TreeArgs = Pick<Tree, 'objectType' | 'nodes'>;

export class Tree {
  public objectType: ObjectType;

  public nodes: Node[] | any;

  constructor(args: TreeArgs)
  constructor(objectType: ObjectType, literal?: {})
  constructor(args: TreeArgs | ObjectType, literal?: {}) {
    if (args instanceof ObjectType) {
      this.objectType = args;

      if (literal) {
        this.nodes = Object.keys(literal).map((propertyName) => new Node(this.objectType as ObjectType, propertyName, literal));
      }
    } else {
      this.objectType = args.objectType;
      this.nodes = args.nodes;
    }
  }

  public toJSON(): any {
    let nodes = this.nodes;
    if (this.nodes && !(this.nodes instanceof Array)) {
      nodes = Object.keys(this.nodes).map((propertyName) => new Node(this.objectType, propertyName, this.nodes));
    }

    return nodes;
  }
}

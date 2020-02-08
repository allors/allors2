import { ObjectType } from '../meta';
import { Node } from './Node';

export class Tree {

  public objectType: ObjectType;

  public nodes: Node[] | any;

  constructor(fields?: Partial<Tree> | ObjectType, literal?: {}) {
    if (fields instanceof ObjectType) {
      const objectType = fields as ObjectType;
      this.objectType = objectType;

      if (literal) {
        this.nodes = Object.keys(literal)
          .map((propertyName) => {
            const treeNode = new Node();
            treeNode.parse(literal, this.objectType as ObjectType, propertyName);
            return treeNode;
          });
      }
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    let nodes = this.nodes;
    if (this.nodes && !(this.nodes instanceof Array)) {
      nodes = Object.keys(this.nodes)
        .map((propertyName) => {
          const treeNode = new Node();
          treeNode.parse(this.nodes, this.objectType, propertyName);
          return treeNode;
        });
    }

    return nodes;
  }
}

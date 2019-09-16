import { ObjectType } from '../meta';
import { TreeNode } from './TreeNode';

export class Tree {

  public objectType: ObjectType;

  public nodes: TreeNode[] | any;

  constructor(fields?: Partial<Tree> | ObjectType, literal?) {
    if (fields instanceof ObjectType) {
      const objectType = fields as ObjectType;
      this.objectType = objectType;

      if (literal) {
        this.nodes = Object.keys(literal)
          .map((propertyName) => {
            const treeNode = new TreeNode();
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
          const treeNode = new TreeNode();
          treeNode.parse(this.nodes, this.objectType, propertyName);
          return treeNode;
        });
    }

    return {
      composite: this.objectType.id,
      nodes,
    };
  }
}

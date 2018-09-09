import { MetaObjectType, ObjectType } from "../../meta";
import { TreeNode } from "./TreeNode";

export function tree(objectType: ObjectType, literal): Tree {
  var tree = new Tree(objectType);
  tree.nodes = Object.keys(literal)
      .map((roleName) => {
          const treeNode = new TreeNode();
          treeNode.parse(literal, objectType, roleName);
          return treeNode;
      });
  return tree;
}

export class Tree {

  public objectType: ObjectType | MetaObjectType;

  public nodes: TreeNode[] | any;

  constructor(fields?: Partial<Tree> | MetaObjectType | ObjectType) {
    if(fields instanceof ObjectType){
      this.objectType = fields;
    }
    else if((fields as MetaObjectType)._objectType) {
      this.objectType = (fields as MetaObjectType)._objectType;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as MetaObjectType;
    const composite = metaObjectType._objectType ? metaObjectType._objectType : this.objectType as ObjectType;

    let nodes = this.nodes;
    if (this.nodes && !(this.nodes instanceof Array)) {
      nodes = Object.keys(this.nodes)
        .map((roleName) => {
          const treeNode = new TreeNode();
          treeNode.parse(this.nodes, composite, roleName);
          return treeNode;
        });
    }

    return {
      composite: composite.id,
      nodes,
    };
  }
}

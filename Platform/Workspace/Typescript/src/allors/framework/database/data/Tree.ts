import { MetaObjectType, ObjectType } from '../../meta';
import { TreeNode } from './TreeNode';

export class Tree {

  public objectType: ObjectType | MetaObjectType;

  public nodes: TreeNode[] | any;

  constructor(fields?: Partial<Tree> | MetaObjectType | ObjectType, literal?) {
    if (fields instanceof ObjectType || fields && (fields as MetaObjectType).objectType) {
      const objectType = (fields as MetaObjectType).objectType ? (fields as MetaObjectType).objectType : fields as ObjectType;
      this.objectType = objectType;

      if (literal) {
        this.nodes = Object.keys(literal)
          .map((roleName) => {
            const treeNode = new TreeNode();
            treeNode.parse(literal, this.objectType as ObjectType, roleName);
            return treeNode;
          });
      }
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as MetaObjectType;
    const composite = metaObjectType.objectType ? metaObjectType.objectType : this.objectType as ObjectType;

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

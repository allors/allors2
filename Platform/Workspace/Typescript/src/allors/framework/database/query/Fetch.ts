import { ObjectType } from "../../meta/ObjectType";
import { Path } from "./Path";
import { TreeNode } from "./TreeNode";

export class Fetch {
  public name: string;

  public id: string;

  public path: Path | any;

  public include: TreeNode[] | any;

  public objectType: ObjectType;

  constructor(fields?: Partial<Fetch>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    let include = this.include;
    if (this.include && !(this.include instanceof Array)) {
      include = Object.keys(this.include)
        .map((roleName) => {
          const treeNode = new TreeNode();
          treeNode.parse(this.include, this.objectType, roleName);
          return treeNode;
        });
    }

    let path = this.path;
    if (this.path && !this.path.step) {
      path = Object.keys(this.path)
        .map((roleName) => {
          const rolePath = new Path();
          rolePath.parse(this.path, this.objectType, roleName);
          return rolePath;
        })[0];
    }

    let name = this.name;
    if (!this.name) {
      if (path) {
        name = (path as Path).step.name;
      } else {
        name = this.objectType.name;
      }
    }

    return {
      id: this.id,
      include,
      name,
      path,
    };
  }
}

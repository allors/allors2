import { ObjectType } from "../../meta";
import { Path } from "./Path";
import { Tree } from "./Tree";
import { TreeNode } from "./TreeNode";

export class Result {
  public objectType: ObjectType;

  public name: string;

  public skip: number;

  public take: number;

  public path: Path | any;

  public include: Tree | any;

  constructor(fields?: Partial<Result>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    let path = this.path;
    if (this.path && !this.path.step) {
      path = Object.keys(this.path)
        .map((roleName) => {
          const rolePath = new Path();
          rolePath.parse(this.path, this.objectType, roleName);
          return rolePath;
        })[0];
    }

    let objectType = this.objectType;

    let include = this.include;
    if (this.include && !(this.include instanceof Tree)) {
      include = Object.keys(this.include)
        .map((roleName) => {
          const treeNode = new TreeNode();
          treeNode.parse(this.include, objectType, roleName);
          return treeNode;
        });
    }

    return {
      name: this.name,
      skip: this.skip,
      take: this.take,
      path,
      include,
    };
  }
}

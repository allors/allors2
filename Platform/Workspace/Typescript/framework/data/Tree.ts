import { ObjectType } from "../meta";
import { Node } from "./Node";

export interface TreeArgs {
  objectType: ObjectType;

  nodes: Node[] | any;
}

export class Tree {
  public objectType: ObjectType;

  public nodes: Node[] | any;

  constructor(args: TreeArgs);
  constructor(objectType: ObjectType, literal?: {});
  constructor(args: TreeArgs | ObjectType, literal?: {}) {
    if (args instanceof ObjectType) {
      this.objectType = args;

      if (literal) {
        this.nodes = Object.keys(literal).map(
          (propertyName) =>
            new Node(literal, this.objectType as ObjectType, propertyName)
        );
      }
    } else {
      this.objectType = args.objectType;
      this.nodes = args.nodes;
    }
  }

  public toJSON(): any {
    let nodes = this.nodes;
    if (this.nodes && !(this.nodes instanceof Array)) {
      nodes = Object.keys(this.nodes).map(
        (propertyName) => new Node(this.nodes, this.objectType, propertyName)
      );
    }

    return nodes;
  }
}

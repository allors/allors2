import { Path } from "./Path";
import { Tree } from "./Tree";

export class Fetch {

  public path: Path;

  public include: Tree;

  constructor(fields?: Partial<Fetch>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      path: this.path,
      include: this.include,
    };
  }
}

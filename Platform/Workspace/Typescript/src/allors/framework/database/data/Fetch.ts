import { Path } from "./Path";
import { Tree } from "./Tree";

export class Fetch {

  public name: string;

  public skip: number;

  public take: number;

  public path: Path;

  public include: Tree;

  constructor(fields?: Partial<Fetch>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      name: this.name,
      skip: this.skip,
      take: this.take,
      path: this.path,
      include: this.include,
    };
  }
}

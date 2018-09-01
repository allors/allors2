import { Path } from './Path';
import { Tree } from './Tree';

export class Result {
  public name: string;

  public skip: number;

  public take: number;

  public path: Path | any;

  public include: Tree;

  constructor(fields?: Partial<Result>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {

    return {
      name: this.name,
      skip: this.skip,
      take: this.take,
      path: this.path,
      include: this.include
    };
  }
}

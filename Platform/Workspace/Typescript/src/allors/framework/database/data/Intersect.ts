import { Sort } from './Sort';
import { Extent } from './Extent';

export class Intersect {
  public operands: Extent[];

  public sort: Sort[];

  constructor(fields?: Partial<Intersect>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: "Intersect",
      operands: this.operands,
    };
  }
}

import { Sort } from './Sort';
import { Extent } from './Extent';

export class Union {

  public operands: Extent[];

  public sort: Sort[];

  constructor(fields?: Partial<Union>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: "Union",
      operands: this.operands,
    };
  }
}

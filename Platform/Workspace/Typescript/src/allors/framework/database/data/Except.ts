import { Sort } from './Sort';
import { Extent } from './Extent';

export class Except {
  public operands: Extent[];

  public sort: Sort[];

  constructor(fields?: Partial<Except>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: "Except",
      operands: this.operands,
    };
  }
}

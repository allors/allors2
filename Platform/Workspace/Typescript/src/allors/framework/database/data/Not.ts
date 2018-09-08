import { Predicate } from './Predicate';

export class Not implements Predicate {
  public operand: Predicate;

  constructor(fields?: Partial<Not>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Not',
      operand: this.operand,
    };
  }
}

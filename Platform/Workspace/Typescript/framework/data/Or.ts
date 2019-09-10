import { Predicate } from './Predicate';

export class Or implements Predicate {
  public operands: Predicate[];

  constructor(fields?: Partial<Or> | Predicate[]) {
    if (fields instanceof Array) {
      this.operands = fields;
    } else {
      Object.assign(this, fields);
      this.operands = this.operands ? this.operands : [];
    }
  }

  public toJSON(): any {
    return {
      kind: 'Or',
      operands: this.operands,
    };
  }
}

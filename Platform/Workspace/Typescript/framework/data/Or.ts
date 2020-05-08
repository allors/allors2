import { Predicate } from './Predicate';

export class Or implements Predicate {
  dependencies: string[];
  operands: Predicate[];

  constructor(fields?: Partial<Or> | Predicate[]) {
    if (fields instanceof Array) {
      this.operands = fields;
    } else {
      Object.assign(this, fields);
      this.operands = this.operands ? this.operands : [];
    }
  }

  toJSON(): any {
    return {
      kind: 'Or',
      dependencies: this.dependencies,
      operands: this.operands,
    };
  }
}

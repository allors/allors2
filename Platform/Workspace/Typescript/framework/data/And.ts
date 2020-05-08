import { Predicate } from './Predicate';

export class And implements Predicate {
  dependencies: string[];
  operands: Predicate[];

  constructor(fields?: Partial<And> | Predicate[]) {
    if (fields instanceof Array) {
      this.operands = fields;
    } else {
      Object.assign(this, fields);
      this.operands = this.operands ? this.operands : [];
    }
  }

  toJSON(): any {
    return {
      kind: 'And',
      dependencies: this.dependencies,
      operands: this.operands,
    };
  }
}

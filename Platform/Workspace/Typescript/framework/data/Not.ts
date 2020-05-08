import { Predicate } from './Predicate';

export class Not implements Predicate {
  dependencies: string[];
  operand: Predicate;

  constructor(fields?: Partial<Not>) {
    Object.assign(this, fields);
  }

  toJSON(): any {
    return {
      kind: 'Not',
      dependencies: this.dependencies,
      operand: this.operand,
    };
  }
}

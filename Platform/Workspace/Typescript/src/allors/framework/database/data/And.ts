import { Predicate } from './Predicate';

export class And implements Predicate {
  public operands: Predicate[];

  constructor(fields?: Partial<And>) {
    Object.assign(this, fields);
    this.operands = this.operands ? this.operands : [];
  }

  public toJSON(): any {
    return {
      kind: 'And',
      operands: this.operands,
    };
  }
}

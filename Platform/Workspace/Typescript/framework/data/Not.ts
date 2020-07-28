import { Predicate, PredicateArgs } from './Predicate';

export interface NotArgs extends PredicateArgs, Pick<Not, 'operand'> {}

export class Not extends Predicate {
  operand?: Predicate;

  constructor(args: NotArgs);
  constructor(operand?: Predicate);
  constructor(args?: NotArgs | Predicate) {
    super();

    if (args instanceof Predicate) {
      this.operand = args;
    } else {
      Object.assign(this, args);
      this.operand = args?.operand;
    }
  }
  toJSON(): any {
    return {
      kind: 'Not',
      dependencies: this.dependencies,
      operand: this.operand,
    };
  }
}

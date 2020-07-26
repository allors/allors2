import { Predicate, PredicateArgs } from './Predicate';

export interface OrArgs extends PredicateArgs, Pick<Or, 'operands'> {}

export class Or extends Predicate {
  operands: Predicate[];

  constructor(args: OrArgs);
  constructor(...operands: Predicate[]);
  constructor(operands: Predicate[]);
  constructor(args: any) {
    super();

    if (args instanceof Array) {
      this.operands = args;
    } else if (args) {
      Object.assign(this, args);
      this.operands = args.operands;
    } else{
      this.operands = [];
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

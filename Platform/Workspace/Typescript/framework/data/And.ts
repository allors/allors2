import { Predicate, PredicateArgs } from "./Predicate";

export interface AndArgs extends PredicateArgs, Pick<And, "operands"> {}

export class And extends Predicate {
  operands: Predicate[];

  constructor(args: AndArgs);
  constructor(...operands: Predicate[]);
  constructor(operands: Predicate[]);
  constructor(args: any) {
    super();

    if (args instanceof Array) {
      this.operands = args;
    } else {
      Object.assign(this, args);
      this.operands = args.operands;
    }
  }

  toJSON(): any {
    return {
      kind: "And",
      dependencies: this.dependencies,
      operands: this.operands,
    };
  }
}

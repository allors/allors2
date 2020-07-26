import { Sort } from './Sort';
import { Extent } from './Extent';
import { ObjectType } from '../meta';

export type OperatorArgs = Pick<Operator, 'operands' | 'sort'>;

export abstract class Operator {
  public operands?: Extent[];

  public sort?: Sort[];

  public get objectType(): ObjectType | undefined {
    // TODO: get least specific type of all operands
    return this.operands && this.operands.length > 0 ? this.operands[0].objectType : undefined;
  }

  constructor(...operands: Extent[]);
  constructor(operands: Extent[]);
  constructor(args?: OperatorArgs);
  constructor(args: any) {
    if (args instanceof Array) {
      this.operands = args;
    } else if (args) {
      Object.assign(this, args);
      this.operands = args.operands;
    }
  }
}

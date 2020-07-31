import { Sort } from './Sort';
import { IExtent } from './IExtent';
import { ObjectType } from '../meta/ObjectType';

export type OperatorArgs = Pick<Operator, 'operands' | 'sort'>;

export abstract class Operator {
  public operands?: IExtent[];

  public sort?: Sort[];

  public get objectType(): ObjectType | undefined {
    // TODO: get least specific type of all operands
    return this.operands && this.operands.length > 0 ? this.operands[0].objectType : undefined;
  }

  constructor(...operands: IExtent[]);
  constructor(operands: IExtent[]);
  constructor(args?: OperatorArgs);
  constructor(args: any) {
    if (args instanceof Array) {
      this.operands = args;
    } else {
      Object.assign(this, args);
      this.operands = args.operands;
    }
  }
}

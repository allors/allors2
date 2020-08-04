import { IExtent } from './IExtent';
import { Operator, OperatorArgs } from './Operator';

export class Except extends Operator {
  constructor(...operands: IExtent[]);
  constructor(operands: IExtent[]);
  constructor(args?: OperatorArgs);
  constructor(args: any) {
    super(args);
  }

  public toJSON(): any {
    return {
      kind: 'Except',
      operands: this.operands,
      sorting: this.sort,
    };
  }
}

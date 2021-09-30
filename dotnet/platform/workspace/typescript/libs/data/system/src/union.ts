import { Operator, OperatorArgs } from './Operator';
import { IExtent } from './IExtent';

export class Union extends Operator {
  constructor(...operands: IExtent[]);
  constructor(operands: IExtent[]);
  constructor(args?: OperatorArgs);
  constructor(args: any) {
    super(args);
  }

  public toJSON(): any {
    return {
      kind: 'Union',
      operands: this.operands,
      sorting: this.sort,
    };
  }
}

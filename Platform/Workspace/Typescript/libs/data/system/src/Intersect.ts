import { IExtent } from './IExtent';
import { Operator, OperatorArgs } from './Operator';

export class Intersect extends Operator {
  constructor(...operands: IExtent[]);
  constructor(operands: IExtent[]);
  constructor(args?: OperatorArgs);
  constructor(args: any) {
    super(args);
  }

  public toJSON(): any {
    return {
      kind: 'Intersect',
      operands: this.operands,
      sorting: this.sort,
    };
  }
}

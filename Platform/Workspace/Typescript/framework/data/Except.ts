import { Extent } from './Extent';
import { Operator, OperatorArgs } from './Operator';

export class Except extends Operator {
  constructor(...operands: Extent[]);
  constructor(operands: Extent[]);
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

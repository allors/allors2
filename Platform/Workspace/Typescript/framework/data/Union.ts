import { Operator, OperatorArgs } from './Operator';
import { Extent } from './Extent';

export class Union extends Operator {
  constructor(...operands: Extent[]);
  constructor(operands: Extent[]);
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

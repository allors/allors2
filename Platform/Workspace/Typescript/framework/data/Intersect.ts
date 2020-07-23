import { Extent } from './Extent';
import { Operator, OperatorArgs } from './Operator';

export class Intersect extends Operator {
  constructor(...operands: Extent[]);
  constructor(operands: Extent[]);
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

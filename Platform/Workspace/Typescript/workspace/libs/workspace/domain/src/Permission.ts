import { ObjectType, OperandType } from '@allors/workspace/meta';
import { Operations } from '@allors/workspace/protocol';

export class Permission {
  constructor(
    public id: string,
    public objectType: ObjectType,
    public operandType: OperandType,
    public operation: Operations
  ) {}
}

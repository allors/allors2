import { ObjectType, OperandType } from '@allors/meta/system';
import { Operations } from '@allors/protocol/system';

export class Permission {
  constructor(
    public id: string,
    public objectType: ObjectType,
    public operandType: OperandType,
    public operation: Operations
  ) {}
}

import { OperandType } from '../meta/OperandType';
import { ObjectType } from '../meta/ObjectType';
import { Operations } from '../protocol/Operations';

export class Permission {
  constructor(public id: string, public objectType: ObjectType, public operandType: OperandType, public operation: Operations) {}
}

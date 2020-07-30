import { OperandType } from '@allors/meta/OperandType';
import { ObjectType } from '@allors/meta/ObjectType';
import { Operations } from '../protocol/Operations';

export class Permission {
  constructor(public id: string, public objectType: ObjectType, public operandType: OperandType, public operation: Operations) {}
}

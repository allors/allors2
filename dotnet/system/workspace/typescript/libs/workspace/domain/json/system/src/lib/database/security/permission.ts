import { Operations } from "@allors/workspace/domain/system";
import { Class, OperandType } from "@allors/workspace/meta/system";

export class Permission {
  constructor(public id: number, public cls: Class, public operandType: OperandType, public operation: Operations) {}
}

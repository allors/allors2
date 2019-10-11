namespace Allors {
  import ObjectType = Meta.ObjectType;
  import OperandType = Meta.OperandType;
  import Operations = Protocol.Operations;

  export class Permission {
        constructor(public id: string, public objectType: ObjectType, public operandType: OperandType, public operation: Operations) {
        }
    }
}

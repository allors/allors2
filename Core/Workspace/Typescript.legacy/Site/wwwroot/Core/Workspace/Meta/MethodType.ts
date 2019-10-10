namespace Allors.Meta {
  export class MethodType implements OperandType {
    id: string;
    objectType: ObjectType;
    name: string;

    constructor(public metaPopulation: MetaPopulation) {
    }
  }
}

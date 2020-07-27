import { OperandType } from "./OperandType";
import { MetaPopulation } from "./MetaPopulation";
import { ObjectType } from "./ObjectType";

export class MethodType implements OperandType {
  constructor(
    public metaPopulation: MetaPopulation,
    public id: string,
    public objectType: ObjectType,
    public name: string
  ) {}
}

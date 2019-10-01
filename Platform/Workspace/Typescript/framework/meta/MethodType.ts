import { OperandType } from './OperandType';
import { MetaPopulation } from './MetaPopulation';
import { ObjectType } from './ObjectType';

export class MethodType implements OperandType {
    id: string;
    objectType: ObjectType;
    name: string;

    constructor(public metaPopulation: MetaPopulation) {
    }
}

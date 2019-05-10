import { MetaObject } from './MetaObject';
import { MetaPopulation } from './MetaPopulation';
import { ObjectType } from './ObjectType';

export class MethodType implements MetaObject {
    id: string;
    objectType: ObjectType;
    name: string;

    constructor(public metaPopulation: MetaPopulation) {
    }
}

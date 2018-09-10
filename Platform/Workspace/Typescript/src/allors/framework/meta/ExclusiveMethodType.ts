import { MetaPopulation } from './MetaPopulation';
import { MethodType } from './MethodType';

export class ExclusiveMethodType implements MethodType {
    public id: string;
    public name: string;

    constructor(public metaPopulation: MetaPopulation) {
    }
}

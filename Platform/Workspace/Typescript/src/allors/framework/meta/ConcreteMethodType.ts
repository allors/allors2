import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { MetaObject } from "./MetaObject";
import { MethodType } from "./MethodType";
import { MetaPopulation } from "./MetaPopulation";

export class ConcreteMethodType implements MethodType {

    // Proxy
    public methodType: ExclusiveMethodType;

    constructor(public metaPopulation: MetaPopulation) {
    }

    public get id(): string { return this.methodType.id; }

    public get name(): string { return this.methodType.name; }
}

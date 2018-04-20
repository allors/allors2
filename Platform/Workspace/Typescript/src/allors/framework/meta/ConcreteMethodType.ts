import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { MetaObject } from "./MetaObject";
import { MethodType } from "./MethodType";

export class ConcreteMethodType implements MethodType {

    // Proxy
    public methodType: ExclusiveMethodType;

    public get id(): string{ return this.methodType.id; }

    public get name(): string{ return this.methodType.name; }
}

import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { MetaObject } from "./MetaObject";
import { MethodType } from "./MethodType";

export class ConcreteMethodType implements MethodType {
    public methodType: ExclusiveMethodType;

    public id: string;
    public name: string;
}

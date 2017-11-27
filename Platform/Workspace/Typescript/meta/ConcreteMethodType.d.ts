import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { MethodType } from "./MethodType";
export declare class ConcreteMethodType implements MethodType {
    methodType: ExclusiveMethodType;
    id: string;
    name: string;
}

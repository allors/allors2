import { MetaObject } from './MetaObject';

export interface MethodType extends MetaObject {
    id: string;
    name: string;
}

export class ExclusiveMethodType implements MethodType {
    id: string;
    name: string;
}

export class ConcreteMethodType implements MethodType {

    methodType: ExclusiveMethodType;

    id: string;
    name: string;
}

import { RoleType } from './RoleType';
import { MethodType } from './MethodType';

export enum Kind {
    unit,
    class,
    interface
}

export class ObjectType {
    name: string;
    kind: Kind;
    interfaceByName: { [name: string]: ObjectType; } = {};
    roleTypeByName: { [name: string]: RoleType; } = {};
    methodTypeByName: { [name: string]: MethodType; } = {};

    get isUnit(): boolean{
        return this.kind === Kind.unit;
    }

    get isComposite(): boolean{
        return this.kind !== Kind.unit;
    }

    get isInterface(): boolean{
        return this.kind === Kind.interface;
    }

    get isClass(): boolean{
        return this.kind === Kind.class;
    }
}

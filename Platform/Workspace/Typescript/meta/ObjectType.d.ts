import { AssociationType } from "./AssociationType";
import { ConcreteMethodType } from "./ConcreteMethodType";
import { ConcreteRoleType } from "./ConcreteRoleType";
import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { ExclusiveRoleType } from "./ExclusiveRoleType";
import { MetaObject } from "./MetaObject";
import { MethodType } from "./MethodType";
import { RoleType } from "./RoleType";
export declare enum Kind {
    unit = 0,
    class = 1,
    interface = 2,
}
export declare class ObjectType implements MetaObject {
    id: string;
    name: string;
    kind: Kind;
    interfaceByName: {
        [name: string]: ObjectType;
    };
    roleTypeByName: {
        [name: string]: RoleType;
    };
    exclusiveRoleTypes: ExclusiveRoleType[];
    concreteRoleTypes: ConcreteRoleType[];
    associationTypes: AssociationType[];
    methodTypeByName: {
        [name: string]: MethodType;
    };
    exclusiveMethodTypes: ExclusiveMethodType[];
    concreteMethodTypes: ConcreteMethodType[];
    readonly isUnit: boolean;
    readonly isComposite: boolean;
    readonly isInterface: boolean;
    readonly isClass: boolean;
    derive(): void;
    private addInterfaces(interfaces);
}

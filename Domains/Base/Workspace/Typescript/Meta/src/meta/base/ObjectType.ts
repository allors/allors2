import { RoleType } from "./RoleType";
import { MethodType } from "./MethodType";

export enum Kind {
    unit,
    class,
    interface
}

export class ObjectType {
    name: string;
    kind: Kind;
    interfaces?: ObjectType[];
    roleTypes?: RoleType[];
    methodTypes?: MethodType[];
}
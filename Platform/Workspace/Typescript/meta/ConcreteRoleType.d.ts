import { ExclusiveRoleType } from "./ExclusiveRoleType";
import { ObjectType } from "./ObjectType";
import { RoleType } from "./RoleType";
export declare class ConcreteRoleType implements RoleType {
    roleType: ExclusiveRoleType;
    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isRequired: boolean;
    readonly isMany: boolean;
}

import { ObjectType } from "./ObjectType";
import { RoleType } from "./RoleType";
export declare class ExclusiveRoleType implements RoleType {
    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isRequired: boolean;
    readonly isMany: boolean;
}

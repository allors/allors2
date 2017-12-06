import { MetaObject } from "./MetaObject";
import { ObjectType } from "./ObjectType";

export interface RoleType extends MetaObject {
    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isMany: boolean;
    isRequired: boolean;
}

import { MetaObject } from "./MetaObject";
import { ObjectType } from "./ObjectType";
import { RoleType } from "./RoleType";

export class ExclusiveRoleType implements RoleType {
    public id: string;
    public name: string;
    public objectType: ObjectType;
    public isOne: boolean;
    public isRequired: boolean;

    get isMany(): boolean { return !this.isOne; }
}

import { ExclusiveRoleType } from "./ExclusiveRoleType";
import { MetaObject } from "./MetaObject";
import { ObjectType } from "./ObjectType";
import { RoleType } from "./RoleType";

export class ConcreteRoleType implements RoleType {

    public roleType: ExclusiveRoleType;

    public id: string;
    public name: string;
    public objectType: ObjectType;
    public isOne: boolean;
    public isRequired: boolean;

    public get isMany(): boolean { return !this.isOne; }
}

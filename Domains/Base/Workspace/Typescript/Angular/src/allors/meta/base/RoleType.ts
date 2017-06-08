import { MetaObject } from './MetaObject';
import { ObjectType } from './ObjectType';

export interface RoleType extends MetaObject {
    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isMany: boolean;
    isRequired: boolean;
}

export class ExclusiveRoleType implements RoleType {
    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isRequired: boolean;

    get isMany(): boolean { return !this.isOne; };
}

export class ConcreteRoleType implements RoleType {

    roleType: ExclusiveRoleType;

    id: string;
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isRequired: boolean;

    get isMany(): boolean { return !this.isOne; };
}

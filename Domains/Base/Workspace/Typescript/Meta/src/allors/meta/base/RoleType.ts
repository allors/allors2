import { ObjectType } from "./ObjectType";

export class RoleType {
    name: string;
    objectType: ObjectType;
    isOne: boolean;
    isRequired: boolean;

    get isMany(): boolean { return !this.isOne; };
}
import { MetaObject } from "./MetaObject";
import { MetaPopulation } from "./MetaPopulation";
import { ObjectType } from "./ObjectType";
import { RoleType } from "./RoleType";

export class ExclusiveRoleType implements RoleType {
    public id: string;
    public name: string;
    public singular: string;
    public objectType: ObjectType;
    public isOne: boolean;
    public isDerived: boolean;
    public isRequired: boolean;

    constructor(public metaPopulation: MetaPopulation) {
    }

    get isMany(): boolean { return !this.isOne; }
}

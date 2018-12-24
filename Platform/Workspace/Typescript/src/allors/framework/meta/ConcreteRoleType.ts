import { MetaPopulation } from './MetaPopulation';
import { ObjectType } from './ObjectType';
import { RoleType } from './RoleType';
import { RelationType } from './RelationType';

export class ConcreteRoleType implements RoleType {

    // override
    public isRequired: boolean;

    // proxy subject
    public relationType: RelationType;
    public roleType: RoleType;

    constructor(public metaPopulation: MetaPopulation) {
    }

    public get id(): string { return this.roleType.id; }
    
    public get objectType(): ObjectType { return this.roleType.objectType; }

    public get name(): string { return this.roleType.name; }

    public get singular(): string { return this.roleType.singular; }

    public get isOne(): boolean { return this.roleType.isOne; }

    public get isMany(): boolean { return this.roleType.isMany; }

    public get isDerived(): boolean { return this.roleType.isDerived; }
}

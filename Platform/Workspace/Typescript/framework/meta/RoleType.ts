import { PropertyType } from './PropertyType';
import { ObjectType } from './ObjectType';
import { RelationType } from './RelationType';
import { MetaPopulation } from './MetaPopulation';

export class RoleTypeVirtual {
    isRequired?: boolean;
}

export class RoleType implements PropertyType {
    metaPopulation: MetaPopulation;

    overridesByClass: Map<ObjectType, RoleTypeVirtual>;

    id: string;
    objectType: ObjectType;
    name: string;
    singular: string;
    plural: string;
    isOne: boolean;
    isDerived: boolean;
    mediaType?: string;

    isRequired(objectType: ObjectType): boolean {
        const override = objectType ? this.overridesByClass.get(objectType) : null;
        return override?.isRequired ?? this.virtual.isRequired ?? false;
    }

    constructor(public relationType: RelationType, private virtual: RoleTypeVirtual) {
        relationType.roleType = this;
        this.metaPopulation = relationType.metaPopulation;
        this.overridesByClass = new Map();
    }

    get isMany(): boolean {
        return !this.isOne;
    }
}

import { PropertyType } from './PropertyType';
import { ObjectType } from './ObjectType';
import { RelationType } from './RelationType';
import { MetaPopulation } from './MetaPopulation';
import { RoleTypeData } from './Data';

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
  mediaType?: string;

  private virtual: RoleTypeVirtual;

  isRequired(objectType: ObjectType): boolean {
    const override = objectType ? this.overridesByClass.get(objectType) : null;
    return override?.isRequired ?? this.virtual.isRequired ?? false;
  }

  constructor(public relationType: RelationType, dataRoleType: RoleTypeData) {
    relationType.roleType = this;
    this.metaPopulation = relationType.metaPopulation;
    this.overridesByClass = new Map();

    this.virtual = new RoleTypeVirtual();
    this.virtual.isRequired = dataRoleType.isRequired;

    this.id = dataRoleType.id;
    this.objectType = this.metaPopulation.metaObjectById.get(
      dataRoleType.objectTypeId
    ) as ObjectType;
    this.singular = dataRoleType.singular;
    this.plural = dataRoleType.plural;
    this.isOne = dataRoleType.isOne;
    this.name = this.isOne ? this.singular : this.plural;
    this.mediaType = dataRoleType.mediaType;
  }

  get isMany(): boolean {
    return !this.isOne;
  }
}

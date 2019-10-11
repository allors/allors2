namespace Allors.Meta {
  export class RoleTypeVirtual {
    isRequired: boolean;
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

    isRequired(objectType: ObjectType) {
      const override = this.overridesByClass.get(objectType);
      return override ? override.isRequired : this.virtual.isRequired;
    }

    constructor(public relationType: RelationType, private virtual: RoleTypeVirtual) {
      relationType.roleType = this;
      this.metaPopulation = relationType.metaPopulation;
      this.overridesByClass = new Map();
    }

    get isMany(): boolean { return !this.isOne; }
  }
}

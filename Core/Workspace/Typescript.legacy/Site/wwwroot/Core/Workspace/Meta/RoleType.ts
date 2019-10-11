namespace Allors.Meta {
  export class RoleType implements PropertyType {
    metaPopulation: MetaPopulation;

    id: string;
    objectType: ObjectType;
    name: string;
    singular: string;
    plural: string;
    isOne: boolean;
    isDerived: boolean;
    isRequired: boolean;

    constructor(public relationType: RelationType) {
      this.metaPopulation = relationType.metaPopulation;
    }

    get isMany(): boolean { return !this.isOne; }
  }
}
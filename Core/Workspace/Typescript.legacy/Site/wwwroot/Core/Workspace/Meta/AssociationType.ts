namespace Allors.Meta {
  export class AssociationType implements PropertyType {
    metaPopulation: MetaPopulation;

    id: string;
    objectType: ObjectType;
    name: string;
    singular: string;
    isOne: boolean;

    constructor(public relationType: RelationType) {
      this.metaPopulation = relationType.metaPopulation;
    }

    get isMany(): boolean { return !this.isOne; }
  }
}

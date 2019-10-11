namespace Allors.Meta {
  export class RelationType {
    id: string;
    associationType: AssociationType;
    roleType: RoleType;

    constructor(public metaPopulation: MetaPopulation) {
      this.metaPopulation = metaPopulation;
    }
  }
}

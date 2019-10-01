import { MetaPopulation } from './MetaPopulation';
import { AssociationType } from './AssociationType';
import { RoleType } from './RoleType';

export class RelationType {
  id: string;
  associationType: AssociationType;
  roleType: RoleType;
  concreteRoleTypeByClassId: { [id: string]: RoleType; } = {};

  constructor(public metaPopulation: MetaPopulation) {
    this.metaPopulation = metaPopulation;

    this.associationType = new AssociationType(this);
    this.roleType = new RoleType(this);
  }
}

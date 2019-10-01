import { MetaPopulation } from './MetaPopulation';
import { AssociationType } from './AssociationType';
import { RoleType } from './RoleType';
import { ObjectType } from './ObjectType';

export class RelationType {
  id: string;
  associationType: AssociationType;
  roleType: RoleType;
  concreteRoleTypeByClass: Map<ObjectType, RoleType>;

  constructor(public metaPopulation: MetaPopulation) {
    this.metaPopulation = metaPopulation;
    this.associationType = new AssociationType(this);
    this.roleType = new RoleType(this);
    this.concreteRoleTypeByClass = new Map();
  }
}

import { MetaPopulation } from './MetaPopulation';
import { AssociationType } from './AssociationType';
import { RoleType } from './RoleType';

export class RelationType {
  id: string;
  associationType: AssociationType;
  roleType: RoleType;

  constructor(public metaPopulation: MetaPopulation) {
    this.metaPopulation = metaPopulation;
  }
}

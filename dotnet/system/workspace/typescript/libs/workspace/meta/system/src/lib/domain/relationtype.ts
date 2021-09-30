import { Multiplicity } from '../Multiplicity';
import { AssociationType } from './AssociationType';
import { MetaObject } from './MetaObject';
import { RoleType } from './RoleType';

export interface RelationType extends MetaObject {
  associationType: AssociationType;

  roleType: RoleType;

  multiplicity: Multiplicity;

  isDerived: boolean;
}

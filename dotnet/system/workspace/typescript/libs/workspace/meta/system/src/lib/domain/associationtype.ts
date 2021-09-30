import { PropertyType } from './PropertyType';
import { RelationType } from './RelationType';
import { RoleType } from './RoleType';

export interface AssociationType extends PropertyType {
  relationType: RelationType;

  roleType: RoleType;
}

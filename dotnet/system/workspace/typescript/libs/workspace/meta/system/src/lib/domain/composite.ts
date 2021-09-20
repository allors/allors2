import { AssociationType } from './AssociationType';
import { Class } from './Class';
import { Interface } from './Interface';
import { MethodType } from './MethodType';
import { ObjectType } from './ObjectType';
import { RoleType } from './RoleType';

export interface Composite extends ObjectType {
  directSupertypes: Set<Interface>;
  directAssociationTypes: Set<AssociationType>;
  directRoleTypes: Set<RoleType>;
  directMethodTypes: Set<MethodType>;

  supertypes: Set<Interface>;
  classes: Set<Class>;
  associationTypes: Set<AssociationType>;
  roleTypes: Set<RoleType>;
  methodTypes: Set<MethodType>;

  isAssignableFrom(objectType: Composite): boolean;
}

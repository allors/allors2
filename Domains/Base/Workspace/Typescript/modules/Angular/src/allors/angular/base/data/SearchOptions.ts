import { And,  ObjectType,  RoleType, Predicate } from '../../../framework';

export interface SearchOptions {
  objectType: ObjectType;
  roleTypes: RoleType[];
  predicates?: Predicate[];
  post?: (and: And) => void;
}

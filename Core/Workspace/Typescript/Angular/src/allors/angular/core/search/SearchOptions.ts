import { And,  ObjectType,  RoleType, Predicate, Tree } from '../../../framework';

export interface SearchOptions {
  objectType: ObjectType;
  roleTypes: RoleType[];
  predicates?: Predicate[];
  post?: (and: And) => void;
  include?: Tree | any;
}

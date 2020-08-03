import { ObjectType, RoleType } from '@allors/workspace/meta';
import { And, Predicate, Tree } from '@allors/workspace/data';

export interface SearchOptions {
  objectType: ObjectType;
  roleTypes: RoleType[];
  predicates?: Predicate[];
  post?: (and: And) => void;
  include?: Tree | any;
}

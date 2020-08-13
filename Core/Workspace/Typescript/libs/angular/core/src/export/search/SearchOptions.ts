import { ObjectType, RoleType } from '@allors/meta/system';
import { And, Predicate, Tree } from '@allors/data/system';

export interface SearchOptions {
  objectType: ObjectType;
  roleTypes: RoleType[];
  predicates?: Predicate[];
  post?: (and: And) => void;
  include?: Tree | any;
}

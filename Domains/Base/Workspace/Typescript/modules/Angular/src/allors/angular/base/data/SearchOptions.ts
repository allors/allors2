import { And,  ObjectType,  RoleType, Sort } from '../../../framework';

export interface SearchOptions {
  objectType: ObjectType;
  roleTypes: RoleType[];
  existRoletypes?: RoleType[];
  notExistRoletypes?: RoleType[];
  post?: (and: And) => void;
}

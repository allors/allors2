import { ObjectType, RoleType, ISessionObject } from '../../../../framework';

export interface CreateData {
  objectType?: ObjectType;

  associationId?: string;
  associationObjectType?: ObjectType;
  associationRoleType?: RoleType;

  onCreate?: (object: ISessionObject) => void;
}

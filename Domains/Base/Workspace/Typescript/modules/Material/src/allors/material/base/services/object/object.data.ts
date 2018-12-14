import { ObjectType, RoleType, ISessionObject } from '../../../../framework';

export interface CreateData {
  objectType?: ObjectType;

  associationId?: string;
  associationObjectType?: ObjectType;
  associationRoleType?: RoleType;

  onCreate?: (object: ISessionObject) => void;
}

export interface EditData {
  objectType?: ObjectType;
  id: string;
}

export interface ObjectData {
  objectType?: ObjectType;
  id: string;
}

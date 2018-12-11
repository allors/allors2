import { ObjectType, ISessionObject, RoleType } from '../../../framework';

export interface CreateData {
  objectType?: ObjectType;

  associationId?: string;
  associationObjectType?: ObjectType;
  associationRoleType?: RoleType;

  onCreate?: (object: ISessionObject) => void;
}

export interface EditData {
  id: string;
}

export interface ObjectData {
  id: string;
  objectType?: ObjectType;
}

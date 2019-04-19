import { ObjectType, RoleType, ISessionObject, IObject } from '../../../../framework';

export interface ObjectData extends Partial<IObject> {

  associationId?: string;
  associationObjectType?: ObjectType;
  associationRoleType?: RoleType;

  onCreate?: (object: ISessionObject) => void;
}

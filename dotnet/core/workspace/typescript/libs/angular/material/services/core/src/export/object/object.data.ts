import { IObject, ISessionObject } from '@allors/domain/system';
import { RoleType, ObjectType } from '@allors/meta/system';

export interface ObjectData extends Partial<IObject> {

  associationId?: string;
  associationObjectType?: ObjectType;
  associationRoleType?: RoleType;

  onCreate?: (object: ISessionObject) => void;
}

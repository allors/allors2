import { ObjectType, AssociationType, RoleType, MethodType, OperandType } from '@allors/meta/system';
import { Operations,PushRequestObject, PushRequestNewObject } from '@allors/protocol/system';

import { ISession } from './Session';
import { IWorkspaceObject } from './WorkspaceObject';

export interface IObject {
  id: string;
  objectType: ObjectType;
}

export interface ISessionObject extends IObject {
  objectType: ObjectType;
  id: string;
  newId?: string;
  version?: string;

  isNew: boolean;

  session: ISession;
  workspaceObject?: IWorkspaceObject;

  hasChanges: boolean;
  hasChangedRole(roleType: RoleType);
  restoreRole(roleType: RoleType);

  canRead(roleType: RoleType): boolean | undefined;
  canWrite(roleTyp: RoleType): boolean | undefined;
  canExecute(methodType: MethodType): boolean | undefined;
  isPermited(operandType: OperandType, operation: Operations): boolean | undefined;

  get(roleType: RoleType): any;
  set(roleType: RoleType, value: any): void;
  add(roleType: RoleType, value: any): void;
  remove(roleType: RoleType, value: any): void;

  getAssociation(associationType: AssociationType): any;

  save(): PushRequestObject | undefined;
  saveNew(): PushRequestNewObject;
  reset(): void;
}

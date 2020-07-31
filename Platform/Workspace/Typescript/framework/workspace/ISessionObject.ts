import { ObjectType } from '../meta/ObjectType';
import { ISession } from './Session';
import { IWorkspaceObject } from './WorkspaceObject';
import { RoleType } from '../meta/RoleType';
import { MethodType } from '../meta/MethodType';
import { OperandType } from '../meta/OperandType';
import { Operations } from '../protocol/Operations';
import { AssociationType } from '../meta/AssociationType';
import { PushRequestObject } from '../protocol/push/PushRequestObject';
import { PushRequestNewObject } from '../protocol/push/PushRequestNewObject';

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

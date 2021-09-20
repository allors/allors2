import { Class, RelationType, AssociationType,  RoleType, MethodType} from "@allors/workspace/meta/system";
import { IObject } from './IObject';
import { ISession } from './ISession';
import { UnitTypes } from './Types';

export interface IStrategy {
  object: IObject;

  class: Class;

  id: number;

  session: ISession;

  diff(): RelationType[];

  canRead(roleType: RoleType): boolean;

  canWrite(roleType: RoleType): boolean;

  canExecute(methodType: MethodType): boolean;

  exist(roleType: RoleType): boolean;

  get(roleType: RoleType): unknown;

  getUnit(roleType: RoleType): UnitTypes;

  getComposite<T>(roleType: RoleType): T;

  getComposites<T>(roleType: RoleType): T[];

  set(roleType: RoleType, value: unknown): void;

  setUnit(roleType: RoleType, value: UnitTypes): void;

  setComposite<T>(roleType: RoleType, value: T): void;

  setComposites<T>(roleType: RoleType, value: T[]): void;

  add<T>(roleType: RoleType, value: T): void;

  remove<T>(roleType: RoleType, value: T): void;

  removeAll(roleType: RoleType): void;

  getCompositeAssociation<T>(associationType: AssociationType): T;

  getCompositesAssociation<T>(associationType: AssociationType): T[];

  reset(): void;
}

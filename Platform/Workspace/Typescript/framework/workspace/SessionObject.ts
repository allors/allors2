import {
  ObjectType,
  OperandType,
  RoleType,
  AssociationType,
  MethodType,
} from "../meta";
import { ids } from "../../meta/generated/ids.g";
import {
  UnitTypes,
  CompositeTypes,
  ParameterTypes,
  isSessionObject,
} from "../workspace/Types";

import { PushRequestNewObject } from "./../protocol/push/PushRequestNewObject";
import { PushRequestObject } from "./../protocol/push/PushRequestObject";
import { PushRequestRole } from "./../protocol/push/PushRequestRole";

import { Method } from "./Method";
import { ISession, Session } from "./Session";
import { IWorkspaceObject } from "./WorkspaceObject";
import { Operations } from "../protocol/Operations";

export interface IObject {
  id: string;
  objectType: ObjectType;
}

export interface ISessionObject extends IObject {
  id: string;
  newId: string;
  version: string | null;
  objectType: ObjectType;

  isNew: boolean;

  session: ISession;
  workspaceObject?: IWorkspaceObject;

  hasChanges: boolean;

  canRead(roleType: RoleType): boolean | undefined;
  canWrite(roleTyp: RoleType): boolean | undefined;
  canExecute(methodType: MethodType): boolean | undefined;
  isPermited(
    operandType: OperandType,
    operation: Operations
  ): boolean | undefined;

  get(roleType: RoleType): any;
  set(roleType: RoleType, value: any): void;
  add(roleType: RoleType, value: any): void;
  remove(roleType: RoleType, value: any): void;

  getAssociation(associationType: AssociationType): any;

  save(): PushRequestObject | undefined;
  saveNew(): PushRequestNewObject;
  reset(): void;
}

export class SessionObject implements ISessionObject {
  public session: Session & ISession;
  public workspaceObject?: IWorkspaceObject;
  public objectType: ObjectType;
  public newId: string;
  private changedRoleByRoleType: Map<RoleType, any>;
  private roleByRoleType: Map<RoleType, any>;

  get isNew(): boolean {
    return this.newId ? true : false;
  }

  get hasChanges(): boolean {
    if (this.newId) {
      return true;
    }

    return !!this.changedRoleByRoleType;
  }

  get id(): string {
    return this.workspaceObject ? this.workspaceObject.id : this.newId;
  }

  get version(): string | null {
    return this.workspaceObject?.version ?? null;
  }

  public canRead(roleType: RoleType): boolean | undefined {
    return this.isPermited(roleType, Operations.Read);
  }

  public canWrite(roleType: RoleType): boolean | undefined {
    return this.isPermited(roleType, Operations.Write);
  }

  public canExecute(methodType: MethodType): boolean | undefined {
    return this.isPermited(methodType, Operations.Execute);
  }

  public isPermited(
    operandType: OperandType,
    operation: Operations
  ): boolean | undefined {
    if (this.roleByRoleType === undefined) {
      return undefined;
    }

    if (this.newId) {
      return true;
    } else if (this.workspaceObject) {
      const permission = this.session.workspace.permission(
        this.objectType,
        operandType,
        operation
      );
      return permission ? this.workspaceObject.isPermitted(permission) : false;
    }

    return false;
  }

  public method(methodType: MethodType): Method | undefined {
    if (this.roleByRoleType === undefined) {
      return undefined;
    }

    return new Method(this, methodType);
  }

  public get(roleType: RoleType): any {
    if (this.roleByRoleType === undefined) {
      return undefined;
    }

    let value = this.roleByRoleType.get(roleType);
    if (value === undefined) {
      if (this.newId === undefined) {
        if (roleType.objectType.isUnit) {
          value = this.workspaceObject?.roleByRoleTypeId.get(roleType.id);
          if (value === undefined) {
            value = null;
          }
        } else {
          try {
            if (roleType.isOne) {
              const role: string = this.workspaceObject?.roleByRoleTypeId.get(
                roleType.id
              );
              value = role ? this.session.get(role) : null;
            } else {
              const roles: string[] = this.workspaceObject?.roleByRoleTypeId.get(
                roleType.id
              );
              value = roles
                ? roles.map((role) => {
                    return this.session.get(role);
                  })
                : [];
            }
          } catch (e) {
            let stringValue = "N/A";
            try {
              stringValue = this.toString();
            } catch (e2) {
              throw new Error(
                `Could not get role ${roleType.name} from [objectType: ${this.objectType.name}, id: ${this.id}]`
              );
            }

            throw new Error(
              `Could not get role ${roleType.name} from [objectType: ${this.objectType.name}, id: ${this.id}, value: '${stringValue}']`
            );
          }
        }
      } else {
        if (roleType.objectType.isComposite && roleType.isMany) {
          value = [];
        } else {
          value = null;
        }
      }

      this.roleByRoleType.set(roleType, value);
    }

    return value;
  }

  public getForAssociation(roleType: RoleType): any {
    if (this.roleByRoleType === undefined) {
      return undefined;
    }

    let value = this.roleByRoleType.get(roleType);
    if (value === undefined) {
      if (this.newId === undefined) {
        if (roleType.objectType.isUnit) {
          value = this.workspaceObject?.roleByRoleTypeId.get(roleType.id);
          if (value === undefined) {
            value = null;
          }
        } else {
          if (roleType.isOne) {
            const role: string = this.workspaceObject?.roleByRoleTypeId.get(
              roleType.id
            );
            value = role ? this.session.getForAssociation(role) : null;
          } else {
            const roles: string[] = this.workspaceObject?.roleByRoleTypeId.get(
              roleType.id
            );
            value = roles
              ? roles.map((role) => {
                  return this.session.getForAssociation(role);
                })
              : [];
          }
        }
      } else {
        if (roleType.objectType.isComposite && roleType.isMany) {
          value = [];
        } else {
          value = null;
        }
      }

      this.roleByRoleType.set(roleType, value);
    }

    return value;
  }

  public set(roleType: RoleType, value: any) {
    this.assertExists();

    if (this.changedRoleByRoleType === undefined) {
      this.changedRoleByRoleType = new Map();
    }

    if (value === undefined) {
      value = null;
    }

    if (value === null) {
      if (roleType.objectType.isComposite && roleType.isMany) {
        value = [];
      }
    }

    if (value === "") {
      if (roleType.objectType.isUnit) {
        if (!roleType.objectType.isString) {
          value = null;
        }
      }
    }

    this.roleByRoleType.set(roleType, value);
    this.changedRoleByRoleType.set(roleType, value);

    this.session.hasChanges = true;
  }

  public add(roleType: RoleType, value: ISessionObject) {
    if (!!value) {
      this.assertExists();

      const roles = this.get(roleType);
      if (roles.indexOf(value) < 0) {
        roles.push(value);
      }

      this.set(roleType, roles);

      this.session.hasChanges = true;
    }
  }

  public remove(roleType: RoleType, value: ISessionObject) {
    if (!!value) {
      this.assertExists();

      const roles = this.get(roleType) as [];
      const newRoles = roles.filter((v) => v !== value);

      this.set(roleType, newRoles);

      this.session.hasChanges = true;
    }
  }

  public getAssociation(associationType: AssociationType): any {
    this.assertExists();

    const associations = this.session.getAssociation(this, associationType);

    if (associationType.isOne) {
      const association = associations.length > 0 ? associations[0] : null;
      const roleType = associationType.relationType.roleType;

      if (association) {
        if (roleType.isOne && association.get(roleType) === this) {
          return association;
        }

        if (roleType.isMany && association.get(roleType).indexOf(this) > -1) {
          return association;
        }
      }

      return null;
    }

    return associations;
  }

  public save(): PushRequestObject | undefined {
    if (this.changedRoleByRoleType !== undefined) {
      const data = new PushRequestObject();
      data.i = this.id;
      data.v = this.version;
      data.roles = this.saveRoles();
      return data;
    }

    return undefined;
  }

  public saveNew(): PushRequestNewObject {
    this.assertExists();

    const data = new PushRequestNewObject();
    data.ni = this.newId;
    data.t = this.objectType.id;

    if (this.changedRoleByRoleType !== undefined) {
      data.roles = this.saveRoles();
    }

    return data;
  }

  public reset() {
    if (this.newId) {
      delete this.newId;
      delete this.session;
      delete this.objectType;
      delete this.roleByRoleType;
    } else {
      this.workspaceObject =
        this.workspaceObject?.workspace.get(this.id) ?? undefined;
      this.roleByRoleType = new Map();
    }

    delete this.changedRoleByRoleType;
  }

  public onDelete(deleted: SessionObject) {
    if (this.changedRoleByRoleType !== undefined) {
      for (const [roleType, value] of this.changedRoleByRoleType) {
        if (!roleType.objectType.isUnit) {
          if (roleType.isOne) {
            const role = value as SessionObject;
            if (role && role === deleted) {
              this.set(roleType, null);
            }
          } else {
            const roles = value as SessionObject[];
            if (roles && roles.indexOf(deleted) > -1) {
              this.remove(roleType, deleted);
            }
          }
        }
      }
    }
  }

  protected init() {
    this.roleByRoleType = new Map();
  }

  private assertExists() {
    if (this.roleByRoleType === undefined) {
      throw new Error("Object doesn't exist anymore.");
    }
  }

  private saveRoles(): PushRequestRole[] {
    const saveRoles = new Array<PushRequestRole>();

    if (this.changedRoleByRoleType) {
      for (const [roleType, value] of this.changedRoleByRoleType) {
        const saveRole = new PushRequestRole();
        saveRole.t = roleType.id;

        let role = value;
        if (roleType.objectType.isUnit) {
          role = serialize(role);
          saveRole.s = role;
        } else {
          if (roleType.isOne) {
            saveRole.s = role ? role.id || role.newId : null;
          } else {
            const roleIds = role.map(
              (item: SessionObject) => item.id ?? item.newId
            );
            if (this.newId) {
              saveRole.a = roleIds;
            } else {
              const originalRoleIds = this.workspaceObject?.roleByRoleTypeId.get(
                roleType.id
              ) as string[];
              if (!originalRoleIds) {
                saveRole.a = roleIds;
              } else {
                saveRole.a = roleIds.filter(
                  (v: string) => originalRoleIds.indexOf(v) < 0
                );
                saveRole.r = originalRoleIds.filter(
                  (v) => roleIds.indexOf(v) < 0
                );
              }
            }
          }
        }

        saveRoles.push(saveRole);
      }
    }

    return saveRoles;
  }
}

export function serializeObject(
  roles: { [name: string]: ParameterTypes } | undefined
): { [name: string]: string } {
  if (roles) {
    return Object.keys(roles).reduce((obj, v) => {
      const role = roles[v];
      if (Array.isArray(role)) {
        obj[v] = role.map((w) => serialize(w)).join(",");
      } else {
        obj[v] = serialize(role);
      }
      return obj;
    }, {} as { [key: string]: any });
  }

  return {};
}

export function serializeArray(roles: UnitTypes[]): (string | null)[] {
  if (roles) {
    return roles.map((v) => serialize(v));
  }

  return [];
}

export function serialize(
  role: UnitTypes | CompositeTypes | undefined | null
): string | null {
  if (role === undefined || role === null) {
    return null;
  }

  if (typeof role === "string") {
    return role;
  }

  if (role instanceof Date) {
    return (role as Date).toISOString();
  }

  if (role instanceof SessionObject) {
    return role.id;
  }

  return role.toString();
}

export function deserialize(value: string, objectType: ObjectType): UnitTypes {
  switch (objectType.id) {
    case ids.Boolean:
      return value === "true" ? true : false;
    case ids.Float:
      return parseFloat(value);
    case ids.Integer:
      return parseInt(value, 10);
  }

  return value;
}

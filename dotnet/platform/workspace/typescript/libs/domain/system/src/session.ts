import { ObjectType, AssociationType } from '@allors/meta/system';
import {
  Operations,
  PushRequestObject,
  PushRequest,
  PushResponse,
} from '@allors/protocol/system';

import { ISessionObject } from './ISessionObject';
import { SessionObject } from './SessionObject';
import { IWorkspace, Workspace } from './Workspace';
import { IWorkspaceObject } from './WorkspaceObject';

export interface ISession {
  workspace: IWorkspace;

  hasChanges: boolean;

  get(id: string): ISessionObject | undefined;

  create(objectType: ObjectType | string): ISessionObject;

  delete(object: ISessionObject): void;

  pushRequest(): PushRequest;

  pushResponse(saveResponse: PushResponse): void;

  reset(): void;
}

export class Session implements ISession {
  private static idCounter = 0;

  public hasChanges: boolean;

  private existingSessionObjectById: Map<string, SessionObject>;
  private newSessionObjectById: Map<string, SessionObject>;

  private sessionObjectByIdByClass: Map<ObjectType, Map<string, SessionObject>>;

  constructor(public workspace: Workspace) {
    this.hasChanges = false;

    this.existingSessionObjectById = new Map();
    this.newSessionObjectById = new Map();

    this.sessionObjectByIdByClass = new Map();
  }

  public get(id: string): ISessionObject | undefined {
    if (!id) {
      return undefined;
    }

    let sessionObject = this.existingSessionObjectById.get(id);
    if (sessionObject === undefined) {
      sessionObject = this.newSessionObjectById.get(id);

      if (sessionObject === undefined) {
        const workspaceObject = this.workspace.get(id);
        if (workspaceObject) {
          sessionObject = this.instantiate(workspaceObject);
        }
      }
    }

    return sessionObject;
  }

  public getForAssociation(id: string): ISessionObject | undefined {
    if (!id) {
      return undefined;
    }

    let sessionObject = this.existingSessionObjectById.get(id);
    if (!sessionObject) {
      sessionObject = this.newSessionObjectById.get(id);

      if (!sessionObject) {
        const workspaceObject = this.workspace.getForAssociation(id);

        if (workspaceObject) {
          sessionObject = this.instantiate(workspaceObject);
        }
      }
    }

    return sessionObject;
  }

  public create(objectType: ObjectType | string): ISessionObject {
    const resolvedObjectType =
      typeof objectType === 'string'
        ? this.workspace.metaPopulation.objectTypeByName.get(objectType)
        : objectType;

    if (!resolvedObjectType) {
      throw new Error(`Could not find class for ${objectType}`);
    }

    const constructor = this.workspace.constructorByObjectType.get(
      resolvedObjectType
    );
    if (!constructor) {
      throw new Error(
        `Could not get constructor for ${resolvedObjectType.name}`
      );
    }

    const newSessionObject = new constructor();
    newSessionObject.session = this;
    newSessionObject.objectType = resolvedObjectType;
    newSessionObject.newId = (--Session.idCounter).toString();

    this.newSessionObjectById.set(newSessionObject.newId, newSessionObject);
    this.addByObjectTypeId(newSessionObject);

    this.hasChanges = true;

    return newSessionObject;
  }

  public delete(object: ISessionObject): void {
    if (!object.isNew) {
      throw new Error('Existing objects can not be deleted');
    }

    const newSessionObject = object as SessionObject;
    const newId = newSessionObject.newId!;

    if (this.newSessionObjectById.has(newId)) {
      for (const sessionObject of this.newSessionObjectById.values()) {
        sessionObject.onDelete(newSessionObject);
      }

      for (const sessionObject of this.existingSessionObjectById.values()) {
        sessionObject.onDelete(newSessionObject);
      }

      const objectType = newSessionObject.objectType;
      newSessionObject.reset();

      this.newSessionObjectById.delete(newId);
      this.removeByObjectTypeId(objectType, newId);
    }
  }

  public reset(): void {
    for (const sessionObject of this.newSessionObjectById.values()) {
      sessionObject.reset();
    }

    for (const sessionObject of this.existingSessionObjectById.values()) {
      sessionObject.reset();
    }

    this.hasChanges = false;
  }

  public pushRequest(): PushRequest {
    const newObjects = Array.from(this.newSessionObjectById.values()).map((v) =>
      v.saveNew()
    );
    const objects = Array.from(this.existingSessionObjectById.values())
      .map((v) => v.save())
      .filter((v) => v) as PushRequestObject[];

    return new PushRequest({
      newObjects,
      objects,
    });
  }

  public pushResponse(pushResponse: PushResponse): void {
    if (pushResponse.newObjects) {
      pushResponse.newObjects.forEach((pushResponseNewObject) => {
        const newId = pushResponseNewObject.ni;
        const id = pushResponseNewObject.i;

        const sessionObject = this.newSessionObjectById.get(newId);
        if (sessionObject) {
          delete sessionObject.newId;
          sessionObject.workspaceObject = this.workspace.new(
            id,
            sessionObject.objectType
          );

          this.newSessionObjectById.delete(newId);
          this.existingSessionObjectById.set(id, sessionObject);

          this.removeByObjectTypeId(sessionObject.objectType, newId);
          this.addByObjectTypeId(sessionObject);
        }
      });
    }

    if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
      throw new Error('Not all new objects received ids');
    }
  }

  public getAssociation(
    object: ISessionObject,
    associationType: AssociationType
  ): ISessionObject[] {
    const associationClasses = associationType.objectType.classes;
    const roleType = associationType.relationType.roleType;

    const associationIds = new Set<string>();
    const associations: SessionObject[] = [];

    associationClasses.forEach((associationClass) => {
      this.getAll(associationClass);
      const sessionObjectById = this.sessionObjectByIdByClass.get(
        associationClass
      );
      if (sessionObjectById) {
        for (const association of sessionObjectById.values()) {
          if (
            !associationIds.has(association.id) &&
            association.canRead(roleType)
          ) {
            if (roleType.isOne) {
              const role: SessionObject = association.getForAssociation(
                roleType
              );
              if (role && role.id === object.id) {
                associationIds.add(association.id);
                associations.push(association);
              }
            } else {
              const roles: SessionObject[] = association.getForAssociation(
                roleType
              );
              if (roles && roles.find((v) => v === object)) {
                associationIds.add(association.id);
                associations.push(association);
              }
            }
          }
        }
      }
    });

    if (associationType.isOne && associations.length > 0) {
      return associations;
    }

    associationClasses.forEach((associationClass) => {
      const workspaceObjects = this.workspace.workspaceObjectsByClass.get(
        associationClass
      );
      if (workspaceObjects) {
        for (const workspaceObject of workspaceObjects) {
          if (!associationIds.has(workspaceObject.id)) {
            const permission = this.workspace.permission(
              workspaceObject.objectType,
              roleType,
              Operations.Read
            );
            if (permission && workspaceObject.isPermitted(permission)) {
              if (roleType.isOne) {
                const role: string = workspaceObject.roleByRoleTypeId.get(
                  roleType.id
                );
                if (object.id === role) {
                  associations.push(
                    this.get(workspaceObject.id) as SessionObject
                  );
                  break;
                }
              } else {
                const roles: string[] = workspaceObject.roleByRoleTypeId.get(
                  roleType.id
                );
                if (roles && roles.indexOf(workspaceObject.id) > -1) {
                  associationIds.add(workspaceObject.id);
                  associations.push(
                    this.get(workspaceObject.id) as SessionObject
                  );
                }
              }
            }
          }
        }
      }
    });

    return associations;
  }

  private instantiate(workspaceObject: IWorkspaceObject): SessionObject {
    const constructor = this.workspace.constructorByObjectType.get(
      workspaceObject.objectType
    );
    if (!constructor) {
      throw new Error(
        `Could not get constructor for ${workspaceObject.objectType.name}`
      );
    }

    const sessionObject = new constructor();
    sessionObject.session = this;
    sessionObject.workspaceObject = workspaceObject;
    sessionObject.objectType = workspaceObject.objectType;

    this.existingSessionObjectById.set(sessionObject.id, sessionObject);
    this.addByObjectTypeId(sessionObject);

    return sessionObject;
  }

  private getAll(objectType: ObjectType): void {
    const workspaceObjects = this.workspace.workspaceObjectsByClass.get(
      objectType
    );
    if (workspaceObjects) {
      for (const workspaceObject of workspaceObjects) {
        this.get(workspaceObject.id);
      }
    }
  }

  private addByObjectTypeId(sessionObject: SessionObject) {
    let sessionObjectById = this.sessionObjectByIdByClass.get(
      sessionObject.objectType
    );
    if (!sessionObjectById) {
      sessionObjectById = new Map();
      this.sessionObjectByIdByClass.set(
        sessionObject.objectType,
        sessionObjectById
      );
    }

    sessionObjectById.set(sessionObject.id, sessionObject);
  }

  private removeByObjectTypeId(objectType: ObjectType, id: string) {
    const sessionObjectById = this.sessionObjectByIdByClass.get(objectType);
    if (sessionObjectById) {
      sessionObjectById.delete(id);
    }
  }
}

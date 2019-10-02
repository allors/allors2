import { ObjectType, AssociationType } from '../meta';

import { PushRequest } from './../protocol/push/PushRequest';
import { PushResponse } from './../protocol/push/PushResponse';

import { ISessionObject, SessionObject } from './SessionObject';
import { IWorkspace, Workspace } from './Workspace';
import { WorkspaceObject } from './WorkspaceObject';
import { Operations } from '../protocol/Operations';

export interface ISession {

  workspace: IWorkspace;

  hasChanges: boolean;

  get(id: string): ISessionObject;

  create(objectType: ObjectType | string): ISessionObject;

  delete(object: ISessionObject): void;

  pushRequest(): PushRequest;

  pushResponse(saveResponse: PushResponse): void;

  reset(): void;
}

export class Session implements ISession {
  private static idCounter = 0;

  public hasChanges: boolean;

  private existingSessionObjectById: Map<string, ISessionObject>;
  private newSessionObjectById: Map<string, ISessionObject>;

  private sessionObjectByIdByClass: Map<ObjectType, Map<string, ISessionObject>>;

  constructor(public workspace: IWorkspace) {
    this.hasChanges = false;

    this.existingSessionObjectById = new Map();
    this.newSessionObjectById = new Map();

    this.sessionObjectByIdByClass = new Map();
  }

  public get(id: string): ISessionObject {
    if (!id) {
      return undefined;
    }

    let sessionObject: ISessionObject = this.existingSessionObjectById.get(id);
    if (sessionObject === undefined) {
      sessionObject = this.newSessionObjectById[id];

      if (sessionObject === undefined) {
        const workspaceObject: WorkspaceObject = this.workspace.get(id);

        const constructor: any = this.workspace.constructorByObjectType.get(workspaceObject.objectType);
        sessionObject = new constructor();
        sessionObject.session = this;
        sessionObject.workspaceObject = workspaceObject;
        sessionObject.objectType = workspaceObject.objectType;

        this.existingSessionObjectById.set(sessionObject.id, sessionObject);
        this.addByObjectTypeId(sessionObject);
      }
    }

    return sessionObject;
  }

  public create(objectType: ObjectType): ISessionObject {

    const objectTypeName = objectType instanceof ObjectType ? objectType.name : objectType;
    const constructor: any = this.workspace.constructorByObjectType.get(objectType);
    const newSessionObject: ISessionObject = new constructor();
    newSessionObject.session = this;
    newSessionObject.objectType = this.workspace.metaPopulation.metaObjectById[objectType.id] as ObjectType;
    newSessionObject.newId = (--Session.idCounter).toString();

    this.newSessionObjectById[newSessionObject.newId] = newSessionObject;
    this.addByObjectTypeId(newSessionObject);

    this.hasChanges = true;

    return newSessionObject;
  }

  public delete(object: ISessionObject): void {
    if (!object.isNew) {
      throw new Error('Existing objects can not be deleted');
    }

    const newSessionObject = object as SessionObject;
    const newId = newSessionObject.newId;

    if (this.newSessionObjectById && this.newSessionObjectById.hasOwnProperty(newId)) {

      for (const sessionObject of this.newSessionObjectById.values()) {
        (sessionObject as SessionObject).onDelete(newSessionObject);
      }

      for (const sessionObject of this.existingSessionObjectById.values()) {
        (sessionObject as SessionObject).onDelete(newSessionObject);
      }

      const objectType = newSessionObject.objectType;
      newSessionObject.reset();

      this.newSessionObjectById.delete(newId);
      this.removeByObjectTypeId(objectType, newId);
    }
  }

  public reset(): void {

    for (const sessionObject of this.newSessionObjectById.values()) {
      (sessionObject as SessionObject).reset();
    }

    for (const sessionObject of this.existingSessionObjectById.values()) {
      (sessionObject as SessionObject).reset();
    }

    this.hasChanges = false;
  }

  public pushRequest(): PushRequest {
    return new PushRequest({
      newObjects: [...this.newSessionObjectById.values()].map(v => v.saveNew()).filter(v => v !== undefined),
      objects: [...this.existingSessionObjectById.values()].map(v => v.save()).filter(v => v !== undefined),
    });
  }

  public pushResponse(pushResponse: PushResponse): void {
    if (pushResponse.newObjects) {
      pushResponse.newObjects.forEach((pushResponseNewObject) => {
        const newId: string = pushResponseNewObject.ni;
        const id: string = pushResponseNewObject.i;

        const newSessionObject: ISessionObject = this.newSessionObjectById[newId];
        delete this.newSessionObjectById[newId];
        delete newSessionObject.newId;

        this.workspace.invalidate(id, newSessionObject.objectType);
        const workspaceObject: WorkspaceObject = this.workspace.get(id);
        newSessionObject.workspaceObject = workspaceObject;

        this.existingSessionObjectById[id] = newSessionObject;

        this.addByObjectTypeId(newSessionObject);
        this.removeByObjectTypeId(newSessionObject.objectType, newId);
      });
    }

    if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
      throw new Error('Not all new objects received ids');
    }
  }

  public getAssociation(object: ISessionObject, associationType: AssociationType): ISessionObject[] {
    const associationClasses = associationType.objectType.classes;
    const roleType = associationType.relationType.roleType;

    const associationIds = new Set<string>();
    const associations: ISessionObject[] = [];

    associationClasses.forEach((associationClass) => {
      const sessionObjectById = this.sessionObjectByIdByClass.get(associationClass);
      if (sessionObjectById) {
        for (const association of sessionObjectById.values()) {
          if (!associationIds.has(association.id) && association.canRead(roleType)) {
            if (roleType.isOne) {
              const role: ISessionObject = association.get(roleType);
              if (role && role.id === object.id) {
                associationIds.add(association.id);
                associations.push(association);
              }
            } else {
              const roles: ISessionObject[] = association.get(roleType);
              if (roles && roles.indexOf(association) > -1) {
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
      const workspaceObjectById = (this.workspace as Workspace).workspaceObjectByIdByClass.get(associationClass);
      if (workspaceObjectById) {
        for (const [id, association] of workspaceObjectById) {
          if (!associationIds.has(id)) {
            const permission = this.workspace.permission(association.objectType, roleType, Operations.Read);
            if (association.isPermitted(permission)) {
              if (roleType.isOne) {
                const role: string = association.roleByRoleTypeId.get(roleType.id);
                if (object.id === role) {
                  associations.push(this.get(association.id));
                  break;
                }
              } else {
                const roles: string[] = association.roleByRoleTypeId.get(roleType.id);
                if (roles && roles.indexOf(association.id) > -1) {
                  associationIds.add(association.id);
                  associations.push(this.get(association.id));
                }
              }
            }
          }
        }
      }
    });

    return associations;
  }

  private addByObjectTypeId(sessionObject: ISessionObject) {
    let sessionObjectById = this.sessionObjectByIdByClass.get(sessionObject.objectType);
    if (!sessionObjectById) {
      sessionObjectById = new Map();
      this.sessionObjectByIdByClass.set(sessionObject.objectType, sessionObjectById);
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

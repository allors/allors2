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

  private existingSessionObjectById: { [id: string]: ISessionObject } = {};
  private newSessionObjectById: { [id: string]: ISessionObject } = {};

  private sessionObjectByIdByClassId: { [id: string]: { [id: string]: ISessionObject } } = {};

  constructor(public workspace: IWorkspace) {
    this.hasChanges = false;
  }

  public get(id: string): ISessionObject {
    if (!id) {
      return undefined;
    }

    let sessionObject: ISessionObject = this.existingSessionObjectById[id];
    if (sessionObject === undefined) {
      sessionObject = this.newSessionObjectById[id];

      if (sessionObject === undefined) {
        const workspaceObject: WorkspaceObject = this.workspace.get(id);

        const constructor: any = this.workspace.constructorByName[
          workspaceObject.objectType.name
        ];
        sessionObject = new constructor();
        sessionObject.session = this;
        sessionObject.workspaceObject = workspaceObject;
        sessionObject.objectType = workspaceObject.objectType;

        this.existingSessionObjectById[sessionObject.id] = sessionObject;
        this.addByObjectTypeId(sessionObject);
      }
    }

    return sessionObject;
  }

  public create(objectType: ObjectType): ISessionObject {

    const objectTypeName = objectType instanceof ObjectType ? objectType.name : objectType;
    const constructor: any = this.workspace.constructorByName[objectTypeName];
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

      Object.keys(this.newSessionObjectById).forEach((key: string) =>
        (this.newSessionObjectById[key] as SessionObject).onDelete(newSessionObject),
      );

      if (this.existingSessionObjectById) {
        Object.keys(this.existingSessionObjectById).forEach((key: string) =>
          (this.existingSessionObjectById[key] as SessionObject).onDelete(newSessionObject),
        );
      }

      const objectType = newSessionObject.objectType;
      newSessionObject.reset();

      delete this.newSessionObjectById[newId];
      this.removeByObjectTypeId(objectType, newId);
    }
  }

  public reset(): void {
    if (this.newSessionObjectById) {
      Object.keys(this.newSessionObjectById).forEach((key: string) =>
        this.newSessionObjectById[key].reset(),
      );
    }

    if (this.existingSessionObjectById) {
      Object.keys(this.existingSessionObjectById).forEach((key: string) =>
        this.existingSessionObjectById[key].reset(),
      );
    }

    this.hasChanges = false;
  }

  public pushRequest(): PushRequest {
    return new PushRequest({
      newObjects: Object.values(this.newSessionObjectById).map(v => v.saveNew()).filter(v => v !== undefined),
      objects: Object.values(this.existingSessionObjectById).map(v => v.save()).filter(v => v !== undefined),
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

  // TODO: add caching
  public getAssociation(object: ISessionObject, associationType: AssociationType): ISessionObject[] {
    const associationClasses = associationType.objectType.classes;
    const roleType = associationType.relationType.roleType;

    const associations: ISessionObject[] = [];

    associationClasses.forEach((associationClass) => {
      const sessionObjectById = this.sessionObjectByIdByClassId[associationClass.id];
      if (sessionObjectById) {
        Object
          .keys(sessionObjectById)
          .forEach((v) => {
            const association = sessionObjectById[v];
            if (association.canRead(roleType)) {
              if (roleType.isOne) {
                const role: ISessionObject = association.get(roleType);
                if (role && role.id === object.id) {
                  associations.push(association);
                }
              } else {
                const roles: ISessionObject[] = association.get(roleType);
                if (roles && roles.indexOf(association) > -1) {
                  associations.push(association);
                }
              }
            }
          });
      }
    });

    if (associationType.isOne && associations.length > 0) {
      return associations;
    }

    const associationIds = associations.map((v => v.id));

    associationClasses.forEach((associationClass) => {
      const workspaceObjectById = (this.workspace as Workspace).workspaceObjectByIdByClassId[associationClass.id];
      if (workspaceObjectById) {
        Object
          .keys(workspaceObjectById)
          .filter((v) => associationIds.indexOf(v) < 0)
          .forEach((v) => {
            const association = workspaceObjectById[v];
            const permission = this.workspace.permission(association.objectType, roleType, Operations.Read);
            if (association.isPermitted(permission)) {
              if (roleType.isOne) {
                const role: string = association.roles.get(roleType);
                if (object.id === role) {
                  associations.push(this.get(association.id));
                }
              } else {
                const roles: string[] = association.roles.get(roleType);
                if (roles && roles.indexOf(association.id) > -1) {
                  associations.push(this.get(association.id));
                }
              }
            }
          });
      }
    });

    return associations;
  }

  private addByObjectTypeId(sessionObject: ISessionObject) {
    let sessionObjectById = this.sessionObjectByIdByClassId[sessionObject.objectType.id];
    if (!sessionObjectById) {
      sessionObjectById = {};
      this.sessionObjectByIdByClassId[sessionObject.objectType.id] = sessionObjectById;
    }

    sessionObjectById[sessionObject.id] = sessionObject;
  }

  private removeByObjectTypeId(objectType: ObjectType, id: string) {
    const sessionObjectById = this.sessionObjectByIdByClassId[objectType.id];
    if (sessionObjectById) {
      delete sessionObjectById[id];
    }
  }
}

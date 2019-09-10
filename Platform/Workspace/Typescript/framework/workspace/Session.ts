import { ObjectType, AssociationType } from '../meta';

import { PushRequest } from './../protocol/push/PushRequest';
import { PushRequestNewObject } from './../protocol/push/PushRequestNewObject';
import { PushRequestObject } from './../protocol/push/PushRequestObject';
import { PushResponse } from './../protocol/push/PushResponse';
import { PushResponseNewObject } from './../protocol/push/PushResponseNewObject';
import { ResponseType } from './../protocol/ResponseType';
import { SyncResponse } from './../protocol/sync/SyncResponse';

import { INewSessionObject, ISessionObject, SessionObject } from './SessionObject';
import { IWorkspace, Workspace } from './Workspace';
import { WorkspaceObject } from './WorkspaceObject';

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
  private newSessionObjectById: { [id: string]: INewSessionObject } = {};

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

  public create(objectType: ObjectType | string): ISessionObject {

    const objectTypeName = objectType instanceof ObjectType ? objectType.name : objectType;
    const constructor: any = this.workspace.constructorByName[objectTypeName];
    const newSessionObject: INewSessionObject = new constructor();
    newSessionObject.session = this;
    newSessionObject.objectType = this.workspace.metaPopulation.objectTypeByName[
      objectTypeName
    ];
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
    const data: PushRequest = new PushRequest();
    data.newObjects = [];
    data.objects = [];

    if (this.newSessionObjectById) {
      Object.keys(this.newSessionObjectById).forEach((key: string) => {
        const newSessionObject: INewSessionObject = this.newSessionObjectById[key];
        const objectData: PushRequestNewObject = newSessionObject.saveNew();
        if (objectData !== undefined) {
          data.newObjects.push(objectData);
        }
      });
    }

    if (this.existingSessionObjectById) {
      Object.keys(this.existingSessionObjectById).forEach((key: string) => {
        const sessionObject: ISessionObject = this.existingSessionObjectById[key];
        const objectData: PushRequestObject = sessionObject.save();
        if (objectData !== undefined) {
          data.objects.push(objectData);
        }
      });
    }

    return data;
  }

  public pushResponse(pushResponse: PushResponse): void {
    if (pushResponse.newObjects) {
      Object.keys(pushResponse.newObjects).forEach((key: string) => {
        const pushResponseNewObject: PushResponseNewObject =
          pushResponse.newObjects[key];
        const newId: string = pushResponseNewObject.ni;
        const id: string = pushResponseNewObject.i;

        const newSessionObject: INewSessionObject = this.newSessionObjectById[
          newId
        ];

        const syncResponse: SyncResponse = {
          hasErrors: false,
          objects: [
            {
              i: id,
              methods: [],
              roles: [],
              t: newSessionObject.objectType.name,
              v: '',
            },
          ],
          responseType: ResponseType.Sync,
          userSecurityHash: '#', // This should trigger a load on next check
        };

        delete this.newSessionObjectById[newId];
        delete newSessionObject.newId;

        this.workspace.sync(syncResponse);
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
            if (association.canRead(roleType.name)) {
              if (roleType.isOne) {
                const role: ISessionObject = association.get(roleType.name);
                if (role && role.id === object.id) {
                  associations.push(association);
                }
              } else {
                const roles: ISessionObject[] = association.get(roleType.name);
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
            if (association.canRead(roleType.name)) {
              if (roleType.isOne) {
                const role: string = association.roles[roleType.name];
                if (object.id === role) {
                  associations.push(this.get(association.id));
                }
              } else {
                const roles: string[] = association.roles[roleType.name];
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

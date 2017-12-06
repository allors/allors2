import { PushRequest } from "./../database/push/PushRequest";
import { PushRequestNewObject } from "./../database/push/PushRequestNewObject";
import { PushRequestObject } from "./../database/push/PushRequestObject";
import { PushResponse } from "./../database/push/PushResponse";
import { PushResponseNewObject } from "./../database/push/PushResponseNewObject";
import { ResponseType } from "./../database/ResponseType";
import { SyncResponse } from "./../database/sync/SyncResponse";

import { INewSessionObject, ISessionObject } from "./SessionObject";
import { IWorkspace } from "./Workspace";
import { WorkspaceObject } from "./WorkspaceObject";

export interface ISession {
  hasChanges: boolean;

  get(id: string): ISessionObject;

  create(objectTypeName: string): ISessionObject;

  pushRequest(): PushRequest;

  pushResponse(saveResponse: PushResponse): void;

  reset(): void;
}

export class Session implements ISession {
  private static idCounter: number = 0;

  public hasChanges: boolean;

  private sessionObjectById: { [id: string]: ISessionObject; } = {};
  private newSessionObjectById: { [id: string]: INewSessionObject; } = {};

  constructor(private workspace: IWorkspace) {
    this.hasChanges = false;
  }

  public get(id: string): ISessionObject {
    if (!id) {
      return undefined;
    }

    let sessionObject: ISessionObject = this.sessionObjectById[id];
    if (sessionObject === undefined) {
      sessionObject = this.newSessionObjectById[id];

      if (sessionObject === undefined) {
        const workspaceObject: WorkspaceObject = this.workspace.get(id);

        const constructor: any = this.workspace.constructorByName[workspaceObject.objectType.name];
        sessionObject = new constructor();
        sessionObject.session = this;
        sessionObject.workspaceObject = workspaceObject;
        sessionObject.objectType = workspaceObject.objectType;

        this.sessionObjectById[sessionObject.id] = sessionObject;
      }
    }

    return sessionObject;
  }

  public create(objectTypeName: string): ISessionObject {
    const constructor: any = this.workspace.constructorByName[objectTypeName];
    const newSessionObject: INewSessionObject = new constructor();
    newSessionObject.session = this;
    newSessionObject.objectType = this.workspace.metaPopulation.objectTypeByName[objectTypeName];
    newSessionObject.newId = (--Session.idCounter).toString();

    this.newSessionObjectById[newSessionObject.newId] = newSessionObject;

    this.hasChanges = true;

    return newSessionObject;
  }

  public reset(): void {
    if (this.newSessionObjectById) {
      Object
        .keys(this.newSessionObjectById)
        .forEach((key: string) => this.newSessionObjectById[key].reset());
    }

    if (this.sessionObjectById) {
      Object
        .keys(this.sessionObjectById)
        .forEach((key: string) => this.sessionObjectById[key].reset());
    }

    this.hasChanges = false;
  }

  public pushRequest(): PushRequest {
    const data: PushRequest = new PushRequest();
    data.newObjects = [];
    data.objects = [];

    if (this.newSessionObjectById) {
      Object
        .keys(this.newSessionObjectById)
        .forEach((key: string) => {
          const newSessionObject: INewSessionObject = this.newSessionObjectById[key];
          const objectData: PushRequestNewObject = newSessionObject.saveNew();
          if (objectData !== undefined) {
            data.newObjects.push(objectData);
          }
        });
    }

    if (this.sessionObjectById) {
      Object
        .keys(this.sessionObjectById)
        .forEach((key: string) => {
          const sessionObject: ISessionObject = this.sessionObjectById[key];
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
      Object
        .keys(pushResponse.newObjects)
        .forEach((key: string) => {
          const pushResponseNewObject: PushResponseNewObject = pushResponse.newObjects[key];
          const newId: string = pushResponseNewObject.ni;
          const id: string = pushResponseNewObject.i;

          const newSessionObject: INewSessionObject = this.newSessionObjectById[newId];

          const syncResponse: SyncResponse = {
            hasErrors: false,
            objects: [
              {
                i: id,
                methods: [],
                roles: [],
                t: newSessionObject.objectType.name,
                v: "",
              },
            ],
            responseType: ResponseType.Sync,
            userSecurityHash: "#", // This should trigger a load on next check
          };

          delete (this.newSessionObjectById[newId]);
          delete (newSessionObject.newId);

          this.workspace.sync(syncResponse);
          const workspaceObject: WorkspaceObject = this.workspace.get(id);
          newSessionObject.workspaceObject = workspaceObject;

          this.sessionObjectById[id] = newSessionObject;
        });
    }

    if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
      throw new Error("Not all new objects received ids");
    }
  }
}

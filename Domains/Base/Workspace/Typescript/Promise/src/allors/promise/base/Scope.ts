import {
  InvokeResponse, ISession, ISessionObject, Method, PullResponse,
  PushRequest, PushRequestObject, PushResponse, Session, SyncRequest,
  SyncResponse, Workspace,
} from "../../domain";

import { Database } from "./Database";

import { Invoked } from "./responses/Invoked";
import { Loaded } from "./responses/Loaded";
import { Saved } from "./responses/Saved";

export class Scope {
  public session: ISession;

  constructor(public database: Database, public workspace: Workspace) {
    this.session = new Session(this.workspace);
  }

  public load(service: string, params?: any): Promise<Loaded> {

    return new Promise((resolve, reject) => {
      this.database
        .pull(service, params)
        .then((pullResponse: PullResponse) => {
          const requireLoadIds: SyncRequest = this.workspace.diff(pullResponse);

          if (requireLoadIds.objects.length > 0) {
            return this.database
              .sync(requireLoadIds)
              .then((syncResponse: SyncResponse) => {
                this.workspace.sync(syncResponse);
                const loaded: Loaded = new Loaded(this.session, pullResponse);
                resolve(loaded);
              })
              .catch((e) => {
                reject(e);
              });
          } else {
            const loaded: Loaded = new Loaded(this.session, pullResponse);
            resolve(loaded);
          }
        })
        .catch((e) => {
          reject(e);
        });
    });
  }

  public save(): Promise<Saved> {

    return new Promise((resolve, reject) => {
      const pushRequest: PushRequest = this.session.pushRequest();
      return this.database
        .push(pushRequest)
        .then((pushResponse: PushResponse) => {

          this.session.pushResponse(pushResponse);
          const syncRequest: SyncRequest = new SyncRequest();
          syncRequest.objects = pushRequest.objects.map((v: PushRequestObject) => v.i);
          if (pushResponse.newObjects) {
            for (const newObject of pushResponse.newObjects) {
              syncRequest.objects.push(newObject.i);
            }
          }

          return this.database
            .sync(syncRequest)
            .then((syncResponse: SyncResponse) => {
              this.workspace.sync(syncResponse);
              const saved: Saved = new Saved(this.session, pushResponse);
              resolve(saved);
            })
            .catch((e) => {
              reject(e);
            });
        })
        .catch((e) => {
          reject(e);
        });
    });
  }

  public invoke(method: Method): Promise<Invoked>;
  public invoke(service: string, args?: any): Promise<Invoked>;
  public invoke(methodOrService: Method | string, args?: any): Promise<Invoked> {

    return this.database
      .invoke(methodOrService as any, args)
      .then((invokeResponse: InvokeResponse) => new Invoked(this.session, invokeResponse));
  }
}

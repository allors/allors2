import { Observable } from "rxjs/Observable";

import "rxjs/add/observable/of";
import "rxjs/add/operator/switchMap";

import {
  InvokeResponse, ISession, ISessionObject, Method, PullResponse,
  PushRequest, PushRequestObject, PushResponse, Session, SyncRequest,
  SyncResponse, Workspace,
} from "@allors/framework";

import { Database } from "./Database";

import { Invoked } from "./responses/Invoked";
import { Loaded } from "./responses/Loaded";
import { Saved } from "./responses/Saved";

export class Scope {
  public session: ISession;

  constructor(public database: Database, public workspace: Workspace) {
    this.session = new Session(this.workspace);
  }

  public load(service: string, params?: any): Observable<Loaded> {

    return this.database
      .pull(service, params)
      .switchMap((pullResponse: PullResponse) => {
        const requireLoadIds: SyncRequest = this.workspace.diff(pullResponse);

        if (requireLoadIds.objects.length > 0) {
          return this.database
            .sync(requireLoadIds)
            .map((syncResponse: SyncResponse) => {
              this.workspace.sync(syncResponse);
              const loaded: Loaded = new Loaded(this.session, pullResponse);
              return loaded
            });
        } else {
          const loaded: Loaded = new Loaded(this.session, pullResponse);
          return Observable.of(loaded);
        }
      });
  }

  public save(): Observable<Saved> {

    const pushRequest: PushRequest = this.session.pushRequest();
    return this.database
      .push(pushRequest)
      .switchMap((pushResponse: PushResponse) => {

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
          .map((syncResponse: SyncResponse) => {
            this.workspace.sync(syncResponse);
            const saved: Saved = new Saved(this.session, pushResponse);
            return saved;
          });
      });
  }

  public invoke(method: Method): Observable<Invoked>;
  public invoke(service: string, args?: any): Observable<Invoked>;
  public invoke(methodOrService: Method | string, args?: any): Observable<Invoked> {

    return this.database
      .invoke(methodOrService as any, args)
      .map((invokeResponse: InvokeResponse) => new Invoked(this.session, invokeResponse));
  }
}

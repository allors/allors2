import { Observable } from 'rxjs/Rx';

import {
  Workspace, ISession, Session, ISessionObject, Method,
  SyncRequest, PullResponse, SyncResponse, PushRequest, PushRequestObject,
  PushResponse, InvokeResponse,
} from '../../domain';

import { Database } from './Database';

import { Loaded } from './responses/Loaded';
import { Saved } from './responses/Saved';
import { Invoked } from './responses/Invoked';

export class Scope {
  session: ISession;

  constructor(public database: Database, public workspace: Workspace) {
    this.session = new Session(this.workspace);
  }

  load(service: string, params?: any): Observable<Loaded> {

    return this.database
      .pull(service, params)
      .mergeMap((response: PullResponse) => {
        const requireLoadIds: SyncRequest = this.workspace.diff(response);

        if (requireLoadIds.objects.length > 0) {
          return this.database
            .sync(requireLoadIds)
            .map((loadResponse: SyncResponse) => {
              this.workspace.sync(loadResponse);
              const loaded: Loaded = new Loaded(this.session, response);
              return loaded;
            });
        } else {
          const loaded: Loaded = new Loaded(this.session, response);
          return Observable.of(loaded);
        }
      });
  }

  save(): Observable<Saved> {

    const pushRequest: PushRequest = this.session.pushRequest();
    return this.database
      .push(pushRequest)
      .mergeMap((pushResponse: PushResponse) => {

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

  invoke(method: Method): Observable<Invoked>;
  invoke(service: string, args?: any): Observable<Invoked>;
  invoke(methodOrService: Method | string, args?: any): Observable<Invoked> {

    return this.database
      .invoke(methodOrService as any, args)
      .map((invokeResponse: InvokeResponse) => new Invoked(this.session, invokeResponse));
  }
}

import { Observable } from 'rxjs/Rx';

import { Workspace, ISession, Session, ISessionObject, Method,
         SyncRequest, PullResponse, SyncResponse, PushResponse, InvokeResponse } from '../../domain';

import { Database } from './Database';
import { Result } from './Result';

export class Scope {
  session: ISession;

  objects: { [name: string]: ISessionObject; } = {};
  collections: { [name: string]: ISessionObject[]; } = {};
  values: { [name: string]: any; } = {};

  constructor(public database: Database, public workspace: Workspace) {

    this.session = new Session(this.workspace);
  }

  load(service: string, params?: any): Observable<any> {
    return this.database
      .pull(service, params)
      .mergeMap((response: PullResponse) => {
        const requireLoadIds: SyncRequest = this.workspace.diff(response);

        if (requireLoadIds.objects.length > 0) {
          return this.database
            .sync(requireLoadIds)
            .map((loadResponse: SyncResponse) => {
              this.workspace.sync(loadResponse);
              this.update(response);
              this.session.reset();
            });
        } else {
          this.update(response);
          this.session.reset();
          return Observable.of(undefined);
        }
      });
  }

  fetch(service: string, params?: any): Observable<Result> {
    return this.database
      .pull(service, params)
      .mergeMap(response => {
        const requireLoadIds = this.workspace.diff(response);

        if (requireLoadIds.objects.length > 0) {
          return this.database
            .sync(requireLoadIds)
            .map(u => {
              const loadResponse = u as SyncResponse;
              this.workspace.sync(loadResponse);
              return new Result(this.session, response);
            });
        } else {
          return Observable.of(new Result(this.session, response));
        }
      });
  }

  save(): Observable<PushResponse> {

    const pushRequest = this.session.pushRequest();
    return this.database
      .push(pushRequest)
      .mergeMap((pushResponse: PushResponse) => {
        this.session.pushResponse(pushResponse);

        const syncRequest = new SyncRequest();
        syncRequest.objects = pushRequest.objects.map(v => v.i);
        if (pushResponse.newObjects) {
          for (const newObject of pushResponse.newObjects) {
            syncRequest.objects.push(newObject.i);
          }
        }

        return this.database
          .sync(syncRequest)
          .map((syncResponse) => {
            this.workspace.sync(syncResponse);
            this.session.reset();
            return pushResponse;
          });
      });
  }

  invoke(method: Method): Observable<InvokeResponse>;
  invoke(service: string, args?: any): Observable<InvokeResponse>;
  invoke(methodOrService: Method | string, args?: any): Observable<InvokeResponse> {

    if (methodOrService instanceof Method) {
      return this.database.invoke(methodOrService);
    } else {
      return this.database.invoke(methodOrService as string, args);
    }
  }

  private update(response: PullResponse): void {

    this.objects = {};
    this.collections = {};
    this.values = {};

    // TODO: Deduplicate
    const namedObjects = response.namedObjects;
    const namedCollections = response.namedCollections;
    const namedValues = response.namedValues;

    Object.keys(namedObjects).map((k) => this.objects[k] = this.session.get(namedObjects[k]));
    Object.keys(namedCollections).map((k) => this.collections[k] = namedCollections[k].map((obj) => this.session.get(obj)));
    Object.keys(namedValues).map((k) => this.values[k] = namedValues[k]);
  }
}

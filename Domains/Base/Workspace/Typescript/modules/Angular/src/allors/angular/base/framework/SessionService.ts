import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';

import { MetaDomain, PullFactory, FetchFactory, TreeFactory } from '../../../meta';
import { ISession, Session, PullResponse, SyncRequest, IWorkspace, SyncResponse, PushRequest, PushResponse, PushRequestObject, Method, InvokeOptions, InvokeResponse } from '../../../framework';

import { WorkspaceService } from './WorkspaceService';
import { Loaded } from './responses/Loaded';
import { switchMap, map } from 'rxjs/operators';
import { Saved } from './responses/Saved';
import { Invoked } from './responses/Invoked';
import { Database } from './Database';

@Injectable()
export class SessionService {

  x = {};

  m: MetaDomain;

  pull: PullFactory;
  fetch: FetchFactory;
  tree: TreeFactory;

  database: Database;
  workspace: IWorkspace;
  session: ISession;

  constructor(public workspaceService: WorkspaceService) {
    this.database = workspaceService.databaseService.database;
    this.workspace = workspaceService.workspace;
    this.session = new Session(this.workspace);

    const metaPopulation = workspaceService.metaPopulation;
    this.m = metaPopulation.metaDomain;
    this.pull = new PullFactory(metaPopulation);
    this.fetch = new FetchFactory(metaPopulation);
    this.tree = new TreeFactory(metaPopulation);
  }

  public load(service: string, params?: any): Observable<Loaded> {

    return this.database
      .pull(service, params)
      .pipe(
        switchMap((pullResponse: PullResponse) => {
          const requireLoadIds: SyncRequest = this.workspace.diff(pullResponse);

          if (requireLoadIds.objects.length > 0) {
            return this.database
              .sync(requireLoadIds)
              .pipe(
                map((syncResponse: SyncResponse) => {
                  this.workspace.sync(syncResponse);
                  const loaded: Loaded = new Loaded(this.session, pullResponse);
                  return loaded;
                }));
          } else {
            const loaded: Loaded = new Loaded(this.session, pullResponse);
            return of(loaded);
          }
        })
      );
  }

  public save(): Observable<Saved> {

    const pushRequest: PushRequest = this.session.pushRequest();
    return this.database
      .push(pushRequest)
      .pipe(
        switchMap((pushResponse: PushResponse) => {

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
            .pipe(
              map((syncResponse: SyncResponse) => {
                this.workspace.sync(syncResponse);
                const saved: Saved = new Saved(this.session, pushResponse);
                return saved;
              })
            );
        })
      );
  }

  public invoke(method: Method): Observable<Invoked>;
  public invoke(methods: Method[], options?: InvokeOptions): Observable<Invoked>;
  public invoke(service: string, args?: any): Observable<Invoked>;
  public invoke(methodOrService: Method | Method[] | string, args?: any): Observable<Invoked> {

    return this.database
      .invoke(methodOrService as any, args)
      .pipe(
        map((invokeResponse: InvokeResponse) => new Invoked(this.session, invokeResponse))
      );
  }
}

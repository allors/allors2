import { Observable, of } from 'rxjs';

import { ISession, Session, PullResponse, SyncRequest, IWorkspace, SyncResponse, PushRequest, PushResponse, PushRequestObject, Method, InvokeOptions, InvokeResponse, ObjectType, ISessionObject, SecurityResponse, Workspace } from '../../../framework';

import { Loaded } from './responses/Loaded';
import { switchMap, map } from 'rxjs/operators';
import { Saved } from './responses/Saved';
import { Invoked } from './responses/Invoked';
import { Database } from './Database';


export class Context {
  session: ISession;

  constructor(
    public database: Database,
    public workspace: Workspace
  ) {
    this.session = new Session(this.workspace);
  }

  get hasChanges() {
    return this.session.hasChanges;
  }

  reset() {
    this.session.reset();
  }

  get(id: string): ISessionObject {
    return this.session.get(id);
  }

  create(objectType: ObjectType | string): ISessionObject {
    return this.session.create(objectType);
  }

  delete(object: ISessionObject) {
    this.session.delete(object);
  }

  load(params?: any): Observable<Loaded> {

    return this.database
      .pull(params)
      .pipe(
        switchMap((pullResponse: PullResponse) => {
          const requireLoadIds: SyncRequest = this.workspace.diff(pullResponse);

          if (requireLoadIds.objects.length > 0) {
            return this.database
              .sync(requireLoadIds)
              .pipe(
                switchMap((syncResponse: SyncResponse) => {
                  const securityRequest = this.workspace.sync(syncResponse);
                  if (securityRequest) {
                    return this.database
                      .security(securityRequest)
                      .pipe(
                        map((securityResponse: SecurityResponse) => {
                          this.workspace.security(securityResponse);
                          const loaded: Loaded = new Loaded(this.session, pullResponse);
                          return loaded;
                        })
                      )
                  } else {
                    const loaded: Loaded = new Loaded(this.session, pullResponse);
                    return of(loaded);
                  }
                }));
          } else {
            const loaded: Loaded = new Loaded(this.session, pullResponse);
            return of(loaded);
          }
        })
      );
  }

  save(): Observable<Saved> {

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

  invoke(method: Method): Observable<Invoked>;
  invoke(methods: Method[], options?: InvokeOptions): Observable<Invoked>;
  invoke(service: string, args?: any): Observable<Invoked>;
  invoke(methodOrService: Method | Method[] | string, args?: any): Observable<Invoked> {

    return this.database
      .invoke(methodOrService as any, args)
      .pipe(
        map((invokeResponse: InvokeResponse) => new Invoked(this.session, invokeResponse))
      );
  }
}

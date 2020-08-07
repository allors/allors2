import { ISession, Workspace, Session, Method } from '@allors/domain/system';
import { Pull } from '@allors/data/system';
import { PullRequest, PullResponse, SyncResponse, PushRequest, PushResponse, SyncRequest, PushRequestObject, InvokeResponse } from '@allors/protocol/system';

import { Database } from './Database';

import { Invoked } from './responses/Invoked';
import { Loaded } from './responses/Loaded';
import { Saved } from './responses/Saved';

export class Context {
  public session: ISession;

  constructor(public database: Database, public workspace: Workspace) {
    this.session = new Session(this.workspace);
  }

  public load(pull: Pull | PullRequest): Promise<Loaded>;
  public load(customService: string, customArgs?: any): Promise<Loaded>;
  public load(requestOrCustomService: PullRequest | Pull | string, customArgs?: any): Promise<Loaded> {
    return new Promise((resolve, reject) => {
      this.database
        .pull(requestOrCustomService, customArgs)
        .then((pullResponse: PullResponse) => {
          const syncRequest = this.workspace.diff(pullResponse);
          if (syncRequest.objects.length > 0) {
            return this.database
              .sync(syncRequest)
              .then((syncResponse: SyncResponse) => {
                const securityRequest = this.workspace.sync(syncResponse);
                if (securityRequest) {
                  this.database
                    .security(securityRequest)
                    .then((v) => {
                      const securityRequest2 = this.workspace.security(v);
                      if (securityRequest2) {
                        this.database
                          .security(securityRequest2)
                          .then((v) => {
                            this.workspace.security(v);
                            const loaded = new Loaded(this.session, pullResponse);
                            resolve(loaded);
                          })
                          .catch((e) => {
                            reject(e);
                          });
                      } else {
                        const loaded = new Loaded(this.session, pullResponse);
                        resolve(loaded);
                      }
                    })
                    .catch((e) => {
                      reject(e);
                    });
                } else {
                  const loaded = new Loaded(this.session, pullResponse);
                  resolve(loaded);
                }
              })
              .catch((e) => {
                reject(e);
              });
          } else {
            const loaded = new Loaded(this.session, pullResponse);
            resolve(loaded);
            return;
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
          const syncRequest: SyncRequest = {
            objects: pushRequest.objects?.map((v: PushRequestObject) => v.i) ?? []
          };

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

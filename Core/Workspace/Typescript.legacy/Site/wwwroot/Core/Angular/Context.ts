/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
namespace Allors {
    export class Context {
        $q: angular.IQService;

        session: ISession;

        objects: { [name: string]: ISessionObject; } = {};
        collections: { [name: string]: ISessionObject[]; } = {};
        values: { [name: string]: any; } = {};

        constructor(public name: string, public database: Database, public workspace: Workspace) {

            this.$q = this.database.$q;
            this.session = new Session(this.workspace);
        }

        load(params?: any): angular.IPromise<any> {
            return this.$q((resolve, reject) => {

                return this.database
                    .pull(this.name, params)
                    .then((response: Protocol.PullResponse) => {
                        try {
                            const syncRequest = this.workspace.diff(response);
                            if (syncRequest.objects.length > 0) {
                                this.database.sync(syncRequest)
                                    .then((syncResponse: Protocol.SyncResponse) => {
                                        const securityRequest = this.workspace.sync(syncResponse);
                                        this.update(response);
                                        this.session.reset();

                                        if (securityRequest) {
                                          this.database
                                            .security(securityRequest)
                                            .then(v => {
                                              const securityRequest2 = this.workspace.security(v);
                                              if (securityRequest2) {
                                                this.database
                                                  .security(securityRequest2)
                                                  .then(v => {
                                                    this.workspace.security(v);
                                                    resolve();
                                                  })
                                                  .catch((e) => {
                                                    reject(e);
                                                  });
                                              } else {
                                                resolve();
                                              }
                                            })
                                            .catch((e) => {
                                              reject(e);
                                            });
                                        } else {
                                          resolve();
                                        }
                                    })
                                    .catch(e2 => {
                                        reject(e2);
                                    });
                            } else {
                                this.update(response);
                                this.session.reset();
                                resolve();
                            }
                        } catch (e) {
                            reject(e);
                        }
                    })
                    .catch(e => {
                        reject(e);
                    });

            });
        }

        query(service: string, params: any): angular.IPromise<Loaded> {
            return this.$q((resolve, reject) => {

                this.database.pull(service, params)
                    .then(v => {
                        try {
                            const response = v as Protocol.PullResponse;
                            const requireLoadIds = this.workspace.diff(response);

                            if (requireLoadIds.objects.length > 0) {
                                this.database.sync(requireLoadIds)
                                    .then(u => {
                                        var loadResponse = u as Protocol.SyncResponse;
                                        this.workspace.sync(loadResponse);
                                        const result = new Loaded(this.session, response);
                                        resolve(result);
                                    })
                                    .catch((e2) => reject(e2));
                            } else {
                                const result = new Loaded(this.session, response);
                                resolve(result);
                            }
                        } catch (e) {
                            reject(e);
                        }
                    })
                    .catch((e) => reject(e));

            });
        }
        
        save(): angular.IPromise<Protocol.PushResponse> {
            return this.$q((resolve, reject) => {

                try {
                    const pushRequest = this.session.pushRequest();
                    this.database
                        .push(pushRequest)
                        .then((pushResponse: Protocol.PushResponse) => {
                            try {
                                this.session.pushResponse(pushResponse);

                                const syncRequest = new Protocol.SyncRequest();
                                syncRequest.objects = pushRequest.objects.map(v=>v.i);
                                if (pushResponse.newObjects) {
                                    for (let newObject of pushResponse.newObjects) {
                                        syncRequest.objects.push(newObject.i);
                                    }
                                }

                                this.database.sync(syncRequest)
                                    .then((syncResponse) => {
                                        this.workspace.sync(syncResponse);
                                        this.session.reset();
                                        resolve(pushResponse);
                                    })
                                    .catch((reason) => {
                                        reject(reason);
                                    });
                            } catch (e3) {
                                reject(e3);
                            }
                        })
                        .catch(e2 => {
                            reject(e2);
                        });
                } catch (e) {
                    reject(e);
                }

            });
        }

        invoke(method: Method): angular.IPromise<Protocol.InvokeResponse>;
        invoke(service: string, args?: any): angular.IPromise<Protocol.InvokeResponse>;
        invoke(methodOrService: Method | string, args?: any): angular.IPromise<Protocol.InvokeResponse> {

            if (methodOrService instanceof Method) {
                return this.database.invoke(methodOrService);
            } else {
                return this.database.invoke(methodOrService as string, args);
            }
        }
        
        private update(response: Protocol.PullResponse): void {

            this.objects = { };
            this.collections  = { };
            this.values = { };

            _.map(response.namedObjects, (v, k) => {
                this.objects[k] = this.session.get(v);
            });

            _.map(response.namedCollections, (v, k) => {
                this.collections[k] = _.map(v, (obj) => { return this.session.get(obj) });
            });

            _.map(response.namedValues, (v, k) => {
                this.values[k] = v;
            });
        }
    }
}

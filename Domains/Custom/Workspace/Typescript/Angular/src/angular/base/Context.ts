import { Database } from "./Database";
import { Result } from "./Result";

import { Workspace } from "../../domain/base/Workspace";
import { ISession } from "../../domain/base/Session";
import { Session } from "../../domain/base/Session";
import { ISessionObject } from "../../domain/base/SessionObject";
import { Method } from "../../domain/base/Method";

import { SyncRequest } from "../../domain/base/data/requests/SyncRequest";
import { PullResponse } from "../../domain/base/data/responses/PullResponse";
import { SyncResponse } from "../../domain/base/data/responses/SyncResponse";
import { PushResponse } from "../../domain/base/data/responses/PushResponse";
import { InvokeResponse } from "../../domain/base/data/responses/InvokeResponse";

export class Context {
    session: ISession;

    objects: { [name: string]: ISessionObject; } = {};
    collections: { [name: string]: ISessionObject[]; } = {};
    values: { [name: string]: any; } = {};

    constructor(public name: string, public database: Database, public workspace: Workspace) {

        this.session = new Session(this.workspace);
    }

    load(params?: any): Promise<any> {
        return new Promise((resolve, reject) => {

            return this.database
                .pull(this.name, params)
                .then((response: PullResponse) => {
                    try {
                        const requireLoadIds = this.workspace.diff(response);

                        if (requireLoadIds.objects.length > 0) {
                            this.database.sync(requireLoadIds)
                                .then((loadResponse: SyncResponse) => {
                                    this.workspace.sync(loadResponse);
                                    this.update(response);
                                    this.session.reset();
                                    resolve();
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

    query(service: string, params: any): Promise<Result> {
        return new Promise((resolve, reject) => {

            this.database.pull(service, params)
                .then(v => {
                    try {
                        const response = v as PullResponse;
                        const requireLoadIds = this.workspace.diff(response);

                        if (requireLoadIds.objects.length > 0) {
                            this.database.sync(requireLoadIds)
                                .then(u => {
                                    var loadResponse = u as SyncResponse;
                                    this.workspace.sync(loadResponse);
                                    const result = new Result(this.session, response);
                                    resolve(result);
                                })
                                .catch((e2) => reject(e2));
                        } else {
                            const result = new Result(this.session, response);
                            resolve(result);
                        }
                    } catch (e) {
                        reject(e);
                    }
                })
                .catch((e) => reject(e));

        });
    }
    
    save(): Promise<PushResponse> {
        return new Promise((resolve, reject) => {

            try {
                const pushRequest = this.session.pushRequest();
                this.database
                    .push(pushRequest)
                    .then((pushResponse: PushResponse) => {
                        try {
                            this.session.pushResponse(pushResponse);

                            const syncRequest = new SyncRequest();
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

    invoke(method: Method): Promise<InvokeResponse>;
    invoke(service: string, args?: any): Promise<InvokeResponse>;
    invoke(methodOrService: Method | string, args?: any): Promise<InvokeResponse> {

        if (methodOrService instanceof Method) {
            return this.database.invoke(methodOrService);
        } else {
            return this.database.invoke(methodOrService as string, args);
        }
    }
    
    private update(response: PullResponse): void {

        this.objects = { };
        this.collections  = { };
        this.values = { };

        // TODO: Deduplicate
        var namedObjects = response.namedObjects;
        var namedCollections = response.namedCollections;
        var namedValues = response.namedValues;

        Object.keys(namedObjects).map((k) => this.objects[k] = this.session.get(namedObjects[k]));
        Object.keys(namedCollections).map((k) => this.collections[k] = namedCollections[k].map((obj) => { return this.session.get(obj)}));
        Object.keys(namedValues).map((k) => this.values[k] = this.session.get(namedValues[k]));
    }
}

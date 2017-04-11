import { Context } from "./Context";
import { Database } from "./Database";

import { Workspace } from "../../domain/base/Workspace";
import { ISession } from "../../domain/base/Session";
import { Session } from "../../domain/base/Session";
import { ISessionObject } from "../../domain/base/SessionObject";
import { Method } from "../../domain/base/Method";

import { ResponseType } from "../../domain/base/data/responses/ResponseType";
import { PushRequest } from "../../domain/base/data/requests/PushRequest";
import { SyncRequest } from "../../domain/base/data/requests/SyncRequest";
import { InvokeRequest } from "../../domain/base/data/requests/InvokeRequest";
import { PullResponse } from "../../domain/base/data/responses/PullResponse";
import { SyncResponse } from "../../domain/base/data/responses/SyncResponse";
import { PushResponse } from "../../domain/base/data/responses/PushResponse";
import { InvokeResponse } from "../../domain/base/data/responses/InvokeResponse";

export abstract class Scope {

    context: Context;
    session: ISession;

    constructor(name: string, database: Database, workspace: Workspace) {
        this.context = new Context(name, database, workspace);
        this.session = this.context.session;
   }

    // Context
    get objects(): { [name: string]: ISessionObject; } {
        return this.context.objects;
    }

    get collections(): { [name: string]: ISessionObject[]; } {
        return this.context.collections;
    }

    get values(): { [name: string]: any; } {
        return this.context.values;
    }

    get hasChanges(): boolean {
        return this.context.session.hasChanges;
    }

    // Commands
    load(params?: any): Promise<any> {
        return this.context.load(params);
    }

    save(): Promise<any> {
        return this.context.save();
    }

    invoke(method: Method): Promise<InvokeResponse>;
    invoke(service: string, args?: any): Promise<InvokeResponse>;
    invoke(methodOrService: Method | string, args?: any): Promise<InvokeResponse> {
        return new Promise((resolve, reject) => {

            if (methodOrService instanceof Method) {
                this.context
                    .invoke(methodOrService)
                    .then((invokeResponse: InvokeResponse) => {
                        resolve(invokeResponse);
                    })
                    .catch(()=>reject());;
            } else {
                this.context
                    .invoke(methodOrService as string, args)
                    .then((invokeResponse: InvokeResponse) => {
                        resolve(invokeResponse);
                    })
                    .catch(()=>reject());
            }
        });
    }

    saveAndInvoke(method: Method):Promise<InvokeResponse>;
    saveAndInvoke(service: string, args?: any): Promise<any>;
    saveAndInvoke(methodOrService: Method | string, args?: any): Promise<any> {
        return new Promise((resolve, reject) => {

            return this.context
                .save()
                .then(() => {
                        this.refresh()
                            .then(() => {
                                if (methodOrService instanceof Method) {
                                    this.context.invoke(methodOrService)
                                        .then((invokeResponse: InvokeResponse) => {
                                            resolve(invokeResponse);
                                        })
                                        .catch(e => reject(e))
                                } else {
                                    this.context.invoke(methodOrService as string, args)
                                        .then((invokeResponse: InvokeResponse) => {
                                            resolve(invokeResponse);
                                        })
                                        .catch(e => reject(e))
                                }
                            })
                            .catch(e => reject(e));
                        
                })
                .catch(e => reject(e));
        });
    }

    query(query: string, args: any): Promise<any> {
        return this.context.query(query, args);
    }

    queryResults(query: string, args: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.context
                .query(query, args)
                .then(result => {
                    var results = result.collections["results"];
                    resolve(results);
                })
                .catch((e) => reject(e));
        });
    }

    protected abstract refresh(): Promise<any>;
}

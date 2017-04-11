import {Observable} from "rxjs";

import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Method } from "../../domain/base/Method";

import { ResponseType } from "../../domain/base/data/responses/ResponseType";
import { PushRequest } from "../../domain/base/data/requests/PushRequest";
import { SyncRequest } from "../../domain/base/data/requests/SyncRequest";
import { InvokeRequest } from "../../domain/base/data/requests/InvokeRequest";
import { PullResponse } from "../../domain/base/data/responses/PullResponse";
import { SyncResponse } from "../../domain/base/data/responses/SyncResponse";
import { PushResponse } from "../../domain/base/data/responses/PushResponse";
import { InvokeResponse } from "../../domain/base/data/responses/InvokeResponse";

export class Database {
    options: RequestOptions;

    constructor(private $http: Http, public url: string) {
        let headers = new Headers(
            { 
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            });
        this.options = new RequestOptions({ headers: headers });
    }

    pull(name: string, params?: any): Promise<PullResponse> {
        return new Promise((resolve, reject) => {

            const serviceName = this.fullyQualifiedUrl(name + "/Pull");
            this.$http.post(serviceName, params, this.options)
                .toPromise()
                .then((callbackArg) => {
                    var response = callbackArg.json();
                    response.responseType = ResponseType.Pull;
                    resolve(response);
                })
                .catch(e => {
                    reject(e);
                });

        });
    }

    sync(syncRequest: SyncRequest): Promise<SyncResponse> {
        return new Promise((resolve, reject) => {

            const serviceName = this.fullyQualifiedUrl("Database/Sync");
            this.$http.post(serviceName, syncRequest, this.options)
                .toPromise()
                .then((callbackArg) => {
                    var response = callbackArg.json();
                    response.responseType = ResponseType.Sync;
                    resolve(response);
                })
                .catch(e => {
                    reject(e);
                });

        });
    }

    push(pushRequest: PushRequest): Promise<PushResponse> {
        return new Promise((resolve, reject) => {
         
            const serviceName = this.fullyQualifiedUrl("Database/Push");
            this.$http.post(serviceName, pushRequest, this.options)
                .toPromise()
                .then((callbackArg) => {
                    var response = callbackArg.json();
                    response.responseType = ResponseType.Sync;

                    if (response.hasErrors) {
                        reject(response);
                    } else {
                        resolve(response);
                    }
                })
                .catch(e => {
                    reject(e);
                });

        });
    }

    invoke(method: Method): Promise<InvokeResponse>;
    invoke(service: string, args?: any): Promise<InvokeResponse>;
    invoke(methodOrService: Method | string, args?: any): Promise<InvokeResponse> {
        return new Promise((resolve, reject) => {
    
            if (methodOrService instanceof Method) {
                const method = methodOrService;
                const invokeRequest: InvokeRequest = {
                    i: method.object.id,
                    v: method.object.version,
                    m: method.name
                };

                const serviceName = this.fullyQualifiedUrl("Database/Invoke");
                this.$http.post(serviceName, invokeRequest, this.options)
                    .toPromise()
                    .then((callbackArg) => {
                        var response = callbackArg.json();
                        response.responseType = ResponseType.Invoke;

                        if (response.hasErrors) {
                            reject(response);
                        } else {
                            resolve(response);
                        }
                    })
                    .catch(e => {
                        reject(e);
                    });
            }
            else {
                const service =  this.fullyQualifiedUrl(methodOrService + "/Pull");
                this.$http.post(service, args, this.options)
                    .toPromise()
                    .then((callbackArg) => {
                        var response = callbackArg.json();
                        response.responseType = ResponseType.Invoke;

                        if (response.hasErrors) {
                            reject(response);
                        } else {
                            resolve(response);
                        }
                    })
                    .catch(e => {
                        reject(e);
                    });
            }

        });
    }

    private fullyQualifiedUrl(localUrl: string): string {
        return this.url + localUrl;
    }
}

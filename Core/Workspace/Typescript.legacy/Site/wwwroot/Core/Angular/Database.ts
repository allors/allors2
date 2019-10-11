/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />

namespace Allors {
    export class Database {
        constructor(private $http: angular.IHttpService, public $q: angular.IQService, public postfix: string, public baseUrl: string) {
        }

        authorization: string;

        get headers(): any {
            return this.authorization ? {
                headers: { 'Authorization': this.authorization }
            } : undefined;
        }

        pull(name: string, params?: any): angular.IPromise<Protocol.PullResponse> {
            return this.$q((resolve, reject) => {

                const serviceName = `${this.baseUrl}/${name}${this.postfix}`;
                this.$http.post(serviceName, params || {}, this.headers)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Protocol.PullResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Protocol.ResponseType.Pull;
                        resolve(response);
                    })
                    .catch(e => {
                        reject(e);
                    });

            });
        }

        sync(syncRequest: Protocol.SyncRequest): angular.IPromise<Protocol.SyncResponse> {
            return this.$q((resolve, reject) => {

                const serviceName = `${this.baseUrl}allors/sync`;
                this.$http.post(serviceName, syncRequest, this.headers)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Protocol.SyncResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Protocol.ResponseType.Sync;
                        resolve(response);
                    })
                    .catch(e => {
                        reject(e);
                    });

            });
        }

        push(pushRequest: Protocol.PushRequest): angular.IPromise<Protocol.PushResponse> {
            return this.$q((resolve, reject) => {

                const serviceName = `${this.baseUrl}allors/push`;
                this.$http.post(serviceName, pushRequest, this.headers)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Protocol.PushResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Protocol.ResponseType.Sync;

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

        invoke(method: Method): angular.IPromise<Protocol.InvokeResponse>;
        invoke(methods: Method[], options: Protocol.InvokeOptions): angular.IPromise<Protocol.InvokeResponse>;
        invoke(service: string, args?: any): angular.IPromise<Protocol.InvokeResponse>;
        invoke(methodOrService: Method | Method[] | string, args?: any): angular.IPromise<Protocol.InvokeResponse> {
            if (methodOrService instanceof Method) {
                return this.invokeMethods([methodOrService]);
            } else if (methodOrService instanceof Array) {
                return this.invokeMethods(methodOrService, args);
            } else {
                return this.invokeService(methodOrService, args);
            }
        }

        private invokeMethods(methods: Method[], options?: Protocol.InvokeOptions): angular.IPromise<Protocol.InvokeResponse> {

            return this.$q((resolve, reject) => {

                const invokeRequest: Protocol.InvokeRequest = {
                    i: methods.map(v => {
                        return {
                            i: v.object.id,
                            m: v.name,
                            v: v.object.version,
                        };
                    }),
                    o: options
                };

                const serviceName = `${this.baseUrl}allors/invoke`;
                this.$http.post(serviceName, invokeRequest, this.headers)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Protocol.InvokeResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Protocol.ResponseType.Invoke;

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

        private invokeService(methodOrService: string, args?: any): angular.IPromise<Protocol.InvokeResponse> {
            return this.$q((resolve, reject) => {

                const serviceName = this.baseUrl + methodOrService + this.postfix;
                this.$http.post(serviceName, args, this.headers)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Protocol.InvokeResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Protocol.ResponseType.Invoke;

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
    }
}

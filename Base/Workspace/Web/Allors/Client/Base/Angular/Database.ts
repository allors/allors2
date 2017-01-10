namespace Allors {
    export class Database {
        constructor(private $http: angular.IHttpService, public $q: angular.IQService, public prefix: string, public postfix: string) {
        }

        pull(name: string, params?: any): angular.IPromise<Data.PullResponse> {
            return this.$q((resolve, reject) => {

                const serviceName = name + this.postfix;
                this.$http.post(serviceName, params)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Data.PullResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Data.ResponseType.Pull;
                        resolve(response);
                    })
                    .catch(e => {
                        reject(e);
                    });

            });
        }

        sync(syncRequest: Data.SyncRequest): angular.IPromise<Data.SyncResponse> {
            return this.$q((resolve, reject) => {

                this.$http.post(`${this.prefix}Sync`, syncRequest)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Data.SyncResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Data.ResponseType.Sync;
                        resolve(response);
                    })
                    .catch(e => {
                        reject(e);
                    });

            });
        }

        push(pushRequest: Data.PushRequest): angular.IPromise<Data.PushResponse> {
            return this.$q((resolve, reject) => {

                this.$http.post(`${this.prefix}Push`, pushRequest)
                    .then((callbackArg: angular.IHttpPromiseCallbackArg<Data.PushResponse>) => {
                        var response = callbackArg.data;
                        response.responseType = Data.ResponseType.Sync;

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

        invoke(method: Method): angular.IPromise<Data.InvokeResponse>;
        invoke(service: string, args?: any): angular.IPromise<Data.InvokeResponse>;
        invoke(methodOrService: Method | string, args?: any): angular.IPromise<Data.InvokeResponse> {
            return this.$q((resolve, reject) => {

                if (methodOrService instanceof Method) {
                    const method = methodOrService;
                    const invokeRequest: Data.InvokeRequest = {
                        i: method.object.id,
                        v: method.object.version,
                        m: method.name
                    };

                    this.$http.post(`${this.prefix}Invoke`, invokeRequest)
                        .then((callbackArg: angular.IHttpPromiseCallbackArg<Data.InvokeResponse>) => {
                            var response = callbackArg.data;
                            response.responseType = Data.ResponseType.Invoke;

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
                    const service = methodOrService + this.postfix;
                    this.$http.post(service, args)
                        .then((callbackArg: angular.IHttpPromiseCallbackArg<Data.InvokeResponse>) => {
                            var response = callbackArg.data;
                            response.responseType = Data.ResponseType.Invoke;

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
    }
}
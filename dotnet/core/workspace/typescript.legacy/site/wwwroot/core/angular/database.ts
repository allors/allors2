/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
namespace Allors {

  export class Database {
    constructor(private $http: angular.IHttpService, public $q: angular.IQService, public baseUrl: string) {
    }

    authorization: string;

    get headers(): any {
      return this.authorization ? {
        headers: { 'Authorization': this.authorization }
      } : undefined;
    }

    pull(name: string, params?: any): angular.IPromise<Data.PullResponse> {
      return this.$q((resolve, reject) => {

        const serviceName = `${this.baseUrl}/${name}/Pull`;
        this.$http.post(serviceName, params || {}, this.headers)
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

        const serviceName = `${this.baseUrl}/Sync`;
        this.$http.post(serviceName, syncRequest, this.headers)
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

        const serviceName = `${this.baseUrl}/Push`;
        this.$http.post(serviceName, pushRequest, this.headers)
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

          const serviceName = `${this.baseUrl}/Invoke`;
          this.$http.post(serviceName, invokeRequest, this.headers)
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
          const serviceName = this.baseUrl + methodOrService + "/Pull";
          this.$http.post(serviceName, args, this.headers)
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

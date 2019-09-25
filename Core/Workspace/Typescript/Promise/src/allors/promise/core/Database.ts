import { InvokeRequest, InvokeResponse, PullResponse, PushRequest, PushResponse, ResponseError, ResponseType, SyncRequest, SyncResponse, InvokeOptions } from '../../framework';
import { Method } from '../../framework';
import { services } from '../../framework/database';

import { Http } from './http/Http';
import { HttpResponse } from './http/HttpResponse';

export class Database {

  constructor(private http: Http) {
  }

  public pull(params?: any): Promise<PullResponse> {
    return new Promise((resolve, reject) => {

      this.http.post(services.pull, params || {})
        .then((httpResponse: HttpResponse) => {
          const response = httpResponse.data;
          response.responseType = ResponseType.Pull;
          resolve(response);
        })
        .catch((e) => {
          reject(e);
        });

    });
  }

  public sync(syncRequest: SyncRequest): Promise<SyncResponse> {
    return new Promise((resolve, reject) => {

      this.http.post(services.sync, syncRequest)
        .then((httpResponse: HttpResponse) => {
          const response = httpResponse.data;
          response.responseType = ResponseType.Sync;
          resolve(response);
        })
        .catch((e) => {
          reject(e);
        });

    });
  }

  public push(pushRequest: PushRequest): Promise<PushResponse> {
    return new Promise((resolve, reject) => {

      this.http.post(services.push, pushRequest)
        .then((httpResponse: HttpResponse) => {
          const response = httpResponse.data;
          response.responseType = ResponseType.Sync;

          if (response.hasErrors) {
            reject(response);
          } else {
            resolve(response);
          }
        })
        .catch((e) => {
          reject(e);
        });

    });
  }

  public invoke(method: Method): Promise<InvokeResponse>;
  public invoke(methods: Method[], isolated: boolean, continueOnError: boolean): Promise<InvokeResponse>;
  public invoke(service: string, args?: any): Promise<InvokeResponse>;
  public invoke(methodsOrMethodOrService: Method[] | Method | string, args?: any, continueOnError?: boolean): Promise<InvokeResponse> {
    return new Promise((resolve, reject) => {

      if (methodsOrMethodOrService instanceof Array) {
        const methods = methodsOrMethodOrService as Method[];
        const invokeRequest: InvokeRequest = {
          i: methods.map((v) => {
            return {
              i: v.object.id,
              m: v.name,
              v: v.object.version,
            };
          })
        };

        this.http.post(services.invoke, invokeRequest)
          .then((httpResponse: HttpResponse) => {
            const response = httpResponse.data;
            response.responseType = ResponseType.Invoke;

            if (response.hasErrors) {
              reject(response);
            } else {
              resolve(response);
            }
          })
          .catch((e) => {
            reject(e);
          });
      } else if (methodsOrMethodOrService instanceof Method) {
        const method = methodsOrMethodOrService;
        const invokeRequest: InvokeRequest = {
          i: [{
            i: method.object.id,
            m: method.name,
            v: method.object.version,
          }]
        };

        this.http.post(services.invoke, invokeRequest)
          .then((httpResponse: HttpResponse) => {
            const response = httpResponse.data;
            response.responseType = ResponseType.Invoke;

            if (response.hasErrors) {
              reject(response);
            } else {
              resolve(response);
            }
          })
          .catch((e) => {
            reject(e);
          });
      } else {
        const serviceName = `${methodsOrMethodOrService}`;
        this.http.post(serviceName, args)
          .then((httpResponse: HttpResponse) => {
            const response = httpResponse.data;
            response.responseType = ResponseType.Invoke;

            if (response.hasErrors) {
              reject(response);
            } else {
              resolve(response);
            }
          })
          .catch((e) => {
            reject(e);
          });
      }
    });
  }
}

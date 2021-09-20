import {
  PullRequest,
  PullResponse,
  services,
  ResponseType,
  SyncRequest,
  SyncResponse,
  PushRequest,
  PushResponse,
  SecurityRequest,
  SecurityResponse,
  InvokeResponse,
  InvokeRequest,
} from '@allors/protocol/system';
import { Pull } from '@allors/data/system';
import { Method } from '@allors/domain/system';
import { assert } from '@allors/meta/system';

import { Http } from './http/Http';
import { HttpResponse } from './http/HttpResponse';

export class Database {
  constructor(private http: Http) {}

  pull(requestOrCustomService: PullRequest | Pull | string, customArgs?: any): Promise<PullResponse> {
    return new Promise((resolve, reject) => {
      let service = services.pull;
      let params: PullRequest | any;

      if (typeof requestOrCustomService === 'string') {
        service = requestOrCustomService;
        params = customArgs;
      } else {
        if (requestOrCustomService instanceof Pull) {
          params = new PullRequest({ pulls: [requestOrCustomService] });
        } else {
          params = requestOrCustomService;
        }
      }

      this.http
        .post(service, params || {})
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

  sync(syncRequest: SyncRequest): Promise<SyncResponse> {
    return new Promise((resolve, reject) => {
      this.http
        .post(services.sync, syncRequest)
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

  push(pushRequest: PushRequest): Promise<PushResponse> {
    return new Promise((resolve, reject) => {
      this.http
        .post(services.push, pushRequest)
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

  security(securityRequest: SecurityRequest): Promise<SecurityResponse> {
    return new Promise((resolve, reject) => {
      this.http
        .post(services.security, securityRequest)
        .then((httpResponse: HttpResponse) => {
          const response = httpResponse.data;
          response.responseType = ResponseType.Security;
          resolve(response);
        })
        .catch((e) => {
          reject(e);
        });
    });
  }

  invoke(method: Method): Promise<InvokeResponse>;
  invoke(methods: Method[], isolated: boolean, continueOnError: boolean): Promise<InvokeResponse>;
  invoke(service: string, args?: any): Promise<InvokeResponse>;
  invoke(methodsOrMethodOrService: Method[] | Method | string, args?: any, continueOnError?: boolean): Promise<InvokeResponse> {
    return new Promise((resolve, reject) => {
      if (methodsOrMethodOrService instanceof Array) {
        const methods = methodsOrMethodOrService as Method[];
        const invokeRequest: InvokeRequest = {
          i: methods.map((v) => {
            assert(v.object.version);
            return {
              i: v.object.id,
              v: v.object.version,
              m: v.methodType.id,
            };
          }),
        };

        this.http
          .post(services.invoke, invokeRequest)
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
        assert(method.object.version);
        const invokeRequest: InvokeRequest = {
          i: [
            {
              i: method.object.id,
              v: method.object.version,
              m: method.methodType.id,
            },
          ],
        };

        this.http
          .post(services.invoke, invokeRequest)
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
        this.http
          .post(serviceName, args)
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

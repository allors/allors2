import { Method } from "@allors/base-domain";
import { InvokeRequest, InvokeResponse, PullResponse, PushRequest, PushResponse, ResponseError, ResponseType, SyncRequest, SyncResponse } from "@allors/base-domain";

import { Http } from "./http/Http";
import { HttpResponse } from "./http/HttpResponse";

export class Database {

  constructor(private http: Http) {
  }

  public pull(name: string, params?: any): Promise<PullResponse> {
      return new Promise((resolve, reject) => {

          const serviceName = `${name}/Pull`;
          this.http.post(serviceName, params || {})
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

          const serviceName = `Database/Sync`;
          this.http.post(serviceName, syncRequest)
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

          const serviceName = `Database/Push`;
          this.http.post(serviceName, pushRequest)
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
  public invoke(service: string, args?: any): Promise<InvokeResponse>;
  public invoke(methodOrService: Method | string, args?: any): Promise<InvokeResponse> {
      return new Promise((resolve, reject) => {

          if (methodOrService instanceof Method) {
              const method = methodOrService;
              const invokeRequest: InvokeRequest = {
                  i: method.object.id,
                  m: method.name,
                  v: method.object.version,
              };

              const serviceName = `Database/Invoke`;
              this.http.post(serviceName, invokeRequest)
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
              const serviceName = `${methodOrService}/Pull`;
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

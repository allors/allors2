import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { InvokeRequest, InvokeResponse, PullResponse, PushRequest, PushResponse, ResponseError, ResponseType, SyncRequest, SyncResponse } from '../../../framework';
import { Method } from '../../../framework';

export class Database {

  constructor(private http: HttpClient, public url: string) {
  }

  public pull(name: string, params?: any): Observable<PullResponse> {
    const serviceName: string = this.fullyQualifiedUrl(name + '/Pull');

    return this.http
      .post<PullResponse>(serviceName, params)
      .pipe(
        map((pullResponse) => {
          pullResponse.responseType = ResponseType.Pull;
          return pullResponse;
        })
      );
  }

  public sync(syncRequest: SyncRequest): Observable<SyncResponse> {

    const serviceName: string = this.fullyQualifiedUrl('Database/Sync');
    return this.http
      .post<SyncResponse>(serviceName, syncRequest)
      .pipe(
        map((syncResponse) => {
          syncResponse.responseType = ResponseType.Sync;
          return syncResponse;
        })
      );
  }

  public push(pushRequest: PushRequest): Observable<PushResponse> {

    const serviceName: string = this.fullyQualifiedUrl('Database/Push');
    return this.http
      .post<PushResponse>(serviceName, pushRequest)
      .pipe(
        map((pushResponse) => {
          pushResponse.responseType = ResponseType.Sync;

          if (pushResponse.hasErrors) {
            throw new ResponseError(pushResponse);
          }

          return pushResponse;
        })
      );
  }

  public invoke(method: Method): Observable<InvokeResponse>;
  public invoke(service: string, args?: any): Observable<InvokeResponse>;
  public invoke(methodOrService: Method | string, args?: any): Observable<InvokeResponse> {

    if (methodOrService instanceof Method) {
      return this.invokeMethod(methodOrService);
    } else {
      return this.invokeService(methodOrService, args);
    }
  }

  private invokeMethod(method: Method): Observable<InvokeResponse> {
    const invokeRequest: InvokeRequest = {
      i: method.object.id,
      m: method.name,
      v: method.object.version,
    };

    const serviceName: string = this.fullyQualifiedUrl('Database/Invoke');
    return this.http
      .post<InvokeResponse>(serviceName, invokeRequest)
      .pipe(
        map((invokeResponse) => {
          invokeResponse.responseType = ResponseType.Invoke;

          if (invokeResponse.hasErrors) {
            throw new ResponseError(invokeResponse);
          }

          return invokeResponse;
        }));
  }

  private invokeService(methodOrService: string, args?: any): Observable<InvokeResponse> {
    const service: string = this.fullyQualifiedUrl(methodOrService + '/Pull');
    return this.http
      .post<InvokeResponse>(service, args)
      .pipe(
        map((invokeResponse) => {
          invokeResponse.responseType = ResponseType.Invoke;

          if (invokeResponse.hasErrors) {
            throw new ResponseError(invokeResponse);
          }

          return invokeResponse;
        })
      );
  }

  private fullyQualifiedUrl(localUrl: string): string {
    return this.url + localUrl;
  }
}

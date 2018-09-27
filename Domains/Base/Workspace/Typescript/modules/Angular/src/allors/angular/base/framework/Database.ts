import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { InvokeRequest, InvokeResponse, PullResponse, PushRequest, PushResponse, ResponseError, ResponseType, SyncRequest, SyncResponse, InvokeOptions } from '../../../framework';
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
  public invoke(methods: Method[], options: InvokeOptions): Observable<InvokeResponse>;
  public invoke(service: string, args?: any): Observable<InvokeResponse>;
  public invoke(methodOrService: Method | Method[] | string, args?: any): Observable<InvokeResponse> {

    if (methodOrService instanceof Method) {
      return this.invokeMethods([methodOrService]);
    } else if (methodOrService instanceof Array) {
      return this.invokeMethods(methodOrService, args);
    } else {
      return this.invokeService(methodOrService, args);
    }
  }

  public invokeMethods(methods: Method[], options?: InvokeOptions): Observable<InvokeResponse> {
    const invokeRequest: InvokeRequest = {
      i: methods.map(v => {
        return {
          i: v.object.id,
          m: v.name,
          v: v.object.version,
        };
      }),
      o: options
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

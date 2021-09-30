import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { assert } from '@allors/meta/system';
import { Method } from '@allors/domain/system';
import { Pull } from '@allors/data/system';
import {
  PullResponse,
  PushRequest,
  PushResponse,
  ResponseError,
  SecurityRequest,
  SecurityResponse,
  ResponseType,
  SyncRequest,
  SyncResponse,
  PullRequest,
  InvokeRequest,
  InvokeResponse,
  InvokeOptions,
  services,
} from '@allors/protocol/system';

export class Database {
  constructor(private http: HttpClient, public url: string) {}

  pull(requestOrCustomService: PullRequest | Pull | string, customArgs?: any): Observable<PullResponse> {
    let service = this.fullyQualifiedUrl(services.pull);
    let params: PullRequest | any;

    if (typeof requestOrCustomService === 'string') {
      service = this.fullyQualifiedUrl(requestOrCustomService);
      params = customArgs;
    } else {
      if (requestOrCustomService instanceof Pull) {
        params = new PullRequest({ pulls: [requestOrCustomService] });
      } else {
        params = requestOrCustomService;
      }
    }

    return this.http.post<PullResponse>(service, params).pipe(
      map((pullResponse) => {
        pullResponse.responseType = ResponseType.Pull;
        return pullResponse;
      })
    );
  }

  sync(syncRequest: SyncRequest): Observable<SyncResponse> {
    const service = this.fullyQualifiedUrl(services.sync);
    return this.http.post<SyncResponse>(service, syncRequest).pipe(
      map((syncResponse) => {
        syncResponse.responseType = ResponseType.Sync;
        return syncResponse;
      })
    );
  }

  push(pushRequest: PushRequest): Observable<PushResponse> {
    const service = this.fullyQualifiedUrl(services.push);
    return this.http.post<PushResponse>(service, pushRequest).pipe(
      map((pushResponse) => {
        pushResponse.responseType = ResponseType.Sync;

        if (pushResponse.hasErrors) {
          throw new ResponseError(pushResponse);
        }

        return pushResponse;
      })
    );
  }

  security(securityRequest: SecurityRequest): Observable<SecurityResponse> {
    const service = this.fullyQualifiedUrl(services.security);
    return this.http.post<SecurityResponse>(service, securityRequest).pipe(
      map((securityResponse) => {
        securityResponse.responseType = ResponseType.Security;
        return securityResponse;
      })
    );
  }

  invoke(method: Method): Observable<InvokeResponse>;
  invoke(methods: Method[], options: InvokeOptions): Observable<InvokeResponse>;
  invoke(service: string, args?: any): Observable<InvokeResponse>;
  invoke(methodOrService: Method | Method[] | string, args?: any): Observable<InvokeResponse> {
    if (methodOrService instanceof Method) {
      return this.invokeMethods([methodOrService]);
    } else if (methodOrService instanceof Array) {
      return this.invokeMethods(methodOrService, args);
    } else {
      return this.invokeService(methodOrService, args);
    }
  }

  private invokeMethods(methods: Method[], options?: InvokeOptions): Observable<InvokeResponse> {
    const invokeRequest: InvokeRequest = {
      i: methods.map((v) => {
        assert(v.object.version);
        return {
          i: v.object.id,
          v: v.object.version,
          m: v.methodType.id,
        };
      }),
      o: options,
    };

    const service = this.fullyQualifiedUrl(services.invoke);
    return this.http.post<InvokeResponse>(service, invokeRequest).pipe(
      map((invokeResponse) => {
        invokeResponse.responseType = ResponseType.Invoke;

        if (invokeResponse.hasErrors) {
          throw new ResponseError(invokeResponse);
        }

        return invokeResponse;
      })
    );
  }

  private invokeService(methodOrService: string, args?: any): Observable<InvokeResponse> {
    const service: string = this.fullyQualifiedUrl(methodOrService);
    return this.http.post<InvokeResponse>(service, args).pipe(
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

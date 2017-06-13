import { Observable, Observer } from 'rxjs/Rx';

import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Method } from '../../domain';

import { ResponseType, PushRequest, SyncRequest, InvokeRequest, PullResponse, SyncResponse, PushResponse, InvokeResponse } from '../../domain';

export class Database {
    options: RequestOptions;

    constructor(private http: Http,
                public url: string,
                private postProcessRequestOptions: (requestOptions: RequestOptions) => RequestOptions = (v: RequestOptions) => v) {
        const headers = new Headers(
            {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            });
        this.options = new RequestOptions({ headers: headers });
    }

    pull(name: string, params?: any): Observable<PullResponse> {
        const serviceName = this.fullyQualifiedUrl(name + '/Pull');

        return this.http
            .post(serviceName, params, this.postProcessRequestOptions(this.options))
            .map(v => {
                const response = v.json() as PullResponse;
                response.responseType = ResponseType.Pull;
                return response;
            });
    }

    sync(syncRequest: SyncRequest): Observable<SyncResponse> {

        const serviceName = this.fullyQualifiedUrl('Database/Sync');
        return this.http
            .post(serviceName, syncRequest, this.postProcessRequestOptions(this.options))
            .map(v => {
                const response = v.json() as SyncResponse;
                response.responseType = ResponseType.Sync;
                return response;
            });
    }

    push(pushRequest: PushRequest): Observable<PushResponse> {

        const serviceName = this.fullyQualifiedUrl('Database/Push');
        return this.http
            .post(serviceName, pushRequest, this.postProcessRequestOptions(this.options))
            .map(v => {
                const response = v.json() as PushResponse;
                response.responseType = ResponseType.Sync;

                if (response.hasErrors) {
                    return Observable.throw(response);
                }

                return response;
            });
    }

    invoke(method: Method): Observable<InvokeResponse>;
    invoke(service: string, args?: any): Observable<InvokeResponse>;
    invoke(methodOrService: Method | string, args?: any): Observable<InvokeResponse> {

        if (methodOrService instanceof Method) {
            return this.invokeMethod(methodOrService);
        } else {
            return this.invokeService(methodOrService, args);
        }
    }

    private invokeMethod(method: Method): Observable<InvokeResponse> {
            const invokeRequest: InvokeRequest = {
                i: method.object.id,
                v: method.object.version,
                m: method.name
            };

            const serviceName = this.fullyQualifiedUrl('Database/Invoke');
            return this.http.post(serviceName, invokeRequest, this.postProcessRequestOptions(this.options))
                .map(v => {
                    const response = v.json() as InvokeResponse;
                    response.responseType = ResponseType.Invoke;

                    if (response.hasErrors) {
                        return Observable.throw(response);
                    }

                    return response;
                });
    }

  private invokeService(methodOrService: string, args?: any): Observable<InvokeResponse> {
            const service =  this.fullyQualifiedUrl(methodOrService + '/Pull');
            return this.http.post(service, args, this.options)
                .map(v => {
                    const response = v.json();
                    response.responseType = ResponseType.Invoke;

                    if (response.hasErrors) {
                        return Observable.throw(response);
                    }

                    return response;
                });

    }

    private fullyQualifiedUrl(localUrl: string): string {
        return this.url + localUrl;
    }
}

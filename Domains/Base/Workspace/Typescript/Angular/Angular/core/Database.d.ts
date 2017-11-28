import { InvokeResponse, PullResponse, PushRequest, PushResponse, SyncRequest, SyncResponse } from "@allors/framework";
import { Method } from "@allors/framework";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
export declare class Database {
    private http;
    url: string;
    constructor(http: HttpClient, url: string);
    pull(name: string, params?: any): Observable<PullResponse>;
    sync(syncRequest: SyncRequest): Observable<SyncResponse>;
    push(pushRequest: PushRequest): Observable<PushResponse>;
    invoke(method: Method): Observable<InvokeResponse>;
    invoke(service: string, args?: any): Observable<InvokeResponse>;
    private invokeMethod(method);
    private invokeService(methodOrService, args?);
    private fullyQualifiedUrl(localUrl);
}

import { Method } from "@allors/base-domain";
import { InvokeResponse, PullResponse, PushRequest, PushResponse, SyncRequest, SyncResponse } from "@allors/base-domain";
import { Http } from "./http/Http";
export declare class Database {
    private http;
    constructor(http: Http);
    pull(name: string, params?: any): Promise<PullResponse>;
    sync(syncRequest: SyncRequest): Promise<SyncResponse>;
    push(pushRequest: PushRequest): Promise<PushResponse>;
    invoke(method: Method): Promise<InvokeResponse>;
    invoke(service: string, args?: any): Promise<InvokeResponse>;
}

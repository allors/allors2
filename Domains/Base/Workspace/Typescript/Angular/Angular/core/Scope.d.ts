import { Observable } from "rxjs/Rx";
import { ISession, Method, Workspace } from "@allors/framework";
import { Database } from "./Database";
import { Invoked } from "./responses/Invoked";
import { Loaded } from "./responses/Loaded";
import { Saved } from "./responses/Saved";
export declare class Scope {
    database: Database;
    workspace: Workspace;
    session: ISession;
    constructor(database: Database, workspace: Workspace);
    load(service: string, params?: any): Observable<Loaded>;
    save(): Observable<Saved>;
    invoke(method: Method): Observable<Invoked>;
    invoke(service: string, args?: any): Observable<Invoked>;
}

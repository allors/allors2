import { ISession, Method, Workspace } from "@allors/base-domain";
import { Database } from "./Database";
import { Invoked } from "./responses/Invoked";
import { Loaded } from "./responses/Loaded";
import { Saved } from "./responses/Saved";
export declare class Scope {
    database: Database;
    workspace: Workspace;
    session: ISession;
    constructor(database: Database, workspace: Workspace);
    load(service: string, params?: any): Promise<Loaded>;
    save(): Promise<Saved>;
    invoke(method: Method): Promise<Invoked>;
    invoke(service: string, args?: any): Promise<Invoked>;
}

import { PushRequest } from "./../database/push/PushRequest";
import { PushResponse } from "./../database/push/PushResponse";
import { ISessionObject } from "./SessionObject";
import { IWorkspace } from "./Workspace";
export interface ISession {
    hasChanges: boolean;
    get(id: string): ISessionObject;
    create(objectTypeName: string): ISessionObject;
    pushRequest(): PushRequest;
    pushResponse(saveResponse: PushResponse): void;
    reset(): void;
}
export declare class Session implements ISession {
    private workspace;
    private static idCounter;
    hasChanges: boolean;
    private sessionObjectById;
    private newSessionObjectById;
    constructor(workspace: IWorkspace);
    get(id: string): ISessionObject;
    create(objectTypeName: string): ISessionObject;
    reset(): void;
    pushRequest(): PushRequest;
    pushResponse(pushResponse: PushResponse): void;
}

import { ObjectType } from "@allors/base-meta";
import { PushRequestNewObject } from "./../database/push/PushRequestNewObject";
import { PushRequestObject } from "./../database/push/PushRequestObject";
import { ISession } from "./Session";
import { IWorkspaceObject } from "./WorkspaceObject";
export interface ISessionObject {
    id: string;
    version: string;
    objectType: ObjectType;
    isNew: boolean;
    session: ISession;
    workspaceObject: IWorkspaceObject;
    hasChanges: boolean;
    canRead(roleTypeName: string): boolean;
    canWrite(roleTypeName: string): boolean;
    canExecute(methodName: string): boolean;
    get(roleTypeName: string): any;
    set(roleTypeName: string, value: any): any;
    add(roleTypeName: string, value: any): any;
    remove(roleTypeName: string, value: any): any;
    save(): PushRequestObject;
    saveNew(): PushRequestNewObject;
    reset(): any;
}
export interface INewSessionObject extends ISessionObject {
    newId: string;
}
export declare class SessionObject implements INewSessionObject {
    session: ISession;
    workspaceObject: IWorkspaceObject;
    objectType: ObjectType;
    newId: string;
    private changedRoleByRoleTypeName;
    private roleByRoleTypeName;
    readonly isNew: boolean;
    readonly hasChanges: boolean;
    readonly id: string;
    readonly version: string;
    canRead(roleTypeName: string): boolean;
    canWrite(roleTypeName: string): boolean;
    canExecute(methodName: string): boolean;
    get(roleTypeName: string): any;
    set(roleTypeName: string, value: any): void;
    add(roleTypeName: string, value: any): void;
    remove(roleTypeName: string, value: any): void;
    save(): PushRequestObject;
    saveNew(): PushRequestNewObject;
    reset(): void;
    private assertExists();
    private saveRoles();
}

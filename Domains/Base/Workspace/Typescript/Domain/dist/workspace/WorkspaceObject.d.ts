import { ObjectType } from "@allors/base-meta";
import { SyncResponse, SyncResponseObject } from "../database/sync/SyncResponse";
import { IWorkspace } from "./Workspace";
export interface IWorkspaceObject {
    id: string;
    version: string;
    userSecurityHash: string;
    objectType: ObjectType;
    workspace: IWorkspace;
    roles: any;
    methods: any;
    canRead(roleTypeName: string): boolean;
    canWrite(roleTypeName: string): boolean;
    canExecute(methodName: string): boolean;
}
export declare class WorkspaceObject implements IWorkspaceObject {
    workspace: IWorkspace;
    roles: any;
    methods: any;
    private i;
    private v;
    private u;
    private t;
    constructor(workspace: IWorkspace, loadResponse: SyncResponse, loadObject: SyncResponseObject);
    readonly id: string;
    readonly version: string;
    readonly userSecurityHash: string;
    readonly objectType: ObjectType;
    canRead(roleTypeName: string): boolean;
    canWrite(roleTypeName: string): boolean;
    canExecute(methodName: string): boolean;
}

import { MetaPopulation } from "@allors/base-meta";
import { PullResponse } from "./../database/pull/PullResponse";
import { SyncRequest } from "./../database/sync/SyncRequest";
import { SyncResponse } from "./../database/sync/SyncResponse";
import { SessionObject } from "./SessionObject";
import { WorkspaceObject } from "./WorkspaceObject";
export interface IWorkspace {
    metaPopulation: MetaPopulation;
    constructorByName: {
        [name: string]: typeof SessionObject;
    };
    diff(data: PullResponse): SyncRequest;
    sync(data: SyncResponse): void;
    get(id: string): WorkspaceObject;
}
export declare class Workspace implements IWorkspace {
    metaPopulation: MetaPopulation;
    constructorByName: {
        [name: string]: typeof SessionObject;
    };
    private workspaceObjectById;
    constructor(metaPopulation: MetaPopulation, constructorByName: {
        [name: string]: typeof SessionObject;
    });
    diff(response: PullResponse): SyncRequest;
    sync(syncResponse: SyncResponse): void;
    get(id: string): WorkspaceObject;
}

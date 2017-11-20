import { ObjectType } from "@allors/base-meta";
import { MetaPopulation } from "@allors/base-meta";

import { PullResponse } from "./../database/pull/PullResponse";
import { SyncRequest } from "./../database/sync/SyncRequest";
import { SyncResponse } from "./../database/sync/SyncResponse";

import { SessionObject } from "./SessionObject";
import { WorkspaceObject } from "./WorkspaceObject";

export interface IWorkspace {
    metaPopulation: MetaPopulation;
    constructorByName: { [name: string]: typeof SessionObject };
    diff(data: PullResponse): SyncRequest;
    sync(data: SyncResponse): void;
    get(id: string): WorkspaceObject;
}

export class Workspace implements IWorkspace {

    private workspaceObjectById: { [id: string]: WorkspaceObject; } = {};

    constructor(public metaPopulation: MetaPopulation, public constructorByName: { [name: string]: typeof SessionObject }) {
    }

    public diff(response: PullResponse): SyncRequest {
        const requireLoadIds = new SyncRequest();

        if (response.objects) {
            const userSecurityHash = response.userSecurityHash;

            const requireLoadIdsWithVersion = response.objects.filter((idAndVersion) => {
                const [id, version] = idAndVersion;
                const workspaceObject = this.workspaceObjectById[id];

                return (workspaceObject === undefined) ||
                    (workspaceObject === null) ||
                    (workspaceObject.version !== version) ||
                    (workspaceObject.userSecurityHash !== userSecurityHash);
            });

            requireLoadIds.objects = requireLoadIdsWithVersion.map((idWithVersion) => {
                return idWithVersion[0];
            });
        }

        return requireLoadIds;
    }

    public sync(syncResponse: SyncResponse): void {
        if (syncResponse.objects) {
            Object
                .keys(syncResponse.objects)
                .forEach((v) => {
                    const objectData = syncResponse.objects[v];
                    const workspaceObject = new WorkspaceObject(this, syncResponse, objectData);
                    this.workspaceObjectById[workspaceObject.id] = workspaceObject;
                });
        }
    }

    public get(id: string): WorkspaceObject {
        const workspaceObject = this.workspaceObjectById[id];
        if (workspaceObject === undefined) {
            throw new Error(`Object with id ${id} is not present.`);
        }

        return workspaceObject;
    }
}
